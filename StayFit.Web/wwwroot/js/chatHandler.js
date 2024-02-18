import request from './request.js';

export default class ChatHandler {
    constructor(config) {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("/chathub")
            .build();
        connection.start().catch(err => console.error(err));

        $('#chatButton').on('click', (e) => {
            const inputVal = $('#chatInput').val();

            if (inputVal) {
                this._insertRow(config.userName, inputVal);
                connection.invoke("SendMessage", config.userName, inputVal);

                $('#chatInput').val('');
            }
        });

        connection.on("ReceiveMessage", (user, message) => {
            this._insertRow(user, message);
        });
    }

    async read() {
        const logs = await request('/Chat/ReadLogs', 'GET');
        logs.forEach(l => this._insertRow(l.userName, l.message, l.timestamp));
    }

    _insertRow(userName, message, time) {
        const row = document.createElement("div");
        const timeSpan = document.createElement("span");
        const date = time ? new Date(time) : new Date();
        timeSpan.textContent = this._getTimeString(date);
        const userSpan = document.createElement("span");
        userSpan.textContent = ` ${userName}: `;
        const textSpan = document.createElement("span");
        textSpan.textContent = message;

        row.appendChild(timeSpan);
        row.appendChild(userSpan);
        row.appendChild(textSpan);
        const log = document.getElementById('chatLog');
        const lastRow = log.firstChild;

        log.insertBefore(row, lastRow);
    }

    _getTimeString(date) {
        var hours = date.getHours();
        var minutes = date.getMinutes();

        hours = (hours < 10 ? '0' : '') + hours;
        minutes = (minutes < 10 ? '0' : '') + minutes;

        return hours + ':' + minutes;
    }
}