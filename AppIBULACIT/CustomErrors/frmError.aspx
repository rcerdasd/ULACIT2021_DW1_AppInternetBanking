<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmError.aspx.cs" Inherits="AppIBULACIT.CustomErrors.frmError" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <div class="container" >
  <h3><span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Ocurrió un error</h3>
  <div class="panel-group">
    <div class="panel panel-primary">
      <div class="panel-body">

          <asp:Label ID="Label1" runat="server" Font-Size="Large" Text="Hubo un error al procesar la solicitud"></asp:Label>
          <br />
          <asp:Label ID="lblError" runat="server"></asp:Label>

          </div>
        </div>
      </div>
  </div>
</asp:Content>
