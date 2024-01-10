import AddModelPopup from './AddModelPopup.js';
import request from './request.js';

const popup = new AddModelPopup({
    create: async (value) => {
        try {
            let response = await request('/Equipments/Add', 'POST', {
                name: value
            });

            location.reload();
        } catch (e) {
            alert('There is already an equipment with this name.');
        }
    }
});
$('#addNew').click(() => popup.open());