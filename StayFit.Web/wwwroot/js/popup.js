import request from './request.js';

export default class CalendarDetailPopup {
    open(data) {
        $('#detailPopup').modal('show');

        const tableBody = document.getElementById("exerciseDataTable");
        this.clearTableData(tableBody);

        data.exercises.forEach(function (item) {
            const row = document.createElement("tr");
            const weightInputId = 'weight-' + item.set;
            const repsInputId = 'reps-' + item.set;
            row.innerHTML = "<td>" + item.set + "</td>" +
                "<td><input id='" + weightInputId + "' type='number' min='0' step='1' value='" + item.weight + "' /></td>" +
                "<td><input id='" + repsInputId + "' type='number' min='0' step='1' value='" + item.repetitions + "' /></td>";

            tableBody.appendChild(row);
            const weightInput = document.getElementById(weightInputId);
            const repsInput = document.getElementById(repsInputId);

            weightInput.addEventListener("input", () => {
                item.weight = parseInt(weightInput.value);
            });

            repsInput.addEventListener("input", () => {
                item.repetitions = parseInt(repsInput.value);
            });
        });

        const updateButton = document.querySelector('#detailPopupUpdateBtn');
        updateButton.addEventListener('click', () => {
            request('/Calendar/UpdateDetails', 'POST', data.exercises.map(e => {
                return {
                    Id: e.id,
                    Repetitions: e.repetitions,
                    Set: e.set,
                    Weight: e.weight,
                };
            }));
        });
    }

    clearTableData(tableBody) {
        while (tableBody.firstChild) {
            tableBody.removeChild(tableBody.firstChild);
        }
    }
}