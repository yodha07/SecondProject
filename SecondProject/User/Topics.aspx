<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="Topics.aspx.cs" Inherits="SecondProject.User.Topics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://cdn.tailwindcss.com"></script>
    <style>
        .active-video {
            font-weight: bold;
            color: #3b82f6;
        }

        body {
            background: linear-gradient(to bottom left, #e2e8f0, #0ea5e9);
            height: 100vh;
        }

        html, body {
            height: 100%;
            margin: 0;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <form runat="server">

            <div class="flex justify-evenly py-8 ">

                <div class="flex-1 max-w-3xl p-6 bg-[#bae8fd] shadow-lg rounded-lg px-15">
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                    <asp:MultiView ID="mvTopics" runat="server"></asp:MultiView>
                    <div class="mt-4 flex justify-between">
                        <asp:Button ID="btnPrev" runat="server" Text="Previous" OnClick="btnPrev_Click" class="px-4 py-2 my-10 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300" />
                        <asp:Button ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click" class="px-4 py-2 my-10 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300" />
                    </div>


                    <div class="justify-center max-w-xs items-center flex flex-row space-x-4">
                        <asp:Button ID="btnAssignments" runat="server" Text="Assignments" CommandArgument='<%# Eval("SubCourseId") %>' OnCommand="Assignments_Click"
                            CssClass="w-full bg-green-600 text-white py-2 rounded-md hover:bg-green-700 transition" />
                        <asp:Button ID="btnMCQ" runat="server" Text="MCQs" CommandArgument='<%# Eval("SubCourseId") %>' OnCommand="Mcq_Click"
                            CssClass="w-full bg-purple-600 text-white py-2 rounded-md hover:bg-purple-700 transition" />
                        <asp:LinkButton ID="btnCertificate" runat="server" Text="Certificate" OnCommand="Certificate_Click"
                            CssClass="w-full bg-yellow-500 text-white py-2 rounded-md hover:bg-yellow-600 transition text-center">
                        </asp:LinkButton>
                    </div>
                </div>

                <div class="w-1/3 max-w-xs ml-5 p-6 bg-[#bae8fd] shadow-lg rounded-lg px-15">
                    <h4 class="text-2xl font-semibold text-blue-600 mb-4">Playlist</h4>
                    <asp:Repeater ID="rptPlaylist" runat="server">
                        <ItemTemplate>
                            <div class="playlist-item mb-2 underline">
                                <asp:LinkButton ID="lnkPlay" runat="server" CommandArgument='<%# Container.ItemIndex %>' OnCommand="PlayVideo_Command"
                                    class="text-lg text-gray-800 hover:text-red-500 <%# (Container.ItemIndex == mvTopics.ActiveViewIndex) ? 'active-video' : '' %> transition duration-300">
                <%# Eval("Title") %>
                                </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </form>
</asp:Content>
