<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AlterTodoistWebApp.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p>Username <asp:TextBox ID="tbUsername" runat="server"></asp:TextBox></p>
        <p>Password <asp:TextBox ID="tbPassword" TextMode="Password" runat="server"></asp:TextBox></p>
        <p><asp:Button ID="btnLogin" Text="Login" runat="server" OnClick="btnLogin_Click" /></p>
        
        
    
    </div>
    </form>
</body>
</html>
