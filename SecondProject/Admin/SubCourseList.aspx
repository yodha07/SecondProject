<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="SubCourseList.aspx.cs" Inherits="SecondProject.Admin.SubCourseList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" integrity="sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN" crossorigin="anonymous"></script>
<script src="https://cdn.jsdelivr.net/npm/popper.js@1.12.9/dist/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js" integrity="sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl" crossorigin="anonymous"></script>
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
<link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" rel="stylesheet" />
<style>
    #SubCourseList th,
    #SubCourseList td,
    .table th,
    .table td {
        text-align: center;
        vertical-align: middle;
    }
</style>

        <form id="form1" runat="server">
        <div class="container mt-5">
            <!-- Header and Breadcrumb -->
            <div class="d-md-flex d-block align-items-center justify-content-between page-breadcrumb mb-3">
                <div class="my-auto mb-2">
                    <h3 class="mb-1">Sub Course List</h3>
                    <nav>
                        <ol class="breadcrumb mb-0">
                            <li class="breadcrumb-item">
                                <a href="AdminDashboard.aspx"><i class="bi bi-house"></i></a>
                            </li>
                            <li class="breadcrumb-item">Admin</li>
                            <li class="breadcrumb-item active" aria-current="page">Sub Course List</li>
                        </ol>
                    </nav>
                </div>
            </div>

            <!-- Card and Table -->
            <div class="card">
                <div class="card-header d-flex align-items-center justify-content-between flex-wrap row-gap-3">
                    <h5 class="mb-0">Sub Course List</h5>
                </div>
                <div class="card-body">
                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered mt-4" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="SubCourse ID">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("SubCourseId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Master Course">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("MasterCourse") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sub Course Title">
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Thumbnail">
                                <ItemTemplate>
                                    <img src='<%# Eval("Thumbnail") %>' alt="Thumbnail" style="width: 60px; height: auto;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Price">
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("Price") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rating">
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("Rating") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditSc" CommandArgument='<%# Eval("SubCourseId") %>' CssClass="me-2" ToolTip="Edit">
                                        <i class="bi bi-pencil-square"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteSc" CommandArgument='<%# Eval("SubCourseId") %>' ToolTip="Delete">
                                        <i class="bi bi-trash3 text-danger"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div>
<!-- Modal -->
<div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-body">
          <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
          <asp:UpdatePanel ID="UpdatePanel1" runat="server">
              <ContentTemplate>
                  <asp:HiddenField ID="HiddenField1" runat="server"/>
                  <div class="mb-3">
                    <label for="txtMasterCourse" class="form-label">Master Course</label>
                    <asp:TextBox ID="txtMasterCourse" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>   
                </div>
                  <div class="mb-3">
                    <label for="txtTitle" class="form-label">Title</label>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control" placeholder="Enter Title"></asp:TextBox>
                  </div>
                  <div class="mb-3">
                      <label for="txtThumbnail" class="form-label">Thumbnail</label>
                      <asp:HiddenField ID="HiddenField2" runat="server"/>
                      <asp:TextBox ID="txtThumbnail" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox><br />
                      <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control w-100" />
                  </div>
                  <div class="mb-3">
                  <label for="txtPrice" class="form-label">Price</label>
                  <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" placeholder="Enter Price"></asp:TextBox>
                </div>
                <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnUpdate_Click"/>
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-secondary" OnClick="btnBack_Click"/>
                
              </ContentTemplate>
              <Triggers>
                <asp:PostBackTrigger ControlID="btnUpdate" />
            </Triggers>
          </asp:UpdatePanel>
      </div>
    </div>
  </div>
</div>
        </div>
    </form>

</asp:Content>
