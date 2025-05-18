<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="admindashboard.aspx.cs" Inherits="SecondProject.Admin.admindashboard" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
 <style>
     .card {
         border-radius: 12px;
         box-shadow: 0 0 15px rgba(0, 0, 0, 0.05);
     }
     .dashboard-title {
         font-size: 2rem;
         font-weight: 600;
         margin-bottom: 30px;
         text-align: center;
     }
     .chart-container {
         background: #ffffff;
         padding: 20px;
         border-radius: 12px;
         margin-bottom: 30px;
         box-shadow: 0 0 15px rgba(0, 0, 0, 0.05);
     }
 </style>
        <form id="form1" runat="server">
        
              <div class="container py-5">
            <div class="dashboard-title">📊 Admin Dashboard</div>

            <!-- Stats Section -->
            <div class="row mb-4 text-center">
                <div class="col-md-3 mb-3">
                    <div class="card bg-success text-white p-4">
                        <h6>Active Users</h6>
                        <asp:DataList ID="DataList1" runat="server">
                            <ItemTemplate>
                                <h2><%# Eval("active") %></h2>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </div>
                <div class="col-md-3 mb-3">
                    <div class="card bg-warning text-dark p-4">
                        <h6>InActive Users</h6>
                        <asp:DataList ID="DataList2" runat="server">
                            <ItemTemplate>
                                <h2><%# Eval("inactive") %></h2>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </div>
                <div class="col-md-3 mb-3">
                    <div class="card bg-primary text-white p-4">
                        <h6>Total Master Courses</h6>
                        <asp:DataList ID="DataList3" runat="server">
                            <ItemTemplate>
                                <h2><%# Eval("count") %></h2>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </div>
                <div class="col-md-3 mb-3">
                    <div class="card bg-info text-white p-4">
                        <h6>Total Sub Courses</h6>
                        <asp:DataList ID="DataList4" runat="server">
                            <ItemTemplate>
                                <h2><%# Eval("count") %></h2>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </div>
            </div>

            <!-- Column Chart -->
<%--                  <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>--%>
                  <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                  <asp:UpdatePanel runat="server" ID ="panel1">
                      <ContentTemplate>
       <div class="chart-container bg-white p-4 rounded shadow-sm border mb-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h5 class="text-primary fw-bold mb-0">
            <i class="bi bi-bar-chart-line-fill me-2"></i>Monthly Sales Chart
        </h5>

        <div class="w-25">
            <asp:DropDownList 
                ID="DropDownList2" 
                runat="server" 
                DataTextField="year" 
                DataValueField="year" 
                OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged" 
                AutoPostBack="True"
                CssClass="form-select border-primary shadow-sm">
            </asp:DropDownList>
        </div>
    </div>

    <asp:Chart 
        ID="Chart1" 
        runat="server" 
        Width="1000px" 
        Height="400px" 
        CssClass="bg-light p-2 rounded shadow-sm">
        <series>
            <asp:Series 
                Name="Sales" 
                ChartType="Column" 
                XValueMember="month" 
                YValueMembers="count" 
                ToolTip="#VALX: $#VALY" />
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1">
        <AxisX Title="Month" TitleFont="Arial, 10pt, style=Bold" />
        <AxisY Title="Sales Amount" TitleFont="Arial, 10pt, style=Bold" LabelStyle-ForeColor="DarkGreen" />
    </asp:ChartArea>
        </chartareas>
    </asp:Chart>
</div>

                      </ContentTemplate>
                  </asp:UpdatePanel>
           

            <!-- Pie Chart -->
            <div class="chart-container">
                <h5 class="mb-3">🧮 User Status Pie Chart</h5>
                <asp:Chart ID="Chart2" runat="server" Width="1000px" Height="400px">
                    <series>
                        <asp:Series Name="user" ChartType="Pie" XValueMember="Status" YValueMembers="UserCount" 
           
            ToolTip="#VALX: #VALY user (#PERCENT{P1})"/>
                    </series>
                    <chartareas>
                        <asp:ChartArea Name="ChartArea1" />
                    </chartareas>
                </asp:Chart>
            </div>
                  
                  <asp:UpdatePanel runat ="server" >
                      <ContentTemplate>
                           <div class="mb-4">
     <label class="form-label fw-bold">🔽 Filter By:</label>
     <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" CssClass="form-select w-25 mb-3"  
         OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
     </asp:DropDownList>

     <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-striped"></asp:GridView>
 </div>
                      </ContentTemplate>
                  </asp:UpdatePanel>
            <!-- Dropdown & Grid -->
           
        </div>

    </form>
</asp:Content>

