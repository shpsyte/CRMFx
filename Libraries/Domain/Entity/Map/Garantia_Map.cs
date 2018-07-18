using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Map
{
    public class Garantia_Map : CRMEntityTypeConfiguration<Garantia>, IMapping
    {

        

        public Garantia_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("GARANTIA", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new {x.garantiaid});
            this.Property(x => x.garantiaid).HasColumnName("GARANTIAID").IsRequired();
            this.Property(x => x.cod_cliente).HasColumnName("COD_CLIENTE").IsRequired();
            this.Property(x => x.cod_representante).HasColumnName("COD_REPRESENTANTE").IsOptional();
            this.Property(x => x.dta_inclusao).HasColumnName("DTA_INCLUSAO").IsRequired();
            this.Property(x => x.dta_finalizacao).HasColumnName("DTA_FINALIZACAO").IsOptional();
            this.Property(x => x.cod_status).HasColumnName("COD_STATUS").IsRequired();
            this.Property(x => x.num_nf_cliente).HasColumnName("NUM_NF_CLIENTE").IsOptional();
            this.Property(x => x.obs).HasColumnName("OBS").IsOptional();
            this.Property(x => x.cod_usuario).HasColumnName("COD_USUARIO").IsRequired();
            this.Property(x => x.vlr_garantia).HasColumnName("VLR_GARANTIA").IsRequired();
            this.Property(x => x.vlr_ngarantia).HasColumnName("VLR_NGARANTIA").IsRequired();

            this.Property(x => x.ind_cancelada).HasColumnName("IND_CANCELADA").IsRequired();
            this.Property(x => x.ind_emitido_nf).HasColumnName("IND_EMITIDO_NF").IsRequired();
            this.Property(x => x.ind_emitido_coleta).HasColumnName("IND_EMITIDO_COLETA").IsRequired();
            this.Property(x => x.ind_coleta_direta).HasColumnName("IND_COLETA_DIRETA").IsRequired();
            this.Property(x => x.num_coleta).HasColumnName("NUM_COLETA").IsOptional();
            this.Property(x => x.dta_previsao_coleta).HasColumnName("DTA_PREVISAO_COLETA").IsOptional();
            this.Property(x => x.dta_coleta).HasColumnName("DTA_COLETA").IsOptional();
            this.Property(x => x.cod_transportador).HasColumnName("COD_TRANSPORTADOR").IsOptional();
            this.Property(x => x.protocolo).HasColumnName("PROTOCOLO").IsOptional();

            this.Property(x => x.obscoleta).HasColumnName("OBSCOLETA").IsOptional();
            this.Property(x => x.volumes).HasColumnName("VOLUMES").IsOptional();
            this.Property(x => x.especies).HasColumnName("ESPECIES").IsOptional();
            this.Property(x => x.cod_procedimento_vinculado).HasColumnName("COD_PROCEDIMENTO_VINCULADO").IsOptional();
            this.Property(x => x.cod_procedimento_final).HasColumnName("COD_PROCEDIMENTO_FINAL").IsOptional();
            this.Property(x => x.vlr_credito_vinculado).HasColumnName("VLR_CREDITO_VINCULADO").IsOptional();
            this.Property(x => x.Ind_Coletado).HasColumnName("IND_COLETADO").IsOptional();
            this.Property(x => x.dta_contabil).HasColumnName("DTA_CONTABIL").IsOptional();
            this.Property(x => x.dta_filial).HasColumnName("DTA_FILIAL").IsOptional();


            this.HasRequired(t => t.GE_Status_Garantia).WithMany().HasForeignKey(d => new { d.cod_status });
            this.HasRequired(t => t.Ps_Pessoas).WithMany().HasForeignKey(d => new { d.cod_cliente });
            this.HasRequired(t => t.Ps_Representante).WithMany().HasForeignKey(d => new { d.cod_representante });
            this.HasRequired(t => t.Usuario).WithMany().HasForeignKey(d => new { d.cod_usuario });
            //this.HasRequired(t => t.Transportador).WithMany().HasForeignKey(d => new { d.cod_transportador });
            



        }
    }

    public class GarantiaItem_Map : CRMEntityTypeConfiguration<GarantiaItem>, IMapping
    {
        public GarantiaItem_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("GARANTIAITEM", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.garantiaid, x.garantiaitemid, x.cod_foxlux, x.cod_item });
            this.Property(x => x.garantiaid).HasColumnName("GARANTIAID").IsRequired();
            this.Property(x => x.garantiaitemid).HasColumnName("GARANTIAITEMID").IsRequired();
            this.Property(x => x.cod_item).HasColumnName("COD_ITEM").IsRequired();
            this.Property(x => x.cod_foxlux).HasColumnName("COD_FOXLUX").IsRequired();
            this.Property(x => x.qtd_lancamento).HasColumnName("QTD_LANCAMENTO").IsOptional();
            this.Property(x => x.vlr_lancamento).HasColumnName("VLR_LANCAMENTO").IsOptional();
            this.Property(x => x.vlr_total).HasColumnName("VLR_TOTAL").IsOptional();
            this.Property(x => x.qtd_recebida).HasColumnName("QTD_RECEBIDA").IsOptional();
            this.Property(x => x.qtd_fora_garantia).HasColumnName("QTD_FORA_GARANTIA").IsOptional();
            this.Property(x => x.qtd_outras_marcas).HasColumnName("QTD_OUTRAS_MARCAS").IsOptional();
            this.Property(x => x.qtd_faltante).HasColumnName("QTD_FALTANTE").IsOptional();
            this.Property(x => x.qtd_avariada).HasColumnName("QTD_AVARIADA").IsOptional();
            this.Property(x => x.qtd_atendida).HasColumnName("QTD_ATENDIDA").IsOptional();
            this.Property(x => x.qtd_rejeitada).HasColumnName("QTD_REJEITADA").IsOptional();
            this.Property(x => x.qtd_descartada).HasColumnName("QTD_DESCARTADA").IsOptional();
            this.Property(x => x.qtd_reaproveitada).HasColumnName("QTD_REAPROVEITADA").IsOptional();
            this.Property(x => x.caminho_foto).HasColumnName("CAMINHO_FOTO").IsOptional();
            this.Property(x => x.obs).HasColumnName("OBS").IsOptional();
            this.Property(x => x.justificativa).HasColumnName("JUSTIFICATIVA").IsOptional();
            
            this.Property(x => x.num_nota).HasColumnName("NUM_NOTA").IsOptional();
            this.Property(x => x.conferido).HasColumnName("CONFERIDO").IsOptional();

            this.Property(x => x.vlr_ipi).HasColumnName("VLR_IPI").IsOptional();
            this.Property(x => x.vlr_icms_subs).HasColumnName("VLR_ICMS_SUBS").IsOptional();
            this.Property(x => x.vlr_icms).HasColumnName("VLR_ICMS").IsOptional();
            this.Property(x => x.vlr_base_subs).HasColumnName("VLR_BASE_SUBS").IsOptional();

            this.Property(x => x.picms).HasColumnName("PICMS").IsOptional();
            this.Property(x => x.pipi).HasColumnName("PIPI").IsOptional();
            this.Property(x => x.picmsst).HasColumnName("PICMSST").IsOptional();
            this.Property(x => x.mvast).HasColumnName("MVAST").IsOptional();




            this.HasRequired(t => t.Garantia).WithMany().HasForeignKey(d => new { d.garantiaid });
            this.HasRequired(t => t.IE_Itens).WithMany().HasForeignKey(d => new { d.cod_item }); 




        }
    }





    public class CartItemPrint_Map : CRMEntityTypeConfiguration<CartItemPrint>, IMapping
    {
        public CartItemPrint_Map(string schema)
        {
            this.ToTable("CARTITEMPRINT", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.garantiaId, x.recordId, x.cod_Foxlux, x.cod_item, x.num_nota });


            this.Property(x => x.garantiaId).HasColumnName("GARANTIAID").IsRequired();
            this.Property(x => x.recordId).HasColumnName("RECORDID").IsRequired();
            this.Property(x => x.cod_Foxlux).HasColumnName("COD_FOXLUX").IsRequired();
            this.Property(x => x.num_nota).HasColumnName("NUM_NOTA").IsRequired();
            this.Property(x => x.cod_item).HasColumnName("COD_ITEM").IsRequired();
            


        }
    }



    public class CartItem_Map : CRMEntityTypeConfiguration<CartItem>, IMapping
    {
        public CartItem_Map(string schema)
        {
            this.ToTable("CARTITEM", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.garantiaId, x.recordId, x.cod_Foxlux, x.cod_item, x.num_nota });


            this.Property(x => x.garantiaId).HasColumnName("GARANTIAID").IsRequired();
            this.Property(x => x.recordId).HasColumnName("RECORDID").IsRequired();
            this.Property(x => x.cod_Foxlux).HasColumnName("COD_FOXLUX").IsRequired();
            this.Property(x => x.num_nota).HasColumnName("NUM_NOTA").IsRequired();
            this.Property(x => x.cod_item).HasColumnName("COD_ITEM").IsRequired();

            this.Property(x => x.qtde_Garantia).HasColumnName("QTDE_GARANTIA").IsOptional();
            this.Property(x => x.vlr_Unitario).HasColumnName("VLR_UNITARIO").IsOptional();

            this.Property(x => x.vlr_ipi).HasColumnName("VLR_IPI").IsOptional();
            this.Property(x => x.vlr_icms_subs).HasColumnName("VLR_ICMS_SUBS").IsOptional();
            this.Property(x => x.vlr_icms).HasColumnName("VLR_ICMS").IsOptional();
            this.Property(x => x.vlr_base_subs).HasColumnName("VLR_BASE_SUBS").IsOptional();
            this.Property(x => x.ObsItem).HasColumnName("OBSITEM").IsOptional();
            this.Property(c => c.fator_divisao).HasColumnName("FATOR_DIVISAO").IsOptional();

            this.Property(c => c.picms).HasColumnName("PICMS").IsOptional();
            this.Property(c => c.pipi).HasColumnName("PIPI").IsOptional();
            this.Property(c => c.picmsst).HasColumnName("PICMSST").IsOptional();
            this.Property(c => c.mvast).HasColumnName("MVAST").IsOptional();


        }
    }


    public class Garantia_Troca_Status_Map : CRMEntityTypeConfiguration<Garantia_Troca_Status>, IMapping
    {
        public Garantia_Troca_Status_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("GARANTIA_TROCA_STATUS", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.num_seq, x.garantiaId });


            this.Property(x => x.num_seq).HasColumnName("NUM_SEQ").IsRequired();
            this.Property(x => x.garantiaId).HasColumnName("GARANTIAID").IsRequired();
            this.Property(x => x.cod_status_entrada).HasColumnName("COD_STATUS_ENTRADA").IsOptional();
            this.Property(x => x.dta_entrada).HasColumnName("DTA_ENTRADA").IsOptional();
            this.Property(x => x.dta_saida).HasColumnName("DTA_SAIDA").IsOptional();
            this.Property(x => x.dta_troca).HasColumnName("DTA_TROCA").IsOptional();
            this.Property(x => x.cod_status_anterior).HasColumnName("COD_STATUS_ANTERIOR").IsOptional();
            this.Property(x => x.cod_usuario).HasColumnName("COD_USUARIO").IsOptional();
            this.Property(x => x.obs).HasColumnName("OBS").IsOptional();

            this.HasRequired(t => t.Garantia).WithMany().HasForeignKey(d => new { d.garantiaId });
            this.HasRequired(t => t.Usuario).WithMany().HasForeignKey(d => new { d.cod_usuario });
            this.HasRequired(t => t.StatusEntrada).WithMany().HasForeignKey(d => new { d.cod_status_entrada });
            this.HasRequired(t => t.StatusAnterior).WithMany().HasForeignKey(d => new { d.cod_status_anterior });





        }


        
    }



    public class GarantiaProcedimento_Map : CRMEntityTypeConfiguration<GarantiaProcedimento>, IMapping
    {
        public GarantiaProcedimento_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("GARANTIAPROCEDIMENTO", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.id, x.garantiaId, x.cod_procedimento });


            this.Property(x => x.id).HasColumnName("ID").IsRequired();
            this.Property(x => x.garantiaId).HasColumnName("GARANTIAID").IsRequired();
            this.Property(x => x.cod_procedimento).HasColumnName("COD_PROCEDIMENTO").IsRequired();

        }



    }



    
          public class Tmp_Garantia_Impressao_Map : CRMEntityTypeConfiguration<Tmp_Garantia_Impressao>, IMapping
    {
        public Tmp_Garantia_Impressao_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("TMP_GARANTIA_IMPRESSAO", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.cod_usuario, x.garantiaid });


            this.Property(x => x.cod_usuario).HasColumnName("COD_USUARIO").IsRequired();
            this.Property(x => x.garantiaid).HasColumnName("GARANTIAID").IsRequired();

        }



    }

    public class GarantiaArq_Map : CRMEntityTypeConfiguration<GarantiaArq>, IMapping
    {
        public GarantiaArq_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("GARANTIAARQ", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.id, x.garantiaid });


            this.Property(x => x.id).HasColumnName("ID").IsRequired();
            this.Property(x => x.garantiaid).HasColumnName("GARANTIAID").IsRequired();

            this.Property(x => x.caminho).HasColumnName("CAMINHO").IsOptional();
            this.Property(x => x.des_imagem).HasColumnName("DES_IMAGEM").IsOptional();
            this.Property(x => x.des_contenttype).HasColumnName("DES_CONTENTTYPE").IsOptional();
            this.Property(x => x.nome_arquivo).HasColumnName("NOME_ARQUIVO").IsOptional();

        }



    }


    public class OperacaoFiscal_Map : CRMEntityTypeConfiguration<vWOperacaoFiscal>, IMapping
    {
        public OperacaoFiscal_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("VWOPERACAOFISCAL", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => x.Cod_Operacao );


            this.Property(x => x.Cod_Operacao).HasColumnName("COD_OPERACAO").IsRequired();
            this.Property(x => x.Des_Operacao).HasColumnName("DES_OPERACAO").IsRequired();


            //            this.HasOptional(t => t.IE_Itens).WithMany().HasForeignKey(d => new { d.cod_item });

        }



    }


    public class NotaCliente_Map : CRMEntityTypeConfiguration<NotaCliente>, IMapping
    {
        public NotaCliente_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("NOTACLIENTE", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.Id });


            this.Property(x => x.Id).HasColumnName("ID").IsRequired();
            this.Property(x => x.NotasSelecionadas).HasColumnName("NOTASSELECIONADAS").IsOptional();
            this.Property(x => x.DataImpressao).HasColumnName("DATAIMPRESSAO").IsOptional();
            this.Property(x => x.Obs).HasColumnName("OBS").IsOptional();
            this.Property(x => x.GarantiaId).HasColumnName("GARANTIAID").IsOptional();
            this.Property(x => x.UsuarioId).HasColumnName("USUARIOID").IsOptional();
            this.Property(x => x.ArquivoId).HasColumnName("ARQUIVOID").IsOptional();
            this.Property(x => x.num_nota).HasColumnName("NUM_NOTA").IsOptional();
            this.Property(x => x.cod_serie).HasColumnName("COD_SERIE").IsOptional();
            
            this.Property(x => x.DataEntrada).HasColumnName("DATAENTRADA").IsOptional();
            this.Property(x => x.Vlr_mercadorias).HasColumnName("VLR_MERCADORIAS").IsRequired();
            this.Property(x => x.Vlr_Nota).HasColumnName("VLR_NOTA").IsRequired();
            this.HasRequired(t => t.Garantia).WithMany().HasForeignKey(d => new { d.GarantiaId });


            //            this.HasOptional(t => t.IE_Itens).WithMany().HasForeignKey(d => new { d.cod_item });

        }



    }




    public class NotaItemCliente_Map : CRMEntityTypeConfiguration<NotaItemCliente>, IMapping
    {
        public NotaItemCliente_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("NOTAITEMCLIENTE", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.Id, x.NotaClienteId });


            this.Property(x => x.Id).HasColumnName("ID").IsRequired();
            this.Property(x => x.NotaClienteId).HasColumnName("NOTACLIENTEID").IsRequired();



            this.Property(x => x.GarantiaItemId).HasColumnName("GARANTIAITEMID").IsOptional();
            this.Property(x => x.Cod_Foxlux).HasColumnName("COD_FOXLUX").IsOptional();
            this.Property(x => x.Qtd_Lancamento).HasColumnName("QTD_LANCAMENTO").IsOptional();
            this.Property(x => x.Vlr_icms).HasColumnName("VLR_ICMS").IsOptional();
            this.Property(x => x.Vlr_ipi).HasColumnName("VLR_IPI").IsOptional();
            this.Property(x => x.Vlr_Base_Subs).HasColumnName("VLR_BASE_SUBS").IsOptional();
            this.Property(x => x.Vlr_Imcs_Subs).HasColumnName("VLR_IMCS_SUBS").IsOptional();
            this.Property(x => x.Per_Icms).HasColumnName("PER_ICMS").IsOptional();
            this.Property(x => x.Per_Ipi).HasColumnName("PER_IPI").IsOptional();
            this.Property(x => x.Per_IcmsSt).HasColumnName("PER_ICMSST").IsOptional();
            this.Property(x => x.MVast).HasColumnName("MVAST").IsOptional();
            this.Property(x => x.Vlr_Lancamento).HasColumnName("VLR_LANCAMENTO").IsRequired();
            this.Property(x => x.Vlr_Total).HasColumnName("VLR_TOTAL").IsRequired();
            this.Property(x => x.cd_cfop).HasColumnName("CD_CFOP").IsRequired();


            //            this.HasOptional(t => t.IE_Itens).WithMany().HasForeignKey(d => new { d.cod_item });

        }



    }

    public class GarantiaImagens_Map : CRMEntityTypeConfiguration<GarantiaImagens>, IMapping
    {
        public GarantiaImagens_Map(string schema)
        {
            /*O método ToTable define qual o nome que será dado a tabela no banco de dados*/
            this.ToTable("GARANTIAIMAGENS", schema);

            //Definimos a propriedade como chave primária
            this.HasKey(x => new { x.id, x.garantiaId, x.garantiaitemid });


            this.Property(x => x.id).HasColumnName("ID").IsRequired();
            this.Property(x => x.garantiaId).HasColumnName("GARANTIAID").IsRequired();
            this.Property(x => x.garantiaitemid).HasColumnName("GARANTIAITEMID").IsRequired();
            this.Property(x => x.picture).HasColumnName("PICTURE").IsOptional();
            this.Property(x => x.cod_item).HasColumnName("COD_ITEM").IsOptional();
            this.Property(x => x.des_contenttype).HasColumnName("DES_CONTENTTYPE").IsOptional();
            this.Property(x => x.nome_arquivo).HasColumnName("NOME_ARQUIVO").IsOptional();


            this.HasOptional(t => t.IE_Itens).WithMany().HasForeignKey(d => new { d.cod_item });

        }



    }
    

}
