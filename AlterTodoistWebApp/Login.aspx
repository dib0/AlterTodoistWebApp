<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" MasterPageFile="~/Base.Master" Inherits="AlterTodoistWebApp.Login" %>
<asp:Content ContentPlaceHolderID="body" ID="defaultBody" runat="server">
    <form id="form1" class="pure-form pure-form-aligned loginform" runat="server">
    <div class="loginblock">
        <fieldset>
            <legend class="title">Todoist frontend</legend>
            <div class="pure-control-group">
                <asp:Label id="lblUsername" AssociatedControlID="tbUsername" runat="server" Text="Username"></asp:Label>
                <asp:TextBox ID="tbUsername" runat="server"></asp:TextBox>
            </div>

            <div class="pure-control-group">
                <asp:Label id="Label1" AssociatedControlID="tbPassword" runat="server" Text="Password"></asp:Label>
                <asp:TextBox ID="tbPassword" TextMode="Password" runat="server"></asp:TextBox>
            </div>

            <div class="pure-controls">
                <asp:Button ID="btnLogin" Text="Login" CssClass="pure-button pure-button-primary" runat="server" OnClick="btnLogin_Click" />
            </div>
        </fieldset>
      </div>
    </form>
</asp:Content>
