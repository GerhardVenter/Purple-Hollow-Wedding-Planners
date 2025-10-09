<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Budget.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Budget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  <div class="budget-page">

    <section class="budget-col">
        <h2>Budget</h2>
        <!-- put cards/table/controls here -->
        <div class="budget-kpis">
          <div class="kpi-card">
            <div class="kpi-title">Total Budget</div>
            <div class="kpi-value">R50 000</div>
          </div>
          <div class="kpi-card">
            <div class="kpi-title">Total Spent</div>
            <div class="kpi-value">R18 500</div>
          </div>
          <div class="kpi-card">
            <div class="kpi-title">Remaining</div>
            <div class="kpi-value">R31 500</div>
          </div>
        </div>

        <div class="table-card">
          <!-- put your table here -->
          <!-- header row, rows, etc. -->
        </div>

        <!-- Divider -->
        <div class="plain-divider"></div>

        <!-- RemoveB Button -->
        <div class="budget-actions">
            <button type="button" id="btnRemoveItem" class="add-button-cust">Remove item</button>
        </div> 

    </section>

    <aside class="chart-col">

        <h2>Costs Pie Chart</h2>
        <!-- your chart canvas/control goes here -->
        <!-- <canvas id="costChart"></canvas> -->
        <div class="panel-card">
          <!-- chart canvas / legend goes here -->
        </div>

        <div class="plain-divider"></div>

        <div class="budget-actions right">
            <asp:Button ID="btnBudgetHelp" runat="server" CssClass="help-button" Text="Need help?" OnClick="btnBudgetHelp_Click" />
        </div>

    </aside>

  </div>

</asp:Content>
