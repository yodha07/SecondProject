<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="MyCourses.aspx.cs" Inherits="SecondProject.User.MyCourses" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Repeater ID="rptSubCourses" runat="server">
        <ItemTemplate>
            <div class="subcourse-card">
                <img src='<%# Eval("Thumbnail") %>' alt="Thumbnail" />
                <h3><%# Eval("Title") %></h3>

                <asp:Button ID="btnViewTopics" runat="server" Text="View Topics" CommandArgument='<%# Eval("SubCourseId") %>' OnCommand="ViewTopics_Click" />
                <asp:Button ID="btnAssignments" runat="server" Text="Assignments" CommandArgument='<%# Eval("SubCourseId") %>' OnCommand="Assignments_Click" />
                <asp:Button ID="btnMCQ" runat="server" Text="MCQs" CommandArgument='<%# Eval("SubCourseId") %>' OnCommand="Mcq_Click" />
                <asp:Button ID="btnCertificate" runat="server" Text="Certificate" CommandArgument='<%# Eval("SubCourseId") %>' OnCommand="Certificate_Click" />
            </div>
        </ItemTemplate>
    </asp:Repeater>

</asp:Content>
