"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notification-hub").build();

connection.on("ReceiveNotification", function (notification) {
    console.log(notification)
});

connection.start();