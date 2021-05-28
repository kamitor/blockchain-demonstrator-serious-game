const BeerGame = (() => {

    const configMap = {
        baseUrl: "",
        gameId: "",
        playerId: ""
    }

    const init = (gameId, playerId, baseUrl) => {
        configMap.gameId = gameId;
        configMap.playerId = playerId;
        configMap.baseUrl = baseUrl;
        BeerGame.Signal.init(baseUrl);
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
        sendOrders: sendOrders,
        promptOptions: promptOptions,
        chooseOption: chooseOption,
        updateGameTuningPage: updateGameTuningPage
    }
})();

BeerGame.Signal = (() => {
    
    const configMap = {
        baseUrl: ""
    }

    var connection; 
    $("place-order-button").disabled = true;

    let init = (baseUrl) => {
        configMap.baseUrl = baseUrl;

        connection = new signalR.HubConnectionBuilder().withUrl(`${configMap.baseUrl}/GameHub`).build();

        connection.start().then(function () {
            $("place-order-button").disabled = false;
            if (BeerGame.Cookie.getCookie("JoinedGame") != null) joinGroup();
        }).catch(function (err) {
            return console.error(err.toString());
        });

        connection.on("ShowGame", function (game) {
            $(".lds").hide();
            $(".actor-tab").show();
            BeerGame.updateGameTuningPage(game);
        })

        connection.on("UpdateGame", function (game) {
            BeerGame.updateGameTuningPage(game);
        })

        connection.on("HelloWorld", function () {
            console.log("Hello from Hub");
        })

        connection.on("PromptOptions", function () {
            BeerGame.promptOptions();
        })
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
        connection.invoke("JoinGroup", BeerGame.Cookie.getCookie("JoinedGame")).catch(function (err) {
            return console.error(err.toString());
        })
    }

    let joinGame = (gameId, role, name, playerId) => {
        return connection.invoke("JoinGame", gameId, role, name, playerId);
    }

    return {
        init: init,
        sendOrder: sendOrder,
        joinGame: joinGame
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




