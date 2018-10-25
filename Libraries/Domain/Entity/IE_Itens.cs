using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class IE_Itens
    {
        public int cod_item { get; set; }
        public string cod_foxlux { get; set; }
        public string des_item { get; set; }
        public string ncm { get; set; }
    }

    public class IE_Itens_Vendas
    {
        public int num_seq { get; set; }
        public int cod_cliente { get; set; }
        public string des_pessoa { get; set; }
        public int cod_item { get; set; }
        public string cod_foxlux { get; set; }
        public string des_item { get; set; }
        public DateTime dta_lancamento { get; set; }
        public decimal dias { get; set; }
        public decimal qtd_lancamento { get; set; }
        public string num_documento { get; set; }
        public decimal vlr_total { get; set; }
        public decimal vlr_unitario { get; set; }
        public decimal vlr_ipi { get; set; }
        public decimal vlr_icms_subs { get; set; }

        public virtual Ps_Pessoas Ps_Pessoas { get; set; }

       
    }



    public class V_IE_Itens_Vendas
    {
        public int num_seq { get; set; }
        public int cod_cliente { get; set; }
        public string des_pessoa { get; set; }
        public int cod_item { get; set; }
        public string cod_foxlux { get; set; }
        public string des_item { get; set; }
        public DateTime dta_lancamento { get; set; }
        public decimal dias { get; set; }
        public decimal qtd_lancamento { get; set; }
        public string num_documento { get; set; }
        public decimal vlr_total { get; set; }
        public decimal vlr_unitario { get; set; }
        public decimal vlr_ipi { get; set; }
        public decimal vlr_icms_subs { get; set; }
        public decimal vlr_icms { get; set; }
        public decimal vlr_base_subs { get; set; }
        public decimal picms { get; set; }
        public decimal pipi { get; set; }
        public decimal picmsst { get; set; }
        public decimal mvast { get; set; }

        public int? cod_unidade { get; set; }



    }

}
