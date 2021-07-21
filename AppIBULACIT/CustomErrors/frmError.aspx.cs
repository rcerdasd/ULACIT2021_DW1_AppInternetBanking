using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using AppIBULACIT.Controllers;
using AppIBULACIT.Models;
namespace AppIBULACIT.CustomErrors
{
    public partial class frmError : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            Exception err = Session["LastError"] as Exception;
            //Exception err = Server.GetLastError();
            if (err != null)
            {
                err = err.GetBaseException();
                lblError.Text = err.Message;
                Session["LastError"] = null;

                ErrorManager errorManager = new ErrorManager();
                Error errorAPI = new Error()
                {
                    CodigoUsuario = 0,
                    FechaHora = DateTime.Now,
                    Vista = "frmError.aspx",
                    Accion = "Page_Load",
                    Fuente = err.Source,
                    Numero = err.HResult.ToString(),
                    Descripcion = err.Message
                };

                Error errorIngresado = await errorManager.Ingresar(errorAPI);
            }
        }
    }
}