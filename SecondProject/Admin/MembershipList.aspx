<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="MembershipList.aspx.cs" Inherits="SecondProject.Admin.MembershipList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" integrity="sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN" crossorigin="anonymous"></script>
<script src="https://cdn.jsdelivr.net/npm/popper.js@1.12.9/dist/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js" integrity="sha384-JZR6Spejh4U02d8jOt6vLEHfe/JQGiRRSQQxSfFWpi1MquVdAyjUar5+76PVCmYl" crossorigin="anonymous"></script>
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
<link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" rel="stylesheet" />
<style>
    .topic-header {
        background-color: #549bff;
    }

    .topic-title {
        color: white;
        font-weight: bold;
        margin: 0;
    }
</style>

        <form id="form1" runat="server">
        <div class="container mt-5">
            <!-- Header and Breadcrumb -->
            <div class="d-md-flex d-block align-items-center justify-content-between page-breadcrumb mb-3">
                <div class="my-auto mb-2">
                    <h3 class="mb-1">Membership List</h3>
                    <nav>
                        <ol class="breadcrumb mb-0">
                            <li class="breadcrumb-item">
                                <a href="AdminDashboard.aspx"><i class="bi bi-house"></i></a>
                            </li>
                            <li class="breadcrumb-item">Admin</li>
                            <li class="breadcrumb-item active" aria-current="page">Membership List</li>
                        </ol>
                    </nav>
                </div>
            </div>

            <!-- Membership Cards -->
            <div class="row">
                <asp:DataList ID="DataList1" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" OnItemDataBound="DataList1_ItemDataBound" OnDeleteCommand="DataList1_DeleteCommand" OnEditCommand="DataList1_EditCommand">
                    <ItemTemplate>
                        <div class="col mb-4">
                            <div class="card h-100">
                                <div class="card-header topic-header">
                                    <h5 class="topic-title"><%# Eval("PlanName") %></h5>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("PlanId") %>' CssClass="me-2" ToolTip="Edit">
                                        <i class="bi bi-pencil-square"></i>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("PlanId") %>' ToolTip="Delete">
                                        <i class="bi bi-trash3 text-danger"></i>
                                    </asp:LinkButton>
                                </div>
                                <div class="card-body p-0">
                                    <ul class="list-group list-group-flush">
                                        <asp:DataList ID="DataList2" runat="server">
                                            <ItemTemplate>
                                                <li class="list-group-item"><%# Eval("Title") %></li>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </ul>
                                </div>
                                <div class="card-header topic-header">
                                    <h5 class="topic-title"><%# Eval("Price") %></h5>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
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
                    <label for="DropDownList2" class="form-label">Master Course</label>
                    <asp:DropDownList ID="DropDownList2" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                  <div class="mb-3">
                    <label for="DropDownList1" class="form-label">Select Membership Tier</label>
                    <asp:DropDownList ID="DropDownList1" runat="server" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="mb-3">
                    <label for="CheckBoxList1" class="form-label">Select Sub Course</label>
                    <asp:CheckBoxList ID="CheckBoxList1" runat="server" CssClass="form-check"></asp:CheckBoxList>
                </div>
                <div class="mb-3">
                    <label for="txtPrice" class="form-label">Price</label>
                    <asp:TextBox ID="txtPrice" CssClass="form-control" placeholder="Enter Price" runat="server"></asp:TextBox>
                </div>
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
