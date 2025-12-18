class RequestManager {
    constructor() {
        this.pendingRequests = new Map();
        this.originalButtonStates = new Map();
    }

    async submitForm(form, key, buttonSelector) {
        return await this.submitFormData(
            new FormData(form),
            form.method,
            form.action,
            key,
            buttonSelector
        );
    }

    async submitFormData(formData, method, url, key, buttonSelector) {
        if (this.pendingRequests.has(key)) {
            return this.pendingRequests.get(key);
        }

        const button = buttonSelector ? document.querySelector(buttonSelector) : null;
        if (button) this.setDisabledButton(button);

        const promise = fetch(url, {
            method: method.toUpperCase(),
            body: formData
        })
            .then(response => {
                if (response.redirected) {
                    window.location.href = response.url;
                    return null;
                }
                return response;
            })
            .catch(error => {
                throw error;
            })
            .finally(() => {
                this.pendingRequests.delete(key);
                if (button)
                    this.setEnabledButton(button); 
            });

        this.pendingRequests.set(key, promise);
        return promise;
    }

    setDisabledButton(button) {
        this.originalButtonStates.set(button, button.innerHTML);
        button.disabled = true;
        button.innerHTML = `<span class="spinner"><i data-lucide="loader"></i></span>`;

        lucide.createIcons();
    }

    setEnabledButton(button) {
        if (!button) return; 
        const originalText = this.originalButtonStates.get(button) || '';
        button.disabled = false;
        button.innerHTML = originalText;
        this.originalButtonStates.delete(button);
    }
}

const requestManager = new RequestManager();