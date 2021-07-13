import request from './request.js';

handleWorkoutType();

await addSearchEventToSearchInputs();

let addExerciseBtn = document.querySelector('.add-day');
addExerciseBtn.onclick = async (e) => {
    let dayDiv = e.target.parentElement.parentElement.querySelector('.day').cloneNode(true);
    dayDiv.hidden = false;
    e.target.parentElement.parentElement.insertBefore(dayDiv, e.target.parentElement);
    await addSearchEventToSearchInputs();
}

async function addSearchEventToSearchInputs() {
    let searchInputs = document.getElementsByClassName('find-exercise');

    for (let input of searchInputs) {
        input.oninput = await addExerciseSearching();
    }
}

function addExerciseSearching() {
    return async (e) => {
        let input = e.target;
        let inputValue = input.value;
        let findExerciseUrl = `Exercises/Find?keyword=${inputValue}`;
        let resultsUl = input.parentElement.querySelector('.results');

        if (inputValue.length >= 3) {
            resultsUl.innerHTML = '';

            let response = await request(url + findExerciseUrl);

            for (let r of response) {
                let li = document.createElement('li');
                li.textContent = r.name;
                li.setAttribute('value', r.id);

                li.onclick = addExerciseToList(li, input);

                resultsUl.appendChild(li);
            }

        }
    };

    function addExerciseToList(li, input) {
        return e => {
            let exercisesUl = li.parentElement.parentElement.querySelector('.exercises');
            let exerciseLi = document.createElement('li');

            exerciseLi.innerHTML = li.innerHTML;
            exerciseLi.setAttribute('value', li.getAttribute('value'));

            if (!containsChild(exercisesUl.children, exerciseLi)) {
                exercisesUl.appendChild(exerciseLi);
                input.value = '';
            }
        };
    }
}

function handleWorkoutType() {
    let workoutTypeSelect = document.getElementsByTagName('select')[0];
    let weeklyWorkdaysDiv = document.getElementsByClassName('weekly-workdays')[0];
    let nthWorkdaysDiv = document.getElementsByClassName('nth-day')[0];

    workoutTypeSelect.onchange = hideWeeklyWorkdays;

    function hideWeeklyWorkdays() {
        if (workoutTypeSelect.value != '0') {
            weeklyWorkdaysDiv.hidden = true;
            nthWorkdaysDiv.hidden = false;
        } else {
            weeklyWorkdaysDiv.hidden = false;
            nthWorkdaysDiv.hidden = true;
        }
    }
}

function containsChild(collection, element) {
    for (var i = 0; i < collection.length; i++) {
        if (collection[i].value == element.value) {
            return true;
        }
    }

    return false;
}
