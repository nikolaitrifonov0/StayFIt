handleWorkoutType();

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
