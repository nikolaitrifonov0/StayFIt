var workoutTypeSelect = document.getElementsByTagName('select')[0];
var weeklyWorkdaysDiv = document.getElementsByClassName('weekly-workdays')[0];
var nthWorkdaysDiv = document.getElementsByClassName('nth-day')[0];

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
