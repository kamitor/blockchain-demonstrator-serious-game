const BeerGame = (() => {

    const configMap = {
        baseUrl: "",
        gameId: "",
        playerId: "",
        waiting: false
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

    //#region updateGameTuningPage
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
    //#endregion

    //#region updateGamePlayerPage
    const updateGamePlayerPage = (gameJson) => {
        game = JSON.parse(gameJson);

        updateGamePlayers(game);

        updateGamePlayerBackorder(game);
        updateGamePlayerIncomingOrder(game);
        updateGamePlayerInventory(game);
        updateGamePlayerIncomingDelivery(game);

        updateGamePlayerTeamBalance(game);
        updateGamePlayerTeamProfit(game);
        updateGamePlayerPlayerBalance(game);
        updateGamePlayerPlayerProfit(game);

        updateGamePlayerCurrentRound(game);
        updateGamePlayerLastOrder(game);

        $("#place-order-text").val("");
        $(".cssload-jumping").hide();
        $("#place-order-button").val("Place order");
        configMap.waiting = false;
    }

    const updateGamePlayers = (game) => {
        let modelIndex = game.Players.findIndex(player => {
            return player.Id == configMap.playerId;
        })

        $("#first-player-name").text(game.Players[(0 >= modelIndex) ? 1 : 0].Name);
        $("#second-player-name").text(game.Players[(1 >= modelIndex) ? 2 : 1].Name);
        $("#third-player-name").text(game.Players[(2 >= modelIndex) ? 3 : 2].Name);
        $("#first-player-role").text(game.Players[(0 >= modelIndex) ? 1 : 0].Role.Id);
        $("#second-player-role").text(game.Players[(1 >= modelIndex) ? 2 : 1].Role.Id);
        $("#third-player-role").text(game.Players[(2 >= modelIndex) ? 3 : 2].Role.Id);
    }

    const updateGamePlayerBackorder = (game) => {
        game.Players.forEach(player => {
            $(`#backorder-${player.Role.Id}`).text(player.Backorder);
        });
    }

    const updateGamePlayerIncomingOrder = (game) => {
        game.Players.forEach(player => {
            let incomingOrder = 0;
            player.IncomingOrders.forEach(order => {
                if (order.OrderDay == game.CurrentDay - 7) incomingOrder += order.Volume;
            });
            $(`#incoming-order-${player.Role.Id}`).text(incomingOrder);            
        });
    }

    const updateGamePlayerInventory = (game) => {
        game.Players.forEach(player => {
            $(`#inventory-${player.Role.Id}`).text(player.Inventory);
        });
    }

    const updateGamePlayerIncomingDelivery = (game) => {
        game.Players.forEach(player => {
            let incomingDelivery = 0;
            player.OutgoingOrders.forEach(order => {
                order.Deliveries.forEach(delivery => {
                    if (delivery.ArrivalDay <= game.CurrentDay && delivery.ArrivalDay > game.CurrentDay - 7) incomingDelivery += delivery.Volume;
                });
            });
            $(`#incoming-delivery-${player.Role.Id}`).text(incomingDelivery);
        });
    }

    const updateGamePlayerTeamBalance = (game) => {
        let teamBalance = 0;
        game.Players.forEach(player => {
            teamBalance += player.Balance;
        });
        let minus = (teamBalance < 0) ? "-" : "";
        if (teamBalance < 0) teamBalance = teamBalance * -1;
        teamBalance = String(teamBalance).replace(".", ",");
        let comma = (teamBalance.includes(",")) ? "" : ","; 
        $("#balance-team").text(minus + "\u20AC" + teamBalance + comma + "-");
    }

    const updateGamePlayerTeamProfit = (game) => {
        let teamProfit = 0;
        game.Players.forEach(player => {
            teamProfit += player.Profit;
        });
        let minus = (teamProfit < 0) ? "-" : "";
        if (teamProfit < 0) teamProfit = teamProfit * -1;
        teamProfit = String(teamProfit).replace(".", ",");
        let comma = (teamProfit.includes(",")) ? "" : ","; 
        $("#profit-team").text(minus + "\u20AC" + teamProfit + comma + "-");
    }

    const updateGamePlayerPlayerBalance = (game) => {
        game.Players.forEach(player => {
            let minus = (player.Balance < 0) ? "-" : "";
            if (player.Balance < 0) player.Balance = player.Balance * -1;
            let balance = String(player.Balance).replace(".", ",");
            let comma = (balance.includes(",")) ? "" : ","; 
            $(`#balance-${player.Role.Id}`).text(minus + "\u20AC" + balance + comma + "-");
        });
    }

    const updateGamePlayerPlayerProfit = (game) => {
        game.Players.forEach(player => {
            let minus = (player.Profit < 0) ? "-" : "";
            if (player.Profit < 0) player.Profit = player.Profit * -1;
            let profit = String(player.Profit).replace(".", ",");
            let comma = (profit.includes(",")) ? "" : ","; 
            $(`#profit-${player.Role.Id}`).text(minus + "\u20AC" + profit + comma + "-");
        });
    }

    const updateGamePlayerCurrentRound = (game) => {
        let currentRound = Math.ceil(game.CurrentDay / 7);
        $("#current-round").text(currentRound);
        if (currentRound == 2) $("#ordinal-suffix").text("nd");
        else if (currentRound == 3) $("#ordinal-suffix").text("rd");
        else if (currentRound >= 4) $("#ordinal-suffix").text("th");
    }

    const updateGamePlayerLastOrder = (game) => {
        game.Players.forEach(player => {
            player.OutgoingOrders.sort((a, b) => {
                if (a.OrderDay > b.OrderDay) return 1;
                else if (a.OrderDay < b.OrderDay) return -1;
                return 0;
            });
            $(`#last-order-${player.Role.Id}`).text(player.OutgoingOrders[player.OutgoingOrders.length - 1].Volume);
        });
    }
    //#endregion

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

    const promptOptions = (playersJson) => {
        $(".top-container").hide();
        $(".bottom-container").hide();
        $("body").append(`
            <section class='option-prompt box'>
                <h3 class='option-prompt--text'>Choose your supplychain setup</h3>
                <div id="YouProvide" class="option-prompt-div">
                    <button class='option-prompt--button gradient' type='button' onclick='BeerGame.Signal.chooseOption("YouProvide")'><h5>You provide</h5></button>
                    <h6 style="display:inline-block;padding-left:10px">You are managing your own transportation system. You own the vehicles. You need to ensure the support staff and handle the administration yourself as an organization.</h6>
                    <div style="display:flex;flex-direction:column;padding-left:10px"></div>
                </div>
                <div id="YouProvideWithHelp" class="option-prompt-div">
                    <button class='option-prompt--button gradient' type='button' onclick='BeerGame.Signal.chooseOption("YouProvideWithHelp")'><h5>You provide with help</h5></button>
                    <h6 style="display:inline-block;padding-left:10px">You are leveraging a transportation company. Your order is priority. You own a few vehicles, yet the bigger part of the shipment is handled by an outside company. You partly handle the administration of the shipments.</h6>
                    <div style="display:flex;flex-direction:column;padding-left:10px"></div>
                </div>
                <div id="TrustedParty" class="option-prompt-div">
                    <button class='option-prompt--button gradient' type='button' onclick='BeerGame.Signal.chooseOption("TrustedParty")'><h5>Trusted party</h5></button>
                    <h6 style="display:inline-block;padding-left:10px">With another name, third party logistics. You hire a third company to handle your orders this way you outsource the responsibility a hiring a support and administration staff. You do not own vehicles for shipping. You partly handle the administration of this shipments.</h6>
                    <div style="display:flex;flex-direction:column;padding-left:10px"></div>
                </div>
                <div id="DLT" class="option-prompt-div">
                    <button class='option-prompt--button gradient' type='button' onclick='BeerGame.Signal.chooseOption("DLT")'><h5>DLT</h5></button>
                    <h6 style="display:inline-block;padding-left:10px">The distributed Ledger technology speed up the information flow in the supply chain. The transportation of the shipment is outsourced. Documentation between the stakeholders, authorities, and your own organization works as real time data. This way the cost can be reduced by 15 to 20% and the lead time can be decreased by 40%.</h6>
                    <div style="display:flex;flex-direction:column;padding-left:10px"></div>
                </div>
            </section>
            <div class="endStatistics box">
                <canvas id="inventoryChart" style="max-width:350px;max-height:350px; display:inline-block;"></canvas>
                <canvas id="orderWorthChart" style="max-width:350px;max-height:350px;display:inline-block;"></canvas>
                <canvas id="overallProfitChart" style="max-width:350px;max-height:350px; display:inline-block;"></canvas>
                <canvas id="grossProfitChart" style="max-width:350px;max-height:350px; display:inline-block;"></canvas>
            </div>`);
        let players = JSON.parse(playersJson);
        players.forEach(player => {
            if (player.Id == configMap.playerId) {
                BeerGame.Graphs.drawChart(Graphs.createLabels(player.InventoryHistory), Graphs.createData(player.InventoryHistory), "inventoryChart", "Inventory", "rgba(46, 49, 146, 1)");
                BeerGame.Graphs.drawChart(Graphs.createLabels(player.OrderWorthHistory), Graphs.createData(player.OrderWorthHistory), "orderWorthChart", "Order worth", "rgba(46, 49, 146, 1)");
                BeerGame.Graphs.drawChart(Graphs.createLabels(player.OverallProfitHistory), Graphs.createData(player.OverallProfitHistory), "overallProfitChart", "Overall profit", "rgba(46, 49, 146, 1)");
                BeerGame.Graphs.drawChart(Graphs.createLabels(player.GrossProfitHistory), Graphs.createData(player.GrossProfitHistory), "grossProfitChart", "Gross profit", "rgba(46, 49, 146, 1)");
            }
        });
    }

    const updatePromptOptions = (playerJson) => {
        let player = JSON.parse(playerJson);
        $(`#${player.Id}`).remove();
        $(`#${player.ChosenOption.Name} > div`).append($(`<b id="${player.Id}" class="gradient-font">${player.Name}</b>`));
    }

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
        checkInGame: checkInGame,
        updatePromptOptions: updatePromptOptions,
        updateGamePlayerPage: updateGamePlayerPage,
        waiting: configMap.waiting
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
            $(".top-container").show();
            $(".bottom-container").show();
            if (document.title == "BeerGame - Blockchain Demonstrator") BeerGame.updateGamePlayerPage(game);
            else if (document.title == "BeerGame Admin - Blockchain Demonstrator") BeerGame.updateGameTuningPage(game);
        });

        connection.on("UpdateGame", function (game) {
            BeerGame.waiting = false;
            if (document.title == "BeerGame - Blockchain Demonstrator" || document.title == "BeerGame Game master - Blockchain Demonstrator") BeerGame.updateGamePlayerPage(game);
            else if (document.title == "BeerGame Admin - Blockchain Demonstrator") BeerGame.updateGameTuningPage(game);
        });

        connection.on("PromptOptions", function (playersJson) {
            if (document.title == "BeerGame - Blockchain Demonstrator") BeerGame.promptOptions(playersJson);
        });

        connection.on("UpdatePromptOptions", function (playerJson) {
            if (document.title == "BeerGame - Blockchain Demonstrator") BeerGame.updatePromptOptions(playerJson)
        });

        connection.on("ClosePromptOptions", function (mostChosenOption) {
            $(".option-prompt").empty();
            $(".option-prompt").append(`<h2>Chosen supplychain setup: <span class=gradient-font>${mostChosenOption.replace(/([A-Z])/g, ' $1').trim()}</span></h2>`);
            setTimeout(() => {
                $(".option-prompt").remove();
                $(".top-container").show();
                $(".bottom-container").show();
            }, 3000);
        });

        connection.on("EndGame", function () {
            $('body').append($(`<form method="post" id="endGameForm" action="/beergame/endgame" style="display:none;">
                                    <input name=gameId type="hidden" value="${configMap.gameId}"/>
                                    <input name=playerId type="hidden" value="${configMap.playerId}"/>
                                </form>`));
            $("#endGameForm").submit();
        });

        connection.on("UpdateGraphs", function (game) {
            BeerGame.Graphs.updateGraphs(game);
        });
    }

    let sendOrder = () => {
        connection.invoke("SendOrder",
            $('#place-order-text').val(),
            BeerGame.Cookie.getCookie('JoinedGame'),
            BeerGame.Cookie.getCookie('PlayerId'))
            .catch(function (err) {
                return console.error(err.toString())
            });
        $("#place-order-button").prop("disabled", true);
        $("#place-order-button").css("filter", "grayscale(100%)");
        $("#place-order-button").val("");
        BeerGame.waiting = true;
        $(".cssload-jumping").show();
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
                $(".top-container").show();
                $(".bottom-container").show();
            }
        });
    }

    let flushInventory = () => {
        connection.invoke("FlushInventory", configMap.gameId, configMap.playerId);
        flushOrder();
    }

    let flushOrder = () => {
        connection.invoke("SendOrder",
            "0",
            BeerGame.Cookie.getCookie('JoinedGame'),
            BeerGame.Cookie.getCookie('PlayerId'))
            .catch(function (err) {
                return console.error(err.toString());
            });
        $("#place-order-button").prop("disabled", true);
        $("#place-order-button").css("filter", "grayscale(100%)");
        $("#place-order-button").val("");
        BeerGame.waiting = true;
        $(".cssload-jumping").show();
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
        checkFull: checkFull,
        flushInventory: flushInventory
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
})();

BeerGame.Graphs = (() => {
    const drawChart = function (labels, data, chartId, labelName, lineColour) {
        let chart = new Chart(document.getElementById(chartId), {
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

    const drawMultipleChart = function (labels, datasets, chartId, title) {
        let graphClass = ($(".graphs-grid").children().length % 2 == 0) ? "graph-left" : "graph-right";
        $(".graphs-grid").append($(`<canvas class="${graphClass} box" id="${chartId}"></canvas>`));
        let chart = new Chart(document.getElementById(chartId), {
            type: 'line',
            data: {
                labels: labels,
                datasets: datasets
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                },
                plugins: {
                    title: {
                        display: true,
                        text: title
                    }
                }
            }
        });
    };

    function createLabels(list) {
        let labels = [];
        for (let i = 0; i < list.length; i++)
        {
            labels.push("Round " + (i + 1));
        }
        return labels;
    }

    function createData(list) {
        let data = [];
        for (let i = 0; i < list.length; i++)
        {
            data.push(String(list[i]).replace(',', '.'));
        }
        return data;
    }

    function createDataSet(data, name, lineColour) { 
        return {
                    label: name,
                    data: data,
                    fill: true,
                    backgroundColor: 'rgba(0, 0, 0, 0)',
                    borderColor: lineColour,
                    borderWidth: 1
                };
    }

    function createInventoryDataSets(players) {
        let dataSets = [];
        for (let i = 0; i < players.length; i++)
        {
            if (i == 0) dataSets.push(createDataSet(createData(players[i].InventoryHistory), players[i].Name, "rgba(255, 0, 0, 1)"));
            if (i == 1) dataSets.push(createDataSet(createData(players[i].InventoryHistory), players[i].Name, "rgba(0, 255, 0, 1)"));
            if (i == 2) dataSets.push(createDataSet(createData(players[i].InventoryHistory), players[i].Name, "rgba(0, 0, 255, 1)"));
            if (i == 3) dataSets.push(createDataSet(createData(players[i].InventoryHistory), players[i].Name, "rgba(255, 255, 0, 1)"));
        }

        return dataSets;
    }

    function createOrderWorthDataSets(players) {
        let dataSets = [];
        for (let i = 0; i < players.length; i++) {
            if (i == 0) dataSets.push(createDataSet(createData(players[i].OrderWorthHistory), players[i].Name, "rgba(255, 0, 0, 1)"));
            if (i == 1) dataSets.push(createDataSet(createData(players[i].OrderWorthHistory), players[i].Name, "rgba(0, 255, 0, 1)"));
            if (i == 2) dataSets.push(createDataSet(createData(players[i].OrderWorthHistory), players[i].Name, "rgba(0, 0, 255, 1)"));
            if (i == 3) dataSets.push(createDataSet(createData(players[i].OrderWorthHistory), players[i].Name, "rgba(255, 255, 0, 1)"));
        }

        return dataSets;
    }

    function createOverallProfitDataSets(players) {
        let dataSets = [];
        for (let i = 0; i < players.length; i++) {
            if (i == 0) dataSets.push(createDataSet(createData(players[i].OverallProfitHistory), players[i].Name, "rgba(255, 0, 0, 1)"));
            if (i == 1) dataSets.push(createDataSet(createData(players[i].OverallProfitHistory), players[i].Name, "rgba(0, 255, 0, 1)"));
            if (i == 2) dataSets.push(createDataSet(createData(players[i].OverallProfitHistory), players[i].Name, "rgba(0, 0, 255, 1)"));
            if (i == 3) dataSets.push(createDataSet(createData(players[i].OverallProfitHistory), players[i].Name, "rgba(255, 255, 0, 1)"));
        }

        return dataSets;
    }

    function createGrossProfitDataSets(players) {
        let dataSets = [];
        for (let i = 0; i < players.length; i++) {
            if (i == 0) dataSets.push(createDataSet(createData(players[i].GrossProfitHistory), players[i].Name, "rgba(255, 0, 0, 1)"));
            if (i == 1) dataSets.push(createDataSet(createData(players[i].GrossProfitHistory), players[i].Name, "rgba(0, 255, 0, 1)"));
            if (i == 2) dataSets.push(createDataSet(createData(players[i].GrossProfitHistory), players[i].Name, "rgba(0, 0, 255, 1)"));
            if (i == 3) dataSets.push(createDataSet(createData(players[i].GrossProfitHistory), players[i].Name, "rgba(255, 255, 0, 1)"));
        }

        return dataSets;
    }

    function updateGraphs(game) {
        $(".graphs-grid").empty();
        drawMultipleChart(createLabels(game.Retailer.InventoryHistory), createInventoryDataSets(game.Players), "inventoryChart","Inventory");
        drawMultipleChart(createLabels(game.Retailer.OrderWorthHistory), createInventoryDataSets(game.Players), "orderWorthChart","Order worth");
        drawMultipleChart(createLabels(game.Retailer.OverallProfitHistory), createInventoryDataSets(game.Players), "overallProfitChart","Overall profit");
        drawMultipleChart(createLabels(game.Retailer.GrossProfitHistory), createInventoryDataSets(game.Players), "grossProfitChart", "Gross profit");
    }

    return {
        drawChart: drawChart,
        drawMultipleChart: drawMultipleChart,
        createLabels: createLabels,
        createInventoryDataSets: createInventoryDataSets,
        createOrderWorthDataSets: createOrderWorthDataSets,
        createOverallProfitDataSets: createOverallProfitDataSets,
        createGrossProfitDataSets: createGrossProfitDataSets,
        updateGraphs: updateGraphs
    }
})();



