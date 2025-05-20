<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/User.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SecondProject.User.WebForm1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        <form runat="server">

        <strong>Search By Master Course: </strong> <asp:DropDownList ID="DropDownList1" runat="server"  AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="Title" DataValueField="MasterCourseId" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"   style="padding:6px 12px; border:2px solid #007bff; border-radius:6px; background-color:white  ; color:#003366; font-size:14px; width:250px; outline:none;"></asp:DropDownList>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ELearning_ProjectConnectionString2 %>" SelectCommand="SELECT [MasterCourseId], [Title] FROM [MasterCourse]" ProviderName="<%$ ConnectionStrings:ELearning_ProjectConnectionString2.ProviderName %>"></asp:SqlDataSource>




    <asp:DataList ID="DataList1" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" style="padding:100px" OnItemCommand="DataList1_ItemCommand" >
       <ItemTemplate>
    <div class="card" 
         style="border: 1px solid black ; border-radius: 8px; margin: 10px; 
                box-shadow: 0 4px 6px rgba(0,123,255,0.2); background-color: white; 
                width: 250px; height: 400px; display: flex; flex-direction: column; overflow: hidden;">
        <img class="card-img-top" src='<%# ResolveUrl("~/") + Eval("Thumbnail") %>' alt="Card image cap" 
             style="height: 150px; width: 100%; object-fit: cover; border-bottom: 1px solid #007bff;" />
        <div class="card-body" 
             style="padding: 10px; display: flex; flex-direction: column; justify-content: space-between; flex-grow: 1;">
            <h5 class="card-title" style="font-size: 14px; color: #003366; margin-bottom: 5px;">
                <strong>Course: </strong><%# Eval("Title") %>
            </h5>
            <h5 class="card-title" style="font-size: 14px; color: #003366; margin-bottom: 5px;">
                Total Videos: <%# Eval("TopicCount") %>
            </h5>
            <h5 class="card-title" style="font-size: 14px; color: #003366; margin-bottom: 5px;">
                Total Duration: <%# Eval("TotalDuration") %>
            </h5>
            <h5 class="card-title" style="font-size: 14px; color: #003366; margin-bottom: 5px;">
                Price: <%# Eval("Price") %>
            </h5>
            <h5 class="card-title" style="font-size: 14px; color: #003366; margin-bottom: 5px;">
                Ratings: <%# GetStarsHtml (Eval("Rating")) %> 
            </h5>  
            <asp:Button runat="server" Text="Add To Cart" CommandName="addtocart" CssClass="btn btn-primary" 
                        CommandArgument='<%# Eval("SubCourseId") %>' 
                        style="background-color:#007bff; border:none; border-radius:4px; padding:6px 12px; font-size:14px;" />
        </div>
    </div>   
</ItemTemplate>

</asp:DataList>

</form>




</asp:Content>
