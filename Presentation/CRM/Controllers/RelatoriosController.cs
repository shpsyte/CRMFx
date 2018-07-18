using CRM.Extends;
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
    [Services.Functions.NoCache]
    public class RelatoriosController : BasePublicController
    {
        //
        // GET: /Relatorios/

        public ActionResult Garantias()
        {
            ViewBag.cod_representante = new SelectList(db.Ps_Representantes, "cod_pessoa", "des_pessoa", "");
            return View();
        }

        public ActionResult GarantiasResult(int? cod_garantia, string nome, int? cod_representante, DateTime? dta_inicial, DateTime? dta_final)
        {
            IEnumerable<Garantia> filterGat;
            filterGat = db.Garantia.AsNoTracking();

            if (cod_garantia.HasValue)
            {
                filterGat = filterGat.Where(a => a.garantiaid == cod_garantia);
            }

            if (cod_representante.HasValue)
            {
                filterGat = filterGat.Where(a => a.cod_representante == cod_representante);
            }

            if (dta_inicial.HasValue)
            {
                filterGat = filterGat.Where(p => p.dta_inclusao >= dta_inicial.Value);
            }

            if (dta_final.HasValue)
            {
                filterGat = filterGat.Where(p => p.dta_inclusao <= dta_final.Value);
            }

            if (!string.IsNullOrEmpty(nome))
            {

                filterGat = filterGat.Where(p => p.Ps_Pessoas.des_pessoa.ToUpper().Contains(nome.ToUpper()) || p.cod_cliente.ToString().Contains(nome));
            }

            var data = filterGat.ToList();

            return View(data);
        }


       

        public ActionResult GarantiaEmTransito(int? cod_garantia, string notas, int? cod_cliente, DateTime? dta_inicial, DateTime? dta_final, bool continueAdd, string savecreate)
        {

           

            IEnumerable<Garantia> filterGat;
            filterGat = db.Garantia.AsNoTracking();
            filterGat = filterGat.Where(a => 
                    a.ind_emitido_coleta == "S" 
                 && a.ind_cancelada == "N" 
                 && a.ind_emitido_nf == "S" 
                 && a.dta_finalizacao == null
                 && a.Ind_Coletado.HasValue
                 && a.Ind_Coletado == 1);


            if (!continueAdd)
            {
                filterGat = filterGat.Where(a => a.garantiaid == -1);
            }
            else
            {

                if (cod_garantia.HasValue)
                {
                    filterGat = filterGat.Where(a => a.garantiaid == cod_garantia);
                }


                if (!string.IsNullOrEmpty(notas))
                {
                    filterGat = filterGat.Where(a => a.num_nf_cliente.Contains(notas));
                }

                if (cod_cliente.HasValue)
                {
                    filterGat = filterGat.Where(a => a.cod_cliente == cod_cliente);
                }



            }


            return View(filterGat.ToList());
        }


        public JsonResult GarantiaEmTransitoData(int? cod_garantia, string notas, int? cod_cliente, DateTime? dta_inicial, DateTime? dta_final)
        {
            bool HasFilter = (!string.IsNullOrEmpty(notas) || cod_garantia.HasValue || dta_inicial.HasValue || dta_final.HasValue);

            IEnumerable<Garantia> filterGat;
            filterGat = db.Garantia.AsNoTracking();
            filterGat = filterGat.Where(a => a.ind_emitido_coleta == "S" && a.ind_cancelada == "N" && a.ind_emitido_nf == "S" && a.dta_finalizacao == null);


            if (!HasFilter)
            {
                filterGat = filterGat.Where(a => a.garantiaid == -1);
            }
            else
            {

                if (cod_garantia.HasValue)
                {
                    filterGat = filterGat.Where(a => a.garantiaid == cod_garantia);
                }


                if (!string.IsNullOrEmpty(notas))
                {
                    filterGat = filterGat.Where(a => a.num_nf_cliente.Contains(notas));
                }

                if (cod_cliente.HasValue)
                {
                    filterGat = filterGat.Where(a => a.cod_cliente == cod_cliente);
                }



            }


            return Json(filterGat.ToList(), JsonRequestBehavior.AllowGet);
        }


        //public ActionResult GarantiaEmTransitoResult()
        //{
        //    IEnumerable<Garantia> filterGat;


        //    filterGat = db.Garantia.AsNoTracking();


        //    filterGat = filterGat.Where(a =>
        //        a.ind_emitido_coleta == "S"
        //     && a.ind_cancelada == "N"
        //     && a.ind_emitido_nf == "S"
        //     && a.dta_finalizacao == null
        //     );

        //    return View(data);
        //}




        public ActionResult Sac()
        {
            ViewBag.cod_situacao = new SelectList(db.PS_Situacao_Sac, "cod_situacao", "des_nome", "");
            return View();
        }

        public ActionResult SacResult(int? cod_sac, string nome, int? cod_situacao, DateTime? dta_inicial, DateTime? dta_final, string tags)
        {
            IEnumerable<PS_Sac> filterGat;
            filterGat = db.PS_Sac.AsNoTracking();

            if (cod_sac.HasValue)
            {
                filterGat = filterGat.Where(a => a.cod_sac == cod_sac);
            }

            if (cod_situacao.HasValue)
            {
                filterGat = filterGat.Where(a => a.cod_situacao == cod_situacao);
            }

            if (!string.IsNullOrEmpty(nome))
            {

                filterGat = filterGat.Where(p => p.PS_Pessoas_Sac.des_pessoa.ToUpper().Contains(nome.ToUpper()) || p.PS_Pessoas_Sac.cod_pessoa.ToString().Contains(nome));
            }

            if (!string.IsNullOrEmpty(tags))
            {
                filterGat = filterGat.Where(p => (p.tag ?? "").Contains(tags));

            }


            if (dta_inicial.HasValue)
            {
                filterGat = filterGat.Where(p => p.dta_abertura.Value.Year >= dta_inicial.Value.Year);
                filterGat = filterGat.Where(p => p.dta_abertura.Value.Month >= dta_inicial.Value.Month);
                filterGat = filterGat.Where(p => p.dta_abertura.Value.Day >= dta_inicial.Value.Day);
            }

            if (dta_final.HasValue)
            {
                filterGat = filterGat.Where(p => p.dta_abertura.Value.Year <= dta_final.Value.Year);
                filterGat = filterGat.Where(p => p.dta_abertura.Value.Month <= dta_final.Value.Month);
                filterGat = filterGat.Where(p => p.dta_abertura.Value.Day <= dta_final.Value.Day);
            }

            var data = filterGat.ToList();

            return View(data);
        }


        public FileStreamResult PrinRecebimento(int id)
        {

            Garantia garantia = db.Garantia.Find(id);
            var garantiaitem = db.GarantiaItem.Where(a => a.garantiaid == id).ToList();


            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(iTextSharp.text.PageSize.LETTER);
            Document document = new Document(rec);
            PdfWriter writer = PdfWriter.GetInstance(document, workStream);
            writer.CloseStream = false;


            if (garantia == null)
            {

                writer.PageEvent = new PDFFooter(titulo: "GARANTIA", canhoto: true);
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



            Font palatino_i_small = FontFactory.GetFont(
          "palatino linotype italique",
           BaseFont.CP1252,
           BaseFont.EMBEDDED,
           6,
           Font.NORMAL, Color.GRAY
           );

            writer.PageEvent = new PDFFooter(titulo: "RECEBIMENTO DA GARANTIA", canhoto: true);

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
            _TITULO.Add(new Chunk("Dados da Garantia:", palatino_i_red));
            document.Add(_TITULO);
            document.Add(new Paragraph(" "));

            //linha do numero
            celula = new PdfPCell(new Paragraph(new Chunk("Número:", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(garantia.garantiaid.ToString(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk("Código Cliente:", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(garantia.cod_cliente.ToString(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk("Código Representante:", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(garantia.cod_representante.ToString(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk("Representante:", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(garantia.Ps_Representante.des_pessoa.ToString(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);


            celula = new PdfPCell(new Paragraph(new Chunk("Nome :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(garantia.Ps_Pessoas.des_pessoa, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);


            celula = new PdfPCell(new Paragraph(new Chunk("Email :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(garantia.Ps_Pessoas.des_email, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);



            celula = new PdfPCell(new Paragraph(new Chunk("NF Cliente :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(garantia.num_nf_cliente, palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk("Data Emissão :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(garantia.dta_inclusao.ToString(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);


            celula = new PdfPCell(new Paragraph(new Chunk("Data Finalização :", palatino_bold))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            celula = new PdfPCell(new Paragraph(new Chunk(garantia.dta_finalizacao.ToString(), palatino_i))) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 };
            header.AddCell(celula);

            document.Add(header);

            document.Add(new Paragraph(" "));



            _TITULO = new Paragraph();
            _TITULO.Alignment = Element.ALIGN_LEFT;
            _TITULO.Add(new Chunk("Dados do Recebimento", palatino_i_red));
            document.Add(_TITULO);
            document.Add(new Paragraph(" "));



            header = new PdfPTable(11);
            header.TotalWidth = document.PageSize.Width - 70;
            header.LockedWidth = true;
            header.DefaultCell.BorderWidth = 10;
            header.HorizontalAlignment = Element.ALIGN_LEFT;
            largura_col = new float[] { 8f, 8f, 8f, 8f, 10f, 10f, 10f, 10f, 10f, 10f, 10f };
            header.SetWidths(largura_col);


            header.AddCell(new PdfPCell(new Paragraph(new Chunk("Código", palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
            header.AddCell(new PdfPCell(new Paragraph(new Chunk("Qtd. Lançada", palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
            header.AddCell(new PdfPCell(new Paragraph(new Chunk("Valor Unitário", palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
            header.AddCell(new PdfPCell(new Paragraph(new Chunk("Valor Total", palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
            header.AddCell(new PdfPCell(new Paragraph(new Chunk("Qtde Recebida", palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
            header.AddCell(new PdfPCell(new Paragraph(new Chunk("Qtde Atendida", palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
            header.AddCell(new PdfPCell(new Paragraph(new Chunk("Qtde Fora Prazo", palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
            header.AddCell(new PdfPCell(new Paragraph(new Chunk("Qtde Outra marca", palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
            header.AddCell(new PdfPCell(new Paragraph(new Chunk("Qtde Falta", palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
            header.AddCell(new PdfPCell(new Paragraph(new Chunk("Qtde Avaria", palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
            header.AddCell(new PdfPCell(new Paragraph(new Chunk("Qtde Quebra Direta", palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });

            foreach (var item in garantiaitem)
            {
                header.AddCell(new PdfPCell(new Paragraph(new Chunk(item.cod_foxlux, palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
                header.AddCell(new PdfPCell(new Paragraph(new Chunk(item.qtd_lancamento.ToString(), palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
                header.AddCell(new PdfPCell(new Paragraph(new Chunk(item.vlr_lancamento.ToString("0.00"), palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
                header.AddCell(new PdfPCell(new Paragraph(new Chunk(item.vlr_total.ToString("0.00"), palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
                header.AddCell(new PdfPCell(new Paragraph(new Chunk(item.qtd_recebida.ToString(), palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
                header.AddCell(new PdfPCell(new Paragraph(new Chunk(item.qtd_atendida.ToString(), palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
                header.AddCell(new PdfPCell(new Paragraph(new Chunk(item.qtd_fora_garantia.ToString(), palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
                header.AddCell(new PdfPCell(new Paragraph(new Chunk(item.qtd_outras_marcas.ToString(), palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
                header.AddCell(new PdfPCell(new Paragraph(new Chunk(item.qtd_faltante.ToString(), palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
                header.AddCell(new PdfPCell(new Paragraph(new Chunk(item.qtd_avariada.ToString(), palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });
                header.AddCell(new PdfPCell(new Paragraph(new Chunk(item.qtd_descartada.ToString(), palatino_i_small))) { HorizontalAlignment = Element.ALIGN_LEFT });

            }
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




    }
}
