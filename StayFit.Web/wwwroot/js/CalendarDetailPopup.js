import request from './request.js';

export default class CalendarDetailPopup {
    constructor(config) {
        this.config = config;
        this.updateButton = document.querySelector('#detailPopupUpdateBtn');
        this.addButton = document.querySelector('#detailPopupAddBtn');
        this.createButton = document.querySelector('#detailPopupCreateBtn');
        this.deleteButton = document.querySelector('#detailPopupDeleteBtn');
        this.title = document.querySelector('#modalLabel');

        this.addButton.addEventListener('click', () => this.onAddButtonClick());
        this.createButton.addEventListener('click', () => this.onCreateButtonClick());
        this.deleteButton.addEventListener('click', () => this.onDeleteButtonClick());
    }

    openUpdate(data) {
        this.data = data;
        $('#detailPopup').modal('show');
        $(this.updateButton).show();
        $(this.deleteButton).show();
        $(this.addButton).hide();
        $(this.createButton).hide();
        this.title.textContent = data.title;

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
                
        this.updateButton.addEventListener('click', () => {
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

    openCreate(date, exercise) {
        $('#detailPopup').modal('show');
        $(this.updateButton).hide();
        $(this.deleteButton).hide();
        $(this.createButton).show();
        $(this.addButton).show();
        this.title.textContent = exercise.title;

        this.currentSet = 1;
        this.data = [];
        this.exercise = exercise;
        this.date = date;

        const tableBody = document.getElementById("exerciseDataTable");
        this.clearTableData(tableBody);        
    }

    clearTableData(tableBody) {
        while (tableBody.firstChild) {
            tableBody.removeChild(tableBody.firstChild);
        }
    }

    onAddButtonClick() {
        const tableBody = document.getElementById("exerciseDataTable");

        const row = document.createElement("tr");
        const weightInputId = 'weight-' + this.currentSet;
        const repsInputId = 'reps-' + this.currentSet;
        row.innerHTML = "<td>" + this.currentSet + "</td>" +
            "<td><input id='" + weightInputId + "' type='number' min='0' step='1' /></td>" +
            "<td><input id='" + repsInputId + "' type='number' min='0' step='1' /></td>";

        tableBody.appendChild(row);
        const weightInput = document.getElementById(weightInputId);
        const repsInput = document.getElementById(repsInputId);

        const item = {
            repetitions: null,
            set: this.currentSet,
            weight: null,
        };

        weightInput.addEventListener("input", () => {
            item.weight = parseInt(weightInput.value);
        });

        repsInput.addEventListener("input", () => {
            item.repetitions = parseInt(repsInput.value);
        });

        this.currentSet++;

        this.data.push(item);
    }

    async onCreateButtonClick() {
        const createCalendarRecordUrl = '/Calendar/CreateLog';
        await request(createCalendarRecordUrl, 'POST', {
            logs: this.data,
            exerciseId: this.exercise.id,
            date: this.date.toISOString()
        });

        this.config.onCreated();
    }

    async onDeleteButtonClick() {
        const deleteCalendarRecordUrl = '/Calendar/Delete';
        await request(deleteCalendarRecordUrl, 'POST', this.data.exercises.map(e => e.id));

        this.config.onDeleted();
    }
}