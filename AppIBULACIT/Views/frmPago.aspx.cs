using AppIBULACIT.Controllers;
using AppIBULACIT.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppIBULACIT.Views
{
    public partial class frmPago : System.Web.UI.Page
    {

        IEnumerable<Pago> pagos = new ObservableCollection<Pago>();
        PagoManager pagoManager = new PagoManager();

        ServicioManager servicioManager = new ServicioManager();
        IEnumerable<Servicio> servicios = new ObservableCollection<Servicio>();

        CuentaManager cuentaManager = new CuentaManager();
        IEnumerable<Cuenta> cuentas = new ObservableCollection<Cuenta>();

        public string labelsGraficoVistasGlobal = string.Empty;
        public string dataGraficoVistasGlobal = string.Empty;
        public string backgroundcolorsGraficoVistasGlobal = string.Empty;
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CodigoUsuario"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                {

                    pagos = await pagoManager.ObtenerPagos(Session["Token"].ToString());
                    InicializarControles();
                    
                    ObtenerGrafico();
                }
            }
        }

        protected async void InicializarControles()
        {
            try
            {
                pagos = await pagoManager.ObtenerPagos(Session["Token"].ToString());
                ObtenerGrafico();
                gvPagos.DataSource = pagos.ToList();
                gvPagos.DataBind();

                cuentas = await cuentaManager.ObtenerCuentasUsuario(Session["Token"].ToString(), Session["CodigoUsuario"].ToString());
                ddlCuenta.DataSource = cuentas.ToList();
                ddlCuenta.DataTextField = "IBAN";
                ddlCuenta.DataValueField = "Codigo";               
                ddlCuenta.DataBind();

                servicios = await servicioManager.ObtenerServicios(Session["Token"].ToString());
                ddlCodigoServicio.DataSource = servicios.ToList();
                ddlCodigoServicio.DataTextField = "Descripcion";
                ddlCodigoServicio.DataValueField = "Codigo";
                ddlCodigoServicio.DataBind();
                

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
                lblStatus.Text = "Hubo un error al cargar la lista de pagos.";
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

                
                ltrTituloMantenimiento.Text = "Nuevo pago";
                btnAceptarMant.ControlStyle.CssClass = "btn btn-sucess";
                btnAceptarMant.Visible = true;
                ltrCodigoMant.Visible = true;
                txtCodigoMant.Visible = true;
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
        {
            try
            {
                IngresarEstadistica("btnAceptarModal_Click");
                string resultado = string.Empty;
                resultado = await pagoManager.Eliminar(lblCodigoEliminar.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(resultado))
                {
                    lblCodigoEliminar.Text = string.Empty;
                    ltrModalMensaje.Text = "Pago eliminada";

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
                ltrModalMensaje.Text = "No se pudo eliminar esta pago";
                lblResultado.ForeColor = Color.Maroon;
                lblResultado.Visible = true;
                btnAceptarModal.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function(){openModal(); } );", true);
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
                Cuenta cuentaSeleccionada = await cuentaManager.ObtenerCuenta(Session["Token"].ToString(), ddlCuenta.SelectedValue);

                if (string.IsNullOrEmpty(txtCodigoMant.Text))//Insertar
                {
                    IngresarEstadistica("btnAceptarMant_Click ingresar");
                    Pago pago = new Pago()
                    {
                        CodigoServicio = Convert.ToInt32(ddlCodigoServicio.SelectedValue),
                        CodigoCuenta = Convert.ToInt32(ddlCuenta.SelectedValue),
                        CodigoMoneda = cuentaSeleccionada.CodigoMoneda,
                        FechaHora = DateTime.Now,
                        Monto = Convert.ToDecimal(txtMonto.Text.Trim())
                    };

                    decimal montoEnCuenta = cuentaSeleccionada.Saldo;

                    if (Convert.ToDecimal(txtMonto.Text.Trim()) <= montoEnCuenta)//la cuenta seleccionada tiene suficientes fondos
                    {
                        Pago pagoIngresada = await pagoManager.Ingresar(pago, Session["Token"].ToString());

                        if (pagoIngresada.FechaHora != null)
                        {
                            cuentaSeleccionada.Saldo = (montoEnCuenta - Convert.ToDecimal(txtMonto.Text.Trim()));
                            Cuenta cuentaOrigenActualizada = await cuentaManager.Actualizar(cuentaSeleccionada, Session["Token"].ToString());
                            openModal("Pago procesado con exito", false);
                            InicializarControles();

                        }
                        else
                        {
                            openModal("No se pudo procesar el pago", false);

                        }
                    }
                    else
                    {
                        openModal("No hay suficiente saldo en la cuenta seleccionada para pagar el servicio", false);

                    }
                }
                /*else//Modificar
                {
                    IngresarEstadistica("btnAceptarMant_Click modificar");
                    Pago pago = new Pago()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                        CodigoMoneda = Convert.ToInt32(ddlCodigoMoneda.SelectedValue),
                        Descripcion = txtDescripcion.Text,
                        IBAN = txtIban.Text,
                        Saldo = Convert.ToDecimal(txtSaldo.Text),
                        Estado = ddlEstadoMant.SelectedValue
                    };


                    Pago pagoIntresada = await pagoManager.Actualizar(pago, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(pagoIntresada.Descripcion))
                    {
                        lblResultado.Text = "Pago actualizada con exito";
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
                openModal("No se pudo procesar el pago", false);
            }
        }

        protected void btnCancelarMant_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);
            IngresarEstadistica("btnCancelarMant_Click");
        }

        protected void ObtenerGrafico()
        {
            StringBuilder script = new StringBuilder();
            StringBuilder labelsGraficoVistas = new StringBuilder();
            StringBuilder dataGraficoVistas = new StringBuilder();
            StringBuilder backgroundColorGraficoVistas = new StringBuilder();

            var random = new Random();

            foreach(var pago in pagos.GroupBy(info => info.CodigoCuenta).Select(group => new
            {
                CuentaCodigo = group.Key,
                Cantidad = group.Count()
            }).OrderBy(x => x.CuentaCodigo))
            {
                string color = String.Format("#{0:x6}", random.Next(0x1000000));
                labelsGraficoVistas.Append(string.Format("'{0}',", pago.CuentaCodigo));
                dataGraficoVistas.Append(string.Format("'{0}',", pago.Cantidad));
                backgroundColorGraficoVistas.Append(string.Format("'{0}',", color));

                labelsGraficoVistasGlobal = labelsGraficoVistas.ToString().Substring(0, labelsGraficoVistas.Length - 1);
                backgroundcolorsGraficoVistasGlobal = backgroundColorGraficoVistas.ToString().Substring(0, backgroundColorGraficoVistas.Length - 1);
                dataGraficoVistasGlobal = dataGraficoVistas.ToString().Substring(0, dataGraficoVistas.Length - 1);
            }
        }

    }
}