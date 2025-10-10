<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Vendors.aspx.cs" Inherits="Purple_Hollow_Wedding_Planners.Vendor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Vendor Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ScriptManager runat="server" EnablePageMethods="true" />

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
                                        data-vendor-id='<%# Eval("vendorID") %>'
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

                <!-- Divider under cart title and items -->
                <div class="cart-bottom-divider"></div>

                <!-- Total price display -->
                <div id="cartTotalContainer" class="cart-total-container">
                    <span class="cart-total-label">Total: R</span>
                    <span id="cartTotal" class="cart-total-value">0</span>
                </div>

                <!-- Add to Budget button -->
                <button id="addToBudgetBtn" class="add-button-cust" type="button">Add to Budget</button>
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

        function updateCartTotal() {
            var total = 0;
            document.querySelectorAll('#cartItems .cart-item .cart-price').forEach(function (priceSpan) {
                var priceText = priceSpan.textContent.replace('R', '').replace(/\s/g, '');
                var price = parseFloat(priceText);
                if (!isNaN(price)) total += price;
            });
            document.getElementById('cartTotal').textContent = total.toFixed(2);
        }

        // Utility: Save cart to localStorage
        function saveCartToStorage() {
            var cartItems = [];
            document.querySelectorAll('#cartItems .cart-item').forEach(function (item) {
                cartItems.push({
                    vendorId: item.id.replace('cart-', ''),
                    category: item.querySelector('.cart-category').textContent,
                    name: item.querySelector('.cart-name').textContent,
                    price: item.querySelector('.cart-price').textContent.replace('R', '')
                });
            });
            localStorage.setItem('vendorCart', JSON.stringify(cartItems));
            updateCartTotal();
        }

        // Utility: Load cart from localStorage
        function loadCartFromStorage() {
            var cartItems = JSON.parse(localStorage.getItem('vendorCart') || '[]');
            cartItems.forEach(function (item) {
                // Rebuild cart item
                var cartItem = document.createElement('div');
                cartItem.className = 'cart-item';
                cartItem.id = 'cart-' + item.vendorId;
                cartItem.innerHTML =
                    '<span class="cart-category">' + item.category + '</span>  ' +
                    '<span class="cart-name">' + item.name + '</span>  ' +
                    '<span class="cart-price">R' + item.price + '</span> ' +
                    '<button class="remove-cart-btn" onclick="removeFromCart(\'' + item.vendorId + '\', \'' + item.category + '\')" title="Remove">' +
                    '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="white" viewBox="0 0 24 24" style="vertical-align:middle;"><path d="M3 6h18v2H3V6zm2 3h14v13a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V9zm5 2v8h2v-8h-2zm-4 0v8h2v-8H6zm8 0v8h2v-8h-2z"/></svg>' +
                    '</button>';
                document.getElementById('cartItems').appendChild(cartItem);
            });

            // Disable buttons for categories already in cart
            var categoriesInCart = cartItems.map(function (item) { return item.category; });
            categoriesInCart.forEach(function (category) {
                var btns = document.querySelectorAll('.add-button[data-category="' + category + '"]');
                btns.forEach(function (b) {
                    b.disabled = true;
                    b.classList.add('cart-disabled');
                });
            });

            updateCartTotal();
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
                '<span class="cart-category">' + category + '</span>  ' +
                '<span class="cart-name">' + name + '</span>  ' +
                '<span class="cart-price">R' + price + '</span> ' +
                '<button class="remove-cart-btn" onclick="removeFromCart(\'' + vendorId + '\', \'' + category + '\')" title="Remove">' +
                '<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="white" viewBox="0 0 24 24" style="vertical-align:middle;"><path d="M3 6h18v2H3V6zm2 3h14v13a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V9zm5 2v8h2v-8h-2zm-4 0v8h2v-8H6zm8 0v8h2v-8h-2z"/></svg>' +
                '</button>';

            document.getElementById('cartItems').appendChild(cartItem);

            // Disable all cart buttons for this category
            var btns = document.querySelectorAll('.add-button[data-category="' + category + '"]');
            btns.forEach(function (b) {
                b.disabled = true;
                b.classList.add('cart-disabled');
            });

            saveCartToStorage();
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

            saveCartToStorage();
        }

        // Restore cart on page load
        window.addEventListener('DOMContentLoaded', function () {
            loadCartFromStorage();
        });

        document.getElementById('addToBudgetBtn').addEventListener('click', function () {
            var cartItems = document.querySelectorAll('#cartItems .cart-item');
            if (cartItems.length === 0) {
                alert('Your cart is empty.');
                return;
            }

            // Gather vendor info and total
            // Gather vendor info and total
            var vendors = [];
            var total = 0;
            cartItems.forEach(function (item) {
                var vendorId = item.id.replace('cart-', ''); // ✅ get numeric ID from element id
                var category = item.querySelector('.cart-category').textContent;
                var name = item.querySelector('.cart-name').textContent;
                var priceText = item.querySelector('.cart-price').textContent.replace('R', '').replace(/\s/g, '');
                var price = parseFloat(priceText);
                if (!isNaN(price)) total += price;

                // ✅ include vendorId in the object sent to C#
                vendors.push({
                    vendorId: parseInt(vendorId, 10),
                    category: category,
                    name: name,
                    price: price
                });
            });

            // TESTING: Log to console
            console.log(total, vendors);

            // Send AJAX request to server
            PageMethods.AddToBudget(total, vendors, function (result) {
                // Client-side error display logic
                if (typeof result === "string" && result.startsWith("error:")) {
                    // Show the error message returned from the server
                    alert("Server Error: " + result.substring(6));
                } else if (result === "success") {
                    // Success logic
                    document.getElementById('cartItems').innerHTML = '';
                    localStorage.removeItem('vendorCart');
                    document.getElementById('cartTotal').textContent = '0.00';
                    alert('Added to budget!');
                } else {
                    // Unexpected result
                    alert('Unexpected response: ' + result);
                }
            },
                function (error) {
                    // AJAX/network error
                    /*alert('AJAX error: ' + error.get_message());*/
                    console.error(error);
                });
        });

        // Disable add-buttons for categories already in budget
        window.addEventListener('DOMContentLoaded', function () {
            // existing loadCartFromStorage etc...
            loadCartFromStorage();

            // Now fetch chosen categories from backend
            PageMethods.GetChosenCategories(function (cats) {
                cats.forEach(function (cat) {
                    document.querySelectorAll('.add-button[data-category="' + cat + '"]').forEach(function (b) {
                        b.disabled = true;
                        b.classList.add('cart-disabled');
                    });
                });
            }, function (err) {
                console.error("Error fetching chosen categories:", err);
            });
        });
    </script>
</asp:Content>