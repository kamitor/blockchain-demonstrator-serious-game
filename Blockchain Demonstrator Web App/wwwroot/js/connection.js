
var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44393/GameHub").build();

let sendOrder = (volume, gameId, playerId) => {
    connection.invoke("SendOrder", volume, gameId, playerId).catch(function (err) {
        return console.error(err.toString())
    })
}

connection.start().then(function () {
    console.log("Connection has been made madafaka!!!");
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("UpdateGame", function (game) {
    BeerGame.updateGame(game);
})