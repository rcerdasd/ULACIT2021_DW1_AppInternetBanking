using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppIBULACIT.Models
{
    public class Error
    {
        public int Codigo { get; set; }
        public int CodigoUsuario { get; set; }
        public System.DateTime FechaHora { get; set; }
        public string Fuente { get; set; }
        public string Numero { get; set; }
        public string Descripcion { get; set; }
        public string Vista { get; set; }
        public string Accion { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}