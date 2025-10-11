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
            <div class="kpi-value">
              <asp:Label ID="lblTotalBudget" runat="server" Text="R0" />
            </div>
          </div>

          <div class="kpi-card">
            <div class="kpi-title">Total Spent</div>
            <div class="kpi-value">
              <asp:Label ID="lblTotalSpent" runat="server" Text="R0" />
            </div>
          </div>

          <div class="kpi-card">
            <div class="kpi-title">Remaining</div>
            <div class="kpi-value">
              <asp:Label ID="lblRemaining" runat="server" Text="R0" />
            </div>
          </div>
        </div>

        <!-- Card wrapper -->
        <div class="budget-table-card">
          <div class="budget-table-title">Budget</div>

          <table class="budget-table">
              <thead>
                <tr>
                  <th>Category</th>
                  <th class="col-cost">Cost</th>
                  <th class="col-paid">Paid?</th>
                  <th class="col-remove">Remove</th>
                </tr>
              </thead>
              <tbody>
                <asp:Repeater ID="rptItems" runat="server" OnItemCommand="rptItems_ItemCommand">
                  <ItemTemplate>
                    <tr>
                        <td><%# Eval("category") %></td>
                        <td class="col-cost">R<%# string.Format("{0:0,0.##}", Eval("cost")) %></td>
                        <td class="col-paid">
                          <asp:CheckBox ID="chkPaid"
                              runat="server"
                              AutoPostBack="true"
                              Checked='<%# Convert.ToInt32(Eval("isPaid")) == 1 %>'
                              OnCheckedChanged="chkPaid_CheckedChanged" />
                          <asp:HiddenField ID="hfCategory" runat="server" Value='<%# Eval("category") %>' />
                        </td>
                        <td class="col-remove">
                          <asp:LinkButton ID="lnkRemove"
                              runat="server"
                              CssClass="remove-btn"
                              CommandName="Remove"
                              CommandArgument='<%# Eval("category") %>'>
                              &#10005;
                          </asp:LinkButton>
                        </td>
                    </tr>
                  </ItemTemplate>
                </asp:Repeater>
              </tbody>
            </table>
        </div>

        <!-- Divider -->
        <div class="plain-divider"></div>

        <!-- Go to Vendors Button -->
        <div class="budget-actions">
            <button type="button" id="btnGoToVendors" class="add-button-cust" onclick="location.href='Vendors.aspx'">
                Go to Vendors
            </button>
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
