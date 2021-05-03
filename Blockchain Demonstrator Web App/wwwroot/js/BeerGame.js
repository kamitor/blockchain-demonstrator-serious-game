const BeerGame = (() => {
    
    const configMap = {
        baseUrl: "https://localhost:44393",
        gameId: ""
    }
    
    const init = (gameId) => {
        configMap.gameId = gameId
    }
    
    const getGame = () => {
        return  $.ajax({
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
            retailerOrder: $("#section-Retailer > form > input").val(), //TODO: fix later
            manufacturerOrder: $("#section-Manufacturer > form > input").val(),
            processorOrder: $("#section-Processor > form > input").val(),
            farmerOrder: $("#section-Farmer > form > input").val(),
        };
        console.log(orderData);
        $.ajax({
            url: `${configMap.baseUrl}/api/BeerGame/SendOrders`, //TODO: CORS
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

        $("#section-Retailer > h4").eq(2).text("Inventory: " + gameSerialized.retailer.inventory);
        $("#section-Manufacturer > h4").eq(2).text("Inventory: " + gameSerialized.manufacturer.inventory);
        $("#section-Processor > h4").eq(2).text("Inventory: " + gameSerialized.processor.inventory);
        $("#section-Farmer > h4").eq(2).text("Inventory: " + gameSerialized.farmer.inventory);

        $("#section-Retailer > h4").eq(3).text("Backorder: " + gameSerialized.retailer.backorder);
        $("#section-Manufacturer > h4").eq(3).text("Backorder: " + gameSerialized.manufacturer.backorder);
        $("#section-Processor > h4").eq(3).text("Backorder: " + gameSerialized.processor.backorder);
        $("#section-Farmer > h4").eq(3).text("Backorder: " + gameSerialized.farmer.backorder);
    }

    const updateOrderHistory = (id, orderNumber, orderVolume) => {
        $(`#section-${id} > section:eq(0) > table > tbody`)
            .append($(`<tr><td class="order-history">${orderNumber}</td><td class="order-history">${orderVolume}</td></tr>`));
    }

    const updateIncomingOrder = (id, game) => {
        $(`#section-${id} > section:eq(1) > table > tbody`).empty();
        game[id.toLowerCase()].incomingOrders.forEach(order => {
            if (order.orderDay == game.currentDay - 5)
                $(`#section-${id} > section:eq(1) > table > tbody`)
                    .append($(`<tr>
                        <td class="order-history">${order.volume}</td>
                        </tr>`));
        })
    }

    const updateIncomingDeliveries = (id, game) => {
        $(`#section-${id} > section:eq(2) > table > tbody`).empty();
        game[id.toLowerCase()].incomingDeliveries.forEach(order => {
            if (order.arrivalDay <= game.currentDay && order.arrivalDay > game.currentDay - 5)
            $(`#section-${id} > section:eq(2) > table > tbody`)
                .append($(`<tr>
                        <td class="order-history">${order.orderNumber}</td>
                        <td class="order-history">${order.orderDay}</td>
                        <td class="order-history">${order.arrivalDay}</td>
                        <td class="order-history">${order.volume}</td>
                        </tr>`));
        })
       
            
    }

    const getOrders = (playerId) => {
        $.ajax({
            url: `${configMap.baseUrl}/api/BeerGame/GetOrders`,
            type: "POST",
            data: JSON.stringify(playerId),
            contentType: "application/JSON",
            dataType: "text"
        }).then(result => {
            return JSON.parse(result);
        })
    }
    
    const joinGame = (role, name) => {
        if (configMap.gameId !== null){
            $.ajax({
                url: `${configMap.baseUrl}/api/BeerGame/JoinGame`,
                type: "POST",
                data: JSON.stringify({gameId: configMap.gameId, role: role, name: name}),
                contentType: "application/JSON",
                dataType: "text"
            })
        }
    }
    
    return{
        init: init,
        getGame: getGame,
        joinGame: joinGame,
        updateGame: updateGame,
        sendOrders: sendOrders,
        updateIncomingOrder: updateIncomingOrder,
        updateIncomingDeliveries: updateIncomingDeliveries
    }
})();
 

