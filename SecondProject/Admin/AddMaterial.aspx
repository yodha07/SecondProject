<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="AddMaterial.aspx.cs" Inherits="SecondProject.Admin.AddMaterial" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
<link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" rel="stylesheet" />

    <form id="form1" runat="server">
    <div class="container mt-5">
        <!-- Page Heading -->
        <h3 class="mb-4">Add Material</h3>

        <!-- Breadcrumb -->
        <nav>
            <ol class="breadcrumb mb-0">
                <li class="breadcrumb-item">
                    <a href="AdminDashboard.aspx"><i class="bi bi-house"></i></a>
                </li>
                <li class="breadcrumb-item">Admin</li>
                <li class="breadcrumb-item active" aria-current="page">Add Material</li>
            </ol>
        </nav>

        <!-- Card -->
        <div class="card">
            <div class="card-header">
                <h5 class="mb-0">Add Material</h5>
            </div>
            <div class="card-body" style="max-width: 400px;">
                <div class="mb-3">
                    <label for="DropDownList1" class="form-label">Select Master Course</label>
                    <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div class="mb-3">
                    <label for="DropDownList2" class="form-label">Select Sub Course</label>
                    <asp:DropDownList ID="DropDownList2" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div class="mb-3">
                    <label for="DropDownList3" class="form-label">Select Topic</label>
                    <asp:DropDownList ID="DropDownList3" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="mb-3">
                    <label for="TextBox1" class="form-label">No. of MCQ</label>
                    <asp:TextBox ID="TextBox1" CssClass="form-control" placeholder="Enter No. of MCQ" runat="server" AutoPostBack="True" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
                </div>
                <div class="col mb-4">
                    <div class="card h-100">
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server" />
                    </div>
                </div>
                <div class="mb-3">
                    <label for="FileUpload1" class="form-label">Assignment</label>
                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control w-100" />
                </div>
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" CssClass="btn btn-primary" Text="Add"/>
            </div>
        </div>
    </div>
</form>

</asp:Content>
