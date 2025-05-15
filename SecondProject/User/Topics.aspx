<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="Topics.aspx.cs" Inherits="SecondProject.User.Topics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <link href="https://cdn.jsdelivr.net/npm/tailwindcss@2.0.3/dist/tailwind.min.css" rel="stylesheet">
    <style>
        
        .active-video {
            font-weight: bold;
            color: #3b82f6; 
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="flex justify-evenly bg-white py-8">
        
        <div class="flex-1 max-w-3xl p-6 bg-white shadow-lg rounded-lg px-15">
            <asp:MultiView ID="mvTopics" runat="server"></asp:MultiView>
            <div class="mt-4 flex justify-between">
                <asp:Button ID="btnPrev" runat="server" Text="Previous" OnClick="btnPrev_Click" class="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300" />
                <asp:Button ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" class="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300" />
            </div>
        </div>

       
        <div class="w-1/3 max-w-xs ml-8 p-6 bg-white shadow-lg rounded-lg px-15">
            <h4 class="text-2xl font-semibold text-blue-600 mb-4">Playlist</h4>
            <asp:Repeater ID="rptPlaylist" runat="server">
                <ItemTemplate>
                    <div class="playlist-item mb-2 underline">
                        <asp:LinkButton ID="lnkPlay" runat="server" CommandArgument='<%# Container.ItemIndex %>' OnCommand="PlayVideo_Command" 
                            class="text-lg text-gray-800 hover:text-blue-500 <%# (Container.ItemIndex == mvTopics.ActiveViewIndex) ? 'active-video' : '' %> transition duration-300">
                            <%# Eval("Title") %>
                        </asp:LinkButton>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</asp:Content>
