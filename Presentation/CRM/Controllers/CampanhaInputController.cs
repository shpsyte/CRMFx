using CRM.Extends;
using Data.Context;
using Domain.Entity;
using Services.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CRM.Controllers
{
    [AllowAnonymous]
    public class CampanhaInputController : Controller
    {
        protected b2yweb_entities db = null;
        protected Funcoes _Funcoes = new Funcoes();
        protected SendEmail _email = new SendEmail();
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            db = new b2yweb_entities("oracle");
            _email = new SendEmail();
        }
        public ActionResult Done()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Input()
        {
            ViewBag.cod_representante = new SelectList(db.Ps_Representantes.Where(a => a.ind_inativo == 0) .OrderBy(a => a.des_pessoa), "cod_pessoa", "des_pessoa");
            ViewBag.cod_pessoa = new SelectList(db.Ps_Pessoas.Where(a => a.cod_pessoa == 0), "cod_pessoa", "des_pessoa");
            ViewBag.segmentoid = new SelectList(db.Segmentos, "segmentoid", "des_segmento");
            ViewBag.tipoacaoid = new SelectList(db.TipoAcao.Where(a => a.tipoacaoid == 0), "tipoacaoid", "des_acao");
            ViewBag.formapgtoid = new SelectList(db.FormaPgto, "formapgtoid", "des_forma");
            return View();
        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost, ParameterBasedOnFormName("save-create", "continueAdd")]
        public ActionResult Input(CampanhaMarketing data, bool continueAdd, FormCollection form, IEnumerable<HttpPostedFileBase> files)
        {


            ViewBag.cod_representante = new SelectList(db.Ps_Representantes.Where(a => a.ind_inativo == 0), "cod_pessoa", "des_pessoa", data.cod_representante);
            ViewBag.cod_pessoa = new SelectList(db.Ps_Pessoas.Where(a => a.cod_pessoa == data.cod_pessoa), "cod_pessoa", "des_pessoa", data.cod_pessoa);
            ViewBag.segmentoid = new SelectList(db.Segmentos, "segmentoid", "des_segmento", data.segmentoid);
            ViewBag.tipoacaoid = new SelectList(db.TipoAcao.Where(a => a.tipoacaoid == data.tipoacaoid), "tipoacaoid", "des_acao", data.tipoacaoid);
            ViewBag.formapgtoid = new SelectList(db.FormaPgto, "formapgtoid", "des_forma", data.formapgtoid);

            ModelState.Clear();
            data.dta_inclusao = System.DateTime.Now;
            Estagio estagioinicial = db.Estagio.Where(a => a.ind_inicio == "S").FirstOrDefault();
            data.estagioId = estagioinicial.estagioId;
            data.statusId = Convert.ToInt32(estagioinicial.statusid);
            data.ind_renova = (string.IsNullOrEmpty(data.ind_renova) ? "S" : "N");


            if (data.cod_representante == 0)
            {
                ModelState.AddModelError("cod_representante", "Representante deve ser informado");
                return View(data);
            }

            if (data.cod_pessoa == 0 )
            {
                ModelState.AddModelError("", "Cliente deve ser informado");
                return View(data);
            }

            if (string.IsNullOrEmpty(data.des_nome))
            {
                ModelState.AddModelError("des_nome", "Nome deve ser informado");
                return View(data);
            }

            if (string.IsNullOrEmpty(data.des_email))
            {
                ModelState.AddModelError("des_email", "Email deve ser informado");
                return View(data);
            }

            if (data.segmentoid == 0)
            {
                ModelState.AddModelError("segmentoid", "Tipo deve ser informado");
                return View(data);
            }

            if (data.tipoacaoid == 0)
            {
                ModelState.AddModelError("tipoacaoid", "Ação deve ser informado");
                return View(data);
            }

            if (data.formapgtoid == 0)
            {
                ModelState.AddModelError("formapgtoid", "Forma Pgto deve ser informado");
                return View(data);
            }

            if (!data.dta_inicial.HasValue)
            {
                ModelState.AddModelError("dta_inicial", "Data deve ser informado");
                return View(data);
            }



            if (data.segmentoid == 1 )
            {
                if (!data.vlr_custo_medio.HasValue)
                {
                    ModelState.AddModelError("", "Custo deve ser informado");
                    return View(data);


                }
            }


            if (data.segmentoid == 3)
            {

                if ("V".Equals(data.tip_pgto_premiacao))
                {
                    if (!data.vlr_contrato.HasValue)
                    {
                        ModelState.AddModelError("", "Custo deve ser informado");
                        return View(data);
                    }
                }


                if ("P".Equals(data.tip_pgto_premiacao))
                {
                    if (!data.per_contrato.HasValue)
                    {
                        ModelState.AddModelError("", "Custo deve ser informado");
                        return View(data);
                    }
                }


            }


            if (data.segmentoid == 2)
            {
                if (!data.vlr_meta.HasValue)
                {
                    ModelState.AddModelError("", "Meta deve ser informado");
                    return View(data);


                }
                if (!data.vlr_contrato.HasValue)
                {
                    ModelState.AddModelError("", "Meta deve ser informado");
                    return View(data);


                }


                if (!data.dta_final.HasValue)
                {
                    ModelState.AddModelError("dta_final", "Data deve ser informado");
                    return View(data);
                }

            }


            
            data.campanhaID = db.Database.SqlQuery<Int32>("select CampanhaMarketing_Seq.NextVal from dual ").FirstOrDefault<Int32>();
            data.cod_pessoa_string = data.cod_pessoa.ToString();
            data.des_nome = data.des_nome.ToUpper().FormatToB2y();

            // pega a pessoa do dados crm
            var dados_da_pessoa = db.Dados_crm.Find(data.cod_pessoa.ToString());
            
            if (dados_da_pessoa != null)
            {
                try
                {
                    data.cod_regional = Convert.ToInt32(dados_da_pessoa.cod_gerente);
                }catch
                {
                    data.cod_regional = 1001;
                }


                try
                {
                    data.cod_diretoria = Convert.ToInt32(dados_da_pessoa.cod_diretoria);
                }catch
                {
                    data.cod_diretoria = 2;
                }


                try
                {
                    data.des_segmento = dados_da_pessoa.segmento;
                }catch
                {
                    data.des_segmento = "ELETRICA";
                }
                
            }


            CampanhaMarketingEstagios nova_linha = AddNovaLinhaTrocaEstagio(data.campanhaID, null, data.estagioId, data.des_observacao);
            TryValidateModel(data);
            if (ModelState.IsValid)
            {
                db.CampanhaMarketingEstagios.Add(nova_linha);
                db.CampanhaMarketing.Add(data);
            }
            else {
                return View(data);
            }


            if (files != null)
            {
                foreach (var a in files)
                {
                    if (a != null)
                    {
                        if (a.ContentLength > 0)
                        {


                            int tamanho = (int)a.InputStream.Length;
                            string contentype = a.ContentType;
                            byte[] arq = new byte[tamanho];

                            a.InputStream.Read(arq, 0, tamanho);
                            byte[] arqUp = arq;


                            int CampanhaDocId = db.Database.SqlQuery<Int32>("select CampanhaMarketingDoc_Seq.NextVal from dual ").FirstOrDefault<Int32>();
                            string NomeArquivo = a.FileName; //data.campanhaID.ToString() + '_' + CampanhaDocId.ToString() + '_' + string.Format("{0}", Path.GetFileName(a.FileName));

                            //var path = Path.Combine(Server.MapPath("~/Arquivos/uploads"), NomeArquivo);


                            CampanhaMarketingDoc doc = new CampanhaMarketingDoc
                            {
                                CampanhaMarketingDocId = CampanhaDocId,
                                campanhaID = data.campanhaID,
                                Caminho = NomeArquivo,
                                des_contenttype = contentype,
                                des_imagem = arqUp,
                                nome_arquivo = NomeArquivo
                            };
                            db.CampanhaMarketingDoc.Add(doc);

                            //a.SaveAs(path);



                        }
                    }
                }
            }

            try
            {
                db.SaveChanges();
                string nome = "";

                try
                {
                    nome = data.Ps_Representantes.des_pessoa;
                }
                catch
                {
                    nome = data.des_email;
                }



                try
                { 
                  _email.EnviarEmailCampanhaEntrada(data.campanhaID, "NovaCampanha.htm", data.des_email, nome);
                  _email.EnviarEmailCampanha(data.campanhaID, "ChangeCampanhaEstagio.htm");
                }catch (Exception e)
                {
                    _email.EnviarEmailCampanha(data.campanhaID, "ChangeCampanhaEstagio.htm");
                }


                return RedirectToAction("Done");
            }
            catch (Exception e)
            {
                Response.StatusCode = 500; // Replace .AddHeader
                return Json(new { Error = e.Message }, JsonRequestBehavior.AllowGet);
            }
            return View(data);
        }
        public JsonResult GetEmailPessoa(int id)
        {
            var states = db.Ps_Pessoas.Where(p => p.cod_pessoa == id).Select(a => a.des_email).FirstOrDefault();
            return Json(new { email = states });
        }
        private CampanhaMarketingEstagios AddNovaLinhaTrocaEstagio(int campanhaId, int? cod_estagio_anterior, int? cod_estagio_novo, string msg)
        {
            return new CampanhaMarketingEstagios
            {
                estagioidentrada = cod_estagio_novo,
                estagioidanterior = cod_estagio_anterior,
                campanhaId = campanhaId,
                num_seq = db.Database.SqlQuery<Int32>("select CampanhaMarketingEstagiosSeq.NextVal from dual ").FirstOrDefault<Int32>(),
                dta_entrada = System.DateTime.Now,
                dta_saida = null,
                obs = msg
            };
        }
        public JsonResult GetTipoACao(int id)
        {
            var TipoAction = db.TipoAcao.Where(p => p.segmentoid == id).ToList();
            return Json(TipoAction);
        }
        public JsonResult GetPessoas(int id)
        {



            var states = (from a in
                              db.Clientes.Where(p => p.CD_REPRESENTANTE == id)
                          select new SelectListItem
                          {
                              Value = a.COD_NL,
                              Text = string.Concat(a.RAZAO, "-", a.COD_NL),
                              Selected = false
                          });
            return Json(new SelectList(states.OrderBy(A => A.Text), "Value", "Text"));

            
            
        }
    }
}
