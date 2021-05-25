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
            retailerOrder: $("#order-Retailer").val(),
            manufacturerOrder: $("#order-Manufacturer").val(),
            processorOrder: $("#order-Processor").val(),
            farmerOrder: $("#order-Farmer").val(),
        };
        $.ajax({
            url: `${configMap.baseUrl}/api/BeerGame/SendOrders`,
            type: "POST",
            data: JSON.stringify(orderData),
            contentType: "application/json",
            dataType: "text"
        }).then(result => updateGameTuningPage(result));
    }

    const updateGameTuningPage = (game) => {
        gameSerialized = JSON.parse(game);
        updateGameTuningBalance(gameSerialized);
        updateGameTuningProfit(gameSerialized);
        updateGameTuningInventory(gameSerialized);
        updateGameTuningBackorder(gameSerialized);
        updateGameTuningIncomingOrder(gameSerialized);
        updateGameTuningIncomingDeliveries(gameSerialized);
        updateGameTuningOrderHistory(gameSerialized);
        updateGameTuningPayments(gameSerialized);
    }

    const updateGameTuningBalance = (game) => {
        $("#balance-Retailer").text("Balance: " + game.Retailer.Balance);
        $("#balance-Manufacturer").text("Balance: " + game.Manufacturer.Balance);
        $("#balance-Processor").text("Balance: " + game.Processor.Balance);
        $("#balance-Farmer").text("Balance: " + game.Farmer.Balance);
    }

    const updateGameTuningProfit = (game) => {
        $("#profit-Retailer").text("Profit: " + game.Retailer.Profit)
        $("#profit-Manufacturer").text("Profit: " + game.Manufacturer.Profit)
        $("#profit-Processor").text("Profit: " + game.Processor.Profit)
        $("#profit-Farmer").text("Profit: " + game.Farmer.Profit)
    }

    const updateGameTuningInventory = (game) => {
        $("#inventory-Retailer").text("Inventory: " + game.Retailer.Inventory)
        $("#inventory-Manufacturer").text("Inventory: " + game.Manufacturer.Inventory)
        $("#inventory-Processor").text("Inventory: " + game.Processor.Inventory)
        $("#inventory-Farmer").text("Inventory: " + game.Farmer.Inventory)
    }

    const updateGameTuningBackorder = (game) => {
        $("#backorder-Retailer").text("Backorder: " + game.Retailer.Backorder)
        $("#backorder-Manufacturer").text("Backorder: " + game.Manufacturer.Backorder)
        $("#backorder-Processor").text("Backorder: " + game.Processor.Backorder)
        $("#backorder-Farmer").text("Backorder: " + game.Farmer.Backorder)
    }

    const updateGameTuningIncomingOrder = (game) => {
        updateGameTuningIncomingOrderBody("Retailer", game);
        updateGameTuningIncomingOrderBody("Manufacturer", game);
        updateGameTuningIncomingOrderBody("Processor", game);
        updateGameTuningIncomingOrderBody("Farmer", game);

    }

    const updateGameTuningIncomingOrderBody = (player, game) => {
        $("#incomingOrders-" + player).empty();
        game[player].IncomingOrders.forEach(order => {
            if (order.OrderDay == game.CurrentDay - 7) {
                $("#incomingOrders-" + player).append($(`
                    <tr>
                        <td>${order.OrderNumber}</td>
                        <td>${order.OrderDay}</td>
                        <td>${order.Volume}</td>
                    </tr>`));
            }
        });
    }

    const updateGameTuningIncomingDeliveries = (game) => {
        updateGameTuningIncomingDeliveriesBody("Retailer", game);
        updateGameTuningIncomingDeliveriesBody("Manufacturer", game);
        updateGameTuningIncomingDeliveriesBody("Processor", game);
        updateGameTuningIncomingDeliveriesBody("Farmer", game);

    }

    const updateGameTuningIncomingDeliveriesBody = (player, game) => {
        $("#incomingDeliveries-" + player).empty();
        let table = "<tr>";
        game[player].OutgoingOrders.forEach(order => {
            if (order.Deliveries.some((delivery) => delivery.ArrivalDay <= game.CurrentDay && delivery.ArrivalDay > game.CurrentDay - 7)) {
                table += `<td>${order.OrderNumber}</td>
                        <td>${order.OrderDay}</td>
                        <td>${order.Volume}</td>
                        <td>
                            <table>
                                <thead>
                                    <tr>
                                        <th>Volume</th>
                                        <th>Send day</th>
                                        <th>Arrival day</th>
                                        <th>Price</th>
                                    </tr>
                                </thead>
                                <tbody>`;
                order.Deliveries.forEach(delivery => {
                    let style = "";
                    if (delivery.ArrivalDay <= game.CurrentDay && delivery.ArrivalDay > game.CurrentDay - 7) style = `style="background-color: #f5fac5;"`;
                    table += `<tr ${style}>
                            <td>${delivery.Volume}</td>
                            <td>${delivery.SendDeliveryDay}</td>
                            <td>${roundOff(delivery.ArrivalDay)}</td>
                            <td>${delivery.Price}</td>
                        </tr>`;
                });
                table += `</tbody>
                        </table>
                        </td>
                        </tr>`;
                $("#incomingDeliveries-" + player).append($(`${table}`));
            }
        });
    }

    const updateGameTuningOrderHistory = (game) => {
        updateGameTuningOrderHistoryBody("Retailer", game);
        updateGameTuningOrderHistoryBody("Manufacturer", game);
        updateGameTuningOrderHistoryBody("Processor", game);
        updateGameTuningOrderHistoryBody("Farmer", game);
    }

    const updateGameTuningOrderHistoryBody = (player, game) => {
        let map = game[player].OutgoingOrders.map(o => o.OrderNumber);
        let orderNumber = Math.max(...map, 1);
        let volume = game[player].OutgoingOrders.find(order => order.OrderNumber == orderNumber).Volume;
        $("#orderHistory-" + player).append($(`
            <tr>
                <td>${orderNumber}</td>
                <td>${volume}</td>
            </tr>`));
    }

    updateGameTuningPayments = (game) => {
        updateGameTuningPaymentsBody("Retailer", game);
        updateGameTuningPaymentsBody("Manufacturer", game);
        updateGameTuningPaymentsBody("RetailProcessorer", game);
        updateGameTuningPaymentsBody("Farmer", game);
    }

    updateGameTuningPaymentsBody = (player, game) => {
        $("#payments-" + player).empty();
        let style = "";
        let lastPayment = null;
        game[player].Payments.sort((a, b) => {
            if (a.DueDay > b.DueDay) return 1;
            else if (a.DueDay < b.DueDay) return -1;

            if (a.Topic.toLowerCase() > b.Topic.toLowerCase()) return 1;
            if (a.Topic.toLowerCase() < b.Topic.toLowerCase()) return -1;
            return 0;
        });
        game[player].Payments.forEach(payment => {
            if (payment.DueDay <= game.CurrentDay && payment.DueDay > game.CurrentDay - 7) style = "background-color:#f5fac5;";
            if (lastPayment != null
                && Math.ceil(payment.DueDay / 7 + 1 - (1 / 7))
                > Math.ceil(lastPayment.DueDay / 7 + 1 - (1 / 7))) {
                style += "border-top: 3px solid grey;";
            }
            $("#payments-" + player).append(`<tr style="${style}">
                                                <td>${payment.Topic}</td>
                                                <td style="${(payment.Amount >= 0) ? "color:green" : "color:red"}">${payment.Amount}</td>
                                                <td>${roundOff(payment.DueDay)}</td>
                                                <td>${payment.FromPlayer}</td>
                                            </tr>`);
            lastPayment = payment;
            style = "";
        });
    }


    const updateGame = (game) => {
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
        chooseOption: chooseOption,
        updateGameTuningPage: updateGameTuningPage
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
                $('#place-order-text').val(), 
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
        BeerGame.updateGameTuningPage(game);
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

 

