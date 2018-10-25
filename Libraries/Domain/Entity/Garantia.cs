using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Garantia
    {
        public Garantia()
        {
            this.dta_inclusao = System.DateTime.Now;
            this.cod_status = 1;
            this.ind_coleta_direta = "N";
            this.ind_emitido_coleta = "N";
            this.ind_emitido_nf = "N";
            this.ind_cancelada = "N";
            this.Ind_Coletado = 0;
            this.dta_coleta = null;
            this.dta_previsao_coleta = null;
            this.protocolo = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString() + this.garantiaid.ToString();
        }

        

        [NotMapped]
        public TimeSpan? _total_aberto
        {
            get
            {
                TimeSpan? timestamp = (this.dta_finalizacao.HasValue ? this.dta_finalizacao.Value : DateTime.Now) - (this.dta_inclusao);
                return timestamp;
            }
        }



        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int garantiaid { get; set; }
        [Required]
        public int cod_cliente { get; set; }
        [Required]
        public int cod_representante { get; set; }
        public DateTime dta_inclusao { get; set; }
        public DateTime? dta_finalizacao { get; set; }

        [Required]
        public int cod_status { get; set; }
        public string num_nf_cliente { get; set; }
        [MaxLength(255)]
        public string obs { get; set; }
        [Required]
        public int cod_usuario { get; set; }
        public decimal? vlr_garantia { get; set; }
        public decimal? vlr_ngarantia { get; set; }
        [Required]
        public string ind_emitido_nf { get; set; }
        [Required]
        public string ind_cancelada { get; set; }
        [Required]
        public string ind_emitido_coleta { get; set; }
        [Required]
        public string ind_coleta_direta { get; set; }
        public string num_coleta { get; set; }
        public DateTime? dta_previsao_coleta { get; set; }
        public DateTime? dta_coleta { get; set; }
        public string protocolo { get; set; }
        public Nullable<int> cod_transportador { get; set; }
        public string obscoleta { get; set; }
        public string volumes { get; set; }
        public string especies { get; set; }

        public int? cod_procedimento_vinculado { get; set; }
        public int? cod_procedimento_final  { get; set; }

        public Nullable<decimal> vlr_credito_vinculado { get; set; }
        public int? Ind_Coletado { get; set; }


        public DateTime? dta_contabil { get; set; }
        public DateTime? dta_filial   { get; set; }

       




        public virtual GE_Status_Garantia GE_Status_Garantia { get; set; }
        public virtual Ps_Pessoas Ps_Pessoas { get; set; }
        public virtual Ps_Pessoas Ps_Representante { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual TRANSPORTADOR Transportador { get; set;  }
    }


    public partial class Tmp_Garantia_Impressao
    {
        public int cod_usuario { get; set; }

        public int garantiaid { get; set; }
    }
    public partial class GarantiaArq
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int garantiaid { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string caminho { get; set; }

        public byte[] des_imagem { get; set; }
        public string des_contenttype { get; set; }
        public string nome_arquivo { get; set; }

    }


    public partial class vWOperacaoFiscal
    {
        [Key, Column(Order = 0)]
        public int Cod_Operacao { get; set; }
        public string Des_Operacao { get; set; }

    }
    

    public partial class NotaCliente
    {

        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string NotasSelecionadas { get; set; }
        public DateTime DataImpressao { get; set; }

        public string   Obs { get; set; }
        public int GarantiaId { get; set; }
        public int UsuarioId { get; set; }

        public int ArquivoId { get; set; }  
        public int num_nota { get; set; }
        public string cod_serie { get; set; }
        public DateTime? DataEntrada { get; set; }

        public decimal Vlr_mercadorias { get; set; }
        public decimal Vlr_Nota { get; set; }

        public virtual Garantia Garantia { get; set; }
    }




    public partial class NotaItemCliente
    {


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int NotaClienteId { get; set; }
        public int GarantiaItemId { get; set; }

        public string Cod_Foxlux { get; set; }
        public decimal? Qtd_Lancamento { get; set; }
        public decimal? Vlr_icms { get; set; }
        public decimal? Vlr_ipi { get; set; }
        public decimal? Vlr_Base_Subs { get; set; }
        public decimal? Vlr_Imcs_Subs { get; set; }
        public decimal? Per_Icms { get; set; }
        public decimal? Per_Ipi { get; set; }
        public decimal? Per_IcmsSt { get; set; }
        public decimal? MVast { get; set; }

        public decimal Vlr_Lancamento { get; set; }
        public decimal Vlr_Total { get; set; }

        public int? cd_cfop { get; set; }


    }


    public partial class GarantiaImagens
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int garantiaId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int garantiaitemid { get; set; }

        public int cod_item { get; set; }


        public virtual IE_Itens IE_Itens  { get; set; }

        public byte[] picture { get; set; }

        public string des_contenttype { get; set; }
        public string nome_arquivo { get; set; }



    }


     


    public class GarantiaItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int garantiaid { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int garantiaitemid { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string cod_foxlux { get; set; }
        public int cod_item { get; set; }
        public decimal qtd_lancamento { get; set; }
        public decimal vlr_lancamento { get; set; }
        public decimal vlr_total { get; set; }
        public int num_nota { get; set; }
        public decimal qtd_recebida { get; set; }
        public decimal qtd_fora_garantia { get; set; }
        public decimal qtd_outras_marcas { get; set; }
        public decimal qtd_faltante { get; set; }
        public decimal qtd_avariada { get; set; }
        public decimal qtd_atendida { get; set; }
        public decimal qtd_rejeitada { get; set; }
        public decimal qtd_descartada { get; set; }
        public decimal qtd_reaproveitada { get; set; }
        public string caminho_foto { get; set; }
        public string obs { get; set; }
        public string justificativa { get; set; }
        public string conferido { get; set; }

        public decimal vlr_ipi { get; set; }
        public decimal vlr_icms_subs { get; set; }
        public decimal vlr_icms { get; set; }
        public decimal vlr_base_subs { get; set; }

        public decimal? picms { get; set; }
        public decimal? pipi { get; set; }
        public decimal? picmsst { get; set; }
        public decimal? mvast { get; set; }

        public int? cod_unidade { get; set; }

        [NotMapped]
        public decimal TotalFora { get { return this.qtd_faltante + this.qtd_outras_marcas + this.qtd_fora_garantia; } }

        public virtual Garantia Garantia { get; set; }
        public virtual IE_Itens IE_Itens { get; set; }


    }



    public partial class Garantia_Troca_Status
    {

        [NotMapped]
        public TimeSpan? _total_aberto
        {
            get
            {
                TimeSpan? timestamp = (this.dta_saida.HasValue ? this.dta_saida.Value : DateTime.Now) - (this.dta_entrada.HasValue ? this.dta_entrada.Value : DateTime.Now);
                return timestamp;
            }
        }

        public Garantia_Troca_Status()
        {
            this.dta_troca = System.DateTime.Now;
        }


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int num_seq { get; set; }

        public int garantiaId { get; set; }
        public Nullable<DateTime> dta_troca { get; set; }
        public int? cod_status_entrada { get; set; }
        public Nullable<DateTime> dta_entrada { get; set; }
        public Nullable<DateTime> dta_saida { get; set; }
        public int? cod_status_anterior { get; set; }
        public int? cod_usuario { get; set; }
        public string obs { get; set; }


        public virtual Garantia Garantia { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual GE_Status_Garantia StatusEntrada { get; set; }
        public virtual GE_Status_Garantia StatusAnterior { get; set; }

    }



    public partial class Garantia_Resposta_Model
    {
        public virtual Garantia Garantia { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(255, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string Obs { get; set; }
        public string num_nf_cliente { get; set; }


    }

    public partial class GarantiaProcedimento
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public int garantiaId { get; set; }
        public int cod_procedimento { get; set; }
    }


    public class Garantia_Procedimento_Model
    {
        public virtual ProcedimentoAdm ProcedimentoAdm { get; set; }
        public virtual Garantia Garantia { get; set; }

        public decimal vlr_debito_transportador { get; set; }
        public decimal vlr_debito_representante { get; set; }
        public decimal vlr_debito_foxlux { get; set; }
        public decimal vlr_debito_cliente { get; set; }
        public decimal vlr_credito_cliente { get; set; }


        [Required(ErrorMessage = "*")]
        [StringLength(255, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string Obs { get; set; }
        public int cd_transportador { get; set; }
    }


    public class V_Garantias_Representantes
    {
        public int cod_representante { get; set; }
        public string des_representante { get; set; }
        public int garantiaid { get; set; }
        public string des_nome { get; set; }
        public int cod_cliente { get; set; }
        public string razao { get; set; }
        public DateTime dta_inclusao { get; set; }
        public string num_nf_cliente { get; set; }
        public string obs { get; set; }
        public decimal vlr_garantia { get; set; }
        public string num_coleta { get; set; }
        public DateTime dta_previsao_coleta { get; set; }
        public DateTime dta_coleta { get; set; }

        public string obscoleta { get; set; }
        public string volumes { get; set; }
        public string especies { get; set; }
        public int? cod_procedimento_vinculado  { get; set; }
        

    }

    public class V_Garantia_Total_Representante
    {
        public int Qtde { get; set; }
        public int cod_representante { get; set; }
        public string des_pessoa { get; set; }
        public decimal vlr_garantias { get; set; }
        public DateTime? dta_menor_coleta { get; set; }
        public string lista_nf_garantias { get; set; }
    }


    public partial class CartItemPrint
    {
        public int garantiaId { get; set; }
        public int recordId { get; set; }
        public int cod_item { get; set; }
        public string cod_Foxlux { get; set; }
        public int num_nota { get; set; }
    }
        

    public partial class CartItem
    {
        
        public int garantiaId { get; set; }
        public int recordId { get; set; }
        public int cod_item { get; set; }
        public string cod_Foxlux { get; set; }
        public int num_nota { get; set; }
        public decimal qtde_Garantia { get; set; }
        public decimal vlr_Unitario { get; set; }
        public decimal vlr_ipi { get; set; }
        public decimal vlr_icms_subs { get; set; }
        public decimal vlr_icms { get; set; }
        public decimal vlr_base_subs { get; set; }
        public string ObsItem { get; set; }
        public decimal fator_divisao { get; set; }


        public decimal picms { get; set; }
        public decimal pipi { get; set; }
        public decimal picmsst { get; set; }
        public decimal mvast { get; set; }

        public int? cod_unidade { get; set; }

    }







}
