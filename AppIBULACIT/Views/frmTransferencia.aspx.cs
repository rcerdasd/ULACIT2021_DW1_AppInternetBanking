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

        IEnumerable<Cuenta> cuentas = new ObservableCollection<Cuenta>();
        CuentaManager cuentaManager = new CuentaManager();

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

                cuentas = await cuentaManager.ObtenerCuentasUsuario(Session["Token"].ToString(), Session["CodigoUsuario"].ToString());
                ddlCuentaOrigen.DataSource = cuentas.ToList();
                ddlCuentaOrigen.DataTextField = "IBAN";
                ddlCuentaOrigen.DataValueField = "Codigo";
                ddlCuentaOrigen.DataBind();
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
                txtCodigoMant.Text = string.Empty;
                txtCuentaDestino.Text = string.Empty;
                txtMonto.Text = string.Empty;
                ltrFechaHora.Visible = false;
                txtFechaHora.Visible = false;
                ltrTituloMantenimiento.Text = "Nueva transferencia";
                btnAceptarMant.ControlStyle.CssClass = "btn btn-sucess";
                btnAceptarMant.Visible = true;
                ltrCodigoMant.Visible = true;
                txtCodigoMant.Visible = true;
                txtDescripcion.Visible = true;
                ltrDescripcion.Visible = true;
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
                        CuentaOrigen = Convert.ToInt32(ddlCuentaOrigen.SelectedValue),
                        CuentaDestino = Convert.ToInt32(txtCuentaDestino.Text),
                        //FechaHora = DateTime.Parse(txtFechaHora.Text, new CultureInfo("de-DE")),
                        FechaHora = DateTime.Now,
                        Descripcion = txtDescripcion.Text,
                        Monto = Convert.ToDecimal(txtMonto.Text),
                        Estado = "1"
                    };

                    Cuenta cuentaSeleccionada = await cuentaManager.ObtenerCuenta(Session["Token"].ToString(), ddlCuentaOrigen.SelectedValue);

                    Cuenta cuentaDestino = await cuentaManager.ObtenerCuenta(Session["Token"].ToString(), txtCuentaDestino.Text.Trim());

                    decimal montoEnCuenta = cuentaSeleccionada.Saldo;

                    if (Convert.ToDecimal(txtMonto.Text.Trim()) <= montoEnCuenta)//la cuenta seleccionada tiene suficientes fondos
                    {
                        if (cuentaDestino.Descripcion != null)
                        {
                            Transferencia transferenciaIngresada = await transferenciaManager.Ingresar(transferencia, Session["Token"].ToString());

                            if (!string.IsNullOrEmpty(transferenciaIngresada.Descripcion))
                            {
                                cuentaSeleccionada.Saldo = (montoEnCuenta - Convert.ToDecimal(txtMonto.Text.Trim()));
                                cuentaDestino.Saldo = (cuentaDestino.Saldo + Convert.ToDecimal(txtMonto.Text.Trim()));

                                Cuenta cuentaOrigenActualizada = await cuentaManager.Actualizar(cuentaSeleccionada, Session["Token"].ToString());
                                Cuenta cuentaDestinoActualizada = await cuentaManager.Actualizar(cuentaDestino, Session["Token"].ToString());
                                openModal("Transferencia realizada con exito", false);
                                InicializarControles();

                                Correo correo = new Correo();
                                correo.Enviar("Nueva transferencia incluida", transferencia.Descripcion, "testrolandocerdas@gmail.com", Convert.ToInt32(Session["CodigoUsuario"].ToString()), "Transferencia");
                            }
                            else
                            {
                                openModal("Error al realizar la transferencia", false);
                            }
                        }
                        else
                        {
                            openModal("La cuenta de destino no existe", false);
                        }
                    }
                    else//La cuenta no tiene sufientes fondos
                    {
                        openModal("Error al realizar la transferencia: La cuenta no tiene suficientes fondos", false);
                    }


                }
                else//Modificar
                {
                    IngresarEstadistica("btnAceptarMant_Click modificar");
                    Transferencia transferencia = new Transferencia()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        CuentaOrigen = Convert.ToInt32(ddlCuentaOrigen.Text),
                        CuentaDestino = Convert.ToInt32(txtCuentaDestino.Text),
                        //FechaHora = DateTime.Parse(txtFechaHora.Text, new CultureInfo("de-DE")),
                        FechaHora = DateTime.Parse(txtFechaHora.Text),
                        Descripcion = txtDescripcion.Text,
                        Monto = Convert.ToDecimal(txtMonto.Text),
                        Estado = "1"
                    };


                    Transferencia transferenciaIntresada = await transferenciaManager.Actualizar(transferencia, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(transferenciaIntresada.Descripcion))
                    {
                        openModal("Transferencia actualizada con exito", false);
                        InicializarControles();
                    }
                    else
                    {
                        openModal("Error al modificar la transferencia", false);

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