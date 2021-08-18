﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Async="true" CodeBehind="frmTransferencia.aspx.cs" Inherits="AppIBULACIT.Views.frmTransferencia" %>

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
                $("#MainContent_gvTransferencias tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
    </script>

    <h1>
        <asp:Label Text="Mantenimiento de transferencias" runat="server"></asp:Label></h1>
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
            $('[id*=gvTransferencias]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                dom: 'Bfrtip',
                'aoColumnDefs': [{ 'bSortable': false, 'aTargets': [0] }],
                'iDisplayLength': 5,
                buttons: [
                    { extend: 'copy', text: 'Copy to clipboard', className: 'exportExcel', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'excel', text: 'Export to Excel', className: 'exportExcel', filename: 'Transferencias_Excel', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'csv', text: 'Export to CSV', className: 'exportExcel', filename: 'Transferencias_Csv', exportOptions: { modifier: { page: 'all' } } },
                    { extend: 'pdf', text: 'Export to PDF', className: 'exportExcel', filename: 'Transferencias_Pdf', orientation: 'landscape', pageSize: 'LEGAL', exportOptions: { modifier: { page: 'all' }, columns: ':visible' } }
                ]
            });
        });
    </script>
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvTransferencias" runat="server" AutoGenerateColumns="False"
        CssClass="table table-sm" HeaderStyle-CssClass="thead-dark" HeaderStyle-BackColor="#243054"
        HeaderStyle-ForeColor="White" AlternatingRowStyle-BackColor="LightBlue" Width="100%">
        <Columns>
            <asp:BoundField HeaderText="Codigo" DataField="Codigo" />
            <asp:BoundField HeaderText="CuentaOrigen" DataField="CuentaOrigen" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="CuentaDestino" DataField="CuentaDestino" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="FechaHora" DataField="FechaHora" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Descripcion" DataField="Descripcion" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField HeaderText="Monto" DataField="Monto" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" />
        </Columns>
    </asp:GridView>
    <asp:LinkButton type="button" OnClick="btnNuevo_Click" ValidationGroup="vg1" CssClass="btn btn-success" ID="btnNuevo" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-disk'></span> Nuevo" />
    <br />
    <asp:Label ID="lblStatus" ForeColor="Maroon" runat="server" Visible="false" />
    <!-- VENTANA MODAL -->
    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Mantenimiento de transferencias</h4>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Literal ID="ltrModalMensaje" runat="server" /><asp:Label ID="lblCodigoEliminar" Visible="false" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton type="button" CssClass="btn btn-success" ID="btnAceptarModal" OnClick="btnAceptarModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
                    <asp:LinkButton type="button" CssClass="btn btn-danger" ID="btnCancelarModal" OnClick="btnCancelarModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
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
                                <asp:Literal ID="ltrCuentaOrigen" Text="CuentaOrigen" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlCuentaOrigen" runat="server"></asp:DropDownList>
                                 <asp:RequiredFieldValidator ControlToValidate="ddlCuentaOrigen" ID="rfvCuentaOrigen" runat="server" ErrorMessage="Esta informacion es necesaria"></asp:RequiredFieldValidator>
                            </td>
                        </tr>


                        <tr>
                            <td>
                                <asp:Literal ID="ltrCuentaDestino" Text="Cuenta destino" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtCuentaDestino" Enabled="true" runat="server" CssClass="form-control" />
                                                                 <asp:RequiredFieldValidator ControlToValidate="txtCuentaDestino" ID="rfvCuentaDestino" runat="server" ErrorMessage="Esta informacion es necesaria"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                                                <tr>
                            <td>
                                <asp:Literal ID="ltrFechaHora" Visible="true" Text="Fecha y hora" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtFechaHora" Enabled="false" runat="server" CssClass="form-control" />
                                                                 <asp:RequiredFieldValidator ControlToValidate="txtFechaHora" ID="rfvFechaHora" runat="server" ErrorMessage="Esta informacion es necesaria"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrDescripcion" Text="Descripcion" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtDescripcion" ID="rfvDescripcion" runat="server" ErrorMessage="Esta informacion es necesaria"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrMonto" Text="Monto" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtMonto" runat="server" ValidationGroup="vg1" CssClass="form-control" />
                                <asp:RequiredFieldValidator ControlToValidate="txtMonto" ID="rfvSAldo" runat="server" ErrorMessage="El saldo es necesario"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ControlToValidate="txtMonto" ValidationExpression="^[1-9]\d*(\.\d+)?$" ID="revSaldo" runat="server" ErrorMessage="Solo se pueden ingresar valores numericos"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lblResultado" ForeColor="Maroon" Visible="False" runat="server" />
                </div>
                <div class="modal-footer">
                    <asp:LinkButton type="button" CssClass="btn btn-success" ID="btnAceptarMant" OnClick="btnAceptarMant_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
                    <asp:LinkButton type="button" CssClass="btn btn-danger" ValidationGroup="vg1" ID="btnCancelarMant" OnClick="btnCancelarMant_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
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
                            label: "Transferencias",
                            backgroundColor: [<%= this.backgroundcolorsGraficoVistasGlobal %>],
                              data: [<%= this.dataGraficoVistasGlobal %>]
                        }]
                    },
                    options: {
                        title: {
                            display: true,
                            text: 'Transferencias'
                        }
                    }
                });
            </script>
        </div>
    </div>
</asp:Content>
