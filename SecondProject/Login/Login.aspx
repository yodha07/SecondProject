<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SecondProject.Login.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <!-- Tailwind CDN -->
    <script src="https://cdn.tailwindcss.com"></script>
</head>
<body class="bg-gray-100 flex items-center justify-evenly min-h-screen bg-gradient-to-bl from-slate-200 to-sky-700">

    <div class="bg-white p-8 rounded-2xl shadow-lg w-full max-w-md">
        <image src="logo.jpg"/>
    </div>

    <form id="form1" runat="server" class="bg-white p-8 rounded-2xl shadow-lg w-full max-w-md">
        <h2 class="text-2xl font-bold text-blue-600 mb-6 text-center">Login to Your Account</h2>

        <div class="mb-4">
            <asp:Label ID="lblUsername" runat="server" Text="Username:" AssociatedControlID="txtUsername" CssClass="block font-semibold mb-1 text-gray-700" />
            <asp:TextBox ID="txtUsername" runat="server" CssClass="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500" />
        </div>

        <div class="mb-4">
            <asp:Label ID="lblPassword" runat="server" Text="Password:" AssociatedControlID="txtPassword" CssClass="block font-semibold mb-1 text-gray-700" />
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500" />
        </div>

        <div class="mb-4">
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="w-full bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 rounded-lg transition duration-200" OnClick="btnLogin_Click" />
        </div>

        <div class="text-center mb-2">
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" CssClass="text-red-600 font-medium" />
        </div>

        <div class="text-center mb-4">
            <asp:HyperLink ID="hlRegister" runat="server" NavigateUrl="~/Login/Register.aspx" CssClass="text-sm text-blue-500 hover:underline">
                Don’t have an account? Register here
            </asp:HyperLink>
        </div>

        <div>
            <asp:Button runat="server" Text="Login With Google" OnClick="LoginBtn" CssClass="w-full bg-white border border-gray-300 hover:bg-gray-100 text-gray-800 font-semibold py-2 rounded-lg transition duration-200" />
        </div>
    </form>

</body>
</html>
