
export default class AddModelPopup {
    constructor(config) {
        this.config = config;

        const addButton = document.querySelector('#addModelPopupAddBtn');
        const input = document.querySelector('#modelName');
        addButton.addEventListener('click', () => {
            if (input.value) {
                this.config.create(input.value);
                $('#addModelPopup').modal('hide');
            } else {
                alert("Please enter name");
            }
        });
    }

    open() {
        $('#addModelPopup').modal('show');
    }
}