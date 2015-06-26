<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" MasterPageFile="~/Base.Master" Inherits="AlterTodoistWebApp.Default" %>
<asp:Content ContentPlaceHolderID="head" ID="addedHeaders" runat="server">
    <meta http-equiv="refresh" content="600">
</asp:Content>

<asp:Content ContentPlaceHolderID="body" ID="defaultBody" runat="server">
    <form id="form1" class="mainForm" runat="server">
        <div id="menu" class="menu">
            <asp:ImageButton ID="btnRefresh" ImageUrl="./images/refresh.png" CssClass="pure-img graphbutton" runat="server" />
            <asp:ImageButton ID="btnAdd" ImageUrl="./images/add.png" CssClass="pure-img graphbutton" OnClick="btnAdd_Click" runat="server" />
        </div>

        <div id="itemList" class="todoitemlist" runat="server"></div>
    </form>
</asp:Content>
