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
    public partial class frmEstadistica : System.Web.UI.Page
    {

        IEnumerable<Estadistica> estadisticas = new ObservableCollection<Estadistica>();
        EstadisticaManager estadisticaManager = new EstadisticaManager();

        public string labelsGraficoVistasGlobal = string.Empty;
        public string dataGraficoVistasGlobal = string.Empty;
        public string backgroundcolorsGraficoVistasGlobal = string.Empty;
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

        protected void openModal(string message, bool btnAceptar)
        {
            btnAceptarModal.Visible = btnAceptar;
            ltrModalMensaje.Text = message;
            ltrModalMensaje.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function(){openModal(); } );", true);
        }

        protected async void InicializarControles()
        {
            try
            {
                estadisticas = await estadisticaManager.ObtenerEstadisticas(Session["Token"].ToString());
                ObtenerGrafico();
                gvEstadisticas.DataSource = estadisticas.ToList();
                gvEstadisticas.DataBind();
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
                lblStatus.Text = "Hubo un error al cargar la lista de estadisticas.";
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

                ltrTituloMantenimiento.Text = "Nueva estadistica";
                btnAceptarMant.ControlStyle.CssClass = "btn btn-sucess";
                btnAceptarMant.Visible = true;
                ltrCodigoMant.Visible = true;
                txtCodigoMant.Visible = true;
                ltrCodigoUsuario.Visible = true;
                txtCodigoUsuario.Text = Session["CodigoUsuario"].ToString();
                txtNavegador.Text = HttpContext.Current.Request.Browser.Browser;
                txtPlataforma.Text = Environment.OSVersion.ToString();
                txtFabricante.Text = Environment.MachineName;
                txtCodigoMant.Text = string.Empty;
                txtFechaHora.Text = DateTime.Now.ToString();
                txtAccion.Text = string.Empty;
                txtVista.Text = string.Empty;
                
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
                resultado = await estadisticaManager.Eliminar(lblCodigoEliminar.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(resultado))
                {
                    lblCodigoEliminar.Text = string.Empty;
                    ltrModalMensaje.Text = "Estadistica eliminada";

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
                ltrModalMensaje.Text = "No se pudo eliminar esta estadistica";
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

                if (string.IsNullOrEmpty(txtCodigoMant.Text))//Insertar
                {
                    IngresarEstadistica("btnAceptarMant_Click ingresar");
                    Estadistica estadistica = new Estadistica()
                    {
                        CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                        FechaHora = Convert.ToDateTime(txtFechaHora.Text),
                        Navegador = txtNavegador.Text,
                        PlataformaDispositivo = txtNavegador.Text,
                        FabricanteDispositivo = txtFabricante.Text,
                        Vista = txtVista.Text,
                        Accion = txtAccion.Text
                    };

                    Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(estadisticaIngresada.Navegador))
                    {
                        openModal("Estadistica ingresada", false);
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Nueva estadistica incluida", estadistica.Accion, "testrolandocerdas@gmail.com", Convert.ToInt32(Session["CodigoUsuario"].ToString()), "Estadistica");
                    }
                    else
                    {
                        openModal("No se pudo ingresar la estadistica", false);
                    }
                }
                /*else//Modificar
                {
                    IngresarEstadistica("btnAceptarMant_Click modificar");
                    Estadistica estadistica = new Estadistica()
                    {
                        Codigo = Convert.ToInt32(txtCodigoMant.Text),
                        CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                        CodigoMoneda = Convert.ToInt32(ddlCodigoMoneda.SelectedValue),
                        Descripcion = txtDescripcion.Text,
                        IBAN = txtIban.Text,
                        Saldo = Convert.ToDecimal(txtSaldo.Text),
                        Estado = ddlEstadoMant.SelectedValue
                    };


                    Estadistica estadisticaIntresada = await estadisticaManager.Actualizar(estadistica, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(estadisticaIntresada.Descripcion))
                    {
                        lblResultado.Text = "Estadistica actualizada con exito";
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

            foreach (var usuario in estadisticas.GroupBy(info => info.Accion).Select(group => new
            {
                Accion = group.Key,
                Cantidad = group.Count()
            }).OrderBy(x => x.Accion))
            {
                string color = String.Format("#{0:x6}", random.Next(0x1000000));
                labelsGraficoVistas.Append(string.Format("'{0}',", usuario.Accion));
                dataGraficoVistas.Append(string.Format("'{0}',", usuario.Cantidad));
                backgroundColorGraficoVistas.Append(string.Format("'{0}',", color));

                labelsGraficoVistasGlobal = labelsGraficoVistas.ToString().Substring(0, labelsGraficoVistas.Length - 1);
                backgroundcolorsGraficoVistasGlobal = backgroundColorGraficoVistas.ToString().Substring(0, backgroundColorGraficoVistas.Length - 1);
                dataGraficoVistasGlobal = dataGraficoVistas.ToString().Substring(0, dataGraficoVistas.Length - 1);
            }
        }
    }
}