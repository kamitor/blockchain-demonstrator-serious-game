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
        gameSerialized = JSON.parse(game);
        updateOrderHistory("Retailer", gameSerialized.retailer.currentOrder.orderNumber, gameSerialized.retailer.currentOrder.volume);
        updateOrderHistory("Manufacturer", gameSerialized.manufacturer.currentOrder.orderNumber, gameSerialized.manufacturer.currentOrder.volume);
        updateOrderHistory("Processor", gameSerialized.processor.currentOrder.orderNumber, gameSerialized.processor.currentOrder.volume);
        updateOrderHistory("Farmer", gameSerialized.farmer.currentOrder.orderNumber, gameSerialized.farmer.currentOrder.volume);

        updateIncomingDeliveries("Retailer", gameSerialized);
        updateIncomingDeliveries("Manufacturer", gameSerialized);
        updateIncomingDeliveries("Processor", gameSerialized);
        updateIncomingDeliveries("Farmer", gameSerialized);

        updateIncomingOrder("Retailer", gameSerialized);
        updateIncomingOrder("Manufacturer", gameSerialized);
        updateIncomingOrder("Processor", gameSerialized);
        updateIncomingOrder("Farmer", gameSerialized);

        $("#section-Retailer > h3[name='balance']").text("Balance: " + roundOff(gameSerialized.retailer.balance));
        $("#section-Manufacturer > h3[name='balance']").text("Balance: " + roundOff(gameSerialized.manufacturer.balance));
        $("#section-Processor > h3[name='balance']").text("Balance: " + roundOff(gameSerialized.processor.balance));
        $("#section-Farmer > h3[name='balance']").text("Balance: " + roundOff(gameSerialized.farmer.balance));

        $("#section-Retailer > h3[name='profit']").text("Profit: " + roundOff(gameSerialized.retailer.profit));
        $("#section-Manufacturer > h3[name='profit']").text("Profit: " + roundOff(gameSerialized.manufacturer.profit));
        $("#section-Processor > h3[name='profit']").text("Profit: " + roundOff(gameSerialized.processor.profit));
        $("#section-Farmer > h3[name='profit']").text("Profit: " + roundOff(gameSerialized.farmer.profit));

        $("#section-Retailer > h3[name='margin']").text("Margin: " + roundOff(gameSerialized.retailer.margin));
        $("#section-Manufacturer > h3[name='margin']").text("Margin: " + roundOff(gameSerialized.manufacturer.margin));
        $("#section-Processor > h3[name='margin']").text("Margin: " + roundOff(gameSerialized.processor.margin));
        $("#section-Farmer > h3[name='margin']").text("Margin: " + roundOff(gameSerialized.farmer.margin));

        $("#section-Retailer > h3[name='inventory']").text("Inventory: " + gameSerialized.retailer.inventory);
        $("#section-Manufacturer > h3[name='inventory']").text("Inventory: " + gameSerialized.manufacturer.inventory);
        $("#section-Processor > h3[name='inventory']").text("Inventory: " + gameSerialized.processor.inventory);
        $("#section-Farmer > h3[name='inventory']").text("Inventory: " + gameSerialized.farmer.inventory);

        $("#section-Retailer > h3[name='backorder']").text("Backorder: " + gameSerialized.retailer.backorder);
        $("#section-Manufacturer > h3[name='backorder']").text("Backorder: " + gameSerialized.manufacturer.backorder);
        $("#section-Processor > h3[name='backorder']").text("Backorder: " + gameSerialized.processor.backorder);
        $("#section-Farmer > h3[name='backorder']").text("Backorder: " + gameSerialized.farmer.backorder);
    }

    const updateOrderHistory = (id, orderNumber, orderVolume) => {
        $(`#section-${id} > table[name='orderHistory'] > tbody`)
            .append($(`<tr><td class="order-history">${orderNumber}</td><td class="order-history">${orderVolume}</td></tr>`));
    }

    const updateIncomingOrder = (id, game) => {
        game[id.toLowerCase()].incomingOrders.forEach(order => {
            if (order.orderDay == game.currentDay - 7) {
                $(`#section-${id} > h3[name='incomingOrder']`).text("Incoming order: " + order.volume);
            }
        });
    }

    const updateIncomingDeliveries = (id, game) => {
        $(`#section-${id} > table[name='incomingDeliveries'] > tbody`).empty();
        game[id.toLowerCase()].outgoingOrders.forEach(order => {
            order.deliveries.forEach(delivery => {
                if (delivery.arrivalDay <= game.currentDay && delivery.arrivalDay > game.currentDay - 7) {
                    $(`#section-${id} > table[name='incomingDeliveries'] > tbody`)
                        .append($(`<tr>
                        <td class="order-history">${delivery.volume}</td>
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
    }).catch(function (err) {
        return console.error(err.toString());
    });

    let init = () => {

        console.log("binnen")
    }

    let sendOrder = (volume, gameId, playerId) => {
        
        connection.invoke("SendOrder", $('.place-order-text').val(), BeerGame.Cookie.getCookie('JoinedGame'), BeerGame.Cookie.getCookie('PlayerId')).catch(function (err) {
            return console.error(err.toString())
        })
    }
    
    let helloWorld = () => {
        connection.invoke("HelloWorld").catch(function (err) {
            return console.error(err.toString())
        })
    }

    connection.on("UpdateGame", function (game) {
        BeerGame.updateGame(game);
    })
    
    connection.on("HelloWorld", function () {
        console.log("Hello from Hub");
    })

    return {
        init: init,
        sendOrder: sendOrder,
        helloWorld: helloWorld
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

 

