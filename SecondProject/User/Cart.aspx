<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="SecondProject.User.Cart" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <form runat="server">
    
        <style>
            


     <script src="https://cdn.tailwindcss.com"></script>
    <style>

        body {
            overflow: hidden;
        }
    </style>
    <div class="container-fluid" style="background-color: white; min-height: 100vh;">
        <div class="row justify-content-center">

            <h4>Your Cart(<asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>)</h4>
            <!-- Left: Cart Items -->

            <div id="left" class="bg-white p-4 rounded-start shadow"
                style="width: 60vw; margin-right: 35vw; height: 80vh; overflow-y: scroll; overflow-x: hidden">
                <asp:DataList ID="DataList1" runat="server" OnItemCommand="DataList1_ItemCommand">
                    <ItemTemplate>
                        <div class="d-flex bg-light border rounded p-3 mb-4 shadow-sm align-items-center" style="transition: all 0.3s ease; width: 50vw;">
                            <!-- Image -->
                            <div style="flex: 0 0 140px;">
                                <img src='<%# ResolveUrl("~/") + Eval("Thumbnail") %>' alt="Item Image" class="img-fluid rounded border" style="width: 130px; border: 2px solid #000;" />
                            </div>

                            <!-- Info -->
                            <div class="ms-4 flex-grow-1">
                                <h5 class="fw-bold text-dark mb-1"><%# Eval("Title") %></h5>
                                <p class="mb-1 text-secondary"><strong>Price:</strong> <%# Eval("Price") %></p>

                                <p class="mb-0 text-secondary"><strong>Rating:</strong> <%#GetStarsHtml(Eval("Rating")) %></p>
                            </div>

                            <!-- Remove Button -->
                            <div>
                                <asp:Button
                                    ID="removecartbtn"
                                    runat="server" Text="Remove"
                                    CssClass="btn btn-danger btn-sm"
                                    CommandName="dltcart"
                                    CommandArgument='<%# Eval("CartId") %>'
                                    Style="margin-left: 5px"
                                    OnClientClick="return confirm('Are you sure you want to remove this item from the cart?');" />
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>

                <asp:Label ID="Label1" runat="server" Text="" CssClass="text-muted mt-2 d-block"></asp:Label>
            </div>


            <!-- Info -->
            <div class="ms-4 flex-grow-1">
              <h5 class="fw-bold text-dark mb-1"><%# Eval("Title") %></h5>
              <p class="mb-1 text-secondary"><strong>Price:</strong> <%# Eval("Price") %></p>
             
              <p class="mb-0 text-secondary"><strong>Rating:</strong> <%#GetStarsHtml(Eval("Rating")) %></p>

            <!-- Right: Summary -->
            <div id="right" class="bg-white p-4 rounded-end shadow"
                style="position: fixed; top: 60px; right: 0; height: calc(100vh - 60px); width: 35vw;">


                <div class="card border-0">
                    <div class="card-body">
                        <h4 class="card-title mb-4 text-center">Summary</h4>

                        <div class="mb-3 d-flex justify-content-between">
                            <span>Total Items:</span>
                            <asp:Label ID="lblTotalItems" runat="server" CssClass="fw-bold text-primary"></asp:Label>
                        </div>

                        <div class="mb-4 d-flex justify-content-between">
                            <span>Total Price:</span>
                            <asp:Label ID="lblTotalPrice" runat="server" CssClass="fw-bold text-success"></asp:Label>
                        </div>

                        <asp:Button ID="btnPayNow" runat="server" Text="Pay Now" CssClass="btn btn-primary w-100 fw-bold" OnClick="btnPayNow_Click"
                            OnClientClick="return confirm('Are You Sure You Want To Make Payment ');" />
                    </div>
                </div>

            </div>

        </div>
    </div>


  </div>

        </form>

</asp:Content>
