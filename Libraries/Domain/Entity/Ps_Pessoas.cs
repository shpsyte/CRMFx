using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity
{
    public class Ps_Pessoas
    {
        public int cod_pessoa { get; set; }
        public string des_pessoa { get; set; }
        public string des_fantasia { get; set; }
        public string des_email { get; set; }

        public string des_endereco { get; set; }
        public string num_cep { get; set; }
        public string des_bairro { get; set; }
        public int cod_cidade { get; set; }
        public Nullable<DateTime> dta_cadastro { get; set; }

        
        public virtual G1_Cidades Cidade { get; set; }
        
    }

    public class G1_Cidades
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cod_cidade { get; set; }
        public string cod_uf { get; set; }
        public string des_cidade { get; set; }
    }


    public class Ps_Representantes
    {
        public int cod_pessoa { get; set; }
        public string des_pessoa { get; set; }
        public string des_fantasia { get; set; }


        public string des_endereco { get; set; }
        public string des_cidade { get; set; }
        public string uf { get; set; }
        public string bairro { get; set; }

        public int ind_inativo { get; set; }
    }




}
