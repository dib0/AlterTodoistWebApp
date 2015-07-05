<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" MasterPageFile="./Base.Master" Inherits="AlterTodoistWebApp.Default" %>

<asp:Content ContentPlaceHolderID="head" ID="addedHeaders" runat="server">
    <meta http-equiv="refresh" content="600">

    <link href="./css/select2.min.css" rel="stylesheet" />
    <script src="./Scripts/select2.min.js"></script>
</asp:Content>

<asp:Content ContentPlaceHolderID="body" ID="defaultBody" runat="server">
    <form id="form1" class="mainForm" runat="server">
        <div id="viewselection" class="pure-control-group viewselection">
            <asp:DropDownList ID="ddlProject" CssClass="viewlb" OnSelectedIndexChanged="ddlProject_SelectedIndexChanged" AutoPostBack="true" runat="server" />
        </div>
        
        <div id="menu" class="menu">
            <asp:ImageButton ID="btnRefresh" ImageUrl="./images/refresh.png" CssClass="pure-img graphbutton" runat="server" />
            <asp:ImageButton ID="btnAdd" ImageUrl="./images/add.png" CssClass="pure-img graphbutton" OnClick="btnAdd_Click" runat="server" />
        </div>

        <div id="itemList" class="todoitemlist" runat="server"></div>
    </form>
</asp:Content>

<asp:Content ContentPlaceHolderID="scripts" ID="cntScript" runat="server">
    <script>
        $(document).ready(function () {
            $('.viewlb').select2();
        });


        $('.viewlb').on('open', function () {
            $('.viewlb').select2().animate({
                width: 400
            },
                {
                    step: function () {
                        $('.viewlb').select2("positionDropdown");
                    },
                    complete: function () {
                        $('.viewlb').select2("positionDropdown");
                    }
                });
        });
    </script>
</asp:Content>
