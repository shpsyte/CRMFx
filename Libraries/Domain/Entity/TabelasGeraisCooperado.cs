using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Domain.Entity
{

    //SELECT * FROM sp_table_class where table_name = 'DADOS_CRM_VIEW'
    //SELECT * FROM sp_table_class_map where table_name = 'DADOS_CRM_VIEW'
    public partial class CampanhaMarketingPgto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int campanhamarketingpgtoid { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int campanhaid { get; set; }
        public int formapgtoid { get; set; }
        public int num_pedido { get; set; }
        public int cod_compl { get; set; }
        public int num_titulo { get; set; }
        public int num_parcela { get; set; }
        public string des_documento { get; set; }
        [Required]
        public string des_agencia { get; set; }
        [Required]
        public string des_banco { get; set; }
        [Required]
        public string des_conta { get; set; }
        public int cod_usuario { get; set; }
        public DateTime dta_inclusao { get; set; }
        public decimal vlr_pgto { get; set; }

        public byte[] des_imagem { get; set; }
        public string des_observacao { get; set; }
        public string ind_total { get; set; }
        public string des_contentype { get; set; }


        public virtual Usuario Usuario { get; set; }
        public virtual CampanhaMarketing CampanhaMarketing { get; set; }
        public virtual FormaPgto FormaPgto { get; set; }



    }
    public partial class Status
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int statusId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string descricao { get; set; }
    }
    public partial class Estagio
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int estagioId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string descricao { get; set; }
        public string ind_inicio { get; set; }
        public int? estagioid_proximo { get; set; }
        public int? estagioid_anterior { get; set; }
        public int? statusid { get; set; }
        public string ind_liberado { get; set; }
        public virtual Status Status { get; set; }
        public virtual Estagio estagioProximo { get; set; }
        public virtual Estagio estagioAnterior { get; set; }
    }
    public partial class EstagioUsuario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int estagiousuarioId { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public int estagioId { get; set; }
        public int cd_usuario { get; set; }
        public virtual Estagio Estagio { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
    public partial class UsuarioEstagio_Model
    {
        public virtual EstagioUsuario EstagioUsuario { get; set; }
        public IEnumerable<EstagioUsuario> ListaUsuario { get; set; }
        public int cod_estagio { get; set; }
        public string nome_estagio { get; set; }
    }
    public partial class FormaPgto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int formapgtoid { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string des_forma { get; set; }
    }
    public partial class Segmentos
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int segmentoid { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string des_segmento { get; set; }
    }
    public partial class TipoAcao
    {

      

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int tipoacaoid { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int segmentoid { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string des_acao { get; set; }
    }
    public partial class TipoAcao_Model
    {
        public virtual TipoAcao TipoAcao { get; set; }
        public IEnumerable<TipoAcao> ListaTipoAcao { get; set; }
        public int CodSegmento { get; set; }
        public string NomeSegmento { get; set; }
    }
    public partial class CampanhaMarketingDoc
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CampanhaMarketingDocId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int campanhaID { get; set; }
        public string Caminho { get; set; }

        public byte[] des_imagem { get; set; }
        public string des_contenttype { get; set; }
        public string nome_arquivo { get; set; }
    }
    public partial class CampanhaMarketing
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int campanhaID { get; set; }
        public string des_nome { get; set; }
        public int cod_pessoa { get; set; }
        public string cod_pessoa_string { get; set; }
        public int cod_representante { get; set; }
        public string des_email { get; set; }
        public int segmentoid { get; set; }
        public int tipoacaoid { get; set; }
        public Nullable<DateTime> dta_inicial { get; set; }
        public Nullable<DateTime> dta_final { get; set; }
        public string ind_renova { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Entre com valor válido")]
        public decimal? vlr_meta { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Entre com valor válido")]
        public decimal? vlr_contrato { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Entre com valor válido")]
        public decimal? per_contrato { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Entre com valor válido")]
        public decimal? vlr_custo_medio { get; set; }

        public string tip_apuracao { get; set; }
        public string tip_pgto_premiacao { get; set; }
        public int formapgtoid { get; set; }
        public string des_banco { get; set; }
        public string des_conta { get; set; }
        public string des_agencia { get; set; }
        public string des_observacao { get; set; }
        public string situacao { get; set; }
        public int statusId { get; set; }
        public int estagioId { get; set; }
        public int? cod_usuario_alteracao { get; set; }
        public Nullable<DateTime> dta_alteracao { get; set; }
        public string des_ult_obs { get; set; }
        public DateTime dta_inclusao { get; set; }
        public int cod_regional { get; set; }
        public int cod_diretoria { get; set; }
        public string des_segmento { get; set; }




        public virtual Segmentos Segmentos { get; set; }
        public virtual TipoAcao TipoAcao { get; set; }
        public virtual FormaPgto FormaPgto { get; set; }
        public virtual Ps_Pessoas Ps_Pessoas { get; set; }
        public virtual Ps_Representantes Ps_Representantes { get; set; }
        public virtual Status Status { get; set; }
        public virtual Estagio Estagio { get; set; }
        public virtual Usuario UsuarioAlteracao { get; set; }
        public virtual DadosDoCrm DadosDoCrm { get; set; }

    }
    public partial class CampanhaMarketingEstagios
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int num_seq { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int campanhaId { get; set; }
        public Nullable<DateTime> dta_troca { get; set; }
        public int? estagioidentrada { get; set; }
        public Nullable<DateTime> dta_entrada { get; set; }
        public Nullable<DateTime> dta_saida { get; set; }
        public int? estagioidanterior { get; set; }
        public int? cod_usuario { get; set; }
        public string obs { get; set; }
        public virtual CampanhaMarketing CampanhaMarketing { get; set; }
        public virtual Estagio EstagioEntrada { get; set; }
        public virtual Estagio EstagioAnterior { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
    public partial class CampanhaOrcamento
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int campanhaorcamentoid { get; set; }
        public int ano { get; set; }
        public decimal vlr_orcamento { get; set; }
    }

    public partial class CampanhaOrcamentoRegional
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int campanhaorcamentoregionalid { get; set; }
        public int campanhaorcamentoid { get; set; }
        public int cod_regional { get; set; }
        public decimal vlr_orcamento { get; set; }
        public virtual CampanhaOrcamento CampanhaOrcamento { get; set; }
        public virtual GeUnidades GeUnidades { get; set; }

    }
    public partial class GeUnidades
    {
        public int cod_unidade { get; set; }
        public string des_nome { get; set; }
    }


    public partial class CampanhaOrcamentoRegionalModel
    {
        public virtual CampanhaOrcamentoRegional CampanhaOrcamentoRegional { get; set; }
        public IEnumerable<CampanhaOrcamentoRegional> ListaRegional { get; set; }
        public int campanhaorcamentoid { get; set; }
        public decimal vlr_orcamento { get; set; }


    }
    public partial class CampanhaMarketing_Model
    {
        public virtual CampanhaMarketing CampanhaMarketing { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(4000, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string Obs { get; set; }
    }
    public partial class CampanhaMarketingReviewModel
    {
        public virtual CampanhaMarketing CampanhaMarketing { get; set; }
        public virtual DadosDoCrm DadosDoCrm { get; set; }
        public List<Note> ListaComentarios { get; set; }
        public List<CampanhaMarketingEstagios> ListaEstagio { get; set; }
        public List<CampanhaMarketingPgto> ListaPgto { get; set; }
        public decimal? vlr_faturado { get; set; }
        public decimal? per_atingido { get; set; }

        public decimal? vlr_total_pago { get; set; }

    }


    public partial class CampanhaMarketingPgtoViewModel
    {
        public virtual CampanhaMarketing CampanhaMarketing { get; set; }
        public virtual CampanhaMarketingPgto CampanhaMarketingPgto { get; set; }
        public List<CampanhaMarketingPgto> ListaPgto { get; set; }

    }
    public partial class CampanhaRegionalViewModel
    {
        public int campanharegionalId { get; set; }
        public int wsession { get; set; }
        public int cod_regional { get; set; }
        public string des_regional { get; set; }
        public int cod_diretoria { get; set; }
        public string des_diretoria { get; set; }
        public string des_segmento { get; set; }
        public decimal? vlr_orcado { get; set; }
        public decimal? vlr_gasto { get; set; }
        public decimal? per_uso { get; set; }
        public decimal? per_representatividade { get; set; }
        public decimal? vlr_total { get; set; }
        public string des_status { get; set; }
    }



    ///relatorios
    public class RelatoCampanha
    {
        public int campanhaid { get; set; }
        public string des_nome { get; set; }
        public int cod_pessoa { get; set; }
        public string des_pessoa { get; set; }
        public int segmentoid { get; set; }
        public string des_segmento { get; set; }
        public string des_acao { get; set; }
        public decimal? vlr_meta { get; set; }
        public decimal? vlr_contrato { get; set; }
        public decimal? vlr_pgto { get; set; }
        public decimal? vlr_faturamento { get; set; }
        public decimal? vlr_custo_medio { get; set; }
        public string descricao { get; set; }
        public int cod_regional { get; set; }
        public string des_representante { get; set; }
        public decimal? per_atingido { get; set; }
        public decimal? vlr_saldo { get; set; }





    }



    public class RelatoCampanhaGeral
    {
        public DateTime? dta_inclusao { get; set; }
        public int campanhaid { get; set; }
        public int cod_pessoa { get; set; }
        public string des_nome { get; set; }
        public string des_pessoa { get; set; }
        public string des_segmento { get; set; }
        public int cod_regional { get; set; }
        public string des_representante { get; set; }
        public string des_acao { get; set; }
        public decimal? vlr_meta { get; set; }
        public decimal? vlr_faturamento { get; set; }
        public decimal? per_atingido { get; set; }
        public string des_forma_pgto { get; set; }
        public decimal? vlr_prevista { get; set; }
        public decimal? vlr_pgto { get; set; }
        public DateTime? dta_ultimo_pgto { get; set; }
        public decimal? vlr_faturamento_total { get; set; }


    }






}
