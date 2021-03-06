using AppIBULACIT.Controllers;
using AppIBULACIT.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppIBULACIT.Views
{
    public partial class frmCuenta : System.Web.UI.Page
    {
        IEnumerable<Cuenta> cuentas = new ObservableCollection<Cuenta>();
        CuentaManager cuentaManager = new CuentaManager();

        MonedaManager monedaManager = new MonedaManager();
        IEnumerable<Moneda> monedas = new ObservableCollection<Moneda>();


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

        protected void ObtenerGrafico()
        {
            StringBuilder script = new StringBuilder();
            StringBuilder labelsGraficoVistas = new StringBuilder();
            StringBuilder dataGraficoVistas = new StringBuilder();
            StringBuilder backgroundColorGraficoVistas = new StringBuilder();

            var random = new Random();

            foreach (var cuenta in cuentas.GroupBy(info => info.CodigoMoneda).Select(group => new
            {
                Moneda = group.Key,
                Cantidad = group.Count()
            }).OrderBy(x => x.Moneda))
            {
                string color = String.Format("#{0:x6}", random.Next(0x1000000));
                labelsGraficoVistas.Append(string.Format("'{0}',", cuenta.Moneda));
                dataGraficoVistas.Append(string.Format("'{0}',", cuenta.Cantidad));
                backgroundColorGraficoVistas.Append(string.Format("'{0}',", color));

                labelsGraficoVistasGlobal = labelsGraficoVistas.ToString().Substring(0, labelsGraficoVistas.Length - 1);
                backgroundcolorsGraficoVistasGlobal = backgroundColorGraficoVistas.ToString().Substring(0, backgroundColorGraficoVistas.Length - 1);
                dataGraficoVistasGlobal = dataGraficoVistas.ToString().Substring(0, dataGraficoVistas.Length - 1);
            }
        }

        protected async void InicializarControles()
        {
            try
            {
                cuentas = await cuentaManager.ObtenerCuentasUsuario(Session["Token"].ToString(),Session["CodigoUsuario"].ToString());
                ObtenerGrafico();
                gvCuentas.DataSource = cuentas.ToList();
                gvCuentas.DataBind();
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
                lblStatus.Text = "Hubo un error al cargar la lista de cuentas.";
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


        protected async void gvCuentas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvCuentas.Rows[index];

                switch (e.CommandName)
                {
                    case "Modificar":
                        IngresarEstadistica("gvCuentas_RowCommand modificar");

                        ddlEstadoMant.Enabled = true;
                        monedas = await monedaManager.ObtenerMonedas(Session["Token"].ToString());

                        

                        ddlCodigoMoneda.DataSource = monedas.ToList();
                        ddlCodigoMoneda.DataTextField = "Descripcion";
                        ddlCodigoMoneda.DataValueField = "Codigo";

                        ddlCodigoMoneda.DataBind();

                        ddlCodigoMoneda.SelectedValue = row.Cells[2].Text.Trim();

                        ltrTituloMantenimiento.Text = "Modificar cuenta";
                        btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                        txtCodigoMant.Text = row.Cells[0].Text.Trim();
                        txtCodigoUsuario.Text = row.Cells[1].Text.Trim();
                        txtDescripcion.Text = row.Cells[3].Text.Trim();
                        txtIban.Text = row.Cells[4].Text.Trim();
                        txtSaldo.Text = row.Cells[5].Text.Trim();
                        if (row.Cells[6].Text.Trim().ToLower() == "a")
                            ddlEstadoMant.SelectedIndex = 0;
                        else
                            ddlEstadoMant.SelectedIndex = 1;
                        btnAceptarMant.Visible = true;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                        break;
                    case "Eliminar":
                        IngresarEstadistica("gvCuentas_RowCommand eliminar");
                        btnAceptarModal.Visible = true;
                        lblCodigoEliminar.Text = row.Cells[0].Text;
                        ltrModalMensaje.Text = "Esta seguro que desea eliminar la cuenta #" + lblCodigoEliminar.Text + "?";
                        btnAceptarModal.Visible = true;
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
                error.Accion = "gvCuentas_RowCommand()";
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

                var monedasActivas = (from m in monedas
                                      where m.Estado.ToUpper() != "I"
                                      select m).ToList();

                ddlCodigoMoneda.DataSource = monedas.ToList();
                ddlCodigoMoneda.DataTextField = "Descripcion";
                ddlCodigoMoneda.DataValueField = "Codigo";
                ddlCodigoMoneda.DataBind();
                
                ltrTituloMantenimiento.Text = "Nueva cuenta";
                btnAceptarMant.ControlStyle.CssClass = "btn btn-sucess";
                btnAceptarMant.Visible = true;
                ltrCodigoMant.Visible = true;
                txtCodigoMant.Visible = true;
                txtDescripcion.Visible = true;
                ltrDescripcion.Visible = true;
                ddlEstadoMant.Enabled = false;
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
        {
            try
            {
                IngresarEstadistica("btnAceptarModal_Click");
                string resultado = string.Empty;
                resultado = await cuentaManager.Eliminar(lblCodigoEliminar.Text, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(resultado))
                {
                    lblCodigoEliminar.Text = string.Empty;
                    ltrModalMensaje.Text = "Cuenta eliminada";
                    
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
                ltrModalMensaje.Text = "No se pudo eliminar esta cuenta";
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
                
                if (string.IsNullOrEmpty(txtCodigoMant.Text))//Insertar
                {
                    IngresarEstadistica("btnAceptarMant_Click ingresar");
                    Cuenta cuenta = new Cuenta()
                    {
                        CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                        CodigoMoneda = Convert.ToInt32(ddlCodigoMoneda.SelectedValue),
                        Descripcion = txtDescripcion.Text,
                        IBAN = txtIban.Text,
                        Saldo = Convert.ToDecimal(txtSaldo.Text),
                        Estado = ddlEstadoMant.SelectedValue
                    };

                    Cuenta cuentaIngresada = await cuentaManager.Ingresar(cuenta, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(cuentaIngresada.Descripcion))
                    {
                        lblResultado.Text = "Cuenta ingresada con exito";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Nueva cuenta incluida", cuenta.Descripcion, "testrolandocerdas@gmail.com", Convert.ToInt32(Session["CodigoUsuario"].ToString()), "Cuenta");
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

                    monedas = await monedaManager.ObtenerMonedas(Session["Token"].ToString());

                    var monedaActiva = (from m in monedas
                                          where m.Codigo.ToString().Equals(ddlCodigoMoneda.SelectedValue.ToString())
                                          select m).ToList();

                    if (monedaActiva[0].Estado.ToUpper().Equals("A"))
                    {
                        Cuenta cuenta = new Cuenta()
                        {
                            Codigo = Convert.ToInt32(txtCodigoMant.Text),
                            CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                            CodigoMoneda = Convert.ToInt32(ddlCodigoMoneda.SelectedValue),
                            Descripcion = txtDescripcion.Text,
                            IBAN = txtIban.Text,
                            Saldo = Convert.ToDecimal(txtSaldo.Text),
                            Estado = ddlEstadoMant.SelectedValue
                        };


                        Cuenta cuentaIntresada = await cuentaManager.Actualizar(cuenta, Session["Token"].ToString());

                        if (!string.IsNullOrEmpty(cuentaIntresada.Descripcion))
                        {
                            lblResultado.Text = "Cuenta actualizada con exito";
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
                    else
                    {
                        lblResultado.Text = "La moneda seleccionada esta inactiva. Por favor, seleccione otra.";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Maroon;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
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