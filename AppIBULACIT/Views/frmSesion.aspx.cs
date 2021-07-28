using AppIBULACIT.Controllers;
using AppIBULACIT.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppIBULACIT.Views
{
    public partial class frmSesion : System.Web.UI.Page
    {


        IEnumerable<Sesion> sesions = new ObservableCollection<Sesion>();
        SesionManager sesionManager = new SesionManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CodigoUsuario"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                {
                    InicializarControles();
                }
            }
        }

        protected async void InicializarControles()
        {
            try
            {
                sesions = await sesionManager.ObtenerSesiones(Session["Token"].ToString());
                gvSesions.DataSource = sesions.ToList();
                gvSesions.DataBind();
            }
            catch (Exception ex)
            {

                ErrorManager errorManager = new ErrorManager();
                Error error = new Error();
                error.CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString());
                error.FechaHora = DateTime.Now;
                error.Vista = this.ToString();
                error.Accion = "InicializarControles()";
                error.Fuente = ex.Source;
                error.Numero = ex.HResult.ToString();
                error.Descripcion = ex.Message;
                lblStatus.Text = "Hubo un error al cargar la lista de sesions.";
                lblStatus.Visible = true;
            }
        }

        private async void IngresarEstadistica(string accion)
        {
            try
            {
                EstadisticaManager estadisticaManager = new EstadisticaManager();
                Estadistica estadistica = new Estadistica();
                estadistica.CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString());
                estadistica.FechaHora = DateTime.Now;
                estadistica.Navegador = HttpContext.Current.Request.Browser.Browser;
                estadistica.PlataformaDispositivo = Environment.OSVersion.ToString();
                estadistica.FabricanteDispositivo = Environment.MachineName;
                estadistica.Vista = this.ToString();
                estadistica.Accion = accion;

                Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica, Session["Token"].ToString());
            }
            catch (Exception ex)
            {

                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario =
                    Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    FechaHora = DateTime.Now,
                    Vista = this.ToString(),
                    Accion = "Ingresar estadistica",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };
                Error errorIngresado = await errorManager.Ingresar(error);

                lblStatus.Text = "Hubo un error al ingresar la estadistica";
                lblStatus.Visible = true;
            }


        }

        protected void openModal(string message, bool btnAceptar)
        {
            btnAceptarModal.Visible = btnAceptar;
            ltrModalMensaje.Text = message;
            ltrModalMensaje.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function(){openModal(); } );", true);
        }

        protected async void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                IngresarEstadistica("btnNuevo_Click");

                txtCodigoUsuario.Text = Session["CodigoUsuario"].ToString();
                txtFechaInicio.Text = DateTime.Now.ToString();

                ltrTituloMantenimiento.Text = "Nueva sesion";
                btnAceptarMant.ControlStyle.CssClass = "btn btn-sucess";
                btnAceptarMant.Visible = true;
                ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario =
                    Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    FechaHora = DateTime.Now,
                    Vista = this.ToString(),
                    Accion = "btnNuevo_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };
                Error errorIngresado = await errorManager.Ingresar(error);

                openModal("No se pudo abrir el modal", false);
            }
        }

        protected async void btnAceptarModal_Click(object sender, EventArgs e)
        {/*
            try
            {
                IngresarEstadistica("btnAceptarModal_Click");
                string resultado = string.Empty;
                resultado = await sesionManager.Eliminar(lblCodigoEliminar.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(resultado))
                {
                    lblCodigoEliminar.Text = string.Empty;
                    ltrModalMensaje.Text = "Sesion eliminada";

                    btnAceptarModal.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModal(); } );", true);
                    InicializarControles();
                }
            }
            catch (Exception ex)
            {

                ErrorManager errorManager = new ErrorManager();
                Error error = new Error();

                error.CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString());
                error.FechaHora = DateTime.Now;
                error.Vista = this.ToString();
                error.Accion = "btnAceptarModal_Click";
                error.Fuente = ex.Source;
                error.Numero = ex.HResult.ToString();
                error.Descripcion = ex.Message;

                Error errorIngresado = await errorManager.Ingresar(error);
                ltrModalMensaje.Text = "No se pudo eliminar esta sesion";
                lblResultado.ForeColor = Color.Maroon;
                lblResultado.Visible = true;
                btnAceptarModal.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function(){openModal(); } );", true);
            }*/
        }

        protected void btnCancelarModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModal(); });", true);
            IngresarEstadistica("btnCancelarModal_Click");
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            try
            {
                IngresarEstadistica("btnAceptarMant_Click ingresar");
                if (Convert.ToDateTime(txtFechaExp.Text) > Convert.ToDateTime(txtFechaInicio.Text))
                {
                    Sesion sesion = new Sesion()
                    {
                        CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                        FechaInicio = Convert.ToDateTime(txtFechaInicio.Text),
                        FechaExpiracion = Convert.ToDateTime(txtFechaExp.Text),
                        Estado = ddlEstadoMant.SelectedValue,
                        CodigoSesion = Session.SessionID.ToString()
                    };

                    Sesion sesionIngresada = await sesionManager.Ingresar(sesion, Session["Token"].ToString());

                    if (sesionIngresada.Estado != null)
                    {
                        openModal("Sesion ingresada con exito", false);
                        InicializarControles();

                    }
                    else
                    {
                        openModal("No se pudo ingresar la sesion", false);

                    }
                }
                else
                {
                    openModal("La fecha de expiracion no puede ser anterior a la fecha de inicio", false);

                }


            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario =
                    Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    FechaHora = DateTime.Now,
                    Vista = this.ToString(),
                    Accion = "btnAceptarModal_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult.ToString(),
                    Descripcion = ex.Message
                };
                Error errorIngresado = await errorManager.Ingresar(error);
                openModal("No se pudo procesar el sesion", false);
            }
        }

        protected void btnCancelarMant_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);
            IngresarEstadistica("btnCancelarMant_Click");
        }
    }
}