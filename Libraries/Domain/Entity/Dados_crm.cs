using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entity
{



    //SELECT * FROM sp_table_class where table_name = 'DADOS_CRM_VIEW'
    //SELECT * FROM sp_table_class_map where table_name = 'DADOS_CRM_VIEW'





    public class V_Crm
    {
        public string cod_pessoa { get; set; }
        public string des_pessoa { get; set; }
        public string des_represetante { get; set; }
        public string tel_contato { get; set; }
        public string des_email_cli { get; set; }
        public DateTime? dta_ult_compra { get; set; }
        public System.Nullable<int> daysNotSale { get; set; }
        public decimal vlr_faturamento { get; set; }

    }
    public partial class DadosDoCrm
    {
        [NotMapped]
        public Nullable<int> _daysNotSale
        {
            get
            {
                return this.dta_ult_compra.HasValue ? ((TimeSpan)(System.DateTime.Now - this.dta_ult_compra)).Days : 99999; 
            }
        }

        [NotMapped]
        public string FullName { get
            {
            return String.Concat( (this.des_fantasia  == null ? this.des_pessoa : this.des_fantasia), " - ", this.cod_pessoa); 
            }} 
        public string cod_grupo { get; set; }
        public string des_grupo { get; set; }
        public string cod_pessoa { get; set; }
        public string des_pessoa { get; set; }
        public string des_fantasia { get; set; }
        public decimal? cod_diretoria { get; set; }
        public string des_diretoria { get; set; }
        public int? cod_representante_cli { get; set; }
        public string des_represetante { get; set; }
        public decimal? cod_gerente { get; set; }
        public string des_gerente { get; set; }
        public string cep { get; set; }
        public string bairro { get; set; }
        public decimal? cod_cidade { get; set; }
        public string des_cidade { get; set; }
        public string uf { get; set; }
        public string des_estado { get; set; }
        public string regiao_geografica { get; set; }
        public string atividade { get; set; }
        public string regiao_cliente { get; set; }
        public string empresa_coligada { get; set; }
        public string segmento { get; set; }
        public string cgc_cpf { get; set; }
        public string tel_contato { get; set; }
        public string des_endereco { get; set; }
        public decimal? lim_credito { get; set; }
        public string ie { get; set; }
        public string ind_consumidor { get; set; }
        public decimal? cod_oper { get; set; }
        public Nullable<DateTime> dta_ult_compra { get; set; }
        public decimal? vlr_maior_compra { get; set; }
        public decimal? vlr_fat_ant { get; set; }
        public decimal? per_rent_ant { get; set; }
        public decimal? vlr_faturamento { get; set; }
        public decimal? per_rentabilidade { get; set; }
        public decimal? vlr_dev_ant { get; set; }
        public decimal? vlr_devolucao { get; set; }
        public string des_email_cli { get; set; }
        public string des_email_rep { get; set; }
        public decimal? vlr_investimento { get; set; }
        public decimal? vlr_bonus { get; set; }
        public Nullable<DateTime> dta_ult_alteracao { get; set; }
        public Nullable<DateTime> dta_cadastro { get; set; }
    }
    public partial class ListaPedidos
    {
        public int cod_cliente { get; set; }
        public int num_pedido { get; set; }
        public int cod_compl { get; set; }
        public DateTime dta_emissao { get; set; }
        public string des_situacao { get; set; }
        public decimal? valorpedido { get; set; }
    }
    public partial class ListaNotas
    {
        public int num_nota { get; set; }
        public int cod_cliente { get; set; }
        public int cod_oper { get; set; }
        public string des_oper { get; set; }
        public DateTime dta_emissao { get; set; }
        public decimal vlr_total_liquido { get; set; }
        public decimal vlr_total_bruto { get; set; }
    }

    public partial class ListaOcorrencia
    {
        public int cod_sac { get; set; }
        public string cod_pessoa { get; set; }
        public DateTime dta_abertura { get; set; }
        public string status { get; set; }

    }
    public partial class ListaTitulos
    {
        public int cod_pessoa { get; set; }
        public int num_titulo { get; set; }
        public int num_parcela { get; set; }
        public DateTime dta_emissao { get; set; }
        public DateTime dta_vencimento { get; set; }
        public int diasatraso { get; set; }
        public string cod_compl { get; set; }
        public decimal valortitulo { get; set; }
    }
    public class ListaClientesDaColigada
    {
        public string cod_pessoa { get; set; }
        public string des_pessoa { get; set; }
        public decimal vlr_faturamento { get; set; }
    }

    public partial class Note
    {
        [NotMapped]
        public int MinutesPost
        {
            get
            { 
              return ((TimeSpan)(System.DateTime.Now - this.dta_inclusao)).Minutes; 
            }
        }
        public string cod_interno { get; set; }
        public string tipo_nota { get; set; }
        public int cod_nota { get; set; }
        public int cod_usuario { get; set; }
        public DateTime dta_inclusao { get; set; }
        public string msg { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
    public partial class DadosDoCrmModel
    {
        public virtual DadosDoCrm DadosDoCrm { get; set; }
        public virtual Ps_Perfil_Crm PerfilCrm { get; set; }
        public List<Note> ListaComentarios { get; set; }
        public List<ListaPedidos> Listapedidos { get; set; }
        public List<ListaNotas> Listanotafisccais { get; set; }
        public List<ListaTitulos> ListaTitulos { get; set; }
        public List<ListaOcorrencia> ListaOcorrencia { get; set; }
        public List<ListaClientesDaColigada> ListaClientesDaColigada { get; set; }
        public List<Contatos> ListaContato { get; set; }
        public bool IsColigada { get; set; }
        public string NomeUsuario { get; set; }
    }
    public partial class Ps_Perfil_Crm
    {
        public string cod_pessoa { get; set; }
        public Nullable<DateTime> dta_fundacao { get; set; }
        public string des_evento { get; set; }
        public string des_clube { get; set; }
        public string des_acao { get; set; }
        public string des_outras { get; set; }
    }
    public class GetItensDoc
    {
        public string cod_item { get; set; }
        public string des_item { get; set; }
        public decimal qtd_negociada { get; set; }
        public decimal vlr_uni_bruto { get; set; }
        public decimal vlr_total { get; set; }
    }
}
