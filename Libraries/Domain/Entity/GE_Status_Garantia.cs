using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class GE_Status_Garantia
    {
        //public GE_Status_Garantia()
        //{
        //    this.ind_finalizacao = "N";
        //}
        
            
        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Cod_Status { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(60, ErrorMessage = "Campo inválido, Mínimo: {2} : Máximo {1}", MinimumLength = 2)]
        public string des_nome { get; set; }
        public string ind_finalizacao { get; set; }
        




    }
}
