const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

connection.on("ReceiveUpdate", function (chatId) {
    window.location.href = `/Chat/Index?id=${chatId}`;
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});
