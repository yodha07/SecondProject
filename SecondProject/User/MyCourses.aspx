<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="MyCourses.aspx.cs" Inherits="SecondProject.User.MyCourses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/tailwindcss@2.0.3/dist/tailwind.min.css" rel="stylesheet">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form runat ="server">
         <div class="max-w-7xl mx-auto px-4 py-8 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
     <asp:Repeater ID="rptSubCourses" runat="server">
         <itemtemplate>
             <div class="bg-white rounded-xl shadow-md overflow-hidden hover:shadow-lg transition duration-300">
                 <img class="w-full h-48 object-cover" src='<%# Eval("Thumbnail") %>' alt="Thumbnail" />
                 <div class="p-4">
                     <h3 class="text-lg font-semibold text-gray-800 mb-3"><%# Eval("Title") %></h3>
                     <div class="space-y-2">
                         <asp:Button ID="btnViewTopics" runat="server" Text="View Topics" CommandArgument='<%# Eval("SubCourseId") %>' OnCommand="ViewTopics_Click"
                             CssClass="w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 transition" />
                     </div>
                 </div>
             </div>
         </itemtemplate>
     </asp:Repeater>
 </div>



    </form>
   </asp:Content>
