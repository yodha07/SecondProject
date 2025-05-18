<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="UserDashBoard.aspx.cs" Inherits="SecondProject.User.UserDashBoard" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
<style>
    body {
        background-color: #f8f9fa;
    }
    .card {
        box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        border-radius: 10px;
    }
    .dashboard-title {
        font-size: 2rem;
        font-weight: bold;
        margin-bottom: 30px;
        text-align: center;
    }
</style>
        <form id="form1" runat="server">
      <div class="container py-5">
            <div class="dashboard-title">User Dashboard</div>

            <!-- Course Counts -->
            <div class="row text-center mb-4">
                <div class="col-md-4">
                    <div class="card p-4 bg-primary text-white">
                        <h5>Total Courses</h5>
                        <asp:DataList ID="DataList1" runat="server">
                            <ItemTemplate>
                                <h2><%# Eval("count") %></h2>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card p-4 bg-success text-white">
                        <h5>Completed Courses</h5>
                        <asp:DataList ID="DataList2" runat="server">
                            <ItemTemplate>
                                <h2><%# Eval("count") %></h2>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card p-4 bg-danger text-white">
                        <h5>Incomplete Courses</h5>
                        <asp:DataList ID="DataList3" runat="server">
                            <ItemTemplate>
                                <h2><%# Eval("count") %></h2>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </div>
            </div>

            <!-- Labels -->
            <div class="text-center mb-4">
                <div class="text-center mb-4">
    <asp:Label ID="Label1" runat="server" CssClass="alert alert-warning fw-bold fs-5 d-inline-block px-4 py-2 shadow-sm rounded-pill"></asp:Label>
</div>

                <%--<asp:Label ID="Label1" runat="server" CssClass="h5 d-block text-secondary mb-2"></asp:Label>--%>
                <asp:Label ID="Label2" runat="server" CssClass="h6 text-muted"></asp:Label>
            </div>

            <!-- Pie Chart -->
            <div class="card mb-4 p-4">
                <h5 class="mb-3">Course Status Chart</h5>
                <asp:Chart ID="Chart1" runat="server" Width="600px" Height="400px">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Pie" XValueMember="Status" YValueMembers="UserCount" />
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" />
                    </ChartAreas>
                </asp:Chart>
            </div>

            <!-- Progress Bar -->
            <div class="card p-4">
                <h5 class="mb-3">Overall Completion Progress</h5>
                <div class="progress" style="height: 30px;">
                    <div id="progressBar" runat="server" class="progress-bar bg-success" role="progressbar" style="width: 0%">
                        <asp:Label ID="label4" runat="server" CssClass="text-white ms-2"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="updatepanel1" runat="server" >
                <ContentTemplate>
                              <asp:DataList ID="DataList4" runat="server" OnItemCommand="DataList4_ItemCommand">
    <ItemTemplate>
        <h3>Subcourses</h3>
        <p><%# Eval("SubCourseTitle") %></p>

        <div class="card p-4 shadow rounded">
            <h5 class="mb-3">Overall Completion Progress</h5>
            
            <!-- 🔹 Progress Bar -->
            <div class="progress mb-2" style="height: 30px;">
                <div id="progressBar1" runat="server" class="progress-bar bg-success" role="progressbar" style="width: 0%">
                    <asp:Label ID="label3" runat="server" CssClass="text-white ms-2"></asp:Label>
                </div>
            </div>

            <!-- 🔹 Small Button Inside Card -->
            <asp:Button 
                ID="Button1" 
                runat="server" 
                CommandName="Select" 
                CommandArgument='<%# Eval("SubCourseId") %>' 
                Text="Show Progress" 
                CssClass="btn btn-sm btn-primary" />
        </div>
    </ItemTemplate>
</asp:DataList>
                </ContentTemplate>
            </asp:UpdatePanel>
         


        </div>
    </form>
</asp:Content>
