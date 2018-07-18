using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public partial class W_TopProcedimentoByDepto
    {
        public int cd_departamento { get; set; }
        public string departamento { get; set; }
        public decimal maiorproc { get; set; }
        public decimal rk { get; set; }

    }

    public partial class W_QTDE_SAC_DIA
    {
        public int dia { get; set; }
        public int qtde { get; set; }
    }


    
    public partial class w_sac_por_tipo
    {
        public string Tipo_Ocorrencia { get; set; }
        public int Qtde { get; set; }
        public int Total { get; set; }
        public decimal Percentual { get; set; }
    }
}
