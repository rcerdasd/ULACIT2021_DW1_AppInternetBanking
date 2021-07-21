using AppIBULACIT.Controllers;
using AppIBULACIT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppIBULACIT
{
    public partial class SiteMaster : MasterPage
    {
        protected override void OnInit(EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.Cache.SetExpires(DateTime.MinValue);

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void lnkCerrarSesion_Click(object sender, EventArgs e)
        {
            /*
            SesionManager sesionManager = new SesionManager();
            Sesion sesion = new Sesion();
            sesion.CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString());
            sesion.FechaInicio = Convert.ToDateTime(Session["FechaInicio"].ToString());
            sesion.FechaExpiracion = DateTime.Now;
            sesion.Estado = Session["Estado"].ToString();
            sesion.CodigoSesion = Session.SessionID.ToString();

            Sesion sesionIngresada = await sesionManager.Ingresar(sesion, Session["Token"].ToString());
            */
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("~/Login.aspx");
        }

    }
}