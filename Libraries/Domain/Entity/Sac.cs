using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Domain.Entity
{


    public partial class Ps_Sac_Garantia_Model
    {
        public virtual Garantia Garantia { get; set; }
        public string Nome_Sac { get; set; }
        public int cod_sac { get; set; }
        public string des_assunto { get; set; }
        public string solicitacao { get; set; }
        public TimeSpan? _total_aberto { get; set; }
        public string tipo { get; set; }
        public string origem { get; set; }
        public int cod_cliente { get; set; }
        public int cod_representante { get; set; }
        public string NomeRepresentante { get; set; }

    }

    public partial class Ps_Sac_Procedimento_Model
    {
        public virtual ProcedimentoAdm ProcedimentoAdm { get; set; }
        public string Nome_Sac { get; set; }
        public int cod_sac { get; set; }
        public string des_assunto { get; set; }
        public string solicitacao { get; set; }
        public TimeSpan? _total_aberto { get; set; }
        public string tipo { get; set; }
        public string origem { get; set; }
    }

    public partial class Ps_Sac_Resposta_Model
    {
        public virtual PS_Sac PS_Sac { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(4000, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string Obs { get; set; }
        [Required(ErrorMessage = "*")]
        public string motivo { get; set; }


    }


    public partial class SacProcedimento
    {
        public int cod_sac { get; set; }
        public int cod_procedimento { get; set; }
    }


    public partial class SacGarantia
    {
        public int cod_sac { get; set; }
        public int garantiaId { get; set; }
    }


    public partial class Tp_Procedimento_Motivos
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int COD_TIPO { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MOTIVOID { get; set; }
        [Required(ErrorMessage = "Campo obrigatório")]
        public string DES_NOME { get; set; }

    }


    public partial class sac_troca_estagio
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

        public sac_troca_estagio()
        {
            this.dta_troca = System.DateTime.Now;
        }


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int num_seq { get; set; }

        public int cod_sac { get; set; }
        public Nullable<DateTime> dta_troca { get; set; }
        public int? cod_estagio_entrada { get; set; }
        public Nullable<DateTime> dta_entrada { get; set; }
        public Nullable<DateTime> dta_saida { get; set; }
        public int? cod_estagio_anterior { get; set; }
        public int? cod_usuario { get; set; }
        public string obs { get; set; }


        public virtual PS_Sac PS_Sac { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual PS_Estagio_Sac EstagioEntrada { get; set; }
        public virtual PS_Estagio_Sac EstagioAnterior { get; set; }

    }


    public partial class Ps_Sac_Reabertura
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public int cod_sac { get; set; }

        public DateTime dta_reabertura { get; set; }
        public int cod_usuario { get; set; }
        public string obs { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual PS_Sac PS_Sac { get; set; }


    }



    public partial class SacArquivo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int sacid { get; set; }
        
        public string caminho { get; set; }

        public byte[] des_imagem { get; set; }
        public string des_contenttype { get; set; }
        public string nome_arquivo { get; set; }

    }


    public partial class PS_Sac
    {


        public PS_Sac()
        {
            this.dta_abertura = System.DateTime.Now;
        }

        [NotMapped]
        public TimeSpan? _total_aberto
        {
            get
            {
                TimeSpan? timestamp = (this.dta_finalizacao.HasValue ? this.dta_finalizacao.Value : DateTime.Now) - (this.dta_abertura.HasValue ? this.dta_abertura.Value : DateTime.Now);
                return timestamp;
            }
        }




        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_sac { get; set; }
        public Nullable<DateTime> dta_abertura { get; set; }
        public Nullable<DateTime> dta_finalizacao { get; set; }
        [Required]
        public string des_assunto { get; set; }
        public string des_nome { get; set; }
        [Required]
        public string des_descricao { get; set; }
        public string des_solucao { get; set; }


        public string tp_pessoa { get; set; }
        public string cod_pessoa { get; set; }

        public int? cod_tipo { get; set; }
        public int? cod_origem { get; set; }


        public int? cod_classe { get; set; }

        public int? cod_categoria { get; set; }


        public int? cod_situacao { get; set; }
        public int? cod_estagio { get; set; }
        public int cod_usuario { get; set; }

        public string tag { get; set; }



        public virtual PS_Pessoas_Sac PS_Pessoas_Sac { get; set; }
        public virtual PS_Tipo_Sac PS_Tipo_Sac { get; set; }
        public virtual PS_Situacao_Sac PS_Situacao_Sac { get; set; }
        public virtual PS_Origem_Sac PS_Origem_Sac { get; set; }
        public virtual PS_Classe_Sac PS_Classe_Sac { get; set; }
        public virtual PS_Categorias_Sac PS_Categorias_Sac { get; set; }
        public virtual PS_Estagio_Sac PS_Estagio_Sac { get; set; }
        public virtual Usuario Usuario { get; set; }


    }



    public partial class Ps_Sac_Ps_Sac
    {


        public Ps_Sac_Ps_Sac()
        {
            this.dta_reabertura = System.DateTime.Now;
        }


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int rec { get; set; }
        public int cod_sac_original { get; set; }
        public int cod_sac_novo { get; set; }
        public Nullable<DateTime> dta_reabertura { get; set; }
        public string obs { get; set; }

        public int cod_usuario { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual PS_Sac SacOriginal { get; set; }
        public virtual PS_Sac SacNovo { get; set; }


    }


    public partial class PS_Pessoas_Sac
    {

        [NotMapped]
        public string FullName
        {
            get { return string.Concat(this.tipo.Substring(0, 1), ": ", this.des_pessoa); }
        }

        [NotMapped]
        public string FullNameWithID
        {
            get { return string.Concat(this.cod_pessoa, " ", this.des_pessoa, " ", this.tipo); }
        }

        public string tipo { get; set; }
        public string tipostring { get; set; }
        public string cod_pessoa { get; set; }
        public string des_pessoa { get; set; }
        public string des_fantasia { get; set; }
        public string des_empresa { get; set; }

        public string cgc_cpf { get; set; }
        public string des_email_cli { get; set; }
        public string tel_contato { get; set; }
        


    }


    public partial class PS_Estagio_Sac
    {
        public PS_Estagio_Sac()
        {
            this.ind_finalizacao = "N";
            //  this.ind_cancelado = "N";
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_estagio { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(60, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string des_nome { get; set; }
        public string ind_finalizacao { get; set; }
        //public string ind_cancelado { get; set; }
    }



    public partial class Ps_Estagio_Usuario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int idrow { get; set; }
        [Required]
        public int cd_usuario { get; set; }
        public int cod_estagio { get; set; }

        public virtual Usuario Usuario { get; set; }

    }

    public partial class PS_Estagio_Usuario_Model
    {
        public virtual Ps_Estagio_Usuario Ps_Estagio_Usuario { get; set; }
        public IEnumerable<Ps_Estagio_Usuario> ListaUsuario { get; set; }
        public int cod_estagio { get; set; }
        public string nome_estagio { get; set; }
    }



    public partial class PS_Categorias_Sac_Model
    {
        public virtual PS_Categorias_Sac PS_Categorias_Sac { get; set; }
        public IEnumerable<PS_Categorias_Sac> ListaCategorias { get; set; }
        public int cod_classe { get; set; }
        public string nome_classe { get; set; }
    }

    public class SelectListItemCat
    {


        public bool Selected { get; set; }
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public partial class PS_Categorias_Sac
    {


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_categoria { get; set; }
        public int cod_classe { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(60, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string des_nome { get; set; }
    }
    public partial class PS_Classe_Sac
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_classe { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(60, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string des_nome { get; set; }
    }
    public partial class PS_Origem_Sac
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_origem { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(60, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string des_nome { get; set; }
    }
    public partial class PS_Situacao_Sac
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_situacao { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(60, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string des_nome { get; set; }
    }
    public partial class PS_Tipo_Sac
    {
        //public PS_Tipo_Sac()
        //{
        //    this.tp_finalizacao = "F";
        //}
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_tipo { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(60, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string des_nome { get; set; }
        [Required(ErrorMessage = "*")]
        public string tp_finalizacao { get; set; }
    }
}
