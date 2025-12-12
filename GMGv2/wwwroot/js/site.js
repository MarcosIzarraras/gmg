const notyf = new Notyf();
//let lucide = null;

function InitTableActions(selectorContainer, callbacks) {
    const container = document.querySelector(selectorContainer);
    container.addEventListener('click', function (e) {
        callbacks.forEach(callback => {
            const selector = e.target.closest(callback.selector);
            if (selector) {
                callback.event(selector);
            }
        });
    });
}

lucide.createIcons();

const sidebar = document.getElementById('sidebar');
const overlay = document.getElementById('overlay');

function toggleSidebar() {
    sidebar.classList.add('open');
    overlay.classList.add('show');
}

function closeSidebar() {
    sidebar.classList.remove('open');
    overlay.classList.remove('show');
}

class Modal {
    constructor(selector) {
        this.modalOverlay = document.querySelector(selector);
        this.modal = this.modalOverlay?.querySelector(".modal");

        if (!this.modalOverlay) {
            throw new Error(`Modal con ID "${modalId}" no encontrado.`);
        }

        this.registerEvents();
    }

    open() {
        this.modalOverlay.classList.add("show");
        document.body.classList.add("modal-open");

        setTimeout(() => {
            if (this.modal) {
                this.modal.style.transform = "scale(1)";
                this.modal.style.opacity = "1";
            }
        }, 10);
    }

    close() {
        if (this.modal) {
            this.modal.style.transform = "scale(0.9)";
            this.modal.style.opacity = "0";
        }

        setTimeout(() => {
            this.modalOverlay.classList.remove("show");
            document.body.classList.remove("modal-open");
        }, 300);
    }

    registerEvents() {
        // Botón "cerrar"
        this.modalOverlay.addEventListener("click", (e) => {
            if (e.target.closest(".modal-close")) {
                this.close();
                return;
            }

            // Click en overlay (afuera)
            if (e.target === this.modalOverlay) {
                this.close();
            }
        });

        // Cerrar con ESC — solo si está abierto
        document.addEventListener("keydown", (e) => {
            if (e.key === "Escape" && this.modalOverlay.classList.contains("show")) {
                this.close();
            }
        });
    }
}

class Table {
    constructor({ selector, afterRender, columns, url, actions, onRowClick }) {
        this.container = document.querySelector(selector);
        this.page = 1;
        this.url = url;
        this.columns = columns;
        this.afterRender = afterRender;
        this.actions = actions;
        this.onRowClick = onRowClick;
        this.createTemplate();
        this.registerEvents();
        this.getData();
    }

    createTemplate() {
        this.container.innerHTML = `<div class="table-container">
            <table class="table">
                ${ this.columnsTemplate() }
                <tbody></tbody>
            </table>
            <div data-id="footer" class="table-footer">
                <div class="table-info" data-id="info">Show 1-5 of 23</div>
                    <div class="pagination" data-id="pagination"></div>
                    <div class="per-page">
                        <span class="per-page-label">Per page:</span>
                        <select data-id="size" class="per-page-select">
                            <option value="5">5</option>
                            <option value="10">10</option>
                            <option value="20">20</option>
                        </select>
                    </div>
            </div>
        </div>`;
    }

    columnsTemplate() {
        let columns = '<thead></tr>';

        this.columns.forEach(column => {
            columns += `<th>${ column.title }</th>`;
        });
        if (this.actions)
            columns += '<th>Actions</th>';
        

        return columns += '</tr></thead>';
    }

    renderTable() {
        const items = this.data.items;
        const tableBody = this.container.querySelector('tbody');
        let rows = '';

        items.forEach((item, index) => {
            rows += `<tr data-row="${index}">`;
            this.columns.forEach(column => {
                let itemData = null;
                if (column.data.includes('.')) {
                    const objects = column.data.split('.');
                    objects.forEach(object => {
                        itemData = itemData ? itemData[object] : item[object];
                    });
                }
                else {
                    itemData = item[column.data];
                }

                rows += `<td>${column.render ? column.render(itemData, item) : itemData}</td>`;
            });
            if (this.actions)
                rows += `<td>${this.actionButtons(index)}</td>`;
            rows += `</tr>`;
        });

        tableBody.innerHTML = rows;

        const rowsElements = tableBody.querySelectorAll("tr");
        rowsElements.forEach((row, index) => {
            row.addEventListener("click", (e) => {
                if (this.onRowClick)
                    this.onRowClick(items[index], e);
            });
        });
    }

    actionButtons(rowIndex) {
        if (this.actions) {
            return `
                <div class="actions-cell">
                    ${Object.keys(this.actions)
                            .map(action => {
                                let icon = action === "edit" ? "pencil" :
                                    action === "delete" ? "trash" :
                                        action === "view" ? "eye" :
                                            "circle";

                                return `
                                <button class="btn btn-icon btn-ghost" 
                                        data-action="${action}" 
                                        data-id="${rowIndex}">
                                    <i data-lucide="${icon}" style="width:16px;heigth:16px;"></i>
                                </button>`;
                            })
                            .join("")
                        }
                </div>
            `;
        }
        return '';
    }

    async getData() {
        const size = this.container.querySelector('[data-id="size"]').value;
        const response = await fetch(`${this.url}?page=${this.page}&pagesize=${size}`);
        const data = await response.json();
        this.data = data;
        this.render();
    }

    updateInfo() {
        const infoContainer = this.container.querySelector('[data-id="info"]');
        const size = this.container.querySelector('[data-id="size"]').value;
        const start = (this.page - 1) * size + 1;
        const end = Math.min(this.page * size, this.data.totalCount);
        infoContainer.innerHTML = `Show ${ start }-${end} of ${ this.data.totalCount }`;
    }

    renderPagination() {
        const paginationContainer = this.container.querySelector('[data-id="pagination"]');
        let paginationButtons = `<button class="pagination-btn" data-value="${ this.page - 1 }" ${this.page === 1 ? 'disabled' : ''}>
                <i data-lucide="chevron-left" style="width:16px;height:16px"></i>
            </button>`;

        for (var i = 1; i <= this.data.totalPages; i++) {
            if (i === 1 || i === this.data.totalPages || (i >= this.page - 1 && i <= this.page + 1)) {
                paginationButtons += `<button class="pagination-btn ${i === this.page ? 'active' : ''}" data-value="${i}">${i}</button>`;
            } else if (i === this.page - 2 || i === this.page + 2) {
                paginationButtons += `<span class="pagination-ellipsis">...</span>`;
            }
        }
        paginationButtons += `<button class="pagination-btn" data-value="${this.page + 1}" ${this.page === this.data.totalPages ? 'disabled' : ''}>
                <i data-lucide="chevron-right" style="width:16px;height:16px"></i>
            </button>`;

        paginationContainer.innerHTML = paginationButtons;
    }

    render() {
        this.renderTable();
        this.updateInfo();
        this.renderPagination();
        if (this.afterRender)
            this.afterRender();
    }

    registerEvents() {
        this.container.querySelector('[data-id="pagination"]').addEventListener('click', (e) => {
            const button = e.target.closest('button');
            const value = parseInt(button.dataset.value);
            if (value !== this.page) {
                this.page = value;
                this.getData();
            }
        });

        this.container.querySelector('[data-id="size"]').addEventListener('change', (e) => {
            this.getData();
        });

        this.container.querySelector('tbody').addEventListener("click", (e) => {
            const btn = e.target.closest("[data-action]");
            if (btn) {
                const actionName = btn.dataset.action;
                const id = parseInt(btn.dataset.id);
                const data = this.data.items[id]
                
                if (this.actions?.[actionName]) {
                    this.actions[actionName](data);
                }
            }
        });
    }
}