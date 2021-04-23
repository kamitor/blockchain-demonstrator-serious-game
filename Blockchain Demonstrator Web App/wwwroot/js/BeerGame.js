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

        debugger;
        $("#section-Retailer > h4").eq(2).text("Invetory: " + gameSerialized.Retailer.Inventory);
        $("#section-Manufacturer > h4").eq(2).text("Invetory: " + gameSerialized.Manufacturer.Inventory);
        $("#section-Processor > h4").eq(2).text("Invetory: " + gameSerialized.Processor.Inventory);
        $("#section-Farmer > h4").eq(2).text("Invetory: " + gameSerialized.Farmer.Inventory);

        $("#section-Retailer > h4").eq(3).text("Backorder: " + gameSerialized.Retailer.Backorder);
        $("#section-Manufacturer > h4").eq(3).text("Backorder: " + gameSerialized.Manufacturer.Backorder);
        $("#section-Processor > h4").eq(3).text("Backorder: " + gameSerialized.Processor.Backorder);
        $("#section-Farmer > h4").eq(3).text("Backorder: " + gameSerialized.Farmer.Backorder);
    }

    const updateOrderHistory = (id) => {
        $(`#section-${id} > section > table > tbody > tr`).each((index, element) => {
            if (element.lastChild.text() == null) {
                element.lastChild.text($(`#section-${id} > form > input`).val());
                return false;
            }
        });
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
 

