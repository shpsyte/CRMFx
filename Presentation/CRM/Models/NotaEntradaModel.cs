using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace CRM.Models
{
    public class NotaEntradaModel
    {
      

        public int id { get; set; }
        public NotaCliente NotaCliente { get; set; }
        public IEnumerable<NotaItemCliente> ItensNota { get; set; }

        public int? num_nota { get; set; }
        public string dta_emissao { get; set; }
        [Required]
        public string cod_serie { get; set; }
        public string dta_entrada { get; set; }
        [Required]
        public string cod_operacao { get; set; }
        public int? cod_transportador { get; set; }

        public int? cod_condicao_pgto { get; set; }
        public bool ind_nfe { get; set; }

        [Required]
        [StringLength(44, MinimumLength = 44) ]
        public string des_chave { get; set; }
        //[Required]
        public string des_modelo { get; set; }
        [Required]
        public string des_local { get; set; }
      //  [Required]
        public string des_lote { get; set; }

       // [Required]
        public tp_operacao tp_operacao { get; set; }

        public string obs { get; set; }
        public string msg { get; set; }

        



    }
}