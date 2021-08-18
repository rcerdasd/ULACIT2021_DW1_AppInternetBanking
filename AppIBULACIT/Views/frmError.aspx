<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="frmError.aspx.cs" Inherits="AppIBULACIT.Views.frmError" %>
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
                $("#MainContent_gvErrors tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
    </script>

    <h1>
        <asp:Label Text="Mantenimiento de errores" runat="server"></asp:Label></h1>
                        <link rel="stylesheet" href="https://cdn.datatables.net/1.10.12/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/1.2.2/css/buttons.dataTables.min.css" />
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.12/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.2.2/js/dataTables.buttons.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>
    <script type="text/javascript" src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/pdfmake.min.js"></script>
    <script type="text/javascript" src="https://cdn.rawgit.com/bpampuch/pdfmake/0.1.18/build/vfs_fonts.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.html5.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('[id*=gvErrors]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                dom: 'Bfrtip',
                'aoColumnDefs': [{ 'bSortable': false, 'aTargets': [0] }],
                'iDisplayLength': 5,
                buttons: [
                    { extend: 'copy', text: 'Copy to clipboard', className: 'exportExcel', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'excel', text: 'Export to Excel', className: 'exportExcel', filename: 'Errores_Excel', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'csv', text: 'Export to CSV', className: 'exportExcel', filename: 'Errores_Csv', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'pdf', text: 'Export to PDF', className: 'exportExcel', filename: 'Errores_Pdf', orientation: 'landscape', pageSize: 'LEGAL', exportOptions: { modifier: { page: 'all' }, columns: ':visible' } }
                ]
            });
        });
    </script>
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvErrors" runat="server" AutoGenerateColumns="False"
        CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" HeaderStyle-BackColor="#243054"
        HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="LightBlue" Width="100%">
        <Columns>
            <asp:BoundField HeaderText="Codigo" DataField="Codigo" />
            <asp:BoundField HeaderText="CodigoUsuario" DataField="CodigoUsuario" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="FechaHora" DataField="FechaHora" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Fuente" DataField="Fuente" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Numero" DataField="Numero" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Descripcion" DataField="Descripcion" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
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
                    <h4 class="modal-title">Mantenimiento de errors</h4>
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
                                <asp:Literal ID="ltrFechaHora" Text="Fecha y hora" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtFechaHora" runat="server" CausesValidation="true" ValidationGroup="vg1" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtFechaHora" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Esta informacion es necesaria"></asp:RequiredFieldValidator>
                        </tr>

                        <tr>
                            <td>
                                <asp:Literal ID="ltrFuente" Text="Fuente" runat="server" />

                            </td>
                            <td>
                                <asp:TextBox ID="txtFuente" runat="server" CausesValidation="true" ValidationGroup="vg1" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtFuente" ID="rfvFuente" runat="server" ErrorMessage="Esta informacion es necesaria"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrNumero" Text="Numero" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtNumero" runat="server" ValidationGroup="vg1" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtNumero" ID="rfvNumero" runat="server" ErrorMessage="Esta informacion es necesaria"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="trlDescripcion" Text="Descripcion" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtDescripcion" runat="server" ValidationGroup="vg1" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtDescripcion" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Esta informacion es necesaria"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrVista" Text="Vista" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtVista" runat="server" ValidationGroup="vg1" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtVista" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Esta informacion es necesaria"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                                                <tr>
                            <td>
                                <asp:Literal ID="ltrlAccion" Text="Accion" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtAccion" runat="server" ValidationGroup="vg1" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtAccion" ID="RequiredFieldValidator4" runat="server" ErrorMessage="Esta informacion es necesaria"></asp:RequiredFieldValidator>
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

    
                <div class="row">
        <div class="col-sm">
            <div id="canvas-holder" style="width: 40%">
                <canvas id="vistas-chart"></canvas>
            </div>
            <script>
                new Chart(document.getElementById("vistas-chart"), {
                    type: 'pie',
                    data: {
                        labels: [<%= this.labelsGraficoVistasGlobal %>],
                        datasets: [{
                            label: "Erroes por vista",
                            backgroundColor: [<%= this.backgroundcolorsGraficoVistasGlobal %>],
                              data: [<%= this.dataGraficoVistasGlobal %>]
                        }]
                    },
                    options: {
                        title: {
                            display: true,
                            text: 'Erroes por vista'
                        }
                    }
                });
            </script>
        </div>
    </div>

</asp:Content>


