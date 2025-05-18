<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="AssignmentList.aspx.cs" Inherits="SecondProject.Admin.AssignmentList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" integrity="sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN" crossorigin="anonymous"></script>
<script src="https://cdn.jsdelivr.net/npm/popper.js@1.12.9/dist/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js" integrity="sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl" crossorigin="anonymous"></script>
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
<link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" rel="stylesheet" />
<style>
    #AssignmentList th,
    #AssignmentList td,
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
                    <h3 class="mb-1">Assignment List</h3>
                    <nav>
                        <ol class="breadcrumb mb-0">
                            <li class="breadcrumb-item">
                                <a href="AdminDashboard.aspx"><i class="bi bi-house"></i></a>
                            </li>
                            <li class="breadcrumb-item">Admin</li>
                            <li class="breadcrumb-item active" aria-current="page">Assignment List</li>
                        </ol>
                    </nav>
                </div>
            </div>

            <!-- Card and Table -->
            <div class="card">
                <div class="card-header d-flex align-items-center justify-content-between flex-wrap row-gap-3">
                    <h5 class="mb-0">Assignment List</h5>
                </div>
                <div class="card-body">
                    <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered mt-4" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Assignment ID">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("AssignmentId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Master Course">
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# Eval("MasterCourse") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sub Course">
                                <ItemTemplate>
                                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("SubCourse") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Topic">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Assignment">
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("Assignment") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnView" runat="server" CommandName="ViewAs" CommandArgument='<%# Eval("AssignmentId") %>' CssClass="icon-btn me-2" ToolTip="View">
                                        <i class="bi bi-eye"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditAs" CommandArgument='<%# Eval("AssignmentId") %>' CssClass="me-2" ToolTip="Edit">
                                        <i class="bi bi-pencil-square"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="DeleteAs" CommandArgument='<%# Eval("AssignmentId") %>' ToolTip="Delete">
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
                    <label for="txtSubCourse" class="form-label">Sub Course</label>
                    <asp:TextBox ID="txtSubCourse" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>   
                </div>
                  <div class="mb-3">
                    <label for="txtTopic" class="form-label">Topic</label>
                    <asp:TextBox ID="txtTopic" runat="server" CssClass="form-control"  ReadOnly="true"></asp:TextBox>
                  </div>
                  <div class="mb-3">
                    <label for="txtAssignment" class="form-label">Assignment</label>
                    <asp:HiddenField ID="HiddenField2" runat="server"/>
                    <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control w-100" />
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
