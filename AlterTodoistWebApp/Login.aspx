<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" MasterPageFile="./Base.Master" Inherits="AlterTodoistWebApp.Login" %>
<asp:Content ContentPlaceHolderID="head" ID="addedHeaders" runat="server">
    <meta name="google-signin-scope" content="profile email">
    <meta name="google-signin-client_id" content="273168040450-5pqm41vm31ifeanjj8kroffvsqubfljo.apps.googleusercontent.com">
    <script src="https://apis.google.com/js/platform.js" async defer></script>
</asp:Content>

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

            <input type="hidden" id="googleEmail" />
            <input type="hidden" id="googleToken" />
        </fieldset>
      </div>
    <div class="loginblock">
        <fieldset>
            <legend class="title">Or login with Google</legend>
            <div class="g-signin2" data-onsuccess="onSignIn" data-theme="dark"></div>
        </fieldset>
    </div>
    </form>
</asp:Content>

<asp:Content ContentPlaceHolderID="scripts" ID="cntScript" runat="server">
    <script>
        function onSignIn(googleUser) {
            var token = googleUser.getAuthResponse().access_token;
            var email = googleUser.getBasicProfile().getEmail();
            $.post(window.location.href, { googleToken: token, googleEmail: email });
        };

        $(document).ready(function () {
            $('form:first *:input[type!=hidden]:first').focus();
        });
    </script>
</asp:Content>
