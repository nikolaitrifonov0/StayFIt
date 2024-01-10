import AddModelPopup from './AddModelPopup.js';
import request from './request.js';

const popup = new AddModelPopup({
    create: async (value) => {
        try {
            await request('/BodyParts/Add', 'POST', {
                name: value
            });

            location.reload();
        } catch (e) {
            alert('There is already a body part with this name.');
        }
    }
});
$('#addNew').click(() => popup.open());