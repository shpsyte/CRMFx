using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Functions;

namespace Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using IntlTexto;
    using IntlTexto.Intl;


    public partial class ProcedimentoAdmArq
    {

        
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_ARQ { get; set; }

       
        [Required(ErrorMessageResourceType = typeof(strings), ErrorMessageResourceName = "CampoRequerido")]
        [LocalizedDisplayName("CD_PROCEDIMENTO")]
        public int CD_PROCEDIMENTO { get; set; }

       
        [LocalizedDisplayName("CAMINHO")]
        public string CAMINHO { get; set; }

        public byte[] DES_IMAGEM { get; set; }
        public string DES_CONTENTTYPE { get; set; }
        public string NOME_ARQUIVO { get; set; }






    }
}
