using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class PSSacPSSacModel
    {
        public virtual PS_Sac SacOriginal { get; set; }
        public virtual PS_Sac SacNovo { get; set; }
        /// <summary>
        /// Relaioncmennto dos sac
        /// </summary>
        public virtual Ps_Sac_Ps_Sac Sacs { get; set; }

    }
}