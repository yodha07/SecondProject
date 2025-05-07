<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Login.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SecondProject.Login.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-field {
            margin-bottom: 10px;
        }

            .form-field label {
                display: block;
                font-weight: bold;
            }

            .form-field input {
                width: 100%;
                padding: 6px;
                font-size: 14px;
            }

        .form-button {
            margin-top: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-field">
        <asp:Label ID="lblUsername" runat="server" Text="Username:" AssociatedControlID="txtUsername" />
        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" />
    </div>

    <div class="form-field">
        <asp:Label ID="lblPassword" runat="server" Text="Password:" AssociatedControlID="txtPassword" />
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
    </div>

    <div class="form-button">
        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
    </div>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />

    <asp:HyperLink ID="hlRegister" runat="server" NavigateUrl="~/Login/Register.aspx">
    Don’t have an account? Register here
    </asp:HyperLink>

</asp:Content>
