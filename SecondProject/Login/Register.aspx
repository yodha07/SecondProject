<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SecondProject.Login.Register1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register</title>
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

        .error-message {
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="form-field">
                <asp:Label ID="lblEmail" runat="server" Text="Email:" AssociatedControlID="txtEmail" />
                <asp:TextBox ID="txtEmail" runat="server" />
                <asp:Label ID="lblEmailError" runat="server" CssClass="error-message" />
            </div>

            <div class="form-field">
                <asp:Label ID="LabelName" runat="server" Text="Name:" AssociatedControlID="txtName" />
                <asp:TextBox ID="txtName" runat="server" />
            </div>

            <div class="form-field">
                <asp:Label ID="lblPassword" runat="server" Text="Password:" AssociatedControlID="txtPassword" />
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
            </div>

            <div class="form-field">
                <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password:" AssociatedControlID="txtConfirmPassword" />
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" />
                <asp:Label ID="lblPasswordError" runat="server" CssClass="error-message" />
            </div>

            <div class="form-button">
                <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" />
            </div>

            <asp:Label ID="lblSuccessMessage" runat="server" />
        </div>
    </form>
</body>
</html>
