<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SecondProject.Login.Register1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register</title>
    <!-- Tailwind CSS CDN -->
    <script src="https://cdn.tailwindcss.com"></script>
</head>
<body class="bg-gray-100 min-h-screen flex items-center justify-center bg-gradient-to-bl from-slate-200 to-sky-700">
    <form id="form1" runat="server" class="w-full max-w-md bg-white shadow-lg rounded-lg p-8">
        <h2 class="text-2xl font-bold mb-6 text-center text-gray-800">Register</h2>

        <div class="mb-4">
            <asp:Label ID="lblEmail" runat="server" Text="Email:" AssociatedControlID="txtEmail"
                       CssClass="block text-gray-700 font-semibold mb-1" />
            <asp:TextBox ID="txtEmail" runat="server"
                         CssClass="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500" />
            <asp:Label ID="lblEmailError" runat="server" CssClass="text-sm text-red-600 mt-1 block" />
        </div>

        <div class="mb-4">
            <asp:Label ID="LabelName" runat="server" Text="Name:" AssociatedControlID="txtName"
                       CssClass="block text-gray-700 font-semibold mb-1" />
            <asp:TextBox ID="txtName" runat="server"
                         CssClass="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500" />
        </div>

        <div class="mb-4">
            <asp:Label ID="lblPassword" runat="server" Text="Password:" AssociatedControlID="txtPassword"
                       CssClass="block text-gray-700 font-semibold mb-1" />
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"
                         CssClass="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500" />
        </div>

        <div class="mb-4">
            <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password:" AssociatedControlID="txtConfirmPassword"
                       CssClass="block text-gray-700 font-semibold mb-1" />
            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"
                         CssClass="w-full px-4 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500" />
            <asp:Label ID="lblPasswordError" runat="server" CssClass="text-sm text-red-600 mt-1 block" />
        </div>

        <div class="mt-6">
            <asp:Button ID="btnRegister" runat="server" Text="Register"
                        CssClass="w-full bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 px-4 rounded-md transition duration-200"
                        OnClick="btnRegister_Click" />
        </div>

        <asp:Label ID="lblSuccessMessage" runat="server"
                   CssClass="block text-green-600 text-center mt-4 font-medium" />
    </form>
</body>
</html>
