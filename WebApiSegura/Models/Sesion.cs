//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApiSegura.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sesion
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
