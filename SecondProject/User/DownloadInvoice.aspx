<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="DownloadInvoice.aspx.cs" Inherits="SecondProject.User.DownloadInvoice" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form runat="server">
       <div class="container d-flex justify-content-center align-items-center" style="min-height: 80vh;">
           <div class="card text-center" style="padding: 40px; width: 400px;">
               <h2 class="mb-4">Thank You for Your Payment!</h2>
               <asp:Button ID="Button1" runat="server" 
                   Text="Download invoice" CssClass="btn btn-primary w-100" 
                   OnClick="Button1_Click" />                
           </div>
       </div>
    </form>
</asp:Content>
