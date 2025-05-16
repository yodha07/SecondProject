<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="UserGreet.aspx.cs" Inherits="SecondProject.User.UserGreet" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
</style>
        <form id="form1" runat="server">
        <!-- Chat Toggle Button -->
        

      
        <button type="button" id="chatToggleBtn" class="chat-toggle-btn" >💬</button>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <asp:UpdatePanel runat="server">
<ContentTemplate>
        <!-- Custom Chat Modal -->
        <div id="chatModal" class="chat-modal">
            
            <div class="chat-area">
                <div class="chat-header">
                    Chat Support
                </div>

                <asp:TextBox ID="TextBox2" runat="server" Text="Jane Smith" CssClass="form-control" />
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
        
    </form>

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
</asp:Content>
