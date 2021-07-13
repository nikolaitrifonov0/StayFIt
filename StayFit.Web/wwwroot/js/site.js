import request from './request.js';

handleWorkoutType();

let searchInputs = document.getElementsByClassName('find-exercise');

for (let input of searchInputs) {
    input.oninput = async e => {
        let input = e.target;
        let inputValue = input.value;
        let resultsUl = input.parentElement.querySelector('.results');
        if (inputValue.length >= 3) {
            resultsUl.innerHTML = '';
            let response = await request(url + `Exercises/Find?keyword=${inputValue}`);
            for (let r of response) {
                let li = document.createElement('li');
                li.textContent = r.name;
                li.setAttribute('value', r.id);

                li.onclick = e => {
                    let exercisesUl = li.parentElement.parentElement.querySelector('.exercises');
                    let exerciseLi = document.createElement('li');

                    exerciseLi.innerHTML = li.innerHTML;
                    exerciseLi.setAttribute('value', li.getAttribute('value'));

                    if (!containsChild(exercisesUl.children, exerciseLi)) {
                        exercisesUl.appendChild(exerciseLi);
                    }
                }

                

                resultsUl.appendChild(li);
            }
            
        }
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
