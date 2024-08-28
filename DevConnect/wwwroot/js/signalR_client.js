const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

connection.on("ReceiveUpdate", function (chatId) {
    window.location.href = `/Chat/Index?id=${chatId}`;
});

connection.start().catch(function (err) {
    console.error(err.toString());
});

function ScrollToLastMessages() {
    var element = document.getElementById('last-messages');
    if (element) {
        element.scrollIntoView({ behavior: 'smooth', block: 'end' });
    }
}

window.onload = function () {
    ScrollToLastMessages();
};
