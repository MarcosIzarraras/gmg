class SelectSearch {
    constructor({ selector, url, data, placeholder, label, propText, propValue, propTextRender, valueCallback }) {
        this.label = label ? label : "Search";
        this.container = document.querySelector(selector);
        this.placeholder = placeholder ? placeholder : "Search...";
        this.value = null;
        this.propText = propText;
        this.propValue = propValue;
        this.propTextRender = propTextRender;
        this.valueCallback = valueCallback;
        this.createComponent();
        if (url) {
            this.url = url;
            this.getServerData();
        }
        if (data) {
            this.originalData = data;
            this.data = data;
            this.filterStaticData();
        }
        this.registerEvents();
    }
    async getServerData() {
        const value = this.container.querySelector('input').value;
        const response = await fetch(`${this.url}?search=${value}`);
        if (response.ok) {
            const responseJson = await response.json();
            this.originalData = responseJson;
            this.data = this.propText && this.propValue ?
                responseJson.map(item => ({
                    text: (this.propTextRender ? this.propTextRender(item) : item[this.propText]),
                    value: item[this.propValue],
                    original: item
                }))
                : responseJson;
            this.renderData();
        }
    }
    renderData() {
        if (this.data) {
            const list = this.container.querySelector('[data-id="list"]');
            let buttons = '';
            this.data.forEach((item, index) => {
                buttons += `<div data-id="option" data-index="${index}" class="dropdown-item" data-value="${item.value}">${item.text}</div>`;
            });
            list.innerHTML = buttons;
        }
        this.setValueByExactText();
    }
    filterStaticData() {
        const value = this.container.querySelector('input').value;
        if (value !== '') { this.data = this.originalData.filter(w => w.text.toLowerCase().includes(value.toLowerCase())); }
        else { this.data = this.originalData; }
        this.renderData();
    }
    filterData() {
        if (this.url) {
            this.getServerData();
        }
        else {
            this.filterStaticData();
        }
    }
    createComponent() {
        this.container.innerHTML += `<div class="dropdown w-100">
            <label class="form-label">${this.label}</label>
            <input class="form-input" placeholder="${this.placeholder}" autocomplete="off" />
            <div class="dropdown-menu w-100" data-id="list"></div>
        </div>`;
    }
    registerEvents() {
        this.container.querySelector('input').addEventListener('keyup', (e) => {
            this.filterData();
        });
        this.container.querySelector('[data-id="list"]').addEventListener('click', (e) => {
            const option = e.target.closest('[data-id="option"]');
            if (option) {
                const index = option.dataset.index;
                const dataValue = this.data[index];
                this.container.querySelector('input').value = option.textContent;
                this.value = dataValue.value;
                this.filterData();
                if (this.valueCallback) this.valueCallback(dataValue.original);
            }
        });
    }
    setValueByExactText() {
        const value = this.container.querySelector('input').value;
        const dataValue = this.data.find(w => w.text.toLowerCase() === value.toLowerCase());
        if (dataValue) {
            this.value = dataValue.value;
            if (this.valueCallback) this.valueCallback(dataValue.original);
        }
    }
}