const BeerGame = (() => {

    const configMap = {
        baseUrl: "https://localhost:44393",
        gameId: "",
        playerId: ""
    }

    const init = (gameId, playerId) => {
        configMap.gameId = gameId;
        configMap.playerId = playerId;
        BeerGame.Signal.init();
    }

    const getGame = () => {
        return $.ajax({
            url: `${configMap.baseUrl}/api/BeerGame/GetGame`,
            type: "POST",
            data: JSON.stringify(configMap.gameId),
            contentType: "application/JSON",
            dataType: "text"
        })
    }

    const sendOrders = (gameId) => {
        orderData =
            {
                gameId: gameId,
                retailerOrder: $("#section-Retailer > form > input").val(),
                manufacturerOrder: $("#section-Manufacturer > form > input").val(),
                processorOrder: $("#section-Processor > form > input").val(),
                farmerOrder: $("#section-Farmer > form > input").val(),
            };
        console.log(orderData);
        $.ajax({
            url: `${configMap.baseUrl}/api/BeerGame/SendOrders`,
            type: "POST",
            data: JSON.stringify(orderData),
            contentType: "application/json",
            dataType: "text"
        }).then(result => updateGame(result));
    }

    const updateGame = (game) => {
        console.log("binnen updategame func");
        gameSerialized = JSON.parse(game);
        updateOrderHistory("Retailer", gameSerialized.Retailer.CurrentOrder.OrderNumber, gameSerialized.Retailer.CurrentOrder.Volume);
        updateOrderHistory("Manufacturer", gameSerialized.Manufacturer.CurrentOrder.OrderNumber, gameSerialized.Manufacturer.CurrentOrder.Volume);
        updateOrderHistory("Processor", gameSerialized.Processor.CurrentOrder.OrderNumber, gameSerialized.Processor.CurrentOrder.Volume);
        updateOrderHistory("Farmer", gameSerialized.Farmer.CurrentOrder.OrderNumber, gameSerialized.Farmer.CurrentOrder.Volume);

        updateIncomingDeliveries("Retailer", gameSerialized);
        updateIncomingDeliveries("Manufacturer", gameSerialized);
        updateIncomingDeliveries("Processor", gameSerialized);
        updateIncomingDeliveries("Farmer", gameSerialized);

        updateIncomingOrder("Retailer", gameSerialized);
        updateIncomingOrder("Manufacturer", gameSerialized);
        updateIncomingOrder("Processor", gameSerialized);
        updateIncomingOrder("Farmer", gameSerialized);

        $("#balance-Retailer").html("<b>Balance: </b>" + roundOff(gameSerialized.Retailer.Balance));
        $("#balance-Manufacturer").html("<b>Balance: </b>" + roundOff(gameSerialized.Manufacturer.Balance));
        $("#balance-Processor").html("<b>Balance: </b>" + roundOff(gameSerialized.Processor.Balance));
        $("#balance-Farmer").html("<b>Balance: </b>" + roundOff(gameSerialized.Farmer.Balance));

        /*        $("#profit-Retailer").html("<b>Profit: </b>" + roundOff(gameSerialized.retailer.profit));
                $("#profit-Manufacturer").html("<b>Profit: </b>" + roundOff(gameSerialized.Manufacturer.profit));
                $("#profit-Processor").html("<b>Profit: </b>" + roundOff(gameSerialized.processor.profit));
                $("#profit-Farmer").html("<b>Profit: </b>" + roundOff(gameSerialized.farmer.profit));*/

        /*        $("#section-Retailer").html("Margin: " + roundOff(gameSerialized.retailer.margin));
                $("#section-Manufacturer").html("Margin: " + roundOff(gameSerialized.Manufacturer.margin));
                $("#section-Processor").html("Margin: " + roundOff(gameSerialized.processor.margin));
                $("#section-Farmer").html("Margin: " + roundOff(gameSerialized.farmer.margin));*/

        $("#inventory-Retailer").html("<b>Inventory: </b>" + gameSerialized.Retailer.Inventory);
        $("#inventory-Manufacturer").html("<b>Inventory: </b>" + gameSerialized.Manufacturer.Inventory);
        $("#inventory-Processor").html("<b>Inventory: </b>" + gameSerialized.Processor.Inventory);
        $("#inventory-Farmer").html("<b>Inventory: </b>" + gameSerialized.Farmer.Inventory);

        $("#backorder-Retailer").html("<b>Backorder: </b>" + gameSerialized.Retailer.Backorder);
        $("#backorder-Manufacturer").html("<b>Backorder: </b>" + gameSerialized.Manufacturer.Backorder);
        $("#backorder-Processor").html("<b>Backorder: </b>" + gameSerialized.Processor.Backorder);
        $("#backorder-Farmer").html("<b>Backorder: </b>" + gameSerialized.Farmer.Backorder);

        $("#currentDay").text("day: " + gameSerialized.CurrentDay);
    }

    const updateOrderHistory = (id, orderNumber, orderVolume) => {
        $(`#orderHistory-${id} > tbody`)
            .append($(`<tr><td class="order-history">${orderNumber}</td><td class="order-history">${orderVolume}</td></tr>`));
    }

    const updateIncomingOrder = (id, game) => {
        game[id].IncomingOrders.forEach(order => {
            if (order.OrderDay == game.CurrentDay - 7) {
                $(`#incoming-order-${id}`).html("<b>Incoming order: </b>" + order.Volume);
            }
        });
    }

    const updateIncomingDeliveries = (id, game) => {
        $(`#incomingDeliveries-${id} > tbody`).empty();
        console.log(game[id].OutgoingOrders);
        game[id].OutgoingOrders.forEach(order => {
            order.Deliveries.forEach(delivery => {
                if (delivery.ArrivalDay <= game.CurrentDay && delivery.ArrivalDay > game.CurrentDay - 7) {
                    $(`#incomingDeliveries-${id} > tbody`)
                        .append($(`<tr>
                        <td class="order-history">${delivery.Volume}</td>
                        </tr>`));
                }
            });
        });
    }

    const joinGame = (gameId, role, name, playerId) => {
        if (configMap.gameId !== null) {
            return $.ajax({
                url: `${configMap.baseUrl}/api/BeerGame/JoinGame`,
                type: "POST",
                data: JSON.stringify({gameId: gameId, role: role, name: name, playerId: playerId}),
                contentType: "application/JSON",
                dataType: "text"
            })
        }
    }

    const roundOff = (num) => {
        return +(Math.round(num + "e+2") + "e-2");
    }

    const promptOptions = () => {
        $("body").append(`
            <section class='option-prompt'>
                <p class='option-prompt--text'>Choose your supply chain setup</p>
                <button class='option-prompt--button' type='button' onclick='BeerGame.chooseOption("YouProvide")'>You provide</button>
                <button class='option-prompt--button' type='button' onclick='BeerGame.chooseOption("YouProvideWithHelp")'>You provide with help</button>
                <button class='option-prompt--button' type='button' onclick='BeerGame.chooseOption("TrustedParty")'>Trusted party</button>
                <button class='option-prompt--button' type='button' onclick='BeerGame.chooseOption("DLT")'>DLT</button>
            </section>`);
    }

    const chooseOption = (option) => {
        $(".option-prompt").remove();
        $.ajax({
            url: `${configMap.baseUrl}/api/BeerGame/ChooseOption`,
            type: "POST",
            data: JSON.stringify({option: option, playerId: configMap.playerId}),
            contentType: "application/json",
            dataType: "text"
        }).then(result => {
            //TODO: update game with new setup chosen
        });
    }

    return {
        init: init,
        getGame: getGame,
        joinGame: joinGame,
        updateGame: updateGame,
        sendOrders: sendOrders,
        updateIncomingOrder: updateIncomingOrder,
        updateIncomingDeliveries: updateIncomingDeliveries,
        promptOptions: promptOptions,
        chooseOption: chooseOption
    }
})();

BeerGame.Signal = (() => {

    var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44393/GameHub").build();
    $("place-order-button").disabled = true;

    connection.start().then(function () {
        console.log("Connection has been made!");
        $("place-order-button").disabled = false;
        joinGroup();
    }).catch(function (err) {
        return console.error(err.toString());
    });

    let init = () => {

    }

    let sendOrder = () => {
        connection.invoke("SendOrder",
                $('.place-order-text').val(), 
                BeerGame.Cookie.getCookie('JoinedGame'), 
                BeerGame.Cookie.getCookie('PlayerId'))
            .catch(function (err) {
            return console.error(err.toString())
        })
    }

    let joinGroup = () => {
        console.log("joined group")
        connection.invoke("JoinGroup", BeerGame.Cookie.getCookie("JoinedGame")).catch(function (err) {
            return console.error(err.toString())
        })
    }

    connection.on("UpdateGame", function (game) {
        BeerGame.updateGame(game);
    })

    connection.on("HelloWorld", function () {
        console.log("Hello from Hub");
    })

    connection.on("PromptOptions", function () {
        BeerGame.promptOptions();
    })

    return {
        init: init,
        sendOrder: sendOrder
    }
})();

BeerGame.Cookie = (() => {
    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(';').shift();
    }

    return {
        getCookie: getCookie
    }
})()

 

