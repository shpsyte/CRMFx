using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public partial class Cargos
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_cargo { get; set; }

        [Required(ErrorMessage="*")]
        [StringLength(60, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string des_cargo { get; set; }
    }
}
