using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Entity;
using Data.Context;
using CRM.Models;
using CRM.DataServices;

namespace CRM.Controllers
{
    [Services.Functions.NoCache]
    public class ContabilidadeController : BasePublicController
    {
        //
        // GET: /Contabilidade/
        public IDataServices<NotaCliente> _notaCliente;
        public IDataServices<NotaItemCliente> _notaItemCliente;
        public IDataServices<Clientes> _clientes;
        public IDataServices<vWOperacaoFiscal> _operacoes;

        public ContabilidadeController(IDataServices<NotaCliente> notaCliente, IDataServices<NotaItemCliente> notaItemCliente, IDataServices<Clientes> clientes, IDataServices<vWOperacaoFiscal> operacoes)
        {
            this._notaCliente = notaCliente;
            this._notaItemCliente = notaItemCliente;
            this._clientes = clientes;
            this._operacoes = operacoes;
        }

        public ActionResult List()
        {
            var data = _notaCliente
                    .Query(notaCliente => notaCliente.num_nota == 0)
                    .Include(a => a.Garantia.Ps_Pessoas)
                    .ToList();

            return View(data);
        }



        [HttpGet]
        public ActionResult Entrada(int id)
        {
            var notaCliente = _notaCliente.Find(id);

            if (notaCliente == null)
                throw new Exception("Dados não encontrados");

            if (notaCliente.num_nota > 0)
                return RedirectToAction("List");



            var data = new NotaEntradaModel()
            {
                NotaCliente = notaCliente,
                ItensNota = _notaItemCliente.GetAll(a => a.NotaClienteId == notaCliente.Id).ToList(),
                cod_operacao = "",
                obs = " Nota importada via Garantia " + id.ToString(),
                dta_emissao = System.DateTime.Now.Date.ToShortDateString(),
                dta_entrada = System.DateTime.Now.Date.ToShortDateString(),
                cod_serie = "1",
                id = id,
                //cod_condicao_pgto = 0,
                //cod_transportador = 1,
                des_chave = "",
                des_local = "10",
                des_lote = id.ToString(),
                des_modelo = "55",
                ind_nfe = true,
                msg = "",
                //  num_nota = 0,
                //,
                //num_nota = 1234,
                //
                //obs = "Nota de Teste",
                //msg = "Inicio da Importação de Notas"
            };

            ViewBag.cod_operacao = new SelectList(_operacoes.GetAll().OrderBy(a => a.Des_Operacao), "Cod_Operacao", "Des_Operacao", data.cod_operacao);

            return View(data);
        }

        [HttpPost]
        public ActionResult Entrada(NotaEntradaModel data)
        {

            var notaCliente = _notaCliente.Find(data.id);
            var orderItemCustomer = _notaItemCliente.GetAll(a => a.NotaClienteId == notaCliente.Id).ToList();
            ViewBag.cod_operacao = new SelectList(_operacoes.GetAll().OrderBy(a => a.Des_Operacao), "Cod_Operacao", "Des_Operacao", data.cod_operacao);

            data.NotaCliente = notaCliente;
            data.ItensNota = orderItemCustomer;

            TryUpdateModel(data);
            if (!ModelState.IsValid)
            {
                var messages = ModelState.Values
                                       .SelectMany(x => x.Errors)
                                       .Select(x => x.ErrorMessage);

                ModelState.AddModelError("Id", "NF Inválida para Entrada");
                return Json(new { data = data, Msg = messages, hasError = true });
            }
            var cliente = _clientes.Find(data.NotaCliente.Garantia.cod_cliente);

            // tp_operacao choice;


            // Enum.TryParse(data.tp_operacao.ToString(), out choice);
            // uint value = (uint)choice;



            var chaves = new Ne_Nota_Chave()
            {
                COD_EMP = 1,
                COD_PESSOA_FORN = data.NotaCliente.Garantia.cod_cliente.ToString(),
                COD_SERIE = data.cod_serie,
                COD_UNIDADE = cliente.CD_REGIONAL,
                DTA_EMISSAO = Convert.ToDateTime(data.dta_emissao),
                NUM_NOTA = data.num_nota.Value,
                COD_OPERACAO = data.cod_operacao,
                COD_TRANSPORTADOR = data.cod_transportador,
                DTA_ENTRADA = System.DateTime.Now,
                OBS = data.obs,
                COD_CONDICAO_PGTO = data.cod_condicao_pgto,
                DES_CHAVE = data.des_chave,
                DES_LOCAL = data.des_local,
                DES_LOTE = data.des_lote,
                DES_MODELO = data.des_modelo,
                IND_NFE = data.ind_nfe ? 1 : 0,
                TP_OPERACAO = 2//(int)value
            };



            try
            {
                string comand = $" Begin spcDeleteNFEntrada " +
                    $" ({chaves.COD_EMP}, \'{chaves.COD_PESSOA_FORN}\' , \'{chaves.COD_SERIE}\',{chaves.COD_UNIDADE}, \'{chaves.DTA_EMISSAO.ToShortDateString()}\',  \'{Convert.ToDateTime(data.dta_entrada).ToShortDateString()}\', {chaves.NUM_NOTA}); end;";

                db.Database.ExecuteSqlCommand(comand);

            }
            catch (Exception e)
            {
                return Json(new { data = data, Msg = e.Message, hasError = true });
            }


            IList<NE_NOTA_MENSAGEM> listamsg = null;

            try
            {
                db.Ne_Nota_Chave.Add(chaves);

                string comand = $" Begin spcInsertNFEntrada " +
                    $" ( {data.id}, {chaves.COD_EMP}, \'{chaves.COD_PESSOA_FORN}\' , \'{chaves.COD_SERIE}\',{chaves.COD_UNIDADE}, \'{chaves.DTA_EMISSAO.ToShortDateString()}\',  \'{Convert.ToDateTime(data.dta_entrada).ToShortDateString()}\', {chaves.NUM_NOTA}); end;";

                chaves.CMD = comand;
                db.SaveChanges();


                db.Database.ExecuteSqlCommand(comand);
                 

                listamsg = db.NE_NOTA_MENSAGEM.Where(a => a.NUM_NOTA == chaves.NUM_NOTA
                                                               && a.COD_SERIE == chaves.COD_SERIE
                                                               && a.COD_PESSOA_FORN == chaves.COD_PESSOA_FORN
                                                               && a.COD_UNIDADE == chaves.COD_UNIDADE).ToList();

                return Json(new { data = data, Msg = listamsg.OrderBy(a => a.ID_MENSAGEM), hasError = listamsg.Select(a => a.HASERROR).FirstOrDefault() == 1 });

            }
            catch (Exception e)
            {
                string msg = e.InnerException.Message;
                string details;

                try
                {
                    details = string.IsNullOrEmpty(e.InnerException.InnerException.Message) ? "" : e.InnerException.InnerException.Message;
                }
                catch (Exception)
                {
                    details = "";
                }


                listamsg.Add(new NE_NOTA_MENSAGEM() { MSG = details });
                listamsg.Add(new NE_NOTA_MENSAGEM() { MSG = msg });


                return Json(new { data = data, Msg = listamsg, hasError = true });
            }


        }




        public JsonResult ReadOperacao(int cod_oper)
        {
            var Operacao = db.OperacaoFiscal.Where(a => a.Cod_Operacao == cod_oper).FirstOrDefault();

            if (Operacao == null)
            {
                Operacao = new vWOperacaoFiscal()
                {
                    Cod_Operacao = 0,
                    Des_Operacao = "Erro ao buscar Operação Fiscal"
                };
            }

            return Json(new { Operacao }, JsonRequestBehavior.AllowGet);



        }



        public JsonResult ReadTransportador(int cod_transportador)
        {
            var Transportador = db.TRANSPORTADOR.Where(a => a.COD_TRANSPORTADOR == cod_transportador).FirstOrDefault();

            if (Transportador == null)
            {
                Transportador = new TRANSPORTADOR()
                {
                    COD_TRANSPORTADOR = 0,
                    RAZAO = "Nehum dado retornado"
                };
            }

            return Json(new { Transportador }, JsonRequestBehavior.AllowGet);



        }


        public JsonResult ReadCondicao(int cod_condicao_pgto)

        {
            var Condicao = db.t4_conds_pgto.Where(a => a.COD_COND_PGTO == cod_condicao_pgto).FirstOrDefault();

            if (Condicao == null)
            {
                Condicao = new t4_conds_pgto()
                {
                    COD_COND_PGTO = 0,
                    DES_COND_PGTO = "Erro ao buscar condição"
                };
            }

            return Json(new { Condicao }, JsonRequestBehavior.AllowGet);



        }



        [HttpPost]
        public JsonResult PostValor(int id, int nfid, decimal? vlr_lancamento, decimal? vlr_ipi, decimal? vlr_st)
        {


            var dadosDosItens = db.NotaItemCliente.Find(id, nfid);
            var itens = db.NotaItemCliente.Where(a => a.NotaClienteId == nfid).ToList();
            var nota = db.NotaCliente.Find(nfid);
            string erro = "";
            bool hasError = false;


            dadosDosItens.Vlr_Lancamento = vlr_lancamento.HasValue ? (decimal)vlr_lancamento : dadosDosItens.Vlr_Lancamento;
            dadosDosItens.Vlr_ipi = vlr_ipi.HasValue ? vlr_ipi : dadosDosItens.Vlr_ipi;
            dadosDosItens.Vlr_Imcs_Subs = vlr_st.HasValue ? vlr_st : dadosDosItens.Vlr_Imcs_Subs;
            dadosDosItens.Vlr_Total = (decimal)dadosDosItens.Qtd_Lancamento * dadosDosItens.Vlr_Lancamento;

            var itemDaLista = itens.Find(a => a.Id == id);
            itemDaLista.Vlr_Lancamento = dadosDosItens.Vlr_Lancamento;
            itemDaLista.Vlr_ipi = dadosDosItens.Vlr_ipi;
            itemDaLista.Vlr_Imcs_Subs = dadosDosItens.Vlr_Imcs_Subs;
            itemDaLista.Vlr_Total = dadosDosItens.Vlr_Total;

            nota.Vlr_mercadorias = (decimal)itens.Sum(a => a.Qtd_Lancamento * a.Vlr_Lancamento);
            nota.Vlr_Nota = nota.Vlr_mercadorias;


            db.Entry(nota).State = EntityState.Modified;
            db.Entry(dadosDosItens).State = EntityState.Modified;



            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                hasError = true;
                erro = e.Message;
            }

            Alteravalor data = new Alteravalor()
            {
                 id = id
                , nfid = nfid
                , vlr_lancamento = dadosDosItens.Vlr_Lancamento
                , vlr_ipi = dadosDosItens.Vlr_ipi
                , vlr_st = dadosDosItens.Vlr_Imcs_Subs
                , vlr_total = dadosDosItens.Vlr_Total
                , itens = itens
                , item = dadosDosItens
                , nota = nota
                , haserror = hasError
                , msg = erro
            };


            return Json(data, JsonRequestBehavior.AllowGet);


        }


        class Alteravalor
        {
            public int id { get; set; }
            public int nfid { get; set; }
            public decimal? vlr_lancamento { get; set; }
            public decimal? vlr_ipi { get; set; }
            public decimal? vlr_st { get; set; }
            public List<NotaItemCliente> itens;
            public NotaItemCliente item { get; set; }
            public NotaCliente nota { get; set; }
            public decimal? vlr_total { get; set; }
            public bool haserror { get; set; }
            public string msg { get; set; }
        }

    }
}
