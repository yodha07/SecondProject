<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="course_Membership.aspx.cs" Inherits="SecondProject.User.course_Membership" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

       <div style="height: 80vh; overflow-y: auto; padding: 20px;">
    <asp:DataList ID="DataList1" runat="server" 
                  RepeatColumns="4" RepeatDirection="Horizontal"
                  OnItemCommand="DataList1_ItemCommand"
                  OnItemDataBound="DataList1_ItemDataBound"
                  CellPadding="10">
        <ItemTemplate>
            <!-- CHANGED: Added fixed width to cards for layout consistency -->
            <div class="col mb-4" style="width: 260px;">
                <div class="card h-100 shadow rounded-3">
                    <div class="card-header bg-primary text-white text-center">
                        <h5 class="topic-title mb-0"><%# Eval("PlanName") %></h5>
                    </div>

                    <!-- CHANGED: Applied fixed height + scroll to DataList2 container -->
                    <div class="card-body p-2" style="max-height: 180px; overflow-y: auto;">
                        <h6 class="text-center mb-2"><strong>SubCourses Included</strong></h6>
                        <ul class="list-group list-group-flush">
                            <asp:DataList ID="DataList2" runat="server" OnItemDataBound="DataList2_ItemDataBound" RepeatDirection="Vertical">
                                <ItemTemplate>
                                    <!-- CHANGED: Added numbering using Container.ItemIndex -->
                                    <li class="list-group-item py-1 px-2" runat="server">
                                        <%# (Container.ItemIndex + 1).ToString() + ". " + Eval("Title") %>
                                    </li>
                                </ItemTemplate>
                            </asp:DataList>
                        </ul>
                    </div>

                    <div class="card-footer bg-light text-center">
                        <h6 class="mb-1"><strong>Total Duration:</strong> <%# Eval("TotalDuration") %></h6>
                        <h6 class="mb-2"><strong>Price: ₹</strong> <%# Eval("Price") %></h6>
                        <asp:Button ID="Button1" runat="server" Text="Buy Now"
                                    CssClass="btn btn-primary"
                                    CommandName="buynow"
                                    CommandArgument='<%# Eval("PlanId") %>'
                                    OnClientClick="return confirm('Do You Want Make Payment For This Item?');" />
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:DataList>
</div>
</asp:Content>
