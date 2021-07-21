using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppIBULACIT.Models
{
    public class Transferencia
    {
        public int Codigo { get; set; }
        public int CuentaOrigen { get; set; }
        public int CuentaDestino { get; set; }
        public System.DateTime FechaHora { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }

        public virtual Cuenta Cuenta { get; set; }
    }
}