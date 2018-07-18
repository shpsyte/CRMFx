using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Services.Functions;

using Domain.Entity;
using Data.Context;
using Services.Functions;
using System.Web.Script.Serialization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Data.SqlClient;
using iTextSharp.text.pdf.draw;

namespace b2yweb_mvc4.Areas.Procedimento.Controllers
{
    [AuthFilter]
    public class RelatoriosController : Controller
    {
        private b2yweb_entities db = null;
        readonly Funcoes _Funcoes = new Funcoes();
        //
        // GET: /Procedimento/ProcedimentoAdm/
        /// <summary>
        /// Função Para Verificar se o usuário é autenticado
        /// </summary>
        /// <param name="requestContext"></param>
        [AuthFilter]
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.Session["oEmpresa"] != null)
            {
                db = new b2yweb_entities(requestContext.HttpContext.Session["oEmpresa"].ToString());
            }
        }

        /// <summary>
        /// Necessário para não dar o erro do JSON quando tem muitos registros
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="contentEncoding"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }



        [CustomAuthorize(AccessLevel = "Relatorios")]
        public ActionResult ReadProcessos([DataSourceRequest] DataSourceRequest request)
        {
            return Json(db.wProcedimento.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }



        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public ActionResult ReadProcedimentoDepartamento(int COD_PROCEDIMENTO, [DataSourceRequest] DataSourceRequest request)
        {
            var res = (from a in db.wpa_troca_departamentos
                       where 1 == 1
                       && a.CD_PROCEDIMENTO == COD_PROCEDIMENTO
                       orderby a.NUM_SEQ descending
                       select a);

            return Json(res.ToDataSourceResult(request));
        }

        //



        [AuthFilter]
        [CustomAuthorize(AccessLevel = "Relatorios")]
        public ActionResult TempoLiberacaoResult(int nr_procedimento = 0)
        {
            var ProcedimentoAdm = (
             from a in db.ProcedimentoAdm.ToList() select a);

            var trocas = (from a in db.wpa_troca_departamentos select a.CD_DEPARTAMENTO_NOVA).ToList();


            ViewData["Customers"] = ProcedimentoAdm;
            ViewData["Orders"] = (from a in db.wpa_troca_departamentos
                                  where 1 == 2
                                  orderby a.CD_DEPARTAMENTO_NOVA descending
                                  select a);



            return View();
        }


        // GET: /Procedimento/Relatorios/
        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public ActionResult ProcedimentoDepartamento()
        {
            return View();
        }

        [CustomAuthorize(AccessLevel = "Relatorios")]
        [HttpPost]
        public ActionResult ProcedimentoDepartamentoResult(int nr_procedimento = 0
            , string dt_inicial = ""
            , string dt_final = ""
            , int CD_CADASTRO = 0
            , int CD_TIPO = 0
            , int CD_DEPARTAMENTO = 0
            , int CD_USUARIO = 0
            , int ID_SITUACAO = 0
            , string Destino = "")
        {
            var list_regional = (List<int>)Session["oRegional"];
            int cd_usuario = ((Usuario)Session["oUsuario"]).CD_USUARIO;
            var list_departamento = (from a in db.DepartamentoUsuario.Where(a => a.CD_USUARIO == cd_usuario) select a.CD_DEPARTAMENTO).ToList();
            int cd_grupo = ((Usuario)Session["oUsuario"]).CD_GUSUARIO;

            DateTime _dt_inicial;
            DateTime _dt_final;
            int _nr_procedimento_ini;
            int _nr_procedimento_fim;
            int _cd_cadastro_ini;
            int _cd_cadastro_fim;
            int _cd_tipo_ini;
            int _cd_tipo_fim;
            int _cd_dep_ini;
            int _cd_dep_fim;
            int _cd_usuario_ini;
            int _cd_usuario_fim;
            int _situacao_ini;
            int _situacao_fim;


            if (ID_SITUACAO > 0)
            {
                _situacao_ini = Convert.ToInt32(ID_SITUACAO.ToString());
                _situacao_fim = Convert.ToInt32(ID_SITUACAO.ToString());

            }
            else
            {
                _situacao_ini = 0;
                _situacao_fim = 99999999;

            }


            if (CD_USUARIO > 0)
            {
                _cd_usuario_ini = Convert.ToInt32(CD_USUARIO.ToString());
                _cd_usuario_fim = Convert.ToInt32(CD_USUARIO.ToString());

            }
            else
            {
                _cd_usuario_ini = 0;
                _cd_usuario_fim = 99999999;

            }



            if (dt_inicial.HasValue())
            {
                _dt_inicial = Convert.ToDateTime(Convert.ToDateTime(dt_inicial.ToString()).ToString("yyyy-MM-dd 00:00:00"));
            }
            else
            {
                _dt_inicial = DateTime.Now.AddYears(-1);
            }


            if (dt_final.HasValue())
            {
                _dt_final = Convert.ToDateTime(Convert.ToDateTime(dt_final.ToString()).ToString("yyyy-MM-dd 23:59:59"));
            }
            else
            {
                _dt_final = DateTime.Now.AddYears(1);
            }

            if (CD_DEPARTAMENTO > 0)
            {
                _cd_dep_ini = Convert.ToInt32(CD_DEPARTAMENTO.ToString());
                _cd_dep_fim = Convert.ToInt32(CD_DEPARTAMENTO.ToString());

            }
            else
            {
                _cd_dep_ini = 0;
                _cd_dep_fim = 99999999;

            }

            if (nr_procedimento > 0)
            {
                _nr_procedimento_ini = Convert.ToInt32(nr_procedimento.ToString());
                _nr_procedimento_fim = Convert.ToInt32(nr_procedimento.ToString());

            }
            else
            {
                _nr_procedimento_ini = 0;
                _nr_procedimento_fim = 99999999;

            }

            if (CD_CADASTRO > 0)
            {
                _cd_cadastro_ini = Convert.ToInt32(CD_CADASTRO.ToString());
                _cd_cadastro_fim = Convert.ToInt32(CD_CADASTRO.ToString());
            }
            else
            {
                _cd_cadastro_ini = 0;
                _cd_cadastro_fim = 9999999;

            }


            if (CD_TIPO > 0)
            {
                _cd_tipo_ini = Convert.ToInt32(CD_TIPO.ToString());
                _cd_tipo_fim = Convert.ToInt32(CD_TIPO.ToString());
            }
            else
            {
                _cd_tipo_ini = 0;
                _cd_tipo_fim = 99999999;

            }

            var ProcedimentoAdm =
                  (from a in db.wProcedimento.Where
                      (c =>
                          (c.CD_PROCEDIMENTO >= _nr_procedimento_ini) &&
                          (c.CD_PROCEDIMENTO <= _nr_procedimento_fim) &&

                          (c.DTA_ABERTURA >= _dt_inicial) &&
                          (c.DTA_ABERTURA <= _dt_final) &&

                          (c.CD_CADASTRO >= _cd_cadastro_ini) &&
                          (c.CD_CADASTRO <= _cd_cadastro_fim) &&

                          (c.CD_TIPO >= _cd_tipo_ini) &&
                          (c.CD_TIPO <= _cd_tipo_fim) &&

                          (c.CD_DEPARTAMENTO >= _cd_dep_ini) &&
                          (c.CD_DEPARTAMENTO <= _cd_dep_fim) &&

                          (c.ID_SITUACAO >= _situacao_ini) &&
                          (c.ID_SITUACAO <= _situacao_fim) &&

                          (c.CD_USUARIO >= _cd_usuario_ini) &&
                          (c.CD_USUARIO <= _cd_usuario_fim)).ToList()
                   select a).OrderByDescending(a => a.CD_PROCEDIMENTO).ToList();


            ViewData["Customers"] = ProcedimentoAdm;
            ViewData["Orders"] = (from a in db.wpa_troca_departamentos
                                  where 1 == 2
                                  orderby a.CD_DEPARTAMENTO_NOVA descending
                                  select a);



            return View(ProcedimentoAdm);



        }



        // GET: /Procedimento/Relatorios/
        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public ActionResult TempoLiberacao()
        {
            return View();
        }

        [CustomAuthorize(AccessLevel = "Relatorios")]
        [HttpPost]
        public ActionResult TempoLiberacaoResult(int nr_procedimento = 0
            , string dt_inicial = ""
            , string dt_final = ""
            , int CD_CADASTRO = 0
            , int CD_TIPO = 0
            , int CD_DEPARTAMENTO = 0
            , int CD_USUARIO = 0
            , int ID_SITUACAO = 0
            , string Destino = "")
        {
            var list_regional = (List<int>)Session["oRegional"];
            int cd_usuario = ((Usuario)Session["oUsuario"]).CD_USUARIO;
            var list_departamento = (from a in db.DepartamentoUsuario.Where(a => a.CD_USUARIO == cd_usuario) select a.CD_DEPARTAMENTO).ToList();
            int cd_grupo = ((Usuario)Session["oUsuario"]).CD_GUSUARIO;

            DateTime _dt_inicial;
            DateTime _dt_final;
            int _nr_procedimento_ini;
            int _nr_procedimento_fim;
            int _cd_cadastro_ini;
            int _cd_cadastro_fim;
            int _cd_tipo_ini;
            int _cd_tipo_fim;
            int _cd_dep_ini;
            int _cd_dep_fim;
            int _cd_usuario_ini;
            int _cd_usuario_fim;
            int _situacao_ini;
            int _situacao_fim;


            if (ID_SITUACAO > 0)
            {
                _situacao_ini = Convert.ToInt32(ID_SITUACAO.ToString());
                _situacao_fim = Convert.ToInt32(ID_SITUACAO.ToString());

            }
            else
            {
                _situacao_ini = 0;
                _situacao_fim = 99999999;

            }


            if (CD_USUARIO > 0)
            {
                _cd_usuario_ini = Convert.ToInt32(CD_USUARIO.ToString());
                _cd_usuario_fim = Convert.ToInt32(CD_USUARIO.ToString());

            }
            else
            {
                _cd_usuario_ini = 0;
                _cd_usuario_fim = 99999999;

            }



            if (dt_inicial.HasValue())
            {
                _dt_inicial = Convert.ToDateTime(Convert.ToDateTime(dt_inicial.ToString()).ToString("yyyy-MM-dd 00:00:00"));
            }
            else
            {
                _dt_inicial = DateTime.Now.AddYears(-1);
            }


            if (dt_final.HasValue())
            {
                _dt_final = Convert.ToDateTime(Convert.ToDateTime(dt_final.ToString()).ToString("yyyy-MM-dd 23:59:59"));
            }
            else
            {
                _dt_final = DateTime.Now.AddYears(1);
            }

            if (CD_DEPARTAMENTO > 0)
            {
                _cd_dep_ini = Convert.ToInt32(CD_DEPARTAMENTO.ToString());
                _cd_dep_fim = Convert.ToInt32(CD_DEPARTAMENTO.ToString());

            }
            else
            {
                _cd_dep_ini = 0;
                _cd_dep_fim = 99999999;

            }

            if (nr_procedimento > 0)
            {
                _nr_procedimento_ini = Convert.ToInt32(nr_procedimento.ToString());
                _nr_procedimento_fim = Convert.ToInt32(nr_procedimento.ToString());

            }
            else
            {
                _nr_procedimento_ini = 0;
                _nr_procedimento_fim = 99999999;

            }

            if (CD_CADASTRO > 0)
            {
                _cd_cadastro_ini = Convert.ToInt32(CD_CADASTRO.ToString());
                _cd_cadastro_fim = Convert.ToInt32(CD_CADASTRO.ToString());
            }
            else
            {
                _cd_cadastro_ini = 0;
                _cd_cadastro_fim = 9999999;

            }


            if (CD_TIPO > 0)
            {
                _cd_tipo_ini = Convert.ToInt32(CD_TIPO.ToString());
                _cd_tipo_fim = Convert.ToInt32(CD_TIPO.ToString());
            }
            else
            {
                _cd_tipo_ini = 0;
                _cd_tipo_fim = 99999999;

            }

            var ProcedimentoAdm = (
                  from a in db.wProcedimento.Where
                      (c =>
                          (c.CD_PROCEDIMENTO >= _nr_procedimento_ini) &&
                          (c.CD_PROCEDIMENTO <= _nr_procedimento_fim) &&

                          (c.DTA_ABERTURA >= _dt_inicial) &&
                          (c.DTA_ABERTURA <= _dt_final) &&

                          (c.CD_CADASTRO >= _cd_cadastro_ini) &&
                          (c.CD_CADASTRO <= _cd_cadastro_fim) &&

                          (c.CD_TIPO >= _cd_tipo_ini) &&
                          (c.CD_TIPO <= _cd_tipo_fim) &&

                          (c.CD_DEPARTAMENTO >= _cd_dep_ini) &&
                          (c.CD_DEPARTAMENTO <= _cd_dep_fim) &&

                          (c.ID_SITUACAO >= _situacao_ini) &&
                          (c.ID_SITUACAO <= _situacao_fim) &&

                          (c.CD_USUARIO >= _cd_usuario_ini) &&
                          (c.CD_USUARIO <= _cd_usuario_fim))

                  select a).OrderByDescending(a => a.CD_PROCEDIMENTO).ToList();

            var trocas = (from a in db.pa_troca_departamentos select a.CD_DEPARTAMENTO_NOVA).ToList();


            ViewData["Customers"] = ProcedimentoAdm;
            ViewData["Orders"] = (from a in db.wpa_troca_departamentos
                                  where 1 == 2
                                  orderby a.CD_DEPARTAMENTO_NOVA descending
                                  select a);



            return View();



        }



        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public ActionResult PrintPro()
        {
            return View();
        }


        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public FileStreamResult PrintProPDF(int nr_procedimento = 0)
        {
            MemoryStream workStream = new MemoryStream();
            Document document = new Document();
            //PdfWriter.GetInstance(document, workStream).CloseStream = false;
            string imagepath = Server.MapPath("~/Images");
            Image gif = Image.GetInstance(imagepath + "/Logo_P.gif");
            PdfWriter writer = PdfWriter.GetInstance(document, workStream);
            writer.CloseStream = false;
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times = new Font(bfTimes, 12, Font.NORMAL, Color.RED);
            Font arial8 = FontFactory.GetFont("Arial", 12, Color.BLACK);
            Font verdana = FontFactory.GetFont("Verdana", 16, Font.BOLD, Color.ORANGE);
            Font fverdana = FontFactory.GetFont("Verdana", 12, Color.BLACK);

            Font palatino = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              10,
              Font.ITALIC,
              Color.GREEN
              );
            Font smallfont = FontFactory.GetFont("Arial", 7);
            Font xarial = FontFactory.GetFont("Arial");
            xarial.SetStyle("Bold");


            document.Open();
            document.Add(gif);
            document.Add(new Paragraph(" "));

            //PdfContentByte cb = writer.DirectContent;
            //cb.Rectangle(10f, 200f, 800f, 600f);
            //cb.Stroke();


            ProcedimentoAdm proc = db.ProcedimentoAdm.Find(nr_procedimento);
            var trocas = (from a in db.wpa_troca_departamentos.Where(a => a.CD_PROCEDIMENTO == nr_procedimento) select a).OrderBy(a => a.NUM_SEQ).ToList();
            string ultima = (from a in db.wpa_troca_departamentos.Where(a => a.CD_PROCEDIMENTO == nr_procedimento).OrderByDescending(a => a.NUM_SEQ)
                             select a.OBS).FirstOrDefault();



            Paragraph _TITULO = new Paragraph();
            _TITULO.Alignment = Element.ALIGN_CENTER;
            _TITULO.Add(new Chunk("HISTÓRICO DE PROCEDIMENTO", verdana));
            Chunk linebreak = new Chunk(new LineSeparator(1f, 100f, Color.LIGHT_GRAY, Element.ALIGN_CENTER, -1));

            document.Add(_TITULO);
            //document.Add(new Paragraph("______________________________________________________________________________"));
            document.Add(new Paragraph(" "));
            document.Add(new Paragraph("Dados do Cliente:", arial8));







            PdfContentByte cb = writer.DirectContent;
            //cb.SetColorStroke(new CMYKColor(1f, 0f, 0f, 0f));
            //cb.SetColorFill(new CMYKColor(0f, 0f, 1f, 0f));
            //cb.MoveTo(70, 200);
            //cb.LineTo(170, 200);
            //cb.LineTo(170, 300);
            //cb.LineTo(70, 300);
            //cb.ClosePathStroke();


            document.Add(linebreak);

            PdfPTable table_cliente = new PdfPTable(2);
            table_cliente.TotalWidth = 520f;
            table_cliente.LockedWidth = true;
            table_cliente.DefaultCell.BorderWidth = 1;
            float[] widths = new float[] { 80f, 200f };
            table_cliente.SetWidths(widths);
            table_cliente.HorizontalAlignment = 0;
            //table_cliente.SpacingBefore = 20f;
            //table_cliente.SpacingAfter = 30f;


            fverdana.Size = 8;
            Paragraph paragrafo = new Paragraph();

            table_cliente.AddCell(new Chunk("Nº do Procedimento Administrativo : ", fverdana).ToString());
            table_cliente.AddCell(new Chunk(proc.CD_PROCEDIMENTO.ToString(), fverdana).ToString());

            document.Add(table_cliente);

            //document.Add(paragrafo);

            paragrafo.Clear();
            paragrafo.Add(new Chunk(string.Format("Cód. Cliente: {0}", proc.CD_CADASTRO), fverdana));
            document.Add(paragrafo);

            paragrafo.Clear();
            paragrafo.Add(new Chunk(string.Format("Razão Social: {0}", proc.Clientes.RAZAO), fverdana));
            document.Add(paragrafo);

            paragrafo.Clear();
            paragrafo.Add(new Chunk(string.Format("Transportador: {0}", proc.TRANSPORTADOR.RAZAO), fverdana));
            document.Add(paragrafo);

            document.Add(new Paragraph(" "));
            paragrafo.Clear();
            paragrafo.Add(new Chunk(string.Format("Última: {0}", ultima), fverdana));
            document.Add(paragrafo);


            document.Add(new Paragraph(" "));
            document.Add(new Paragraph(" "));

            PdfPTable table = new PdfPTable(5);
            table.TotalWidth = 540f;
            table.LockedWidth = true;
            table.DefaultCell.BorderWidth = 1;

            float[] widths_tab = new float[] { 40f, 40f, 40f, 40f, 40f };
            table.SetWidths(widths_tab);
            table.HorizontalAlignment = 0;
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;

            table.AddCell("Nome");
            table.AddCell("Departamento");
            table.AddCell("Entrada");
            table.AddCell("Saida");
            table.AddCell("Horas");

            foreach (var x in trocas)
            {
                table.AddCell(x.Usuario.NOME);
                table.AddCell(x.DEPANT.DESC_DEPARTAMENTO);
                table.AddCell(x.DTA_ENTRADA_DEP_NOVA.ToString());
                table.AddCell(x.DTA_SAIDA_DEP_NOVA.ToString());
                table.AddCell(x.HORASCORRIDAS);
                //   table.AddCell(x.OBS);
            }
            document.Add(table);




            /*PdfPTable table_2_colunas = new PdfPTable(2);
            table_2_colunas.TotalWidth = 340f;
            table_2_colunas.LockedWidth = true;
            table_2_colunas.DefaultCell.BorderWidth = 0;
            float[] widths = new float[] { 40f, 20f };
            table_2_colunas.SetWidths(widths);
            table_2_colunas.HorizontalAlignment = 0;
            table_2_colunas.SpacingBefore = 20f;
            table_2_colunas.SpacingAfter = 30f;


            paragrafo.Add(new Chunk("Nº do Procedimento Administrativo :", fverdana));
            table_2_colunas.AddCell(paragrafo);
            table_2_colunas.AddCell(proc.CD_PROCEDIMENTO.ToString());
            document.Add(table_2_colunas);



            PdfPTable table_cliente = new PdfPTable(4);
            paragrafo.Clear();
            table_cliente.TotalWidth = 540f;
            table_cliente.LockedWidth = true;
            table_cliente.DefaultCell.BorderWidth = 1;
            float[] widths_cliente = new float[] { 20f, 40f, 20f, 40f };
            table_cliente.SetWidths(widths_cliente);
            table_cliente.HorizontalAlignment = 0;
            table_cliente.SpacingBefore = 20f;
            table_cliente.SpacingAfter = 30f;
            paragrafo.Add(new Chunk("Nº do Procedimento Administrativo :", fverdana));
            table_cliente.AddCell(paragrafo);
            paragrafo.Clear();
            paragrafo.Add(new Chunk(proc.CD_CADASTRO.ToString(), fverdana));
            table_cliente.AddCell(paragrafo);
            paragrafo.Clear();
            paragrafo.Add(new Chunk("Razão Social", fverdana));
            table_cliente.AddCell(paragrafo);
            paragrafo.Clear();
            paragrafo.Add(new Chunk(proc.Clientes.RAZAO, fverdana));
            table_cliente.AddCell(paragrafo);
            document.Add(table_cliente);
            
            */




            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return new FileStreamResult(workStream, "application/pdf");
        }

        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public ActionResult TMR()
        {
            return View();
        }

        [CustomAuthorize(AccessLevel = "Relatorios")]
        [HttpPost]
        public ActionResult TMRR(
            int nr_procedimento = 0
            , string dt_inicial = ""
            , string dt_final = ""
            , int CD_TIPO = 0
             , int CD_DEPARTAMENTO = 0
            , string UF = ""
            , string Destino = "")
        {
            var list_regional = (List<int>)Session["oRegional"];
            int cd_usuario = ((Usuario)Session["oUsuario"]).CD_USUARIO;
            var list_departamento = (from a in db.DepartamentoUsuario.Where(a => a.CD_USUARIO == cd_usuario) select a.CD_DEPARTAMENTO).ToList();
            int cd_grupo = ((Usuario)Session["oUsuario"]).CD_GUSUARIO;

            DateTime _dt_inicial;
            DateTime _dt_final;
            int _nr_procedimento_ini;
            int _nr_procedimento_fim;
            int _cd_cadastro_ini;
            int _cd_cadastro_fim;
            int _cd_tipo_ini;
            int _cd_tipo_fim;
            int _cd_dep_ini;
            int _cd_dep_fim;
            int _cd_usuario_ini;
            int _cd_usuario_fim;
            int _situacao_ini;
            int _situacao_fim;




            if (dt_inicial.HasValue())
            {
                _dt_inicial = Convert.ToDateTime(Convert.ToDateTime(dt_inicial.ToString()).ToString("yyyy-MM-dd 00:00:00"));
            }
            else
            {
                _dt_inicial = DateTime.Now.AddYears(-1);
            }


            if (dt_final.HasValue())
            {
                _dt_final = Convert.ToDateTime(Convert.ToDateTime(dt_final.ToString()).ToString("yyyy-MM-dd 23:59:59"));
            }
            else
            {
                _dt_final = DateTime.Now.AddYears(1);
            }

            if (CD_DEPARTAMENTO > 0)
            {
                _cd_dep_ini = Convert.ToInt32(CD_DEPARTAMENTO.ToString());
                _cd_dep_fim = Convert.ToInt32(CD_DEPARTAMENTO.ToString());

            }
            else
            {
                _cd_dep_ini = 0;
                _cd_dep_fim = 99999999;

            }

            if (CD_TIPO > 0)
            {
                _cd_tipo_ini = Convert.ToInt32(CD_TIPO.ToString());
                _cd_tipo_fim = Convert.ToInt32(CD_TIPO.ToString());
            }
            else
            {
                _cd_tipo_ini = 0;
                _cd_tipo_fim = 99999999;

            }


            if (nr_procedimento > 0)
            {
                _nr_procedimento_ini = Convert.ToInt32(nr_procedimento.ToString());
                _nr_procedimento_fim = Convert.ToInt32(nr_procedimento.ToString());

            }
            else
            {
                _nr_procedimento_ini = 0;
                _nr_procedimento_fim = 99999999;

            }

            decimal Vlrp = new decimal(0.02);


            var retorno_aux =
              (from a in db.wpa_troca_departamentos
               where a.DTA_TROCA >= _dt_inicial &&
                a.DTA_TROCA <= _dt_final &&
                a.ProcedimentoAdm.CD_TIPO >= _cd_tipo_ini &&
                a.ProcedimentoAdm.CD_TIPO <= _cd_tipo_fim &&
                a.CD_DEPARTAMENTO_NOVA >= _cd_dep_ini &&
                a.CD_DEPARTAMENTO_NOVA <= _cd_dep_fim &&
                a.CD_PROCEDIMENTO >= _nr_procedimento_ini &&
                a.CD_PROCEDIMENTO <= _nr_procedimento_fim &&
                (!string.IsNullOrEmpty(UF) ? UF.Contains(a.ProcedimentoAdm.Clientes.CD_ESTADO.ToUpper()) : 1 == 1)
               select a).ToList();



            var retorno =
                from a in db.wpa_troca_departamentos
                where a.DTA_TROCA >= _dt_inicial &&
                 a.DTA_TROCA <= _dt_final &&
                 a.ProcedimentoAdm.CD_TIPO >= _cd_tipo_ini &&
                 a.ProcedimentoAdm.CD_TIPO <= _cd_tipo_fim &&
                 a.CD_DEPARTAMENTO_NOVA >= _cd_dep_ini &&
                 a.CD_DEPARTAMENTO_NOVA <= _cd_dep_fim &&
                 a.CD_PROCEDIMENTO >= _nr_procedimento_ini &&
                 a.CD_PROCEDIMENTO <= _nr_procedimento_fim &&
                 (!string.IsNullOrEmpty(UF) ? UF.Contains(a.ProcedimentoAdm.Clientes.CD_ESTADO.ToUpper()) : 1 == 1)
                 && a.HORASNUMBER >= Vlrp
                group a by new
                {
                    a.CD_PROCEDIMENTO,
                    a.ProcedimentoAdm.Clientes.FANTASIA,
                    a.ProcedimentoAdm.tp_procedimento.DES_TIPO,
                    a.DEPNOVA.DESC_DEPARTAMENTO
                } into g
                select new wpa_troca_departamentos_unico
                {
                    Procedimento = g.Key.CD_PROCEDIMENTO,
                    Fantasia = g.Key.FANTASIA,
                    Tipo = g.Key.DES_TIPO,
                    Departamento = g.Key.DESC_DEPARTAMENTO,
                    Horas = g.Sum(p => p.HORASNUMBER),
                    HorasString = ""
                };


            var retorno_result = retorno.ToList();

            var retorno_json = (from a in retorno_result
                                select new wpa_troca_departamentos_unico
                                {
                                    Procedimento = a.Procedimento,
                                    Fantasia = a.Fantasia,
                                    Tipo = a.Tipo,
                                    Departamento = a.Departamento,
                                    HorasString = string.Concat(
                                    Math.Truncate(Convert.ToDecimal(a.Horas)).ToString(), ":",
                                    Convert.ToString(Math.Truncate(Math.Round(((Convert.ToDecimal(a.Horas) - Math.Truncate(Convert.ToDecimal(a.Horas))) * 60), 2))).PadLeft(2, '0')
                                    ),
                                    Horas = 0
                                });




            decimal? averageTicks = 0;

            averageTicks = retorno_result.Where(erika => erika.Horas > Vlrp).Select(d => d.Horas).Average();

            string tempo_medio = string.Concat(Math.Truncate(Convert.ToDecimal(averageTicks)).ToString().PadLeft(2, '0'), ":", Convert.ToString(Math.Truncate(Math.Round(((Convert.ToDecimal(averageTicks) - Math.Truncate(Convert.ToDecimal(averageTicks))) * 60), 2))).PadLeft(2, '0'));

            decimal? qtde_fora = retorno_aux.Where(a => a.PERCENTUAL > 100).Count();

            ViewData["tempo_medio_resposta"] = tempo_medio;
            ViewData["qtde_fora"] = qtde_fora;



            return View(retorno_json);


        }




        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public ActionResult TempoMedio()
        {
            return View();
        }

        [CustomAuthorize(AccessLevel = "Relatorios")]
        [HttpPost]
        public ActionResult TempoMedioResult(
            int nr_procedimento = 0
            , string dt_inicial = ""
            , string dt_final = ""
            , int CD_TIPO = 0
             , int CD_DEPARTAMENTO = 0
            , string UF = ""
            , string Destino = "",
            [DataSourceRequest] DataSourceRequest request = null)
        {
            var list_regional = (List<int>)Session["oRegional"];
            int cd_usuario = ((Usuario)Session["oUsuario"]).CD_USUARIO;
            var list_departamento = (from a in db.DepartamentoUsuario.Where(a => a.CD_USUARIO == cd_usuario) select a.CD_DEPARTAMENTO).ToList();
            int cd_grupo = ((Usuario)Session["oUsuario"]).CD_GUSUARIO;

            DateTime _dt_inicial;
            DateTime _dt_final;
            int _nr_procedimento_ini;
            int _nr_procedimento_fim;
            int _cd_cadastro_ini;
            int _cd_cadastro_fim;
            int _cd_tipo_ini;
            int _cd_tipo_fim;
            int _cd_dep_ini;
            int _cd_dep_fim;
            int _cd_usuario_ini;
            int _cd_usuario_fim;
            int _situacao_ini;
            int _situacao_fim;




            if (dt_inicial.HasValue())
            {
                _dt_inicial = Convert.ToDateTime(Convert.ToDateTime(dt_inicial.ToString()).ToString("yyyy-MM-dd 00:00:00"));
            }
            else
            {
                _dt_inicial = DateTime.Now.AddYears(-1);
            }


            if (dt_final.HasValue())
            {
                _dt_final = Convert.ToDateTime(Convert.ToDateTime(dt_final.ToString()).ToString("yyyy-MM-dd 23:59:59"));
            }
            else
            {
                _dt_final = DateTime.Now.AddYears(1);
            }

            if (CD_DEPARTAMENTO > 0)
            {
                _cd_dep_ini = Convert.ToInt32(CD_DEPARTAMENTO.ToString());
                _cd_dep_fim = Convert.ToInt32(CD_DEPARTAMENTO.ToString());

            }
            else
            {
                _cd_dep_ini = 0;
                _cd_dep_fim = 99999999;

            }

            if (CD_TIPO > 0)
            {
                _cd_tipo_ini = Convert.ToInt32(CD_TIPO.ToString());
                _cd_tipo_fim = Convert.ToInt32(CD_TIPO.ToString());
            }
            else
            {
                _cd_tipo_ini = 0;
                _cd_tipo_fim = 99999999;

            }


            if (nr_procedimento > 0)
            {
                _nr_procedimento_ini = Convert.ToInt32(nr_procedimento.ToString());
                _nr_procedimento_fim = Convert.ToInt32(nr_procedimento.ToString());

            }
            else
            {
                _nr_procedimento_ini = 0;
                _nr_procedimento_fim = 99999999;

            }


            //var ProcedimentoAdm =
            //      (from a in db.wProcedimento.Where
            //          (c =>

            //              (c.DTA_ABERTURA >= _dt_inicial) &&
            //              (c.DTA_ABERTURA <= _dt_final) &&

            //              (c.CD_TIPO >= _cd_tipo_ini) &&
            //              (c.CD_TIPO <= _cd_tipo_fim) &&

            //              (c.CD_DEPARTAMENTO >= _cd_dep_ini) &&
            //              (c.CD_DEPARTAMENTO <= _cd_dep_fim) &&

            //              ( !string.IsNullOrEmpty(UF) ?  UF.Contains(c.Clientes.CD_ESTADO.ToUpper()) : 1 == 1  )

            //              ).ToList()
            //       select a).OrderByDescending(a => a.CD_PROCEDIMENTO).ToList();

            decimal Vlrp = new decimal(0.02);
            var retorno =
                (from a in db.wpa_troca_departamentos.Where
                 (c =>
                     (c.DTA_TROCA >= _dt_inicial) &&
                     (c.DTA_TROCA <= _dt_final) &&

                     (c.ProcedimentoAdm.CD_TIPO >= _cd_tipo_ini) &&
                     (c.ProcedimentoAdm.CD_TIPO <= _cd_tipo_fim) &&

                     (c.CD_DEPARTAMENTO_NOVA >= _cd_dep_ini) &&
                     (c.CD_DEPARTAMENTO_NOVA <= _cd_dep_fim) &&

                     (c.CD_PROCEDIMENTO >= _nr_procedimento_ini) &&
                     (c.CD_PROCEDIMENTO <= _nr_procedimento_fim) &&

                     (!string.IsNullOrEmpty(UF) ? UF.Contains(c.ProcedimentoAdm.Clientes.CD_ESTADO.ToUpper()) : 1 == 1)

                     ).ToList()
                 select a


                     ).OrderByDescending(a => a.CD_PROCEDIMENTO).ToList();


            if (retorno.Count() > 0)
            {
                decimal? averageTicks = retorno.Where(erika => erika.HORASNUMBER > Vlrp).Select(d => d.HORASNUMBER).Average();

                decimal? qtde_fora = retorno.Where(a => a.PERCENTUAL > 100).Count();
                //decimal? qtde_dentro = retorno.Where(a => a.PERCENTUAL > a.DEPANT.TEMPO_PADRAO).Count();

                //string tempo_medio = string.Concat(Math.Truncate(Convert.ToDecimal(averageTicks)).ToString(),":", Convert.ToString( Math.Round(   ((Convert.ToDecimal(averageTicks) -  Math.Truncate(Convert.ToDecimal(averageTicks)) ) * 60 ) , 4)   ));
                string tempo_medio = string.Concat(Math.Truncate(Convert.ToDecimal(averageTicks)).ToString().PadLeft(2, '0'), ":", Convert.ToString(Math.Truncate(Math.Round(((Convert.ToDecimal(averageTicks) - Math.Truncate(Convert.ToDecimal(averageTicks))) * 60), 2))).PadLeft(2, '0'));

                ViewData["tempo_medio_resposta"] = tempo_medio;
                ViewData["qtde_fora"] = qtde_fora;
            }
            else
            {
                ViewData["tempo_medio_resposta"] = 0;
                ViewData["qtde_fora"] = 0;
            }


            /*    var serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                var result = new ContentResult();
                result.Content = serializer.Serialize(retorno);
                result.ContentType = "text/json"; */
            // var jsonResult = Json(retorno, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;
            //return View(jsonResult);


            /* var serializer = new JavaScriptSerializer();
             var result = new ContentResult();
             serializer.MaxJsonLength = Int32.MaxValue; // Whatever max length you want here
             result.Content = serializer.Serialize(retorno.ToDataSourceResult(request));
             result.ContentType = "application/json";
             return result;
             * */


            return View(retorno);


        }




        [CustomAuthorize(AccessLevel = "Relatorios")]
        [AuthFilter]
        public ActionResult ListagemGeral()
        {
            return View();
        }

        [CustomAuthorize(AccessLevel = "Relatorios")]
        [HttpPost]
        public ActionResult ListagemGeralResult(int nr_procedimento = 0
            , string dt_inicial = ""
            , string dt_final = ""
            , int CD_CADASTRO = 0
            , int CD_TIPO = 0
            , int CD_DEPARTAMENTO = 0
            , int CD_USUARIO = 0
            , int ID_SITUACAO = 0
            , string Destino = "")
        {
            var list_regional = (List<int>)Session["oRegional"];
            int cd_usuario = ((Usuario)Session["oUsuario"]).CD_USUARIO;
            var list_departamento = (from a in db.DepartamentoUsuario.Where(a => a.CD_USUARIO == cd_usuario) select a.CD_DEPARTAMENTO).ToList();
            int cd_grupo = ((Usuario)Session["oUsuario"]).CD_GUSUARIO;

            DateTime _dt_inicial;
            DateTime _dt_final;
            int _nr_procedimento_ini;
            int _nr_procedimento_fim;
            int _cd_cadastro_ini;
            int _cd_cadastro_fim;
            int _cd_tipo_ini;
            int _cd_tipo_fim;
            int _cd_dep_ini;
            int _cd_dep_fim;
            int _cd_usuario_ini;
            int _cd_usuario_fim;
            int _situacao_ini;
            int _situacao_fim;


            if (ID_SITUACAO > 0)
            {
                _situacao_ini = Convert.ToInt32(ID_SITUACAO.ToString());
                _situacao_fim = Convert.ToInt32(ID_SITUACAO.ToString());

            }
            else
            {
                _situacao_ini = 0;
                _situacao_fim = 99999999;

            }


            if (CD_USUARIO > 0)
            {
                _cd_usuario_ini = Convert.ToInt32(CD_USUARIO.ToString());
                _cd_usuario_fim = Convert.ToInt32(CD_USUARIO.ToString());

            }
            else
            {
                _cd_usuario_ini = 0;
                _cd_usuario_fim = 99999999;

            }



            if (dt_inicial.HasValue())
            {
                _dt_inicial = Convert.ToDateTime(Convert.ToDateTime(dt_inicial.ToString()).ToString("yyyy-MM-dd 00:00:00"));
            }
            else
            {
                _dt_inicial = DateTime.Now.AddYears(-1);
            }


            if (dt_final.HasValue())
            {
                _dt_final = Convert.ToDateTime(Convert.ToDateTime(dt_final.ToString()).ToString("yyyy-MM-dd 23:59:59"));
            }
            else
            {
                _dt_final = DateTime.Now.AddYears(1);
            }

            if (CD_DEPARTAMENTO > 0)
            {
                _cd_dep_ini = Convert.ToInt32(CD_DEPARTAMENTO.ToString());
                _cd_dep_fim = Convert.ToInt32(CD_DEPARTAMENTO.ToString());

            }
            else
            {
                _cd_dep_ini = 0;
                _cd_dep_fim = 99999999;

            }

            if (nr_procedimento > 0)
            {
                _nr_procedimento_ini = Convert.ToInt32(nr_procedimento.ToString());
                _nr_procedimento_fim = Convert.ToInt32(nr_procedimento.ToString());

            }
            else
            {
                _nr_procedimento_ini = 0;
                _nr_procedimento_fim = 99999999;

            }

            if (CD_CADASTRO > 0)
            {
                _cd_cadastro_ini = Convert.ToInt32(CD_CADASTRO.ToString());
                _cd_cadastro_fim = Convert.ToInt32(CD_CADASTRO.ToString());
            }
            else
            {
                _cd_cadastro_ini = 0;
                _cd_cadastro_fim = 9999999;

            }


            if (CD_TIPO > 0)
            {
                _cd_tipo_ini = Convert.ToInt32(CD_TIPO.ToString());
                _cd_tipo_fim = Convert.ToInt32(CD_TIPO.ToString());
            }
            else
            {
                _cd_tipo_ini = 0;
                _cd_tipo_fim = 99999999;

            }

            var ProcedimentoAdm =
                  (from a in db.wProcedimento.Where
                      (c =>
                          (c.CD_PROCEDIMENTO >= _nr_procedimento_ini) &&
                          (c.CD_PROCEDIMENTO <= _nr_procedimento_fim) &&

                          (c.DTA_ABERTURA >= _dt_inicial) &&
                          (c.DTA_ABERTURA <= _dt_final) &&

                          (c.CD_CADASTRO >= _cd_cadastro_ini) &&
                          (c.CD_CADASTRO <= _cd_cadastro_fim) &&

                          (c.CD_TIPO >= _cd_tipo_ini) &&
                          (c.CD_TIPO <= _cd_tipo_fim) &&

                          (c.CD_DEPARTAMENTO >= _cd_dep_ini) &&
                          (c.CD_DEPARTAMENTO <= _cd_dep_fim) &&

                          (c.ID_SITUACAO >= _situacao_ini) &&
                          (c.ID_SITUACAO <= _situacao_fim) &&

                          (c.CD_USUARIO >= _cd_usuario_ini) &&
                          (c.CD_USUARIO <= _cd_usuario_fim)).ToList()
                   select a).OrderByDescending(a => a.CD_PROCEDIMENTO).ToList();




            return View(ProcedimentoAdm);



        }


        [CustomAuthorize(AccessLevel = "procedimentoadmExportXls")]
        public void ExportXls(wProcedimento wpro)
        {


            var procedimentoadm = db.wProcedimento.ToList();
            //Codigo Gerou na Index desta Controller (Final da Página)

            var grid = new System.Web.UI.WebControls.GridView();
            grid.DataSource = from _data in procedimentoadm select _data;
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=procedimentoadm.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }



    }
}
