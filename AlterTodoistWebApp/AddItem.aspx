<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddItem.aspx.cs" MasterPageFile="~/Base.Master" Inherits="AlterTodoistWebApp.AddItem" %>

<asp:Content ContentPlaceHolderID="body" ID="defaultBody" runat="server">
    <form id="form1" class="pure-form pure-form-aligned addform" runat="server">
            <div class="addblock">
                <legend class="title">Add task</legend>

                <div class="pure-control-group">
                    <asp:Label id="lblTodoText" AssociatedControlID="tbTodoText" runat="server" Text="Description"></asp:Label>
                    <asp:TextBox ID="tbTodoText" runat="server"></asp:TextBox>
                </div>

                <div class="pure-control-group">
                    <asp:Label id="lblDueDate" AssociatedControlID="tbDueDate" runat="server" Text="Due date"></asp:Label>
                    <asp:TextBox ID="tbDueDate" runat="server"></asp:TextBox>
                </div>

                <div class="pure-control-group">
                    <asp:Label id="lblProject" AssociatedControlID="ddlProject" runat="server" Text="Project"></asp:Label>
                    <asp:DropDownList ID="ddlProject" CssClass="projectlb" runat="server" />
                </div>

                <div class="pure-controls">
                    <asp:Button ID="btnSave" Text="Save" CssClass="pure-button pure-button-primary" runat="server" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" Text="Cancel" CssClass="pure-button" runat="server" OnClick="btnCancel_Click" />
                </div>
            </div>
    </form>
</asp:Content>

<asp:Content ContentPlaceHolderID="scripts" ID="cntScript" runat="server">
    <script>
        $(document).ready(function () {
            $('form:first *:input[type!=hidden]:first').focus();
        });
    </script>
</asp:Content>
