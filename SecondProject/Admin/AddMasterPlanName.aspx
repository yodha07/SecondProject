<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="AddMasterPlanName.aspx.cs" Inherits="SecondProject.Admin.AddMasterPlanName" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
<link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" rel="stylesheet" />

    <form id="form1" runat="server">
    <div class="container mt-5">
        <!-- Page Heading -->
        <h3 class="mb-4">Add Master Plan Name</h3>

        <!-- Breadcrumb -->
        <nav>
            <ol class="breadcrumb mb-0">
                <li class="breadcrumb-item">
                    <a href="AdminDashboard.aspx"><i class="bi bi-house"></i></a>
                </li>
                <li class="breadcrumb-item">Admin</li>
                <li class="breadcrumb-item active" aria-current="page">Add Master Plan Name</li>
            </ol>
        </nav>

        <!-- Card -->
        <div class="card">
            <div class="card-header">
                <h5 class="mb-0">Add Master Plan Name</h5>
            </div>
            <div class="card-body" style="max-width: 400px;">
                <div class="mb-3">
                    <label for="TextBox1" class="form-label">Master Plan Name</label>
                    <asp:TextBox ID="TextBox1" CssClass="form-control" placeholder="Enter Master Plan Name" runat="server"></asp:TextBox>
                </div>
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" CssClass="btn btn-primary" Text="Add"/>
            </div>
        </div>
    </div>
</form>
</asp:Content>
