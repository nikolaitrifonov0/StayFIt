import CalendarDetailPopup from './CalendarDetailPopup.js';
import SelectExercisePopup from './SelectExercisePopup.js';

const detailPopup = new CalendarDetailPopup({
    onCreated: () => calendar.refetchEvents(),
    onDeleted: () => calendar.refetchEvents()
});
const exercisePopup = new SelectExercisePopup({
    onExerciseSelected: (exercise, date) => onExerciseSelected(exercise, date)
});

let addSetBtns = document.querySelectorAll('.add-set');
for (var btn of addSetBtns) {
    btn.onclick = (e) => {
        let oldDivSet = e.target.parentElement.parentElement.querySelector('.set');
        let newDivSet = oldDivSet.cloneNode(true);

        let oldSetNumber = oldDivSet.querySelector('.set-num');
        let newSetNumber = newDivSet.querySelector('.set-num');

        newSetNumber.value = Number(oldSetNumber.value) + 1;

        e.target.parentElement.parentElement.querySelector('.form-inline').appendChild(newDivSet);
    }
}

const calendarEl = document.getElementById('calendar');
const calendar = new FullCalendar.Calendar(calendarEl, {
    initialView: 'dayGridMonth',
    timeZone: 'UTC',
    events: {
        url: '/Calendar/GetData',
        method: 'GET',
        color: '#6c6e99',
        textColor: 'white'
    },
    eventClick: async (info) => {
        const result = await $.ajax({
            url: '/Calendar/GetDetails',
            data: {
                exerciseName: info.event.title,
                date: info.event.startStr
            }
        });
        detailPopup.openUpdate(result);
    },
    dateClick: (info) => {
        exercisePopup.open(info.date);
    }
});
calendar.render();

function onExerciseSelected(exercise, date) {
    detailPopup.openCreate(exercise, date);
}