<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="MCQs.aspx.cs" Inherits="SecondProject.User.MCQs" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/tailwindcss@2.0.3/dist/tailwind.min.css" rel="stylesheet">

<style>
    body {
        background: linear-gradient(to bottom left, #e2e8f0, #0ea5e9);
        height: 100vh;
    }
</style>
    <form runat="server">
        <div class="max-w-3xl mx-auto mt-10 p-6 bg-white rounded-xl shadow-md">
            <h1 class="text-2xl font-bold text-gray-800 mb-6 border-b pb-2">MCQs for
            <asp:Label ID="TitleLabel" runat="server" CssClass="text-indigo-600 font-semibold" />
            </h1>

            <asp:Repeater runat="server" ID="rptMcqs">
                <ItemTemplate>
                    <div class="mb-6 p-4 border rounded shadow bg-white" id="mcqsDiv">
                        <p class="font-semibold text-lg mb-2"><%# Eval("Question") %></p>
                        <div class="flex flex-col gap-2">
                            <asp:RadioButton ID="optA" runat="server" GroupName='<%# "question_" + Container.ItemIndex %>' Text='<%# Eval("OptionA") %>' CssClass="text-gray-700" />
                            <asp:RadioButton ID="optB" runat="server" GroupName='<%# "question_" + Container.ItemIndex %>' Text='<%# Eval("OptionB") %>' CssClass="text-gray-700" />
                            <asp:RadioButton ID="optC" runat="server" GroupName='<%# "question_" + Container.ItemIndex %>' Text='<%# Eval("OptionC") %>' CssClass="text-gray-700" />
                            <asp:RadioButton ID="optD" runat="server" GroupName='<%# "question_" + Container.ItemIndex %>' Text='<%# Eval("OptionD") %>' CssClass="text-gray-700" />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Button ID="btnSubmitMCQs" runat="server" Text="Submit Answers" CssClass="bg-blue-600 text-white px-4 py-2 rounded" OnClick="btnSubmitMCQs_Click" />
        </div>
    </form>
</asp:Content>

