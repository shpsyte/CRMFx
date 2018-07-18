using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class PSSacModel
    {
        public virtual PS_Sac Ps_Sac { get; set; }
        public int cod_original { get; set; }
        public int cod_novo { get; set; }
    }
}