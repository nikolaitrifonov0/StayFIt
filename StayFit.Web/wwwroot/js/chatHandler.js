export default class ChatHandler {
    constructor(config) {
        $('#chatButton').on('click', (e) => {
            const row = document.createElement("div");
            const userSpan = document.createElement("span");
            userSpan.textContent = config.userName;
            const textSpan = document.createElement("span");
            textSpan.textContent = $('#chatInput').val();

            row.appendChild(userSpan);
            row.appendChild(textSpan);
            document.getElementById('chatLog').appendChild(row);
        });
    }
}