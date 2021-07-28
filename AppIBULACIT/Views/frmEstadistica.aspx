<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Async="true" CodeBehind="frmEstadistica.aspx.cs" Inherits="AppIBULACIT.Views.frmEstadistica" %>
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
                $("#MainContent_gvEstadisticas tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
    </script>

    <h1>
        <asp:Label Text="Mantenimiento de estadisticas" runat="server"></asp:Label></h1>
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvEstadisticas" runat="server" AutoGenerateColumns="False"
        CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" HeaderStyle-BackColor="#243054"
        HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="LightBlue" Width="100%">
        <Columns>
            <asp:BoundField HeaderText="Codigo" DataField="Codigo" />
            <asp:BoundField HeaderText="CodigoUsuario" DataField="CodigoUsuario" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="FechaHora" DataField="FechaHora" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Navegador" DataField="Navegador" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="PlataformaDispositivo" DataField="PlataformaDispositivo" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="FabricanteDispositivo" DataField="FabricanteDispositivo" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />            
            <asp:BoundField HeaderText="Vista" DataField="Vista" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Accion" DataField="Accion" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
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
                    <h4 class="modal-title">Mantenimiento de estadisticas</h4>
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
                                <asp:Literal ID="ltrCodigoMant" Text="Codigo" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtCodigoMant" runat="server" Enabled="false" CssClass="form-control" /></td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Literal ID="ltrCodigoUsuario" Text="Codigo usuario" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtCodigoUsuario" Enabled="false" runat="server" CssClass="form-control" /></td>
                        </tr>


                        <tr>
                            <td>
                                <asp:Literal ID="ltrFechaHora" Text="Fecha hora" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtFechaHora" runat="server" CausesValidation="true" ValidationGroup="vg1" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtFechaHora" ID="RequiredFieldValidator1" runat="server" EstadisticaMessage="Esta informacion es necesaria"></asp:RequiredFieldValidator>
                        </tr>

                        <tr>
                            <td>
                                <asp:Literal ID="ltrNavegador" Text="Navegador" runat="server" />

                            </td>
                            <td>
                                <asp:TextBox ID="txtNavegador" runat="server" CausesValidation="true" ValidationGroup="vg1" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtNavegador" ID="rfvFuente" runat="server" EstadisticaMessage="Esta informacion es necesaria"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrPlataforma" Text="Plataforma" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtPlataforma" runat="server" ValidationGroup="vg1" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtPlataforma" ID="rfvNumero" runat="server" EstadisticaMessage="Esta informacion es necesaria"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrFabricante" Text="Fabricante" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtFabricante" runat="server" ValidationGroup="vg1" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtFabricante" ID="RequiredFieldValidator2" runat="server" EstadisticaMessage="Esta informacion es necesaria"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrVista" Text="Vista" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtVista" runat="server" ValidationGroup="vg1" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtVista" ID="RequiredFieldValidator3" runat="server" EstadisticaMessage="Esta informacion es necesaria"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                                                <tr>
                            <td>
                                <asp:Literal ID="ltrlAccion" Text="Accion" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtAccion" runat="server" ValidationGroup="vg1" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtAccion" ID="RequiredFieldValidator4" runat="server" EstadisticaMessage="Esta informacion es necesaria"></asp:RequiredFieldValidator>
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


