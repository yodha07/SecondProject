<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reason_form.aspx.cs" Inherits="SecondProject.Login.reason_form" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Submit Reason</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .form-card {
            max-width: 500px;
            margin: 100px auto;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
            background-color: #ffffff;
        }

        .form-title {
            font-size: 1.5rem;
            font-weight: bold;
            text-align: center;
            margin-bottom: 25px;
        }

        .form-label {
            font-weight: 500;
        }
    </style>
</head>
<body style="background-color: #f8f9fa;">
    <form id="form1" runat="server">
        <div class="form-card">
            <div class="form-title">📋 Submit Your Grievance</div>

            <div class="mb-3">
                <label class="form-label">Name</label>
                <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control" placeholder="Enter your name"></asp:TextBox>
            </div>

            <div class="mb-3">
                <label class="form-label">Email</label>
                <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" TextMode="Email" placeholder="Enter your email"></asp:TextBox>
            </div>

            <div class="mb-3">
                <label class="form-label">Reason</label>
                <asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="4" placeholder="Describe your reason..."></asp:TextBox>
            </div>

            <div class="d-grid">
                <asp:Button ID="Button1" runat="server" Text="Save" CssClass="btn btn-primary btn-block" OnClick="Button1_Click" />
                
            </div>
        </div>
    </form>
</body>
</html>