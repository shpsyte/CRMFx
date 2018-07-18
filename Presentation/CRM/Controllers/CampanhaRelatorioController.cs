using CRM.Extends;
using CRM.Filters;
using Domain.Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    [iFilterRelatorioMkt]
    public class CampanhaRelatorioController : BasePublicController
    {



        public ActionResult CampanhasPorDataVencimento()
        {
            return View();
        }

        public ActionResult CampanhasPorDataVencimentoFilter(DateTime? dta_inicial, 
            DateTime? dta_final, 
            int? statusId, 
            int? regionalid, 
            int? segmentoId, 
            int? pessoaId)
        {

            var sstatusId = statusId ?? 0;
            var sregionalid = regionalid ?? 0;
            var ssegmentoId = segmentoId ?? 0;
            var scod_pessoa = pessoaId ?? 0;


            string SqlBase = " select a.campanhaid, a.cod_pessoa, a.des_nome, b.des_pessoa, c.des_segmento, d.des_acao, a.vlr_meta, a.vlr_contrato, " +
                                " (select sum(pg.vlr_pgto) from campanhamarketingpgto pg where pg.campanhaid = a.campanhaid) vlr_pgto, e.descricao, " +
                                " a.cod_regional, rep.des_pessoa  des_representante, a.vlr_custo_medio, a.segmentoid " +
                                " from Campanhamarketing a " +
                                " Inner Join Ps_Pessoas b on a.cod_pessoa = b.cod_pessoa " +
                                " Inner Join Segmentos c on c.segmentoid = a.segmentoid " +
                                " Inner join Tipoacao d on d.tipoacaoid = a.tipoacaoid " +
                                " Inner join status e on e.statusid = a.statusid " +
                                " Inner Join Ps_Pessoas rep on rep.cod_pessoa = a.cod_representante " +
                                " Where 1=1 ";

            if (scod_pessoa > 0)
            {
                SqlBase += string.Format(" And a.cod_pessoa = {0}", scod_pessoa.ToString());
            }


            if (dta_inicial.HasValue)
            {
                SqlBase += string.Format(" And a.dta_final >= \'{0}\'", dta_inicial.Value.ToShortDateString());
            }

            if (dta_final.HasValue)
            {
                SqlBase += string.Format(" And a.dta_final <= \'{0}\'", dta_final.Value.ToShortDateString());
            }

            if (sstatusId > 0)
            {
                SqlBase += string.Format(" and a.statusid = decode({0}, 0, a.statusid, {1}) ", sstatusId, sstatusId);
            }

            if (sregionalid > 0)
            {
                SqlBase += string.Format(" and a.cod_regional = {0} ", sregionalid);
            }

            if (ssegmentoId > 0)
            {
                SqlBase += string.Format(" and a.segmentoId = decode({0}, 0, a.segmentoId, {1}) ", ssegmentoId, ssegmentoId);
            }

            
            //db.Database.ExecuteSqlCommand(string.Format(" Begin SpcGetAnaliseRegional({0},{1}); end; ", ano, sstatusId));
            // int sessao = db.Database.SqlQuery<Int32>("select USERENV('SESSIONID') from dual ").FirstOrDefault<Int32>();
            var dados = db.Database.SqlQuery<RelatoCampanha>(SqlBase).ToList();
            return View(dados);


        }




        public ActionResult Campanhas()
        {
            return View();
        }

        public ActionResult CampanhasFilter(int? campanhaId, DateTime? dta_inicial, DateTime? dta_final, int? statusId, int? regionalid, int? pessoaId)
        {

            var sstatusId = statusId ?? 0;
            var sregionalid = regionalid ?? 0;
            var scod_pessoa = pessoaId ?? 0;
            var scampanhaid = campanhaId ?? 0;


            string SqlBase = " select a.campanhaid, a.cod_pessoa, a.des_nome, b.des_pessoa, c.des_segmento, d.des_acao, a.vlr_meta, a.vlr_contrato, " +
                                " (select sum(pg.vlr_pgto) from campanhamarketingpgto pg where pg.campanhaid = a.campanhaid) vlr_pgto, e.descricao, " +
                                " a.cod_regional, rep.des_pessoa  des_representante, a.vlr_custo_medio, a.segmentoid " +
                                " from Campanhamarketing a " +
                                " Inner Join Ps_Pessoas b on a.cod_pessoa = b.cod_pessoa " +
                                " Inner Join Segmentos c on c.segmentoid = a.segmentoid " +
                                " Inner join Tipoacao d on d.tipoacaoid = a.tipoacaoid " +
                                " Inner join status e on e.statusid = a.statusid " +
                                " Inner Join Ps_Pessoas rep on rep.cod_pessoa = a.cod_representante " +
                                " Where 1=1 ";
                                //" and a.segmentoid in (1,3) ";

            if (scod_pessoa > 0)
            {
                SqlBase += string.Format(" And a.cod_pessoa = {0}", scod_pessoa.ToString());
            }

            if (scampanhaid > 0)
            {

                SqlBase += string.Format(" And a.campanhaid = {0} ", scampanhaid.ToString());
            }

            if (dta_inicial.HasValue)
            {
                SqlBase += string.Format(" And a.dta_inclusao >= \'{0}\'", dta_inicial.Value.ToShortDateString());
            }

            if (dta_final.HasValue)
            {
                SqlBase += string.Format(" And a.dta_inclusao <= \'{0}\'", dta_final.Value.ToShortDateString());
            }

            if (sstatusId > 0)
            {
                SqlBase += string.Format(" and a.statusid = decode({0}, 0, a.statusid, {1}) ", sstatusId, sstatusId);
            }

            if (sregionalid > 0)
            {
                SqlBase += string.Format(" and a.cod_regional = {0} ", sregionalid);
            }

            //db.Database.ExecuteSqlCommand(string.Format(" Begin SpcGetAnaliseRegional({0},{1}); end; ", ano, sstatusId));
            // int sessao = db.Database.SqlQuery<Int32>("select USERENV('SESSIONID') from dual ").FirstOrDefault<Int32>();
            var dados = db.Database.SqlQuery<RelatoCampanha>(SqlBase).ToList();
            return View(dados);


        }


        public ActionResult CampanhasVsPgto()
        {
            return View();
        }

        public ActionResult CampanhasVsPgtoFilter(DateTime? dta_inicial, DateTime? dta_final, int? statusId, int? regionalid)
        {

            var sstatusId = statusId ?? 0;
            var sregionalid = regionalid ?? 0;
            string SqlBase = " select a.campanhaid, a.cod_pessoa, a.des_nome, b.des_pessoa, c.des_segmento, d.des_acao, a.vlr_meta, a.vlr_contrato, " +
                                " (select sum(pg.vlr_pgto) from campanhamarketingpgto pg where pg.campanhaid = a.campanhaid) vlr_pgto, e.descricao, " +
                                " a.cod_regional, rep.des_pessoa des_representante, a.segmentoid " +
                                " from Campanhamarketing a " +
                                " Inner Join Ps_Pessoas b on a.cod_pessoa = b.cod_pessoa " +
                                " Inner Join Segmentos c on c.segmentoid = a.segmentoid " +
                                " Inner join Tipoacao d on d.tipoacaoid = a.tipoacaoid " +
                                " Inner join status e on e.statusid = a.statusid " +
                                " Inner Join Ps_Pessoas rep on rep.cod_pessoa = a.cod_representante " +
                                " Where 1=1 ";

            if (dta_inicial.HasValue)
            {
                SqlBase += string.Format(" And a.dta_inclusao >= \'{0}\'", dta_inicial.Value.ToShortDateString());
            }

            if (dta_final.HasValue)
            {
                SqlBase += string.Format(" And a.dta_inclusao <= \'{0}\'", dta_final.Value.ToShortDateString());
            }

            if (sstatusId > 0)
            {
                SqlBase += string.Format(" and a.statusid = decode({0}, 0, a.statusid, {1}) ", sstatusId, sstatusId);
            }

            if (sregionalid > 0)
            {
                SqlBase += string.Format(" and a.cod_regional = {0} ", sregionalid);
            }


            //db.Database.ExecuteSqlCommand(string.Format(" Begin SpcGetAnaliseRegional({0},{1}); end; ", ano, sstatusId));
            // int sessao = db.Database.SqlQuery<Int32>("select USERENV('SESSIONID') from dual ").FirstOrDefault<Int32>();
            var dados = db.Database.SqlQuery<RelatoCampanha>(SqlBase).ToList();
            return View(dados);


        }




        public ActionResult CampanhasMetaXRealizado()
        {
            return View();
        }

        public ActionResult CampanhasMetaXRealizadoFilter(DateTime? dta_inicial, DateTime? dta_final, int? statusId, int? regionalid)
        {

            var sstatusId = statusId ?? 0;
            var sregionalid = regionalid ?? 0;

            string SqlBase = " select a.campanhaid, a.cod_pessoa, a.des_nome, b.des_pessoa, c.des_segmento, d.des_acao, NVL(a.vlr_meta,0) vlr_meta, nvl(a.vlr_contrato,0) vlr_contrato, " +
                                         " nvl((select sum(pg.vlr_pgto) from campanhamarketingpgto pg where pg.campanhaid = a.campanhaid),0) vlr_pgto, e.descricao, " +
                                         " nvl(Retorna_Valor_Fat_Campanha(A.DTA_INICIAL, A.DTA_FINAL, A.COD_PESSOA),0) vlr_faturamento, " +
                                         " (round( (nvl(Retorna_Valor_Fat_Campanha(A.DTA_INICIAL, A.DTA_FINAL, A.COD_PESSOA),0)  / case nvl(a.vlr_meta,0) when 0 then 1 else nvl(a.vlr_meta,0) end ) , 4) * 100) per_atingido, " +
                                         " nvl(Retorna_Valor_Fat_Campanha(A.DTA_INICIAL, A.DTA_FINAL, A.COD_PESSOA),0) - nvl(a.vlr_meta,0) vlr_saldo ," +
                                         " a.cod_regional, rep.des_pessoa des_representante, a.segmentoid  " +
                                         " from Campanhamarketing a " +
                                         " Inner Join Ps_Pessoas b on a.cod_pessoa = b.cod_pessoa " +
                                         " Inner Join Segmentos c on c.segmentoid = a.segmentoid " +
                                         " Inner join Tipoacao d on d.tipoacaoid = a.tipoacaoid " +
                                         " Inner join status e on e.statusid = a.statusid " +
                                         " Inner Join Ps_Pessoas rep on rep.cod_pessoa = a.cod_representante " +
                                         " Where 1=1 " +
                                         " and  a.segmentoid = 2 ";



            if (dta_inicial.HasValue)
            {
                SqlBase += string.Format(" And a.dta_inclusao >= \'{0}\'", dta_inicial.Value.ToShortDateString());
            }

            if (dta_final.HasValue)
            {
                SqlBase += string.Format(" And a.dta_inclusao <= \'{0}\'", dta_final.Value.ToShortDateString());
            }

            if (sstatusId > 0)
            {
                SqlBase += string.Format(" and a.statusid = decode({0}, 0, a.statusid, {1}) ", sstatusId, sstatusId);
            }

            if (sregionalid > 0)
            {
                SqlBase += string.Format(" and a.cod_regional = {0} ", sregionalid);
            }


            //db.Database.ExecuteSqlCommand(string.Format(" Begin SpcGetAnaliseRegional({0},{1}); end; ", ano, sstatusId));
            // int sessao = db.Database.SqlQuery<Int32>("select USERENV('SESSIONID') from dual ").FirstOrDefault<Int32>();
            var dados = db.Database.SqlQuery<RelatoCampanha>(SqlBase).ToList();
            return View(dados);


        }



        public ActionResult CampanhasOrcadoXDiretoria()
        {
            return View();
        }

        public ActionResult CampanhasOrcadoXDiretoriaFilter(DateTime? dta_inicial, DateTime? dta_final, int? statusId, int? regionalid)
        {

            var sstatusId = statusId ?? 0;
            var sregionalid = regionalid ?? 0;

            string SqlBase = " SELECT a.cod_regional, a.vlr_pago vlr_pgto, a.vlr_verba vlr_meta,  a.vlr_verba - a.vlr_pago vlr_faturamento,  a.per_uso per_atingido FROM TmpRelOrcDiretoria a  ";



            if (!dta_inicial.HasValue)
                dta_inicial = Convert.ToDateTime("01/01/2010");

            if (!dta_final.HasValue)
                dta_final = Convert.ToDateTime("01/01/2070");

            string sql = string.Format(" Begin spc_RelOrcDiretoria(\'{0}\',\'{1}\',{2},{3}); end; ", dta_inicial.Value.ToShortDateString(), dta_final.Value.ToShortDateString(), sstatusId, sregionalid);
            db.Database.ExecuteSqlCommand(sql);
            // int sessao = db.Database.SqlQuery<Int32>("select USERENV('SESSIONID') from dual ").FirstOrDefault<Int32>();


            var dados = db.Database.SqlQuery<RelatoCampanha>(SqlBase).ToList();
            return View(dados);


        }



        

        public FileStreamResult PrintFormaPgtos(int campanhaid, string obs, string nome_forma, string vlr_pgto, string ind_total)
        {

            CampanhaMarketing campanha = db.CampanhaMarketing.Find(campanhaid);

            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(iTextSharp.text.PageSize.A5);
            Document document = new Document(rec);
            PdfWriter writer = PdfWriter.GetInstance(document, workStream);
            writer.CloseStream = false;


            if (campanha == null)
            {

                writer.PageEvent = new PDFFooter(titulo: "CAMPANHA DE MARKETING", canhoto: true);
                document.Open();
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph("  NÃO EXISTEM DADSO PARA LISTAR "));

                document.Close();

                byte[] byteInfoS = workStream.ToArray();
                workStream.Write(byteInfoS, 0, byteInfoS.Length);
                workStream.Position = 0;


                return new FileStreamResult(workStream, "application/pdf");
            }


            //PdfWriter.GetInstance(document, workStream).CloseStream = false;
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times_12 = new Font(bfTimes, 12, Font.NORMAL, Color.RED);
            Font arial_12 = FontFactory.GetFont("Arial", 12, Color.BLACK);
            Font verdana_16 = FontFactory.GetFont("Verdana", 16, Font.BOLD, Color.ORANGE);
            Font verdana_12 = FontFactory.GetFont("Verdana", 12, Color.BLACK);
            Font verdana_8 = FontFactory.GetFont("Verdana", 8, Font.BOLD, Color.BLACK);

            Font palatino = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              10,
              Font.NORMAL, Color.ORANGE
              );

            Font palatino_xs = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              12,
              Font.BOLD, Color.RED
              );


            Font smallfont = FontFactory.GetFont("Arial", 6, Color.GRAY);
            Font xarial = FontFactory.GetFont("Arial");
            xarial.SetStyle("Bold");

            Font palatino_i = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              8,
              Font.NORMAL, Color.GRAY
              );

            Font palatino_i_red = FontFactory.GetFont(
            "palatino linotype italique",
             BaseFont.CP1252,
             BaseFont.EMBEDDED,
             8,
             Font.NORMAL, Color.RED
             );

            Font palatino_bold = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              8,
              Font.BOLD, Color.BLACK
              );


            writer.PageEvent = new PDFFooter(titulo: "DADOS DE PAGAMENTO CAMPANHA", canhoto: true);

            document.Open();

            //gif.ScalePercent(45);
            //document.Add(gif);
            document.Add(new Paragraph(" "));

            //PdfContentByte cb = writer.DirectContent;
            //cb.Rectangle(10f, 200f, 800f, 600f);
            //cb.Stroke();

            float[] largura_colunas = new float[] { 1f, 2f };
            Chunk linebreak = new Chunk(new LineSeparator(1f, 100f, Color.LIGHT_GRAY, Element.ALIGN_CENTER, -1));
            PdfPCell celula = new PdfPCell();





            Paragraph _TituloParagrafo = new Paragraph();
            _TituloParagrafo.Alignment = Element.ALIGN_LEFT;






            PdfPTable header = new PdfPTable(2);
            header.TotalWidth = document.PageSize.Width - 10;
            header.LockedWidth = true;
            header.DefaultCell.BorderWidth = 1;
            header.HorizontalAlignment = Element.ALIGN_LEFT;
            float[] largura_col = new float[] { 20f, 100f };
            header.SetWidths(largura_col);





            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");


            Paragraph _TITULO = new Paragraph();
            _TITULO.Alignment = Element.ALIGN_CENTER;
            _TITULO.Add(new Chunk("Pinhais " + System.DateTime.Now.ToLongDateString(), verdana_8));
            document.Add(linebreak);
            document.Add(_TITULO);
            document.Add(linebreak);



            document.Add(new Paragraph(" "));

            _TITULO = new Paragraph();
            _TITULO.Alignment = Element.ALIGN_LEFT;
            _TITULO.Add(new Chunk("Dados da Campanha:", palatino_i_red));
            document.Add(_TITULO);
            document.Add(new Paragraph(" "));

            //linha do numero
            celula = new PdfPCell(new Paragraph(new Chunk("Número:", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(campanha.campanhaID.ToString(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk("Código Cliente:", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(campanha.cod_pessoa_string, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);


            celula = new PdfPCell(new Paragraph(new Chunk("Nome :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(campanha.DadosDoCrm.des_pessoa, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk("Descrição :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(campanha.des_nome, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            document.Add(header);

            document.Add(new Paragraph(" "));



            _TITULO = new Paragraph();
            _TITULO.Alignment = Element.ALIGN_LEFT;
            _TITULO.Add(new Chunk("Dados do Pagamento", palatino_i_red));
            document.Add(_TITULO);
            document.Add(new Paragraph(" "));



            header = new PdfPTable(2);
            header.TotalWidth = document.PageSize.Width - 10;
            header.LockedWidth = true;
            header.DefaultCell.BorderWidth = 1;
            header.HorizontalAlignment = Element.ALIGN_LEFT;
            largura_col = new float[] { 20f, 100f };
            header.SetWidths(largura_col);



            //linha do numero
            celula = new PdfPCell(new Paragraph(new Chunk("Forma:", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(nome_forma, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);


            celula = new PdfPCell(new Paragraph(new Chunk("Obs:", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(obs, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);


            celula = new PdfPCell(new Paragraph(new Chunk("Valor :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(vlr_pgto, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);


            celula = new PdfPCell(new Paragraph(new Chunk("Usuário :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(db.Usuario.Where(a => a.CD_USUARIO == cd_usuario).Select(a => a.NOME).FirstOrDefault(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);


            celula = new PdfPCell(new Paragraph(new Chunk("Data Inclusão :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(System.DateTime.Now.ToString(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);



            document.Add(header);


            //document.Add(new Paragraph(" "));

            //_TITULO = new Paragraph();
            //_TITULO.Alignment = Element.ALIGN_CENTER;
            //_TITULO.Add(new Chunk("Comunicamos que lançamos o seguinte débito em sua conta na Foxlux Ltda", verdana_8));
            //document.Add(_TITULO);



            //PdfPTable table = new PdfPTable(3);
            //table.TotalWidth = document.PageSize.Width - 10;
            //table.LockedWidth = true;
            //table.DefaultCell.BorderWidth = 0;
            //table.HorizontalAlignment = 1;
            //float[] largura_colunas_coleta = new float[] { 300f, 100f, 100f };
            //table.SetWidths(largura_colunas_coleta);
            //document.Add(new Paragraph(" "));
            //celula = new PdfPCell(new Paragraph(new Chunk("Dados do lançamento ", palatino))) { Colspan = 3, Border = 0, HorizontalAlignment = Element.ALIGN_CENTER };
            //table.AddCell(celula);
            //document.Add(table);

            //_TITULO = new Paragraph();
            //_TITULO.Alignment = Element.ALIGN_RIGHT;
            //_TITULO.Add(new Chunk("Total Geral da Nota " + campanha.des_nome, palatino_xs));
            //document.Add(_TITULO);


            //document.Add(new Paragraph(" "));

            //_TITULO = new Paragraph();
            //_TITULO.Alignment = Element.ALIGN_CENTER;
            //_TITULO.Add(new Chunk("Data de Vencimento .: ", verdana_8));
            //document.Add(_TITULO);


            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return new FileStreamResult(workStream, "application/pdf");


        }




        public FileStreamResult PrintFormaPgto(int campanhaid, int campanhamarketingpgtoid)
        {

            CampanhaMarketing campanha = db.CampanhaMarketing.Find(campanhaid);
            CampanhaMarketingPgto campanhapgto = db.CampanhaMarketingPgto.Find(campanhamarketingpgtoid, campanhaid);

            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(iTextSharp.text.PageSize.A5);
            Document document = new Document(rec);
            PdfWriter writer = PdfWriter.GetInstance(document, workStream);
            writer.CloseStream = false;


            if (campanha == null)
            {

                writer.PageEvent = new PDFFooter(titulo: "CAMPANHA DE MARKETING", canhoto: true);
                document.Open();
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph("  NÃO EXISTEM DADSO PARA LISTAR "));

                document.Close();

                byte[] byteInfoS = workStream.ToArray();
                workStream.Write(byteInfoS, 0, byteInfoS.Length);
                workStream.Position = 0;


                return new FileStreamResult(workStream, "application/pdf");
            }


            //PdfWriter.GetInstance(document, workStream).CloseStream = false;
            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times_12 = new Font(bfTimes, 12, Font.NORMAL, Color.RED);
            Font arial_12 = FontFactory.GetFont("Arial", 12, Color.BLACK);
            Font verdana_16 = FontFactory.GetFont("Verdana", 16, Font.BOLD, Color.ORANGE);
            Font verdana_12 = FontFactory.GetFont("Verdana", 12, Color.BLACK);
            Font verdana_8 = FontFactory.GetFont("Verdana", 8, Font.BOLD, Color.BLACK);

            Font palatino = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              10,
              Font.NORMAL, Color.ORANGE
              );

            Font palatino_xs = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              12,
              Font.BOLD, Color.RED
              );


            Font smallfont = FontFactory.GetFont("Arial", 6, Color.GRAY);
            Font xarial = FontFactory.GetFont("Arial");
            xarial.SetStyle("Bold");

            Font palatino_i = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              8,
              Font.NORMAL, Color.GRAY
              );

            Font palatino_i_red = FontFactory.GetFont(
            "palatino linotype italique",
             BaseFont.CP1252,
             BaseFont.EMBEDDED,
             8,
             Font.NORMAL, Color.RED
             );

            Font palatino_bold = FontFactory.GetFont(
             "palatino linotype italique",
              BaseFont.CP1252,
              BaseFont.EMBEDDED,
              8,
              Font.BOLD, Color.BLACK
              );


            writer.PageEvent = new PDFFooter(titulo: "DADOS DE PAGAMENTO CAMPANHA", canhoto: true);

            document.Open();

            //gif.ScalePercent(45);
            //document.Add(gif);
            document.Add(new Paragraph(" "));

            //PdfContentByte cb = writer.DirectContent;
            //cb.Rectangle(10f, 200f, 800f, 600f);
            //cb.Stroke();

            float[] largura_colunas = new float[] { 1f, 2f };
            Chunk linebreak = new Chunk(new LineSeparator(1f, 100f, Color.LIGHT_GRAY, Element.ALIGN_CENTER, -1));
            PdfPCell celula = new PdfPCell();





            Paragraph _TituloParagrafo = new Paragraph();
            _TituloParagrafo.Alignment = Element.ALIGN_LEFT;






            PdfPTable header = new PdfPTable(2);
            header.TotalWidth = document.PageSize.Width - 10;
            header.LockedWidth = true;
            header.DefaultCell.BorderWidth = 1;
            header.HorizontalAlignment = Element.ALIGN_LEFT;
            float[] largura_col = new float[] { 20f, 100f };
            header.SetWidths(largura_col);





            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");


            Paragraph _TITULO = new Paragraph();
            _TITULO.Alignment = Element.ALIGN_CENTER;
            _TITULO.Add(new Chunk("Pinhais " + System.DateTime.Now.ToLongDateString(), verdana_8));
            document.Add(linebreak);
            document.Add(_TITULO);
            document.Add(linebreak);



            document.Add(new Paragraph(" "));

            _TITULO = new Paragraph();
            _TITULO.Alignment = Element.ALIGN_LEFT;
            _TITULO.Add(new Chunk("Dados da Campanha:", palatino_i_red));
            document.Add(_TITULO);
            document.Add(new Paragraph(" "));

            //linha do numero
            celula = new PdfPCell(new Paragraph(new Chunk("Número:", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(campanha.campanhaID.ToString(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk("Código Cliente:", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(campanha.cod_pessoa_string, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);


            celula = new PdfPCell(new Paragraph(new Chunk("Nome :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(campanha.DadosDoCrm.des_pessoa, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk("Descrição :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(campanha.des_nome, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            document.Add(header);

            document.Add(new Paragraph(" "));



            _TITULO = new Paragraph();
            _TITULO.Alignment = Element.ALIGN_LEFT;
            _TITULO.Add(new Chunk("Dados do Pagamento", palatino_i_red));
            document.Add(_TITULO);
            document.Add(new Paragraph(" "));



            header = new PdfPTable(2);
            header.TotalWidth = document.PageSize.Width - 10;
            header.LockedWidth = true;
            header.DefaultCell.BorderWidth = 1;
            header.HorizontalAlignment = Element.ALIGN_LEFT;
            largura_col = new float[] { 20f, 100f };
            header.SetWidths(largura_col);



            //linha do numero
            celula = new PdfPCell(new Paragraph(new Chunk("Forma:", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(campanhapgto.FormaPgto.des_forma, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);



            if (campanhapgto.formapgtoid == 1)
            {

                celula = new PdfPCell(new Paragraph(new Chunk("Título :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk(campanhapgto.num_titulo.ToString(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk("Parcela :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk(campanhapgto.num_parcela.ToString(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);
            }


            if (campanhapgto.formapgtoid == 2)
            {

                celula = new PdfPCell(new Paragraph(new Chunk("Pedido :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk(campanhapgto.num_pedido.ToString(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk("Complemento :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk(campanhapgto.cod_compl.ToString(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);
            }



            if (campanhapgto.formapgtoid == 4)
            {

                celula = new PdfPCell(new Paragraph(new Chunk("Agência :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk(campanhapgto.des_agencia, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk("Banco :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk(campanhapgto.des_banco, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk("Conta :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk(campanhapgto.des_conta, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);
            }


            celula = new PdfPCell(new Paragraph(new Chunk("Nº Documento :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk(campanhapgto.des_documento, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);


                celula = new PdfPCell(new Paragraph(new Chunk("Valor :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);

                celula = new PdfPCell(new Paragraph(new Chunk(campanhapgto.vlr_pgto.ToString("c"),palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
                header.AddCell(celula);


            celula = new PdfPCell(new Paragraph(new Chunk("Usuário :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(campanhapgto.Usuario.NOME, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);


            celula = new PdfPCell(new Paragraph(new Chunk("Data Inclusão :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(campanhapgto.dta_inclusao.ToString(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);



            document.Add(header);


            //document.Add(new Paragraph(" "));

            //_TITULO = new Paragraph();
            //_TITULO.Alignment = Element.ALIGN_CENTER;
            //_TITULO.Add(new Chunk("Comunicamos que lançamos o seguinte débito em sua conta na Foxlux Ltda", verdana_8));
            //document.Add(_TITULO);



            //PdfPTable table = new PdfPTable(3);
            //table.TotalWidth = document.PageSize.Width - 10;
            //table.LockedWidth = true;
            //table.DefaultCell.BorderWidth = 0;
            //table.HorizontalAlignment = 1;
            //float[] largura_colunas_coleta = new float[] { 300f, 100f, 100f };
            //table.SetWidths(largura_colunas_coleta);
            //document.Add(new Paragraph(" "));
            //celula = new PdfPCell(new Paragraph(new Chunk("Dados do lançamento ", palatino))) { Colspan = 3, Border = 0, HorizontalAlignment = Element.ALIGN_CENTER };
            //table.AddCell(celula);
            //document.Add(table);

            //_TITULO = new Paragraph();
            //_TITULO.Alignment = Element.ALIGN_RIGHT;
            //_TITULO.Add(new Chunk("Total Geral da Nota " + campanha.des_nome, palatino_xs));
            //document.Add(_TITULO);


            //document.Add(new Paragraph(" "));

            //_TITULO = new Paragraph();
            //_TITULO.Alignment = Element.ALIGN_CENTER;
            //_TITULO.Add(new Chunk("Data de Vencimento .: ", verdana_8));
            //document.Add(_TITULO);


            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return new FileStreamResult(workStream, "application/pdf");


        }



        public ActionResult CampanhasGerais()
        {
            return View();
        }

        public ActionResult CampanhasGeraisFilter(int? campanhaId,  DateTime? dta_inicial, DateTime? dta_final, int? statusId, int? regionalid, int? segmentoId)
        {

            var sstatusId = statusId ?? 0;
            var sregionalid = regionalid ?? 0;
            var ssegmentoId = segmentoId ?? 0;
            var scampanhaid = campanhaId ?? 0;

            string SqlBase = " select tpg.des_forma des_forma_pgto, a.dta_inclusao , a.campanhaid, a.cod_pessoa, a.des_nome, b.des_pessoa, c.des_segmento, a.cod_regional, d.des_acao, NVL(a.vlr_meta,0) vlr_meta, nvl(a.vlr_contrato,0) vlr_contrato, " +
                                                  " nvl((select sum(pg.vlr_pgto) from campanhamarketingpgto pg  where pg.campanhaid = a.campanhaid),0) vlr_pgto, " +
                                                  " nvl(Retorna_Valor_Fat_Campanha(A.DTA_INICIAL, A.DTA_FINAL, A.COD_PESSOA),0) vlr_faturamento, " +
                                                  " (select pg.dta_inclusao from campanhamarketingpgto pg  where pg.campanhaid = a.campanhaid and rownum <= 1) dta_ultimo_pgto, e.descricao, " +
                                                  " nvl(Retorna_Valor_Fat_Campanha(A.DTA_INICIAL, A.DTA_FINAL, A.COD_PESSOA),0) vlr_faturamento, " +
                                                  " (round( (nvl(Retorna_Valor_Fat_Campanha(A.DTA_INICIAL, A.DTA_FINAL, A.COD_PESSOA),0) / case nvl(a.vlr_meta,0) when 0 then 1 else nvl(a.vlr_meta,0) end ), 4) * 100) per_atingido, " +
                                                  " nvl(Retorna_Valor_Fat_Campanha(A.DTA_INICIAL, A.DTA_FINAL, A.COD_PESSOA),0) - nvl(a.vlr_meta,0) vlr_saldo , " +
                                                  " rep.des_pessoa des_representante, a.segmentoid " +
                                                  " from Campanhamarketing a " +
                                                  " Inner Join Ps_Pessoas b on a.cod_pessoa = b.cod_pessoa " +
                                                  " Inner Join Segmentos c on c.segmentoid = a.segmentoid " +
                                                  " Inner join Tipoacao d on d.tipoacaoid = a.tipoacaoid " +
                                                  " Inner join status e on e.statusid = a.statusid " +
                                                  " Inner Join Ps_Pessoas rep on rep.cod_pessoa = a.cod_representante " +
                                                  "  Inner Join Formapgto tpg on tpg.formapgtoid = a.formapgtoid " +
                                                  " Where 1=1 ";



            if (scampanhaid > 0)
            {

                SqlBase += string.Format(" And a.campanhaid = {0} ", scampanhaid.ToString());
            }
            if (dta_inicial.HasValue)
            {
                SqlBase += string.Format(" And a.dta_inclusao >= \'{0}\'", dta_inicial.Value.ToShortDateString());
            }

            if (dta_final.HasValue)
            {
                SqlBase += string.Format(" And a.dta_inclusao <= \'{0}\'", dta_final.Value.ToShortDateString());
            }

            if (sstatusId > 0)
            {
                SqlBase += string.Format(" and a.statusid = decode({0}, 0, a.statusid, {1}) ", sstatusId, sstatusId);
            }

            if (sregionalid > 0)
            {
                SqlBase += string.Format(" and a.cod_regional = {0} ", sregionalid);
            }


            if (ssegmentoId > 0)
            {
                SqlBase += string.Format(" and a.segmentoid = decode({0}, 0, a.segmentoid, {1}) ", ssegmentoId, ssegmentoId);
            }


            //db.Database.ExecuteSqlCommand(string.Format(" Begin SpcGetAnaliseRegional({0},{1}); end; ", ano, sstatusId));
            // int sessao = db.Database.SqlQuery<Int32>("select USERENV('SESSIONID') from dual ").FirstOrDefault<Int32>();
            var dados = db.Database.SqlQuery<RelatoCampanhaGeral>(SqlBase).ToList();
            return View(dados);

        }


    }
}
