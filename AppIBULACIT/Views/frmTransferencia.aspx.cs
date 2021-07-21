using AppIBULACIT.Controllers;
using AppIBULACIT.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppIBULACIT.Views
{
    public partial class frmTransferencia : System.Web.UI.Page
    {
        IEnumerable<Transferencia> transferencias = new ObservableCollection<Transferencia>();
        TransferenciaManager transferenciaManager = new TransferenciaManager();

        MonedaManager monedaManager = new MonedaManager();
        IEnumerable<Moneda> monedas = new ObservableCollection<Moneda>();



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
                transferencias = await transferenciaManager.ObtenerTransferencias(Session["Token"].ToString());
                gvTransferencias.DataSource = transferencias.ToList();
                gvTransferencias.DataBind();
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
                lblStatus.Text = "Hubo un error al cargar la lista de transferencias.";
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

        protected async void gvTransferencias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvTransferencias.Rows[index];

                switch (e.CommandName)
                {
                    case "Modificar":
                        IngresarEstadistica("gvTransferencias_RowCommand modificar");
                        ddlEstadoMant.Enabled = true;
                        monedas = await monedaManager.ObtenerMonedas(Session["Token"].ToString());
                        ltrTituloMantenimiento.Text = "Modificar transferencia";
                        btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                        txtCodigoMant.Text = row.Cells[0].Text.Trim();
                        txtCuentaOrigen.Text = row.Cells[1].Text.Trim();
                        txtCuentaDestino.Text = row.Cells[2].Text.Trim();
                        txtFechaHora.Text = row.Cells[3].Text.Trim();
                        txtDescripcion.Text = row.Cells[4].Text.Trim();
                        txtMonto.Text = row.Cells[5].Text.Trim();
                        ddlEstadoMant.SelectedValue = row.Cells[6].Text.Trim().ToUpper();

                        btnAceptarMant.Visible = true;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                        break;
                    case "Eliminar":
                        IngresarEstadistica("gvTransferencias_RowCommand eliminar");
                        btnAceptarModal.Visible = true;
                        lblCodigoEliminar.Text = row.Cells[0].Text;
                        ltrModalMensaje.Text = "Esta seguro que desea eliminar la transferencia #" + lblCodigoEliminar.Text + "?";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function(){openModal(); } );", true);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                ErrorManager errorManager = new ErrorManager();
                Error error = new Error();
                error.CodigoUsuario = Convert.ToInt32(Session["CodigoUsuario"].ToString());
                error.FechaHora = DateTime.Now;
                error.Vista = this.ToString();
                error.Accion = "gvTransferencias_RowCommand()";
                error.Fuente = ex.Source;
                error.Numero = ex.HResult.ToString();
                error.Descripcion = ex.Message;

                lblStatus.Text = "Accion no identificada";
                lblStatus.Visible = true;
            }
        }



        protected async void btnNuevo_Click(object sender, EventArgs e)
        {
            try
            {
                IngresarEstadistica("btnNuevo_Click");

                monedas = await monedaManager.ObtenerMonedas(Session["Token"].ToString());
                ltrTituloMantenimiento.Text = "Nueva transferencia";
                btnAceptarMant.ControlStyle.CssClass = "btn btn-sucess";
                btnAceptarMant.Visible = true;
                ltrCodigoMant.Visible = true;
                txtCodigoMant.Visible = true;
                txtDescripcion.Visible = true;
                ltrDescripcion.Visible = true;
                ddlEstadoMant.Enabled = false;
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
        {
            try
            {
                IngresarEstadistica("btnAceptarModal_Click");
                string resultado = string.Empty;
                resultado = await transferenciaManager.Eliminar(lblCodigoEliminar.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(resultado))
                {
                    lblCodigoEliminar.Text = string.Empty;
                    ltrModalMensaje.Text = "Transferencia eliminada";

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
            }
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

                if (string.IsNullOrEmpty(txtCodigoMant.Text))//Insertar
                {
                    IngresarEstadistica("btnAceptarMant_Click ingresar");
                    Transferencia transferencia = new Transferencia()
                    {
                        CuentaOrigen = Convert.ToInt32(txtCuentaOrigen.Text),
                        CuentaDestino = Convert.ToInt32(txtCuentaDestino.Text),
                        FechaHora = DateTime.Parse(txtFechaHora.Text, new CultureInfo("de-DE")),
                        Descripcion = txtDescripcion.Text,
                        Monto = Convert.ToDecimal(txtMonto.Text),
                        Estado = ddlEstadoMant.SelectedValue
                    };

                    Transferencia transferenciaIngresada = await transferenciaManager.Ingresar(transferencia, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(transferenciaIngresada.Descripcion))
                    {
                        lblResultado.Text = "Transferencia ingresada con exito";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Nueva transferencia incluida", transferencia.Descripcion, "testrolandocerdas@gmail.com", Convert.ToInt32(Session["CodigoUsuario"].ToString()), "Transferencia");
                    }
                    else
                    {
                        lblResultado.Text = "Hubo un error al efectuar la operacion";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Maroon;

                    }
                }
                else//Modificar
                {
                    IngresarEstadistica("btnAceptarMant_Click modificar");
                    Transferencia transferencia = new Transferencia()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        CuentaOrigen = Convert.ToInt32(txtCuentaOrigen.Text),
                        CuentaDestino = Convert.ToInt32(txtCuentaDestino.Text),
                        FechaHora = DateTime.Parse(txtFechaHora.Text, new CultureInfo("de-DE")),
                        Descripcion = txtDescripcion.Text,
                        Monto = Convert.ToDecimal(txtMonto.Text),
                        Estado = ddlEstadoMant.SelectedValue
                    };


                    Transferencia transferenciaIntresada = await transferenciaManager.Actualizar(transferencia, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(transferenciaIntresada.Descripcion))
                    {
                        lblResultado.Text = "Transferencia actualizada con exito";
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

            }
        }

        protected void btnCancelarMant_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);
            IngresarEstadistica("btnCancelarMant_Click");
        }
    }
}