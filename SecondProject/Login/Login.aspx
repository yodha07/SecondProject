<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SecondProject.Login.Login" %>


<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title></title>

</head>

<body>

    <form id="form1" runat="server">

        <div>

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


            <asp:HyperLink ID="hlRegister" runat="server" NavigateUrl="~/Login/Register.aspx">Don’t have an account? Register here

            </asp:HyperLink>

            <asp:Button runat="server" Text="Login With Google" OnClick="LoginBtn" />

        </div>

    </form>

</body>

</html> 