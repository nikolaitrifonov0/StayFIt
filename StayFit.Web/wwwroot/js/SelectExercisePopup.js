import request from './request.js';

export default class SelectExercisePopup {
    constructor(config) {
        this.config = config;
        this.isPopupOpen = false;
        window.addEventListener("scrollend", ((e) => {
            if (this.isPopupOpen) {
                window.scroll({
                    top: this.scrollPosition
                });
            }
        }));

        const selectButton = document.querySelector('#selectExerciseBtn');
        selectButton.addEventListener('click', () => {
            if (this.selectedExercise) {
                this.config.onExerciseSelected(this.date, this.selectedExercise);
            } else {
                alert("Please select an exercise");
            }
            this.isPopupOpen = false;
        });

        const closeButton = document.querySelector('#selectExerciseCloseBtn');
        closeButton.addEventListener('click', () => this.isPopupOpen = false);
    }

    async open(date) {
        this.selectedExercise = null;
        this.date = date;
        this.scrollPosition = window.scrollY || document.documentElement.scrollTop;
        this.isPopupOpen = true;
        $('#exercisesPopup').modal('show');
        const findExerciseUrl = `/Exercises/LoadExercises`;
        const response = await request(findExerciseUrl);
        const exercisesDropdown = document.getElementById("exercisesDropdown");
        exercisesDropdown.innerHTML = "";
        const exercisesCombobox = document.getElementById("exercisesCombobox");
        exercisesCombobox.value = "";

        response.forEach((item) => {
            const li = document.createElement("li");
            const a = document.createElement("a");
            a.href = "#";
            a.innerHTML = item.name;
            a.addEventListener("click", () => this.onExerciseSelected(item));
            li.appendChild(a);
            exercisesDropdown.appendChild(li);
        });        
    }

    onExerciseSelected(item) {
        const exercisesCombobox = document.getElementById("exercisesCombobox");
        exercisesCombobox.value = item.name;
        this.selectedExercise = item;        
    }
}