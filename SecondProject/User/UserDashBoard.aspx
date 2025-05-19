<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="UserDashBoard.aspx.cs" Inherits="SecondProject.User.UserDashBoard" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <script src="https://cdn.tailwindcss.com"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/css/bootstrap.min.css" crossorigin="anonymous" />
<script src="https://code.jquery.com/jquery-3.2.1.slim.min.js" crossorigin="anonymous"></script>
<script src="https://cdn.jsdelivr.net/npm/popper.js@1.12.9/dist/umd/popper.min.js" crossorigin="anonymous"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.0.0/dist/js/bootstrap.min.js" crossorigin="anonymous"></script>

    <style>
    body {
        margin: 0;
        font-family: 'Roboto', sans-serif;
        background-color: #e5e5e5;
        height: 100vh;
        overflow: hidden;
    }

    .chat-toggle-btn {
        position: fixed;
        bottom: 20px;
        left: 20px;
        background-color: #007bff;
        color: white;
        border: none;
        border-radius: 50%;
        width: 60px;
        height: 60px;
        font-size: 24px;
        cursor: pointer;
        box-shadow: 0 4px 8px rgba(0,0,0,0.2);
        z-index: 9999;
    }

    .chat-modal {
        position: fixed;
        bottom: 90px;
        left: 20px;
        width: 350px;
        height: 500px;
        background-color: #fff;
        border-radius: 10px;
        box-shadow: 0 5px 15px rgba(0,0,0,0.3);
        display: none;
        flex-direction: column;
        z-index: 9998;
        overflow: hidden;
    }

    .chat-area {
        flex: 1;
        display: flex;
        flex-direction: column;
        background-color: #f0f2f7;
        height: 100%;
    }

    .chat-header {
        background-color: #ffffff;
        padding: 15px;
        border-bottom: 1px solid #ddd;
        font-weight: 500;
    }

    .messages {
        flex: 1;
        padding: 15px;
        overflow-y: auto;
    }

    .message {
        margin-bottom: 12px;
    }

    .message.you {
        text-align: right;
    }

    .message-text {
        display: inline-block;
        padding: 10px 15px;
        border-radius: 10px;
        background-color: #ffffff;
        max-width: 60%;
    }

    .message.you .message-text {
        background-color: #d1e7ff;
    }

    .input-area {
        display: flex;
        padding: 12px;
        border-top: 1px solid #ccc;
        background-color: #ffffff;
    }

    .input-area input[type="text"] {
        flex: 1;
        padding: 10px;
        border: 1px solid #ccc;
        border-radius: 20px;
        margin-right: 10px;
    }

    .input-area button {
        padding: 10px 20px;
        border: none;
        background-color: #007bff;
        color: white;
        border-radius: 20px;
        cursor: pointer;
    }

    .input-area button:hover {
        background-color: #0056b3;
    }

    .message-time {
        font-size: 12px;
        color: #888;
        margin-top: 4px;
    }

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

                    <form id="form2" runat="server">
        <!-- Chat Toggle Button -->
        

      
        <button type="button" id="chatToggleBtn" class="chat-toggle-btn" >💬</button>
        <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>

        <asp:UpdatePanel runat="server">
<ContentTemplate>
        <!-- Custom Chat Modal -->
        <div id="chatModal" class="chat-modal">
            
            <div class="chat-area">
                <div class="chat-header">
                    Chat Support
                </div>
                <asp:Label ID="Label5" runat="server" Text="Label"></asp:Label>
                <%--<asp:TextBox ID="TextBox2" runat="server" Text="Jane Smith" CssClass="form-control" />--%>
                <div class="messages" id="chatMessages">
                    
                    <asp:Repeater ID="rptChatMessagess" runat="server">
                        
                        <ItemTemplate>
                            <!-- User Message -->
                            <asp:PlaceHolder ID="phUserMsg" runat="server"
                                Visible='<%# !string.IsNullOrEmpty(Eval("Send")?.ToString()) && Eval("SendTime") != DBNull.Value %>'>
                                <div class="message you">
                                    <div class="message-text"><%# Eval("Send") %></div>
                                    <div class="message-time"><%# Eval("SendTime", "{0:g}") %></div>
                                </div>
                            </asp:PlaceHolder>

                            <!-- Admin Reply -->
                            <asp:PlaceHolder ID="phReplyMsg" runat="server"
                                Visible='<%# !string.IsNullOrEmpty(Eval("Reply")?.ToString()) && Eval("ReplyTime") != DBNull.Value %>'>
                                <div class="message">
                                    <div class="message-text"><%# Eval("Reply") %></div>
                                    <div class="message-time"><%# Eval("ReplyTime", "{0:g}") %></div>
                                </div>
                            </asp:PlaceHolder>
                        </ItemTemplate>
                    </asp:Repeater>

                </div>

                <div class="input-area">
                    <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" placeholder="Type a message..." />
                    <asp:Button ID="Button1" runat="server" Text="Send" OnClick="Button1_Click"
                        ForeColor="#000099" Width="90px"
                        Style="border-radius: 20px;" BackColor="#99CCFF" BorderStyle="None" />

                </div>
                        
            </div>
                    
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
        

    <script>
        // Toggle chat modal visibility
        document.getElementById("chatToggleBtn").addEventListener("click", function () {
            var modal = document.getElementById("chatModal");
            var isHidden = modal.style.display === "none" || modal.style.display === "";

            if (isHidden) {
                modal.style.display = "flex";

                // Scroll to bottom after rendering
                setTimeout(function () {
                    scrollToBottom();
                }, 100);
            } else {
                modal.style.display = "none";
            }
        });

        // Scrolls message container to bottom
        function scrollToBottom() {
            var chatDiv = document.getElementById("chatMessages");
            if (chatDiv) {
                chatDiv.scrollTop = chatDiv.scrollHeight;
            }
        }

        // Ensure modal is hidden on initial load
        window.onload = function () {
            document.getElementById("chatModal").style.display = "none";
        };
    </script>
<%--        <form id="form1" runat="server">--%>
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
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
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
