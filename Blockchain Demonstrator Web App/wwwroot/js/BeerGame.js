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
        updateOrderHistory("Retailer");
        updateOrderHistory("Manufacturer");
        updateOrderHistory("Processor");
        updateOrderHistory("Farmer");

        updateIncomingDeliveries("Retailer", gameSerialized);
        updateIncomingDeliveries("Manufacturer", gameSerialized);
        updateIncomingDeliveries("Processor", gameSerialized);
        updateIncomingDeliveries("Farmer", gameSerialized);

        $("#section-Retailer > h4").eq(2).text("Inventory: " + gameSerialized.retailer.inventory);
        $("#section-Manufacturer > h4").eq(2).text("Inventory: " + gameSerialized.manufacturer.inventory);
        $("#section-Processor > h4").eq(2).text("Inventory: " + gameSerialized.processor.inventory);
        $("#section-Farmer > h4").eq(2).text("Inventory: " + gameSerialized.farmer.inventory);

        $("#section-Retailer > h4").eq(3).text("Backorder: " + gameSerialized.retailer.backorder);
        $("#section-Manufacturer > h4").eq(3).text("Backorder: " + gameSerialized.manufacturer.backorder);
        $("#section-Processor > h4").eq(3).text("Backorder: " + gameSerialized.processor.backorder);
        $("#section-Farmer > h4").eq(3).text("Backorder: " + gameSerialized.farmer.backorder);
    }

    const updateOrderHistory = (id) => {
        $(`#section-${id} > section:eq(0) > table > tbody > tr`).each((index, element) => {
            if ($(element).find("td:eq(1)").text() == "") {
                $(element).find("td:eq(1)").text($(`#section-${id} > form > input`).val());
                return false;
            }
        });
    }

    const updateIncomingDeliveries = (id, game) => {
        let orderHistory = getOrders(game[id].id);

        $(`#section-${id} > section:eq(1) > table > tbody > tr`).each((index, element) => {
            $(element).find("td:eq(0)").text(orderHistory.ordernumber);
            $(element).find("td:eq(0)").text(orderHistory.orderday);
            $(element).find("td:eq(1)").text(orderHistory.arrivalday);
            $(element).find("td:eq(2)").text(orderHistory.volume);
        });
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
        sendOrders: sendOrders
    }
})();
 

