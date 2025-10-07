<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Vendors.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Vendor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Vendor Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Vendor Page Container -->
    <div class="vendor-layout">

        <!-- Successful Add Message Pop-Up -->
        <asp:Panel ID="pnlVendorSuccess" runat="server" CssClass="popup" Style="display:none;">
            <h3>Vendor successfully added!</h3>
            <asp:Button ID="btnCloseVendorSuccess" runat="server" CssClass="close-button" Text="Close" OnClientClick="closeVendorSuccessPopup(); return false;" />
        </asp:Panel>

        <!-- LEFT: Column Vendors -->
        <div class="column vendors">
            <div class="vendors-top">
                <h2>Vendors</h2>
            </div>

            <div class="vendors-middle">
                <% string currentCategory = Request.QueryString["category"] ?? "Photography"; %>
                <ul class="vendor-categories">
                    <li class='<% = (currentCategory == "Photography") ? "active" : "" %>'><a href="Vendors.aspx?category=Photography">Photographers</a></li>
                    <li class='<% = (currentCategory == "Bakery") ? "active" : "" %>'><a href="Vendors.aspx?category=Bakery">Bakers</a></li>
                    <li class='<% = (currentCategory == "Music") ? "active" : "" %>'><a href="Vendors.aspx?category=Music">DJs</a></li>
                    <li class='<% = (currentCategory == "Flowers") ? "active" : "" %>'><a href="Vendors.aspx?category=Flowers">Florists</a></li>
                    <li class='<% = (currentCategory == "Catering") ? "active" : "" %>'><a href="Vendors.aspx?category=Catering">Catering</a></li>
                    <li class='<% = (currentCategory == "Venue") ? "active" : "" %>'><a href="Vendors.aspx?category=Venue">Venues</a></li>
                    <li class='<% = (currentCategory == "Videography") ? "active" : "" %>'><a href="Vendors.aspx?category=Videography">Videographers</a></li>
                    <li class='<% = (currentCategory == "Jewelry") ? "active" : "" %>'><a href="Vendors.aspx?category=Jewelry">Jewelers</a></li>
                    <li class='<% = (currentCategory == "Dance Lessons") ? "active" : "" %>'><a href="Vendors.aspx?category=Dance Lessons">Dance Lessons</a></li>
                    <li class='<% = (currentCategory == "Dress Designers") ? "active" : "" %>'><a href="Vendors.aspx?category=Dress Designers">Dress Designers</a></li>
                </ul>
            </div>

            <div class="vendors-bottom">
                <asp:Button ID="Button1" runat="server" Text="Customise Vendors" CssClass="customise-button" OnClick="btnCustomiseVendors_Click" />
            </div>
        </div>

        <!-- Fancy Divider -->
        <div class="divider-wrapper">
            <img src="Images/Divider.svg" alt="Divider" id="fancyDivider" />
        </div>

        <!-- MIDDLE: Column Photographers -->
        <div class="column photographers">
            <h2>Wedding Photographers</h2>
            
            <!-- Toolbar -->
            <div class="vendor-toolbar">
                <asp:DropDownList ID="ddlProvince" runat="server" CssClass="province-dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged">
                    <asp:ListItem Value="">Select a province</asp:ListItem>
                    <asp:ListItem>Eastern Cape</asp:ListItem>
                    <asp:ListItem>Free State</asp:ListItem>
                    <asp:ListItem>Gauteng</asp:ListItem>
                    <asp:ListItem>KwaZulu-Natal</asp:ListItem>
                    <asp:ListItem>Limpopo</asp:ListItem>
                    <asp:ListItem>Mpumalanga</asp:ListItem>
                    <asp:ListItem>North West</asp:ListItem>
                    <asp:ListItem>Northern Cape</asp:ListItem>
                    <asp:ListItem>Western Cape</asp:ListItem>
                </asp:DropDownList>

                <asp:Button ID="btnShowVendorHelp" runat="server" CssClass="help-button" Text="Need help?" OnClick="btnShowVendorHelp_Click" />

                <asp:DropDownList ID="ddlSortPrice" runat="server" CssClass="sort-dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlSortPrice_SelectedIndexChanged">
                    <asp:ListItem Value="price-asc">Price: Low to High ↑</asp:ListItem>
                    <asp:ListItem Value="price-desc">Price: High to Low ↓</asp:ListItem>
                </asp:DropDownList>
            </div>


            <!-- Vendor Cards -->
            <div class="vendor-cards-container">
                <asp:Repeater ID="rptVendors" runat="server">
                    <ItemTemplate>
                        <div class="vendor-card">
                            <div class="frame">
                                <img src='<%# Eval("imagePath") %>' alt="Vendor Image" class="vendor-image" />
                            </div>

                            <!-- Horizontal layout: text + button -->
                            <div class="vendor-details">
                                <div class="vendor-info">
                                    <p class="vendor-name"><%# Eval("vendorName") %></p>
                                    <p class="vendor-location"><%# Eval("vendorCity") %>, <%# Eval("vendorProvince") %></p>
                                    <p class="vendor-price">R<%# Eval("vendorPrice") %></p>
                                </div>
                                <button class="add-button"
                                    onclick="addToCart(this)"
                                    data-vendor-id='<%# Eval("vendorName") + "-" + Eval("vendorCity") + "-" + Eval("vendorProvince") %>'
                                    data-category='<%# Request.QueryString["category"] ?? "Photography" %>'
                                    data-name='<%# Eval("vendorName") %>'
                                    data-price='<%# Eval("vendorPrice") %>'>
                                    <img src="Images/cart-icon.svg" alt="Cart Icon" />
                                </button>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

        </div>

        <!-- Help Popup -->
        <asp:Panel ID="pnlVendorHelp" runat="server" CssClass="popup help-popup" Visible="false">
            <asp:Label ID="lblVendorHelpTitle" runat="server" CssClass="popup-title" Text="Vendor Page Help"></asp:Label>
            <div class="form-group">
                <p>
                    <b>How to use this page:</b><br />
                    <span style="color:#4e2459;">- Browse Vendors:</span> Use the category list on the left to view different vendor types.<br />
                    <span style="color:#4e2459;">- Filter Vendors:</span> Use the province dropdown to filter vendors by location.<br />
                    <span style="color:#4e2459;">- Sort Vendors:</span> Use the sort dropdown to order vendors by price.<br />
                    <span style="color:#4e2459;">- Add to List:</span> Click the cart icon to add a vendor to your list.<br />
                    <span style="color:#4e2459;">- Customise Vendors:</span> Click "Customise Vendors" to manage your own vendor list.<br />
                    <br />
                    For further assistance, contact support or refer to the documentation.
                </p>
            </div>
            <div class="button-group">
                <asp:Button ID="btnCloseVendorHelp" runat="server" CssClass="close-button" Text="Close" OnClick="btnCloseVendorHelp_Click" />
            </div>
        </asp:Panel>

        <!-- Plain Divider -->
        <div class="plain-divider"></div>

        <!-- RIGHT: Column List -->
        <div class="column list">
            <h2><%--List--%>Your Cart</h2>
            <!-- Add Cart/List Items Here -->
            <div id="cartContainer">
                <%--<h3>Your Cart</h3>--%>
                <div id="cartItems"></div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function showVendorSuccessPopup() {
            document.getElementById('<%= pnlVendorSuccess.ClientID %>').style.display = 'block';
        }
        function closeVendorSuccessPopup() {
            document.getElementById('<%= pnlVendorSuccess.ClientID %>').style.display = 'none';
        }

        function addToCart(btn) {
            var vendorId = btn.getAttribute('data-vendor-id');
            var category = btn.getAttribute('data-category');
            var name = btn.getAttribute('data-name');
            var price = btn.getAttribute('data-price');

            // Prevent duplicate
            if (document.getElementById('cart-' + vendorId)) return;

            // Create cart item
            var cartItem = document.createElement('div');
            cartItem.className = 'cart-item';
            cartItem.id = 'cart-' + vendorId;
            cartItem.innerHTML =
                '<span class="cart-category">' + category + '</span> | ' +
                '<span class="cart-name">' + name + '</span> | ' +
                '<span class="cart-price">R' + price + '</span> ' +
                '<button class="remove-cart-btn" onclick="removeFromCart(\'' + vendorId + '\', \'' + category + '\')">Remove</button>';

            document.getElementById('cartItems').appendChild(cartItem);

            // Disable all cart buttons for this category
            var btns = document.querySelectorAll('.add-button[data-category="' + category + '"]');
            btns.forEach(function (b) {
                b.disabled = true;
                b.classList.add('cart-disabled');
            });
        }

        function removeFromCart(vendorId, category) {
            var cartItem = document.getElementById('cart-' + vendorId);
            if (cartItem) cartItem.remove();

            // Check if any cart items of this category remain
            var stillInCart = false;
            var cartItems = document.querySelectorAll('#cartItems .cart-item');
            cartItems.forEach(function (item) {
                var itemCategory = item.querySelector('.cart-category').textContent;
                if (itemCategory === category) {
                    stillInCart = true;
                }
            });

            // If none remain, re-enable all cart buttons for this category
            if (!stillInCart) {
                var btns = document.querySelectorAll('.add-button[data-category="' + category + '"]');
                btns.forEach(function (b) {
                    b.disabled = false;
                    b.classList.remove('cart-disabled');
                });
            }
        }
    </script>
</asp:Content>