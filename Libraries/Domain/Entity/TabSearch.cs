using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{

    public partial class vw_Ps_Leads
    {
        public string cod_lead { get; set; }
        public string des_nome { get; set; }
        public string des_email { get; set; }
        public string des_fone { get; set; }
        public string des_documento { get; set; }
    }




    public class vw_Contatos
    {
        public string cod_contato { get; set; }
        public string cod_empresa { get; set; }
        public string des_empresa { get; set; }
        public string cnpj { get; set; }
        public string des_nome { get; set; }
        public string des_email { get; set; }
        public string des_fone { get; set; }
    }

    public class w_garantia
    {
        public string garantiaid     { get; set; }
        public string cod_cliente { get; set; }
        public string des_pessoa { get; set; }
        public string num_nf_cliente { get; set; }
        public string obs { get; set; }
        public string cod_representante { get; set; }
    }



    public partial class vw_Sac
    {
        public string cod_sac { get; set; }
        public string des_assunto { get; set; }
        public string cod_pessoa { get; set; }
        public string des_pessoa { get; set; }
        public string des_estagio { get; set; }
    }

    public partial class vw_pessoas
    {
        public string cod_pessoa { get; set; }
        public string des_pessoa { get; set; }
        public string des_email { get; set; }
    }
    

    public partial class SearchModel
    {
        public string param { get; set; }

        public IEnumerable<vw_Ps_Leads> Leads { get; set; }
        public IEnumerable<vw_Contatos> Contatos { get; set; }
        public IEnumerable<vw_Sac> Sac { get; set; }
        public IEnumerable<w_garantia> Garantia { get; set; }
        public IEnumerable<vw_pessoas> Pessoas { get; set; }


    }

}
