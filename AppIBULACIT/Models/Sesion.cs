using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppIBULACIT.Models
{
    public class Sesion
    {
        public int Codigo { get; set; }
        public int CodigoUsuario { get; set; }
        public System.DateTime FechaInicio { get; set; }
        public System.DateTime FechaExpiracion { get; set; }
        public string Estado { get; set; }

        public string CodigoSesion { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}