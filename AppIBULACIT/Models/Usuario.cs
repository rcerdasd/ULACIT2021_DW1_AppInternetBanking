using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppIBULACIT.Models
{
    public class Usuario
    {
        public Usuario()
        {
            this.Cuenta = new HashSet<Cuenta>();
            this.Error = new HashSet<Error>();
            this.Estadistica = new HashSet<Estadistica>();
            this.Sesion = new HashSet<Sesion>();
        }

        public int Codigo { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public System.DateTime FechaNacimiento { get; set; }
        public string Estado { get; set; }
        public string Token { get; set; }

        public virtual ICollection<Cuenta> Cuenta { get; set; }
        public virtual ICollection<Error> Error { get; set; }
        public virtual ICollection<Estadistica> Estadistica { get; set; }
        public virtual ICollection<Sesion> Sesion { get; set; }
    }
}