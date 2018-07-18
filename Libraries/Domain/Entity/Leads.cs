using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Domain.Entity
{
    public partial class Ps_LeadsInteresse
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ps_leadsInteresseid { get; set; }

        public int cod_lead { get; set; }
        public int cod_interesse { get; set; }
    }


    public partial class LeadModel
    {
        public virtual Ps_Leads DadosLead { get; set; }
        public List<Note> ListaComentarios { get; set; }
    }
    public partial class Ps_Leads
    {

        public Ps_Leads()
        {
            dta_criado = System.DateTime.Now;
        }

        [NotMapped]
        public string FullName
        {
            get
            {
                return String.Concat(this.des_nome, " ", this.des_sobrenome);
            }
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_lead { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(60, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string des_nome { get; set; }
        public string des_sobrenome { get; set; }
        public int? cod_tipo { get; set; }
        [Required]
        public int cod_origem { get; set; }
        public string des_empresa { get; set; }
        public string des_fone { get; set; }
        public string des_email { get; set; }
        public string des_documento { get; set; }
        public int? cod_interesse { get; set; }
        [Required]
        public string des_interesse { get; set; }
        public string des_info { get; set; }
        public int? cod_situacao { get; set; }
        public Nullable<DateTime> dta_criado { get; set; }
        public virtual Ps_Interesse_Lead Ps_Interesse_Lead { get; set; }
        public virtual Ps_Origem_Lead Ps_Origem_Lead { get; set; }
        public virtual Ps_Tipo_Lead Ps_Tipo_Lead { get; set; }
        public virtual Ps_Situacao_Lead Ps_Situacao_Lead { get; set; }
    }
    public partial class Ps_Interesse_Lead
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_interesse { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(60, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string des_nome { get; set; }
    }
    public partial class Ps_Origem_Lead
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_origem { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(60, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string des_nome { get; set; }
    }
    /// <summary>
    /// Tipo do Lead, Consumidor, Empresa, Prospect
    /// </summary>
    public partial class Ps_Tipo_Lead
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_tipo { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(60, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string des_nome { get; set; }
    }
    /// <summary>
    /// Tipo do Lead, Consumidor, Empresa, Prospect
    /// </summary>
    public partial class Ps_Situacao_Lead
    {
        public Ps_Situacao_Lead()
        {
            this.cod_situacao = 1;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_situacao { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(60, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string des_nome { get; set; }
    }
}
