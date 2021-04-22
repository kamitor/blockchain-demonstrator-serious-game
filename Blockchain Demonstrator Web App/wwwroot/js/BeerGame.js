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
        return $.ajax({
            url: `${configMap.baseUrl}/api/BeerGame/SendOrders`, //TODO: CORS
            type: "POST",
            data: JSON.stringify(orderData),
            contentType: "application/json",
            dataType: "text"
        })
    }
    
    const updateGame = () => {
        getGame().then(result => {
            let game = JSON.parse(result);
            game.farmer.incomingOrder.forEach(order => {

            })
            //$("#section-Farmer").append("" + game.farmer.)
            
        })
    }
    
    const joinGame = (role, name) => {
        if (configMap.gameId !== null){
            $.ajax({
                url: `${configMap.baseUrl}/BeerGame/JoinGame`,
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
 

