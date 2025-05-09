<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SecondProject.Login.Register1" %>

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
                <asp:Label ID="lblEmail" runat="server" Text="Email:" AssociatedControlID="txtUsername" />
                <asp:TextBox ID="txtEmail" runat="server" />
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
        </div>
    </form>
</body>
</html>
