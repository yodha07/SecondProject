<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="emptyCart.aspx.cs" Inherits="SecondProject.User.emptyCart" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        
        <style>
        .empty-cart {
            min-height: 60vh;
            display: flex;
            justify-content: center;
            align-items: center;
        }
        .cart-box {
            text-align: center;
            padding: 40px;
            border-radius: 12px;
            box-shadow: 0 0 20px rgba(0,0,0,0.1);
            background-color: #fff;
        }
        .cart-icon {
            font-size: 50px;
            color: #888;
        }
    </style>
        
          <div class="container empty-cart">
            <div class="cart-box col-md-6 mx-auto">
                <div class="cart-icon mb-3">
                    <i class="bi bi-cart-x-fill"></i> <!-- Bootstrap Icon -->
                </div>
                <h3>Your cart is empty</h3>
                <p class="text-muted">Looks like you haven’t added anything to your cart yet.</p>
                <asp:Button ID="btnContinue" runat="server" Text="Continue Shopping" CssClass="btn btn-primary mt-3" OnClick="btnContinue_Click"  />
            </div>
    

    <!-- Bootstrap JS and Icons -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.js"></script>

    </div>

</asp:Content>
