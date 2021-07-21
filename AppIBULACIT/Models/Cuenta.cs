using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppIBULACIT.Models
{
    public class Cuenta
    {
        public Cuenta()
        {
            this.Pago = new HashSet<Pago>();
            this.Transferencia = new HashSet<Transferencia>();
        }

        public int Codigo { get; set; }
        public int CodigoUsuario { get; set; }
        public int CodigoMoneda { get; set; }
        public string Descripcion { get; set; }
        public string IBAN { get; set; }
        public decimal Saldo { get; set; }
        public string Estado { get; set; }

        public virtual Moneda Moneda { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Pago> Pago { get; set; }
        public virtual ICollection<Transferencia> Transferencia { get; set; }
    }
}