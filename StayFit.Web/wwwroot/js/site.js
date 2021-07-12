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
                let a = document.createElement('a');
                a.textContent = r.name;

                li.appendChild(a);

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
