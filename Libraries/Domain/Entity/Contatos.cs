using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public partial class Contatos
    {
        public Contatos()
        {
            this.cod_cargo = 0;
        }



        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        //[Required(ErrorMessage = "*")]
        //[StringLength(200, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        //[DataType(DataType.EmailAddress, ErrorMessage = "*")]

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_contato { get; set; }

        public string des_nome { get; set; }
        public string des_email { get; set; }
        public string des_fone { get; set; }
        public string des_celular { get; set; }
        public Nullable<int> cod_cargo { get; set; }

        [Required(ErrorMessage = "*")]
        public string cod_conta { get; set; }

        public Nullable<DateTime> dta_nascimento { get; set; }
        public string des_filhos { get; set; }
        public decimal cod_estado_civil { get; set; }
        public string des_time { get; set; }
        public string des_hobby { get; set; }
        public string des_outras { get; set; }

        public virtual DadosDoCrm DadosDoCrm { get; set; }
        public virtual Cargos Cargos { get; set; }

    }
}
