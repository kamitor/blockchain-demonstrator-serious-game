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
        BeerGame.Signal.init(baseUrl, gameId, playerId);
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
        $("#orderHistory-" + player).empty();
        game[player].OutgoingOrders.sort((a, b) => {
            if (a.OrderDay > b.OrderDay) return 1;
            else if (a.OrderDay < b.OrderDay) return -1;
            return 0;
        });
        game[player].OutgoingOrders.forEach(order => {
            if (order.OrderNumber != 0) {
                $("#orderHistory-" + player).append($(`
            <tr>
                <td>${order.OrderNumber}</td>
                <td>${order.Volume}</td>
            </tr>`));
            }
        });
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
        $(".actor-tab").hide();
        $("body").append(`
            <section class='option-prompt box'>
                <h3 class='option-prompt--text'>Choose your supplychain setup</h3>
                <div id="YouProvide" class="option-prompt-div">
                    <button class='option-prompt--button gradient' type='button' onclick='BeerGame.Signal.chooseOption("YouProvide")'><h5>You provide</h5></button>
                    <h6 style="display:inline-block;padding-left:10px">Description alksdkjlasd jlkadjlkasdjkla djlk asdjklas djkl;as djlk;adjklasd</h6>
                    <div style="display:flex;flex-direction:column;padding-left:10px"></div>
                </div>
                <div id="YouProvideWithHelp" class="option-prompt-div">
                    <button class='option-prompt--button gradient' type='button' onclick='BeerGame.Signal.chooseOption("YouProvideWithHelp")'><h5>You provide with help</h5></button>
                    <h6 style="display:inline-block;padding-left:10px">Description alksdkjlasd jlkadjlkasdjkla djlk asdjklas djkl;as djlk;adjklasd</h6>
                    <div style="display:flex;flex-direction:column;padding-left:10px"></div>
                </div>
                <div id="TrustedParty" class="option-prompt-div">
                    <button class='option-prompt--button gradient' type='button' onclick='BeerGame.Signal.chooseOption("TrustedParty")'><h5>Trusted party</h5></button>
                    <h6 style="display:inline-block;padding-left:10px">Description alksdkjlasd jlkadjlkasdjkla djlk asdjklas djkl;as djlk;adjklasd</h6>
                    <div style="display:flex;flex-direction:column;padding-left:10px"></div>
                </div>
                <div id="DLT" class="option-prompt-div">
                    <button class='option-prompt--button gradient' type='button' onclick='BeerGame.Signal.chooseOption("DLT")'><h5>DLT</h5></button>
                    <h6 style="display:inline-block;padding-left:10px">Description alksdkjlasd jlkadjlkasdjkla djlk asdjklas djkl;as djlk;adjklasd</h6>
                    <div style="display:flex;flex-direction:column;padding-left:10px"></div>
                </div>
            </section>`);
    }

    const updatePromptOption = (playerJson) => {
        let player = JSON.parse(playerJson);
        $(`#${player.Id}`).remove();
        $(`#${player.ChosenOption.Name} > div`).append($(`<b id="${player.Id}" class="gradient-font">${player.Name}</b>`));
    }

    const drawChart = function (labels, data, chartId, labelName, lineColour) {
        let ctx = document.getElementById(chartId).getContext('2d');
        myChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [{
                    label: labelName,
                    data: data,
                    fill: true,
                    backgroundColor: 'rgba(46, 49, 146, 0.2)',
                    borderColor: lineColour,

                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    };

    const checkInGame = (playerId) => {
        return $.ajax({
            url: `${configMap.baseUrl}/api/BeerGame/CheckInGame`,
            type: "POST",
            data: JSON.stringify(playerId),
            contentType: "application/JSON",
            dataType: "text"
        })
    }

    return {
        init: init,
        getGame: getGame,
        joinGame: joinGame,
        sendOrders: sendOrders,
        promptOptions: promptOptions,
        updateGameTuningPage: updateGameTuningPage,
        drawChart: drawChart,
        checkInGame: checkInGame,
        updatePromptOption: updatePromptOption
    }
})();

BeerGame.Signal = (() => {
    
    const configMap = {
        baseUrl: "",
        gameId: "",
        playerId: "",
        ready: false,
        interval: null
    }

    var connection; 
    $("place-order-button").disabled = true;

    let init = (baseUrl, gameId, playerId) => {
        configMap.baseUrl = baseUrl;
        configMap.gameId = gameId;
        configMap.playerId = playerId;

        connection = new signalR.HubConnectionBuilder().withUrl(`${configMap.baseUrl}/GameHub`).build();

        connection.start().then(function () {
            configMap.ready = true;
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

        connection.on("UpdatePromptOptions", function (playerJson) {
            BeerGame.updatePromptOption(playerJson)
        })

        connection.on("ClosePromptOptions", function (mostChosenOption) {
            $(".option-prompt").empty();
            $(".option-prompt").append(`<h2>Chosen supplychain setup: <span class=gradient-font>${mostChosenOption.replace(/([A-Z])/g, ' $1').trim()}</span></h2>`);
            setTimeout(() => {
                $(".option-prompt").remove();
                $(".actor-tab").show();
            },3000);
        })

        connection.on("EndGame", function () {
            $('body').append($(`<form id="endGameForm" action="/beergame/endgame" style="display:none;">
                                    <input name=gameId type="hidden" value="${configMap.gameId}"/>
                                    <input name=playerId type="hidden" value="${configMap.playerId}"/>
                                </form>`));
            $("#endGameForm").submit();

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

    let chooseOption = (option) => {
        connection.invoke("ChooseOption", configMap.playerId, option).then((result) => {
            if (result) {
                $(".option-prompt").remove();
                $(".actor-tab").show();
            }
        });
    }

    let checkAvailableRoles = () => {
        if (configMap.ready) {
            connection.invoke("CheckAvailableRoles", configMap.gameId).then((result) => {
                if ($("#name").val() != "") {
                    if (result.includes("Retailer")) $("input[value=Retailer]").prop("disabled", false);
                    else $("input[value=Retailer]").prop("disabled", true);

                    if (result.includes("Manufacturer")) $("input[value=Manufacturer]").prop("disabled", false);
                    else $("input[value=Manufacturer]").prop("disabled", true);

                    if (result.includes("Processor")) $("input[value=Processor]").prop("disabled", false);
                    else $("input[value=Processor]").prop("disabled", true);

                    if (result.includes("Farmer")) $("input[value=Farmer]").prop("disabled", false);
                    else $("input[value=Farmer]").prop("disabled", true);
                }
                else {
                    $("input[type=button]").prop("disabled", true);
                }
            });
            
        }
    }

    let checkFull = (gameId) => {
        return connection.invoke("CheckAvailableRoles", gameId);
    }

    let startAvailableRolesInterval = () => { configMap.interval =  setInterval(checkAvailableRoles, 1000); }

    return {
        init: init,
        sendOrder: sendOrder,
        joinGame: joinGame,
        chooseOption: chooseOption,
        startAvailableRolesInterval: startAvailableRolesInterval,
        checkFull: checkFull
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




