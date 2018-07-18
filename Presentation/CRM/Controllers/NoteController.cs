using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    [CustomAuthorize(AccessLevel = "Admin;User")]
    public class NoteController : BasePublicController
    {
        //
        // GET: /Note/

        [HttpPost]
        public JsonResult PostComentario(string id, string msg, string origem)
        {
            Int32? NextSeq = db.ListaComentarios.Max(s => (Int32?)s.cod_nota);
            if (NextSeq != null)
            {
                NextSeq++;
            }
            else
            {
                NextSeq = 1;
            }

            if ("SAC".Equals(origem))
            {
                _email.EnviarEmailSac(Convert.ToInt32(id), "BasicSacInformation.htm");
            }

            string sql = string.Format(" INSERT INTO NOTE VALUES (\'{0}\',\'{1}\',{2},{3},{4},\'{5}\')",
                   id.Trim(),
                   origem,
                   NextSeq,
                   cookie.Values["cd_usuario"],
                   "SYSDATE",
                    msg.Replace("\'", "").Replace("\'\'", "")
                   );
            db.Database.ExecuteSqlCommand(sql);


            List<Object> Adicionado = new List<Object>();
            Adicionado.Add(new
            {
                DataPequena = System.DateTime.Now.ToShortDateString(),
                Horapequena = System.DateTime.Now.ToShortTimeString(),
                Mensagem = msg,
                User = cookie.Values["usuario"]
            });

            //for (int i = 0; i < 10; i++)
            //{
            //    listaUsuarios.Add(new { User = "José Luiz " + i.ToString("000"), Msg = msg });
            //}

            return Json(Adicionado);
        }


    }
}
