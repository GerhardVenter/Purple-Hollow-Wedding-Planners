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
                              Checked='<%# Convert.ToBoolean(Eval("isPaid")) %>'
                              OnCheckedChanged="chkPaid_CheckedChanged" />
                          <asp:HiddenField ID="hfCategory" runat="server" Value='<%# Eval("category") %>' />
                            <asp:HiddenField ID="hfItemID" runat="server" Value='<%# Eval("itemID") %>' />
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
            <button type="button" id="btnGoToVendors" class="help-button" onclick="location.href='Vendors.aspx'">
                Go to Vendors
            </button>
        </div>

    </section>

    <aside class="chart-col">
        <h2>Costs Pie Chart</h2>
        <div class="panel-card">
            <canvas id="costsChart"></canvas>
        </div>

        <div class="plain-divider"></div>

        <div class="budget-actions right">
            <asp:Button ID="btnBudgetHelp" runat="server" CssClass="help-button" Text="Need help?" OnClientClick="showBudgetHelpPopup(); return false;" />
        </div>
    </aside>

  </div>

    <%--<asp:Panel ID="pnlBudgetHelp" runat="server" CssClass="popup help-popup" Visible="false">
        <asp:Label ID="lblBudgetHelpTitle" runat="server" CssClass="popup-title" Text="Budget Page Help"></asp:Label>
        <div class="form-group">
            <p>
                <b>How to use this page:</b><br />
                <span style="color:#4e2459;">- View Budget:</span> See your total budget, spent, and remaining amounts.<br />
                <span style="color:#4e2459;">- Mark Paid:</span> Use the checkbox to mark a vendor as paid.<br />
                <span style="color:#4e2459;">- Remove Vendor:</span> Click the X to remove a vendor from your budget.<br />
                <span style="color:#4e2459;">- Pie Chart:</span> Visualize your costs by category.<br />
                <br />
                For further assistance, contact support or refer to the documentation.
            </p>
        </div>
        <div class="button-group">
            <asp:Button ID="btnCloseBudgetHelp" runat="server" CssClass="close-button" Text="Close" OnClick="btnCloseBudgetHelp_Click" />
        </div>
    </asp:Panel>--%>

    <div id="toast" class="toast" aria-live="polite" aria-atomic="true"></div>
    <script>
      function showToast(msg, duration = 1800) {
        var t = document.getElementById('toast');
        if (!t) { t = document.createElement('div'); t.id='toast'; t.className='toast'; document.body.appendChild(t); }
        t.textContent = msg;
        t.classList.add('show');
        clearTimeout(showToast._hide);
        showToast._hide = setTimeout(function(){ t.classList.remove('show'); }, duration);
      }
    </script>

    <asp:ScriptManager runat="server" />

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            if (typeof budgetChartData === "undefined" || budgetChartData.length === 0) return;

            const ctx = document.getElementById('costsChart').getContext('2d');
            const labels = budgetChartData.map(i => i.category);
            const data = budgetChartData.map(i => i.cost);

            new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: labels,
                    datasets: [{
                        data: data,
                        backgroundColor: [
                            '#7a3fa1',  // purple
                            '#c6b0e0',  // light purple
                            '#ffda6b',  // yellow
                            '#9fc5f8',  // light blue
                            '#b15c9d'   // accent purple
                        ],
                        borderWidth: 0
                    }]
                },
                options: {
                    plugins: {
                        legend: {
                            position: 'bottom',
                            labels: {
                                color: '#3c2244',
                                font: { size: 14 }
                            }
                        }
                    }
                }
            });
        });

        function showBudgetHelpPopup() {
            document.getElementById('budgetHelpPopup').style.display = 'flex';
        }
        function closeBudgetHelpPopup() {
            document.getElementById('budgetHelpPopup').style.display = 'none';
        }
    </script>

    <!-- Move this block inside <asp:Content ID="Content3" ...> -->
    <div id="budgetHelpPopup" class="popupOverlayToDo" style="display: <%# pnlBudgetHelp.Visible ? "flex" : "none" %>;">
        <div class="popup-content">
            <img src="Images/helpGojo.png" alt="image of gojo being confused" class="popup-img" />
            <p>
                <b>Budget Page Help</b><br />
                <br />- <span style="color:#4e2459;">View Budget:</span> See your total budget, spent, and remaining amounts.<br />
                <br />- <span style="color:#4e2459;">Mark Paid:</span> Use the checkbox to mark a vendor as paid.<br />
                <br />- <span style="color:#4e2459;">Remove Vendor:</span> Click the X to remove a vendor from your budget.<br />
                <br />- <span style="color:#4e2459;">Pie Chart:</span> Visualize your costs by category.<br />
                <br />For further assistance, contact support or refer to the documentation.
            </p>
            <button onclick="closeBudgetHelpPopup()" class="close-btn">Close</button>
        </div>
    </div>

</asp:Content>
