using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppIBULACIT.Models
{
    public class Servicio
    {
        public Servicio()
        {
            this.Pago = new HashSet<Pago>();
        }

        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }

        public virtual ICollection<Pago> Pago { get; set; }
    }
}