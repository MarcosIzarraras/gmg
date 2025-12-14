// POS - Point of Sale System
class POSSystem {
    constructor() {
        this.cart = [];
        this.products = [];
        this.init();
        this.checkoutModal = new Modal('#checkout-modal');
    }

    init() {
        this.loadProducts();
        this.attachEventListeners();
        this.updateCart();
    }

    loadProducts() {
        // Load products from DOM
        const productCards = document.querySelectorAll('.product-card');
        productCards.forEach(card => {
            this.products.push({
                id: card.dataset.id,
                name: card.dataset.name,
                price: parseFloat(card.dataset.price),
                category: card.dataset.category
            });
        });
    }

    attachEventListeners() {
        // Product selection
        document.querySelectorAll('.product-card').forEach(card => {
            card.addEventListener('click', () => {
                const productId = card.dataset.id;
                this.addToCart(productId);
            });
        });

        // Category filter
        document.querySelectorAll('.category-btn').forEach(btn => {
            btn.addEventListener('click', () => {
                this.filterByCategory(btn.dataset.category);

                // Update active state
                document.querySelectorAll('.category-btn').forEach(b => b.classList.remove('active'));
                btn.classList.add('active');
            });
        });

        // Search
        const searchInput = document.getElementById('product-search');
        searchInput.addEventListener('input', (e) => {
            this.searchProducts(e.target.value);
        });

        // Clear ticket
        document.getElementById('clear-ticket').addEventListener('click', () => {
            if (this.cart.length > 0) {
                if (confirm('Are you sure you want to clear the ticket?')) {
                    this.cart = [];
                    this.updateCart();
                }
            }
        });

        // Checkout button
        document.getElementById('btn-checkout').addEventListener('click', () => {
            this.openCheckoutModal();
        });

        // Modal controls
        document.getElementById('close-modal').addEventListener('click', () => {
            this.closeCheckoutModal();
        });

        document.getElementById('cancel-checkout').addEventListener('click', () => {
            this.closeCheckoutModal();
        });

        // Payment method change
        document.getElementById('payment-method').addEventListener('change', (e) => {
            this.handlePaymentMethodChange(e.target.value);
        });

        // Amount received change
        document.getElementById('amount-received').addEventListener('input', (e) => {
            this.calculateChange(parseFloat(e.target.value) || 0);
        });

        // Confirm checkout
        document.getElementById('confirm-checkout').addEventListener('click', () => {
            this.processCheckout();
        });
    }

    addToCart(productId) {
        const product = this.products.find(p => p.id === productId);
        if (!product) return;

        const existingItem = this.cart.find(item => item.id === productId);

        if (existingItem) {
            existingItem.quantity++;
        } else {
            this.cart.push({
                id: product.id,
                name: product.name,
                price: product.price,
                quantity: 1
            });
        }

        this.updateCart();
        this.showNotification('Product added', 'success');
    }

    removeFromCart(productId) {
        this.cart = this.cart.filter(item => item.id !== productId);
        this.updateCart();
        this.showNotification('Product removed', 'info');
    }

    updateQuantity(productId, delta) {
        const item = this.cart.find(item => item.id === productId);
        if (!item) return;

        item.quantity += delta;

        if (item.quantity <= 0) {
            this.removeFromCart(productId);
        } else {
            this.updateCart();
        }
    }

    updateCart() {
        const container = document.getElementById('ticket-items');

        if (this.cart.length === 0) {
            container.innerHTML = `
                <div class="empty-ticket">
                    <i data-lucide="shopping-cart"></i>
                    <p>No products in the ticket</p>
                </div>
            `;
            lucide.createIcons();
            document.getElementById('btn-checkout').disabled = true;
        } else {
            container.innerHTML = this.cart.map(item => `
                <div class="ticket-item" data-id="${item.id}">
                    <div class="ticket-item-header">
                        <div class="ticket-item-name">${item.name}</div>
                        <button class="ticket-item-remove" onclick="pos.removeFromCart('${item.id}')">
                            <i data-lucide="x"></i>
                        </button>
                    </div>
                    <div class="ticket-item-footer">
                        <div class="quantity-controls">
                            <button class="quantity-btn" onclick="pos.updateQuantity('${item.id}', -1)">
                                <i data-lucide="minus"></i>
                            </button>
                            <span class="quantity-display">${item.quantity}</span>
                            <button class="quantity-btn" onclick="pos.updateQuantity('${item.id}', 1)">
                                <i data-lucide="plus"></i>
                            </button>
                        </div>
                        <div class="ticket-item-price">$${(item.price * item.quantity).toFixed(2)}</div>
                    </div>
                </div>
            `).join('');
            lucide.createIcons();
            document.getElementById('btn-checkout').disabled = false;
        }

        this.updateTotals();
    }

    updateTotals() {
        const subtotal = this.cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);
        const tax = 0; // 0% tax for now
        const total = subtotal + tax;

        document.getElementById('subtotal').textContent = `$${subtotal.toFixed(2)}`;
        document.getElementById('tax').textContent = `$${tax.toFixed(2)}`;
        document.getElementById('total').textContent = `$${total.toFixed(2)}`;
    }

    filterByCategory(categoryId) {
        const productCards = document.querySelectorAll('.product-card');

        productCards.forEach(card => {
            if (categoryId === 'all' || card.dataset.category === categoryId) {
                card.classList.remove('hidden');
            } else {
                card.classList.add('hidden');
            }
        });
    }

    searchProducts(query) {
        const productCards = document.querySelectorAll('.product-card');
        const searchTerm = query.toLowerCase().trim();

        productCards.forEach(card => {
            const productName = card.dataset.name.toLowerCase();

            if (searchTerm === '' || productName.includes(searchTerm)) {
                card.classList.remove('hidden');
            } else {
                card.classList.add('hidden');
            }
        });

        // Reset category filter when searching
        if (searchTerm) {
            document.querySelectorAll('.category-btn').forEach(btn => {
                btn.classList.remove('active');
            });
        }
    }

    openCheckoutModal() {
        const total = this.cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);
        document.getElementById('modal-total').textContent = `$${total.toFixed(2)}`;
        this.checkoutModal.open();

        // Reset form
        document.getElementById('payment-method').value = 'cash';
        document.getElementById('amount-received').value = '';
        document.getElementById('customer-name').value = '';
        document.getElementById('change-display').style.display = 'none';
        this.handlePaymentMethodChange('cash');
    }

    closeCheckoutModal() {
        this.checkoutModal.close();
    }

    handlePaymentMethodChange(method) {
        const cashPaymentGroup = document.getElementById('cash-payment-group');
        const changeDisplay = document.getElementById('change-display');

        if (method === 'cash') {
            cashPaymentGroup.style.display = 'block';
        } else {
            cashPaymentGroup.style.display = 'none';
            changeDisplay.style.display = 'none';
        }
    }

    calculateChange(amountReceived) {
        const total = this.cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);
        const change = amountReceived - total;

        const changeDisplay = document.getElementById('change-display');
        const changeAmount = document.getElementById('change-amount');

        if (change >= 0) {
            changeAmount.textContent = `$${change.toFixed(2)}`;
            changeDisplay.style.display = 'flex';
        } else {
            changeDisplay.style.display = 'none';
        }
    }

    async processCheckout() {
        const paymentMethod = document.getElementById('payment-method').value;
        const amountReceived = parseFloat(document.getElementById('amount-received').value) || 0;
        const customerName = document.getElementById('customer-name').value;

        // Validate cash payment
        if (paymentMethod === 'cash') {
            const total = this.cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);
            if (amountReceived < total) {
                this.showNotification('Insufficient amount received', 'error');
                return;
            }
        }

        // Prepare sale data
        const saleData = {
            CustomerId: null, // Default customer (nullable)
            Details: this.cart.map(item => ({
                ProductId: item.id,
                Quantity: item.quantity,
                UnitPrice: item.price
            }))
        };

        console.log('Sending sale data:', saleData);
        const formData = new FormData();
        formData.append("customerId", saleData.CustomerId);
        saleData.Details.forEach((detail, index) => {
            formData.append(`Details[${index}].ProductId`, detail.ProductId);
            formData.append(`Details[${index}].Quantity`, detail.Quantity);
            formData.append(`Details[${index}].UnitPrice`, detail.UnitPrice);
        });

        try {
            const response = await fetch('/Sales/Create', {
                method: 'POST',
                body: formData
            });

            if (response.ok) {
                const result = await response.json();
                this.showNotification('Sale processed successfully', 'success');
                this.closeCheckoutModal();

                // Clear cart
                this.cart = [];
                this.updateCart();

                // Print receipt or redirect
                setTimeout(() => {
                    // You can redirect to a receipt page or print
                    console.log('Sale completed:', result);
                }, 1000);
            } else {
                const error = await response.json();
                this.showNotification('Error processing sale: ' + error.message, 'error');
            }
        } catch (error) {
            console.error('Error:', error);
            this.showNotification('Error processing sale', 'error');
        }
    }

    showNotification(message, type = 'success') {
        if (typeof notyf !== 'undefined') {
            if (type === 'success') {
                notyf.success(message);
            } else if (type === 'error') {
                notyf.error(message);
            } else if (type === 'info') {
                // Notyf no tiene info por defecto, usar success con mensaje diferente
                notyf.success(message);
            } else if (type === 'warning') {
                // Notyf no tiene warning por defecto, usar error con estilo más suave
                notyf.error(message);
            }
        } else {
            console.log(`[${type}] ${message}`);
        }
    }
}

// Initialize POS System
let pos;
document.addEventListener('DOMContentLoaded', () => {
    pos = new POSSystem();
});
