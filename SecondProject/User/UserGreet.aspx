<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="UserGreet.aspx.cs" Inherits="SecondProject.User.UserGreet" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lblGreeting" runat="server" Text="Hello User" Font-Size="Large" ForeColor="Blue" />                                                                                                   
</asp:Content>
