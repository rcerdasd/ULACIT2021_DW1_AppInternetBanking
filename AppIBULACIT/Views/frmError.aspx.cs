using AppIBULACIT.Controllers;
using AppIBULACIT.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppIBULACIT.Views
{
    public partial class frmError : System.Web.UI.Page
    {


        IEnumerable<Error> errors = new ObservableCollection<Error>();
        ErrorManager errorManager = new ErrorManager();




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
                errors = await errorManager.ObtenerErrores(Session["Token"].ToString());
                gvErrors.DataSource = errors.ToList();
                gvErrors.DataBind();
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
                lblStatus.Text = "Hubo un error al cargar la lista de errors.";
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

        

        protected async void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                IngresarEstadistica("btnNuevo_Click");

                ltrTituloMantenimiento.Text = "Nueva error";
                btnAceptarMant.ControlStyle.CssClass = "btn btn-sucess";
                btnAceptarMant.Visible = true;
                ltrCodigoMant.Visible = true;
                txtCodigoMant.Visible = true;
                txtDescripcion.Visible = true;
                ltrCodigoUsuario.Visible = true;
                txtCodigoUsuario.Text = Session["CodigoUsuario"].ToString();
                txtCodigoMant.Text = string.Empty;
                txtDescripcion.Text = string.Empty;
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

                lblStatus.Text = "Hubo un error al cargar el modal";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarModal_Click(object sender, EventArgs e)
        {/*
            try
            {
                IngresarEstadistica("btnAceptarModal_Click");
                string resultado = string.Empty;
                resultado = await errorManager.Eliminar(lblCodigoEliminar.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(resultado))
                {
                    lblCodigoEliminar.Text = string.Empty;
                    ltrModalMensaje.Text = "Error eliminada";

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
                ltrModalMensaje.Text = "No se pudo eliminar esta error";
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

        protected void openModal(string message, bool btnAceptar)
        {
            btnAceptarModal.Visible = btnAceptar;
            ltrModalMensaje.Text = message;
            ltrModalMensaje.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function(){openModal(); } );", true);
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(txtCodigoMant.Text))//Insertar
                {
                    IngresarEstadistica("btnAceptarMant_Click ingresar");
                    Error error = new Error()
                    {
                        CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                        FechaHora = Convert.ToDateTime(txtFechaHora.Text),
                        Fuente = txtFuente.Text,
                        Numero = txtNumero.Text,
                        Descripcion = txtDescripcion.Text,
                        Vista = txtVista.Text,
                        Accion = txtAccion.Text
                    };

                    Error errorIngresada = await errorManager.Ingresar(error);

                    if (!string.IsNullOrEmpty(errorIngresada.Descripcion))
                    {
                        openModal("Error ingresado", false);
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Nueva error incluida", error.Descripcion, "testrolandocerdas@gmail.com", Convert.ToInt32(Session["CodigoUsuario"].ToString()), "Error");
                    }
                    else
                    {                        
                        openModal("Error no ingresado", false);
                    }
                }
                /*else//Modificar
                {
                    IngresarEstadistica("btnAceptarMant_Click modificar");
                    Error error = new Error()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                        CodigoMoneda = Convert.ToInt32(ddlCodigoMoneda.SelectedValue),
                        Descripcion = txtDescripcion.Text,
                        IBAN = txtIban.Text,
                        Saldo = Convert.ToDecimal(txtSaldo.Text),
                        Estado = ddlEstadoMant.SelectedValue
                    };


                    Error errorIntresada = await errorManager.Actualizar(error, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(errorIntresada.Descripcion))
                    {
                        lblResultado.Text = "Error actualizada con exito";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();
                    }
                    else
                    {
                        lblResultado.Text = "Hubo un error al efectuar la operacion";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Maroon;
                    }
                }*/
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
                openModal("Error no ingresado", false);
            }
        }

        protected void btnCancelarMant_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);
            IngresarEstadistica("btnCancelarMant_Click");
        }
    }
}