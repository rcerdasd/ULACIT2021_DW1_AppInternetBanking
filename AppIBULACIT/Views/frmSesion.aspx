<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmSesion.aspx.cs" Inherits="AppIBULACIT.Views.frmSesion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function openModal() {
            $('#myModal').modal('show'); //ventana de mensajes
        }

        function openModalMantenimiento() {
            $('#myModalMantenimiento').modal('show'); //ventana de mantenimiento
        }

        function CloseModal() {
            $('#myModal').modal('hide');//cierra ventana de mensajes
        }

        function CloseMantenimiento() {
            $('#myModalMantenimiento').modal('hide'); //cierra ventana de mantenimiento
        }

        $(document).ready(function () { //filtrar el datagridview
            $("#myInput").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#MainContent_gvSesions tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
    </script>

    <h1>
        <asp:Label Text="Mantenimiento de sesiones" runat="server"></asp:Label></h1>
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvSesions" runat="server" AutoGenerateColumns="False"
        CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" HeaderStyle-BackColor="#243054"
        HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="LightBlue" Width="100%">
        <Columns>
            <asp:BoundField HeaderText="Codigo" DataField="Codigo" />
            <asp:BoundField HeaderText="CodigoUsuario" DataField="CodigoUsuario" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="FechaInicio" DataField="FechaInicio" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="FechaExpiracion" DataField="FechaExpiracion" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Estado" DataField="Estado" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField HeaderText="CodigoSesion" DataField="CodigoSesion" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />

        </Columns>
    </asp:GridView>
    <asp:LinkButton type="button" OnClick="btnNuevo_Click" CssClass="btn btn-success" ValidationGroup="vg1" ID="btnNuevo" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-disk'></span> Nuevo" />
    <br />
    <asp:Label ID="lblStatus" ForeColor="Maroon" runat="server" Visible="false" />
    <!-- VENTANA MODAL -->
    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Mantenimiento de sesiones</h4>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Literal ID="ltrModalMensaje" runat="server" /><asp:Label Visible="false" ID="lblCodigoEliminar" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton type="button" CssClass="btn btn-success" CausesValidation="false" ValidationGroup="vg1" ID="btnAceptarModal" OnClick="btnAceptarModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
                    <asp:LinkButton type="button" CausesValidation="false" CssClass="btn btn-danger" ID="btnCancelarModal" OnClick="btnCancelarModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
                </div>
            </div>
        </div>
    </div>
    <!--VENTANA DE MANTENIMIENTO -->
    <div id="myModalMantenimiento" class="modal fade" role="dialog">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">
                        <asp:Literal ID="ltrTituloMantenimiento" runat="server"></asp:Literal></h4>
                </div>
                <div class="modal-body">
                    <table style="width: 100%;">

                        <tr>
                            <td>
                                <asp:Literal ID="ltrCodigoUsuario" Text="Codigo usuario" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtCodigoUsuario" Enabled="true" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtCodigoUsuario" ID="rfvCodigoUsuario" runat="server" ErrorMessage="El codigo de usuario es necesario"></asp:RequiredFieldValidator>
                            </td>
                        </tr>


                        <tr>
                            <td>
                                <asp:Literal ID="ltrFechaInicio" Text="Fecha inicio" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtFechaInicio" runat="server" CausesValidation="true" ValidationGroup="vg1" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtFechaInicio" ID="RequiredFieldValidator1" runat="server" ErrorMessage="La fecha de inicio es necesaria"></asp:RequiredFieldValidator>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrFechaExp" Text="Fecha de expiracion" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtFechaExp" runat="server" CausesValidation="true" ValidationGroup="vg1" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtFechaExp" ID="RequiredFieldValidator2" runat="server" ErrorMessage="La fecha de expiracion es necesaria"></asp:RequiredFieldValidator>
                        </tr>
 
                        <tr>
                            <td>
                                <asp:Literal Text="Estado" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlEstadoMant" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="A">Activo</asp:ListItem>
                                    <asp:ListItem Value="I">Inactivo</asp:ListItem>
                                </asp:DropDownList>

                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lblResultado" ForeColor="Maroon" Visible="False" runat="server" />
                </div>
                <div class="modal-footer">
                    <asp:LinkButton type="button" CssClass="btn btn-success" ID="btnAceptarMant" OnClick="btnAceptarMant_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
                    <asp:LinkButton type="button" CssClass="btn btn-danger" CausesValidation="false" ID="btnCancelarMant" OnClick="btnCancelarMant_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

