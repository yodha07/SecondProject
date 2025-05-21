<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="Topics.aspx.cs" Inherits="SecondProject.User.Topics" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://cdn.tailwindcss.com"></script>
    <style>
        .active-video {
            font-weight: bold;
            color: #3b82f6;
        }

        html, body {
            height: 100%;
            margin: 0;
            padding: 0;
            background: linear-gradient(to bottom left, #e2e8f0, #0ea5e9);
            background-repeat: no-repeat;
            background-attachment: fixed;
            background-size: cover;
        }

        textarea {
            resize: none;
        }
    </style>
    <form runat="server">
        <asp:Panel ID="Panel1" runat="server">
            <!-- MAIN FLEX CONTAINER -->
            <div class="flex flex-row flex-wrap md:flex-nowrap gap-4 p-8">

                <!-- VIDEO SECTION -->
                <div class="flex-1 p-4 bg-[#bae8fd] shadow-lg rounded-lg">
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                    <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                    <asp:MultiView ID="mvTopics" runat="server"></asp:MultiView>

                    <div class="mt-4 flex justify-between">
                        <asp:Button ID="btnPrev" runat="server" Text="Previous" OnClick="btnPrev_Click"
                            CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300" />
                        <asp:Button ID="btnNext" runat="server" Text="Next" OnClick="btnNext_Click"
                            CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300" />
                    </div>

                    <div class="mt-6 flex space-x-4">
                        <asp:Button ID="btnAssignments" runat="server" Text="Assignments" CommandArgument='<%# Eval("SubCourseId") %>' OnCommand="Assignments_Click"
                            CssClass="flex-1 bg-green-600 text-white py-2 rounded-md hover:bg-green-700 transition" />
                        <asp:Button ID="btnMCQ" runat="server" Text="MCQs" CommandArgument='<%# Eval("SubCourseId") %>' OnCommand="Mcq_Click"
                            CssClass="flex-1 bg-purple-600 text-white py-2 rounded-md hover:bg-purple-700 transition" />
                        <asp:LinkButton ID="btnCertificate" runat="server" Text="Certificate" OnCommand="Certificate_Click"
                            CssClass="flex-1 bg-yellow-500 text-white py-2 rounded-md hover:bg-yellow-600 transition text-center" />
                    </div>
                </div>

                <!-- PLAYLIST SECTION -->
                <div class="w-full md:w-1/3 p-4 bg-[#bae8fd] shadow-lg rounded-lg">
                    <h4 class="text-2xl font-semibold text-blue-600 mb-4">Playlist</h4>
                    <asp:Repeater ID="rptPlaylist" runat="server">
                        <ItemTemplate>
                            <div class="playlist-item mb-2 underline">
                                <asp:Label ID="lnkPlay" runat="server" CommandArgument='<%# Container.ItemIndex %>' OnCommand="PlayVideo_Command"
                                    CssClass='<%# (Container.ItemIndex == mvTopics.ActiveViewIndex) ? "text-lg text-blue-600 font-bold" : "text-lg text-gray-800 hover:text-red-500" %>'>
                                    <%# Eval("Title") %>
                                </asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <asp:Panel ID="pnlReview" runat="server" CssClass="bg-white p-6 rounded-xl shadow-md max-w-2xl mx-auto my-6 justify-start">
                    <asp:Label ID="lblReview" runat="server" Text="Leave a Review:" CssClass="block text-lg font-semibold text-gray-700 mb-2 " />

                    <asp:TextBox
                        ID="txtReview"
                        runat="server"
                        TextMode="MultiLine"
                        Rows="5"
                        Columns="50"
                        CssClass="w-full p-3 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 resize-none mb-4" />

                    <asp:Button
                        ID="btnSubmitReview"
                        runat="server"
                        Text="Submit"
                        OnClick="btnSubmitReview_Click"
                        CssClass="bg-blue-600 text-white px-5 py-2 rounded-lg hover:bg-blue-700 transition-all duration-200 font-medium" />
                </asp:Panel>


            </div>
        </asp:Panel>
    </form>
</asp:Content>
