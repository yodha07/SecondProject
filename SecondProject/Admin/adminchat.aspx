<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Admin.Master" AutoEventWireup="true" CodeBehind="adminchat.aspx.cs" Inherits="SecondProject.Admin.adminchat" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <style>
        body {
            margin: 0;
            font-family: 'Roboto', sans-serif;
            background-color: #e5e5e5;
            height: 100vh;
            overflow: hidden;
        }

        .container {
            display: inline-flex;
            height: 92vh;
            max-width:2000px;
            padding-top: 80px;
        }

        .sidebar {
            width: 250px;
            background-color: #2a2f4a;
            color: white;
            overflow-y: auto;
            padding: 20px 0;
            height: 92vh;
        }

        .user {
            display: block;
            padding: 15px 20px;
            cursor: pointer;
            border-bottom: 1px solid #3d425c;
            text-decoration: none;
            color: white;
        }

        .user:hover, .user.active {
            background-color: #3d425c;
        }

        

        .chat-area {
            flex: 1;
            display: flex;
            flex-direction: column;
            background-color: #f0f2f7;
            height: 92vh;
        }

        .chat-header {
            background-color: #ffffff;
            padding: 20px;
            border-bottom: 1px solid #ddd;
            font-weight: 500;
        }

        .messages {
            flex: 1;
            padding: 20px;
            overflow-y: auto;
        }

        .message {
            margin-bottom: 15px;
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
            padding: 15px;
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
        .mt-3, .my-3{
         margin-top: -1rem !important;
        }

    </style>
    <form id="form1" runat="server">
    <div>
    
    <div class="container">
        <!-- Sidebar -->
        <div class="sidebar">
            <asp:Repeater ID="rptUsers" runat="server" DataSourceID="SqlDataSource1" OnItemCommand="rptUsers_ItemCommand">
                <ItemTemplate>
                    <asp:LinkButton runat="server"
                                    CommandName="SelectUser"
                                    CommandArgument='<%# Eval("FullName") %>'
                                    CssClass="user">
                        <%# Eval("FullName") %>
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:Repeater>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ELearning_ProjectConnectionString %>" ProviderName="<%$ ConnectionStrings:ELearning_ProjectConnectionString.ProviderName %>" SelectCommand="SELECT [FullName] FROM [User]"></asp:SqlDataSource>
        </div>

        <!-- Chat Area -->
        
        <div class="chat-area">
            <div class="chat-header">
                <asp:Label ID="lblChatHeader" runat="server" Text="Select a user to chat"></asp:Label>
                <asp:Button ID="Button2" runat="server" PostBackUrl="~/userchatpop.aspx" Text="userchat" />
            </div>
   

            <!-- Scrollable messages area -->
            <div class="messages" id="chatMessages">
                <asp:Repeater ID="rptChatMessagess" runat="server">
       <ItemTemplate>
           <!-- User Message: only if Send and SendTime exist -->
           <asp:PlaceHolder ID="phUserMsg" runat="server" 
               Visible='<%# !string.IsNullOrEmpty(Eval("Send")?.ToString()) && Eval("SendTime") != DBNull.Value %>'>
               <div class="message">
                   <div class="message-text"><%# Eval("Send") %></div>
                   <div class="message-time"><%# Eval("SendTime", "{0:g}") %></div>
               </div>
           </asp:PlaceHolder>

           <!-- Admin Reply: only if Reply and ReplyTime exist -->
           <asp:PlaceHolder ID="phReplyMsg" runat="server" 
               Visible='<%# !string.IsNullOrEmpty(Eval("Reply")?.ToString()) && Eval("ReplyTime") != DBNull.Value %>'>
               <div class="message you">
                   <div class="message-text"><%# Eval("Reply") %></div>
                   <div class="message-time"><%# Eval("ReplyTime", "{0:g}") %></div>
               </div>
           </asp:PlaceHolder>
       </ItemTemplate>
   </asp:Repeater>
               </div>
            
            <!-- Fixed bottom input -->
            <div class="input-area">
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="Send" OnClick="Button1_Click" ForeColor="#000099" Width="125px" Style="border-radius:8px;" BackColor="#99CCFF" BorderStyle="None" />
                
            </div>
        </div>
               
    </div>


        </div>
        </form>
   <script type="text/javascript">
       function scrollToBottom() {
           var chatDiv = document.getElementById("chatMessages");
           if (chatDiv) {
               chatDiv.scrollTop = chatDiv.scrollHeight;
           }
       }

       // Optional: auto-scroll on page load
       window.onload = function () {
           scrollToBottom();
       };
   </script>

</asp:Content>

