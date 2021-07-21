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
    public partial class frmEstadistica : System.Web.UI.Page
    {

        IEnumerable<Estadistica> estadisticas = new ObservableCollection<Estadistica>();
        EstadisticaManager estadisticaManager = new EstadisticaManager();


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
                estadisticas = await estadisticaManager.ObtenerEstadisticas(Session["Token"].ToString());
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

        protected async void gvEstadisticas_RowCommand(object sender, GridViewCommandEventArgs e)
        {/*
            try
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvEstadisticas.Rows[index];

                switch (e.CommandName)
                {
                    case "Modificar":
                        IngresarEstadistica("gvEstadisticas_RowCommand modificar");
                        ddlEstadoMant.Enabled = true;
                        monedas = await monedaManager.ObtenerMonedas(Session["Token"].ToString());

                        ddlCodigoMoneda.DataSource = monedas.ToList();
                        ddlCodigoMoneda.DataTextField = "Descripcion";
                        ddlCodigoMoneda.DataValueField = "Codigo";
                        ddlCodigoMoneda.DataBind();

                        ddlCodigoMoneda.SelectedValue = row.Cells[2].Text.Trim();

                        ltrTituloMantenimiento.Text = "Modificar estadistica";
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
                        IngresarEstadistica("gvEstadisticas_RowCommand eliminar");
                        btnAceptarModal.Visible = true;
                        lblCodigoEliminar.Text = row.Cells[0].Text;
                        ltrModalMensaje.Text = "Esta seguro que desea eliminar la estadistica #" + lblCodigoEliminar.Text + "?";
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
                error.Accion = "gvEstadisticas_RowCommand()";
                error.Fuente = ex.Source;
                error.Numero = ex.HResult.ToString();
                error.Descripcion = ex.Message;

                lblStatus.Text = "Accion no identificada";
                lblStatus.Visible = true;
            }*/
        }



        protected async void btnNuevo_Click(object sender, EventArgs e)
        {/*
            try
            {
                IngresarEstadistica("btnNuevo_Click");

                monedas = await monedaManager.ObtenerMonedas(Session["Token"].ToString());

                ddlCodigoMoneda.DataSource = monedas.ToList();
                ddlCodigoMoneda.DataTextField = "Descripcion";
                ddlCodigoMoneda.DataValueField = "Codigo";
                ddlCodigoMoneda.DataBind();

                ltrTituloMantenimiento.Text = "Nueva estadistica";
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
            }*/
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
        {/*
            try
            {

                if (string.IsNullOrEmpty(txtCodigoMant.Text))//Insertar
                {
                    IngresarEstadistica("btnAceptarMant_Click ingresar");
                    Estadistica estadistica = new Estadistica()
                    {
                        CodigoUsuario = Convert.ToInt32(txtCodigoUsuario.Text),
                        CodigoMoneda = Convert.ToInt32(ddlCodigoMoneda.SelectedValue),
                        Descripcion = txtDescripcion.Text,
                        IBAN = txtIban.Text,
                        Saldo = Convert.ToDecimal(txtSaldo.Text),
                        Estado = ddlEstadoMant.SelectedValue
                    };

                    Estadistica estadisticaIngresada = await estadisticaManager.Ingresar(estadistica, Session["Token"].ToString());

                    if (!string.IsNullOrEmpty(estadisticaIngresada.Descripcion))
                    {
                        lblResultado.Text = "Estadistica ingresada con exito";
                        lblResultado.Visible = true;
                        lblResultado.ForeColor = Color.Green;
                        btnAceptarMant.Visible = false;
                        InicializarControles();

                        Correo correo = new Correo();
                        correo.Enviar("Nueva estadistica incluida", estadistica.Descripcion, "testrolandocerdas@gmail.com", Convert.ToInt32(Session["CodigoUsuario"].ToString()), "Estadistica");
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

            }*/
        }

        protected void btnCancelarMant_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);
            IngresarEstadistica("btnCancelarMant_Click");
        }
    }
}