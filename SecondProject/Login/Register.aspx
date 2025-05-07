<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Login.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SecondProject.Login.Register" %>
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
        <asp:TextBox ID="txtUsername" runat="server" />
    </div>

    <div class="form-field">
        <asp:Label ID="lblPassword" runat="server" Text="Password:" AssociatedControlID="txtPassword" />
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
    </div>

    <div class="form-field">
        <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password:" AssociatedControlID="txtConfirmPassword" />
        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" />
    </div>

    <div class="form-button">
        <asp:Button ID="btnRegister" runat="server" Text="Register" />
    </div>
</asp:Content>
