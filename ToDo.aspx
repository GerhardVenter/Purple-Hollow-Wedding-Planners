<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ToDo.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.ToDo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">To-Do List
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
<h2 id="toDoHeading">To-Do List <img src="Images/clipboard.png" alt="picture of clipboard" id="clipboard" /></h2>

<section class="todo-container">
   <div class="myList">
     <form action="">

             <div class="actions">
                 <asp:TextBox ID="txtTaskDescription" runat="server" CssClass="messageAdder" Placeholder="Add your task"></asp:TextBox>
                 <asp:Button ID="btnAddTask" runat="server" CssClass="addMessageButton" Text="Add Task" OnClick="btnAddTask_Click" />
                 <asp:Label ID="lblMsg" runat="server"></asp:Label>
                 </div>
         </form>
       <asp:Table ID="taskTable" runat="server" CssClass="taskTable">
     
       </asp:Table>
       <asp:Button ID="btnHelp" runat="server" Text="Need help?" CssClass="helpButton"/>
       </div>
 
      
    </section>
       
   
</asp:Content>
