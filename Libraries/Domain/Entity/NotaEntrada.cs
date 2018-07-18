using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{


    public class t4_conds_pgto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int COD_COND_PGTO { get; set; }
        public string DES_COND_PGTO { get; set; }
    }
    public class Ne_Nota_Chave
    {
        [Key, Column(Order = 0)]
        public int COD_EMP { get; set; }
        [Key, Column(Order = 1)]
        public int COD_UNIDADE { get; set; }
        [Key, Column(Order = 2)]
        public int NUM_NOTA { get; set; }
        [Key, Column(Order = 3)]
        public string COD_SERIE { get; set; }
        [Key, Column(Order = 4)]
        public string COD_PESSOA_FORN { get; set; }
        public DateTime DTA_EMISSAO { get; set; }
        public DateTime DTA_ENTRADA { get; set; }
        public string COD_OPERACAO { get; set; }
        public int? COD_TRANSPORTADOR { get; set; }

        public int? COD_CONDICAO_PGTO { get; set; }
        public int IND_NFE { get; set; }
        public string DES_CHAVE { get; set; }
        public string DES_MODELO { get; set; }
        public string DES_LOCAL { get; set; }
        public string DES_LOTE { get; set; }
        public int TP_OPERACAO { get; set; }
        public string OBS { get; set; }
        public string CMD { get; set; }

    }

    


    public class AI_NE_NOTAS
    {
      

        [Key, Column(Order = 0)]
        public int COD_EMP { get; set; }
        [Key, Column(Order = 1)]
        public int COD_UNIDADE { get; set; }
        [Key, Column(Order = 2)]
        public int NUM_NOTA { get; set; }
        public string COD_SERIE { get; set; }

        [Key, Column(Order = 3)]
        public string COD_PESSOA_FORN { get; set; }
        public int IND_ERRO { get; set; }
        public DateTime DTA_EMISSAO { get; set; }
        public DateTime DTA_RECEBIMENTO { get; set; }
        public int? NUM_ULT_NF { get; set; }
        public int? NUM_AUTORIZACAO_TRANSPORTE { get; set; }
        public DateTime? DTA_BASE_CP { get; set; }
        public int? COD_COND_PGTO { get; set; }
        public int? COD_CIDADE { get; set; }
        public int? COD_PAIS { get; set; }
        public string COD_UF { get; set; }
        public string DES_MODELO { get; set; }
        public int? IND_RESP_FRETE { get; set; }
        public int? NUM_NOTA_PRODUTOR { get; set; }
        public DateTime? DTA_NOTA_PRODUTOR { get; set; }
        public int? COD_LISTA { get; set; }
        public DateTime? DTA_SAIDA { get; set; }
        public int? QTD_ITENS { get; set; }
        public int? COD_MASCARA_FORN { get; set; }
        public int? COD_MASCARA_ITEM { get; set; }
        public int? IND_IMPORTA { get; set; }
        public DateTime? DTA_TRANSACAO { get; set; }
        public int? NUM_NOTA_DEV { get; set; }
        public string COD_SERIE_DEV { get; set; }
        public string COD_PESSOA { get; set; }
        public int? QTD_PMC { get; set; }
        public int? QTD_VOLUMES { get; set; }
        public int? IND_LANCADO_CB { get; set; }
        public int? TIP_STATUS_TRANSACAO { get; set; }
        public int? COD_GRUPO { get; set; }
        public int? IND_STATUS { get; set; }
        public int? IND_NFE { get; set; }
        public int? COD_RESULTADO_NFE { get; set; }
        public string NUM_RECIBO_NFE { get; set; }
        public string DES_CHAVE_NFE { get; set; }
        public DateTime? DTH_DANFE { get; set; }
        public int? NUM_FORMULARIO { get; set; }
        public int? COD_CLIENTE { get; set; }
        public string NUM_MODELO { get; set; }
        public int? NUM_SEQ_LC_SEFAZ { get; set; }
        public int? COD_MAQ_LC_SEFAZ { get; set; }
        public int? NUM_NSU_NF { get; set; }
        public string COD_NAT_FRETE { get; set; }
        public int? NUM_SEQ_ARQUIVO { get; set; }
        public int? COD_TRANSPORTADOR { get; set; }
        public int? NUM_CNPJ_TRANSPORTADOR { get; set; }
        public string NUM_INSC_EST_TRANSP { get; set; }
        public DateTime? DTA_VENCIMENTO { get; set; }










    }
    public class AI_NE_NOTAS_ITENS
    {
      
        [Key, Column(Order = 0)]
        public int COD_EMP { get; set; }
        [Key, Column(Order = 1)]
        public int COD_UNIDADE { get; set; }
        [Key, Column(Order = 2)]
        public int NUM_NOTA { get; set; }
        public string COD_SERIE { get; set; }
        [Key, Column(Order = 3)]
        public string COD_PESSOA_FORN { get; set; }
        [Key, Column(Order = 4)]
        public int NUM_SEQ { get; set; }
        public int? COD_OPER { get; set; }
        public int NUM_SEQ_OPER { get; set; }
        public int? IND_BX_OC { get; set; }
        public int? IND_ATENDIMENTO { get; set; }
        public int? NUM_ORDEM_COMPRA { get; set; }
        public int? NUM_SEQ_OC { get; set; }
        public string COD_ITEM { get; set; }
        public string COD_UM_ESTOQUE { get; set; }
        public decimal? QTD_ESTOQUE { get; set; }
        public string COD_UM_COMPRA { get; set; }
        public decimal? IDX_CONV_COMPRA { get; set; }
        public decimal? QTD_COMPRA { get; set; }
        public decimal VLR_UNITARIO { get; set; }
        public decimal? VLR_ACRESCIMO { get; set; }
        public decimal? VLR_DESCONTO { get; set; }
        public decimal? VLR_TOTAL { get; set; }
        public string COD_LOCAL { get; set; }
        public string COD_CC { get; set; }
        public string NUM_LOTE { get; set; }
        public DateTime? DTA_FABRICACAO { get; set; }
        public DateTime? DTA_VALIDADE { get; set; }
        public string TXT_OBSERVACAO { get; set; }
        public decimal? VLR_FRETE { get; set; }
        public decimal? VLR_SEGURO { get; set; }
        public decimal? VLR_DESPESAS_FN { get; set; }
        public decimal? VLR_IPI { get; set; }
        public int? IND_TRIB_IPI { get; set; }
        public decimal? PER_IPI { get; set; }
        public decimal? VLR_ICMS { get; set; }
        public decimal? VLR_FUNRURAL { get; set; }
        public decimal? VLR_IRRF { get; set; }
        public decimal? VLR_INSS { get; set; }
        public decimal? VLR_BC_ICMS { get; set; }
        public decimal? VLR_IS_ICMS { get; set; }
        public decimal? VLR_DV_ICMS { get; set; }
        public decimal? VLR_ICMS_ST { get; set; }
        public decimal? VLR_BASE_ICMS_ST { get; set; }
        public decimal? PER_ICMS { get; set; }
        public decimal? VLR_OU_ICMS { get; set; }
        public decimal? VLR_PIS { get; set; }
        public decimal? VLR_COFINS { get; set; }
        public int? TIP_STATUS_TRANSACAO { get; set; }
        public int? SEQ_RESERVA { get; set; }
        public int? COD_CONTA { get; set; }
        public int? SEQ_LOCAL { get; set; }
        public string NUM_SERIE { get; set; }
        public decimal? VLR_BC_FUNRURAL { get; set; }
        public decimal? VLR_OU_IPI { get; set; }
        public decimal? VLR_IS_IPI { get; set; }
        public decimal? VLR_BC_IPI { get; set; }
        public decimal? VLR_OPERACAO { get; set; }
        public int? TIP_IPI { get; set; }
        public decimal? PER_FUNRURAL { get; set; }
        public decimal? PER_ICMS_CONSUMO { get; set; }
        public decimal? VLR_ICMS_CONSUMO { get; set; }
        public decimal? VLR_BC_ICMS_CONSUMO { get; set; }
        public string DES_OPERACAO { get; set; }
        public int? COD_GR_FISCAL { get; set; }
        public int? NUM_CFOP { get; set; }
        public decimal? VLR_ACRE_TN { get; set; }
        public decimal? VLR_DCTO_TN { get; set; }
        public decimal? VLR_DIF_ICMS { get; set; }
        public decimal? VLR_ICMS_FN { get; set; }
        public decimal? VLR_BC_ICMS_ST { get; set; }
        public decimal? VLR_DESPESAS_NOTA { get; set; }
        public decimal? VLR_DESCONTO_OP { get; set; }
        public decimal? VLR_CREDPRES { get; set; }
        public decimal? VLR_CREDPRES_FRE { get; set; }
        public int? COD_CAMPANHA { get; set; }
        public int? COD_CANAL { get; set; }
        public decimal? VLR_BASE_CREDPRES { get; set; }
        public int? IND_ICMS_ST { get; set; }
        public decimal? VLR_GERENCIAL1_FN { get; set; }
        public decimal? VLR_GERENCIAL2_FN { get; set; }
        public int? COD_REPRESENTANTE { get; set; }
        public int? COD_ATENDENTE { get; set; }
        public decimal? VLR_PIS_ST { get; set; }
        public decimal? VLR_COFINS_ST { get; set; }
        public decimal? VLR_PIS_RET { get; set; }
        public decimal? VLR_COFINS_RET { get; set; }
        public decimal? VLR_CSLL_RET { get; set; }
        public string NUM_CERTIFICADO { get; set; }
        public string NUM_VOLUME { get; set; }
        public string NUM_REGISTRO { get; set; }
        public string NUM_RNCMA { get; set; }
        public decimal? QTD_LARGURA { get; set; }
        public int? NUM_CFOP_ORI { get; set; }
        public string COD_SIT_IPI { get; set; }
        public string COD_SIT_PIS { get; set; }
        public string COD_SIT_COFINS { get; set; }
        public string COD_SIT_ICMS { get; set; }
        public int? NUM_SEQ_DEV { get; set; }
        public int? COD_MAQ_DEV { get; set; }
        public decimal? VLR_ST_OBS { get; set; }
        public decimal? VLR_BASE_ST_OBS { get; set; }
        public decimal? VLR_BASE_II { get; set; }
        public decimal? VLR_II { get; set; }
        public decimal? VLR_DESP_ADU { get; set; }
        public decimal? VLR_IOF { get; set; }
        public decimal? VLR_TAXA_SISCOMEX { get; set; }
        public decimal? VLR_CAPATAZIAS { get; set; }
        public int? NUM_SEQ_DI_ADICAO { get; set; }
        public int? NUM_ADICAO { get; set; }
        public decimal? VLR_ACRE_IMPORTA { get; set; }
        public decimal? VLR_NAO_INCIDENTE { get; set; }
        public string COD_MOT_DES_ICMS { get; set; }
        public string COD_SIT_SIMPLES_NAC { get; set; }
        public string COD_TRIB_ISSQN { get; set; }
        public string COD_CONTRIBUICAO_PISCOF { get; set; }
        public string COD_TIPO_CRED_PISCOF { get; set; }
        public string COD_NAT_BC_PISCOF { get; set; }
        public string COD_NAT_REC_PISCOF { get; set; }
        public DateTime DTA_EMISSAO { get; set; }
        public decimal? VLR_BASE_SIMPLES { get; set; }
        public decimal? PER_SIMPLES { get; set; }
        public decimal? VLR_SIMPLES { get; set; }
        public int? SEQ_GRADE { get; set; }
        public decimal? VLR_ICMS_RECOLHIDO { get; set; }
        public DateTime? DTA_PREV_ENTR_OC { get; set; }
        public decimal? VLR_BC_DIFERENCIAL_ICMS { get; set; }
        public decimal? PER_DIFERENCIAL_ICMS { get; set; }
        public decimal? VLR_DIFERENCIAL_ICMS { get; set; }
        public decimal? VLR_RED_BC_INSS { get; set; }
        public int? TIP_ICMS_IMP_NFE { get; set; }
        public int? TIP_IPI_IMP_NFE { get; set; }
        public int? TIP_PIS_IMP_NFE { get; set; }
        public int? TIP_COFINS_IMP_NFE { get; set; }
        public int? IND_BONIFICACAO { get; set; }
        public int? TIP_ICMS_ST_IMP_NFE { get; set; }
        public decimal? VLR_DESCONTO_IT { get; set; }
        public decimal? VLR_ACRESCIMO_COB { get; set; }
        public decimal? VLR_CUSTOS_COMPRA { get; set; }
        public decimal? VLR_CUSTO_BONIFICACAO { get; set; }
        public decimal? VLR_CUSTO_ESTOCAGEM { get; set; }
        public decimal? VLR_CONTR_COBERTURA { get; set; }
        public string NUM_SEQ_ITEM_NFE { get; set; }
        public int? COD_PADRAO { get; set; }
        public string DES_PADRAO { get; set; }
        public decimal? PER_CARBONO { get; set; }
        public decimal? PER_MANGANES { get; set; }
        public decimal? PER_FOSFORO { get; set; }
        public decimal? PER_ENXOFRE { get; set; }
        public decimal? VLR_DESC_COMERCIAL { get; set; }
        public decimal? VLR_ABAT_ICMS { get; set; }
        public decimal? VLR_ABAT_PIS { get; set; }
        public decimal? VLR_ABAT_COFINS { get; set; }
        public decimal? VLR_PIS_CC { get; set; }
        public decimal? VLR_COFINS_CC { get; set; }
        public decimal? VLR_ICMS_CC { get; set; }
        public decimal? VLR_BASE_SIMPLES_ISSQN { get; set; }
        public decimal? PER_SIMPLES_ISSQN { get; set; }
        public decimal? VLR_SIMPLES_ISSQN { get; set; }
        public string COD_NCM { get; set; }
        public int? COD_ORIGEM_ICMS { get; set; }
        public string COD_ENQ_IPI { get; set; }
        public string COD_CEST { get; set; }
        public decimal? PER_ICMS_DEST { get; set; }
        public decimal? PER_ICMS_INTER { get; set; }
        public decimal? PER_ICMS_PART { get; set; }
        public decimal? VLR_ICMS_DEST { get; set; }
        public decimal? VLR_ICMS_REMET { get; set; }
        public decimal? PER_FCP_DEST { get; set; }
        public decimal? VLR_FCP_DEST { get; set; }
        public decimal? VLR_BC_PARTILHA_ICMS { get; set; }



    }
    public class AI_NE_CONHECIMENTOS
    {
        [Key, Column(Order = 0)]
        public int COD_EMP { get; set; }
        [Key, Column(Order = 1)]
        public int COD_UNIDADE { get; set; }
        [Key, Column(Order = 2)]
        public int NUM_NOTA { get; set; }
        public string COD_SERIE { get; set; }
        [Key, Column(Order = 3)]
        public string COD_PESSOA_FORN { get; set; }
        [Key, Column(Order = 4)]
        public int SEQ_NOTA { get; set; }
        public int? NUM_SEQ_NS { get; set; }
        public int? COD_MAQ_NS { get; set; }
        public int? NUM_SEQ_NE { get; set; }
        public int? COD_MAQ_NE { get; set; }
        public DateTime DTA_EMISSAO { get; set; }
        public int? TIP_NOTA_CONHE { get; set; }
        public int? NUM_NOTA_CONHE { get; set; }
        public string COD_SERIE_CONHE { get; set; }
        public int? COD_PESSOA_CONHE { get; set; }
        public DateTime? DTA_EMISSAO_CONHE { get; set; }
        public int? COD_ITEM_CONHE { get; set; }
        public int? QTD_LCTO_CONHE { get; set; }
        public decimal? VLR_TOTAL_CONHE { get; set; }
        public decimal? VLR_FRETE_CONHE { get; set; }
        public int? NUM_ORDEM_COMPRA_CONHE { get; set; }


    }
    public class AI_NE_NOTAS_ICMS
    {
        [Key, Column(Order = 0)]
        public int COD_EMP { get; set; }
        [Key, Column(Order = 1)]
        public int COD_UNIDADE { get; set; }
        [Key, Column(Order = 2)]
        public int NUM_NOTA { get; set; }
        public string COD_SERIE { get; set; }
        [Key, Column(Order = 3)]
        public string COD_PESSOA_FORN { get; set; }
        [Key, Column(Order = 4)]
        public int COD_OPER { get; set; }
        [Key, Column(Order = 5)]
        public int NUM_SEQ { get; set; }
        public decimal PER_ICMS { get; set; }
        public decimal? VLR_BC_ICMS { get; set; }
        public decimal? VLR_ICMS { get; set; }
        public decimal? VLR_IS_ICMS { get; set; }
        public decimal? VLR_OU_ICMS { get; set; }
        public decimal? VLR_BC_ICMS_ST { get; set; }
        public decimal? VLR_ICMS_ST { get; set; }
        public int? IND_FRETE { get; set; }
        public int? IND_ICMS_ST { get; set; }
        public decimal? VLR_NAO_INCIDENTE { get; set; }
        public DateTime DTA_EMISSAO { get; set; }


    }
    public class AI_NE_NOTAS_OPERACOES
    {
        [Key, Column(Order = 0)]
        public int COD_EMP { get; set; }
        [Key, Column(Order = 1)]
        public int COD_UNIDADE { get; set; }
        [Key, Column(Order = 2)]
        public int NUM_NOTA { get; set; }
        public string COD_SERIE { get; set; }
        [Key, Column(Order = 3)]

        public string COD_PESSOA_FORN { get; set; }
        public int? COD_OPER { get; set; }
        [Key, Column(Order = 4)]
        public int NUM_SEQ_OPER { get; set; }
        public int? NUM_CFOP { get; set; }
        public int? COD_GR_FISCAL { get; set; }
        public decimal? VLR_PRODUTOS { get; set; }
        public decimal? VLR_OPERACAO { get; set; }
        public decimal? VLR_FRETE_NOTA { get; set; }
        public decimal? VLR_SEGURO_NOTA { get; set; }
        public decimal? VLR_DESPESAS_NOTA { get; set; }
        public decimal? VLR_DESPESAS_FN { get; set; }
        public decimal? VLR_DESCONTOS { get; set; }
        public int? TIP_IPI { get; set; }
        public decimal? VLR_BC_IPI { get; set; }
        public decimal? VLR_IPI { get; set; }
        public decimal? VLR_IS_IPI { get; set; }
        public decimal? VLR_OU_IPI { get; set; }
        public decimal? VLR_BC_ICMS_CONSUMO { get; set; }
        public decimal? PER_ICMS_CONSUMO { get; set; }
        public decimal? VLR_ICMS_CONSUMO { get; set; }
        public decimal? VLR_DIF_ICMS { get; set; }
        public decimal? VLR_ICMS_FN { get; set; }
        public decimal? VLR_ACRE_TN { get; set; }
        public decimal? VLR_DCTO_TN { get; set; }
        public decimal? VLR_BC_FUNRURAL { get; set; }
        public decimal? ALIQ_FUNRURAL { get; set; }
        public decimal? VLR_FUNRURAL { get; set; }
        public decimal? VLR_IRRF { get; set; }
        public decimal? VLR_INSS { get; set; }
        public decimal? VLR_PIS { get; set; }
        public decimal? VLR_COFINS { get; set; }
        public int? TIP_STATUS_TRANSACAO { get; set; }
        public decimal? VLR_CREDPRES { get; set; }
        public decimal? VLR_BASE_CREDPRES { get; set; }
        public decimal? VLR_GERENCIAL1_FN { get; set; }
        public decimal? VLR_GERENCIAL2_FN { get; set; }
        public decimal? VLR_PIS_ST { get; set; }
        public decimal? VLR_COFINS_ST { get; set; }
        public decimal? VLR_PIS_RET { get; set; }
        public decimal? VLR_COFINS_RET { get; set; }
        public decimal? VLR_CSLL_RET { get; set; }
        public decimal? VLR_BASE_II { get; set; }
        public decimal? VLR_II { get; set; }
        public decimal? VLR_DESP_ADU { get; set; }
        public decimal? VLR_IOF { get; set; }
        public DateTime? DTA_EMISSAO { get; set; }
        public decimal? VLR_BASE_SIMPLES { get; set; }
        public decimal? PER_SIMPLES { get; set; }
        public decimal? VLR_SIMPLES { get; set; }
        public decimal? VLR_RED_BC_INSS { get; set; }
        public decimal? VLR_ACRESCIMO_COB { get; set; }
        public decimal? VLR_DESC_COMERCIAL { get; set; }
        public decimal? VLR_ABAT_ICMS { get; set; }
        public decimal? VLR_ABAT_PIS { get; set; }
        public decimal? VLR_ABAT_COFINS { get; set; }
        public decimal? VLR_BASE_SIMPLES_ISSQN { get; set; }
        public decimal? PER_SIMPLES_ISSQN { get; set; }
        public decimal? VLR_SIMPLES_ISSQN { get; set; }

    }
    public class AI_NE_OBSERVACOES
    {
        [Key, Column(Order = 0)]
        public int COD_EMP { get; set; }
        [Key, Column(Order = 1)]
        public int COD_UNIDADE { get; set; }
        [Key, Column(Order = 2)]
        public int NUM_NOTA { get; set; }
        public string COD_SERIE { get; set; }
        [Key, Column(Order = 3)]
        public string COD_PESSOA_FORN { get; set; }
        [Key, Column(Order = 4)]
        public int COD_OPER { get; set; }
        [Key, Column(Order = 5)]
        public int NUM_SEQ_OBS { get; set; }
        public string TXT_OBS { get; set; }
        public int? IND_NF { get; set; }
        public int? IND_REGISTRO { get; set; }
        public DateTime DTA_EMISSAO { get; set; }

    }
    public class AI_NE_TITULOS
    {
        [Key, Column(Order = 0)]
        public int COD_EMP { get; set; }
        [Key, Column(Order = 1)]
        public int COD_UNIDADE { get; set; }
        [Key, Column(Order = 2)]
        public int NUM_NOTA { get; set; }
        public string COD_SERIE { get; set; }
        [Key, Column(Order = 3)]
        public string COD_PESSOA_FORN { get; set; }
        [Key, Column(Order = 4)]
        public int NUM_TITULO { get; set; }
        [Key, Column(Order = 5)]
        public int NUM_PARCELA { get; set; }
        public decimal VLR_TITULO { get; set; }
        public DateTime DTA_VENCIMENTO { get; set; }
        public int IND_PAGO { get; set; }
        public int? TIP_TITULO { get; set; }
        public int COD_PORTADOR { get; set; }
        public int COD_POSICAO { get; set; }
        public int? COD_MOEDA { get; set; }
        public int IND_DC { get; set; }
        public int? COD_OPER { get; set; }
        public DateTime? DTA_VCTO_ESP { get; set; }
        public decimal? PERC_DESC_ESP { get; set; }
        public decimal? VLR_DESC_ESP { get; set; }
        public DateTime? DTA_ACEITE { get; set; }
        public decimal? VLR_JURO_DIA { get; set; }
        public int? COD_COND_PGTO { get; set; }
        public string TXT_OBSERVACAO { get; set; }
        public decimal? QTD_MOEDA { get; set; }
        public int? TIP_STATUS_TRANSACAO { get; set; }
        public int? COD_VERBA { get; set; }
        public decimal? PER_VERBA { get; set; }
        public int? TIP_ORIGEM { get; set; }
        public DateTime DTA_EMISSAO { get; set; }
        public string NUM_TITULO_NFE { get; set; }
        public int IND_MANTEM_PARCELA { get; set; }

    }

    public class AI_NE_CONTROLE
    {
        [Key, Column(Order = 0)]
        public int COD_EMP { get; set; }
        [Key, Column(Order = 1)]
        public int COD_UNIDADE { get; set; }
        [Key, Column(Order = 2)]
        public int NUM_NOTA { get; set; }
        [Key, Column(Order = 3)]
        public string COD_PESSOA { get; set; }
        public string COD_SERIE { get; set; }
        public DateTime DTA_EMISSAO { get; set; }
        public int COD_APLICACAO { get; set; }
        [Key, Column(Order = 4)]
        public int NUM_SEQ { get; set; }
        

    }

    public class NE_NOTA_MENSAGEM
    {

        [Key, Column(Order = 0)]

        public int ID_MENSAGEM { get; set; }

        [Key, Column(Order = 1)]

        public int NUM_NOTA { get; set; }

        [Key, Column(Order = 2)]
        public string COD_SERIE { get; set; }

        [Key, Column(Order = 3)]
        public string COD_PESSOA_FORN { get; set; }

        [Key, Column(Order = 4)]
        public int COD_UNIDADE { get; set; }

        public DateTime DTA_EMISSAO { get; set; }

        public DateTime DTA_RECEBIMENTO { get; set; }

        public string MSG { get; set; }
        public int HASERROR { get; set; }



    }



}
