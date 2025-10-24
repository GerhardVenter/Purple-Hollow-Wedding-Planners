<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Itinerary_Timeline.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Itinerary_Timeline" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">Itinerary Timeline
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Repeater ID="rptTimeline" runat="server">
    <ItemTemplate>
        <div class="timeline-item">
            <strong><%# Eval("EventName") %></strong> — <%# Eval("Description") %>
        </div>
    </ItemTemplate>
</asp:Repeater>

</asp:Content>
