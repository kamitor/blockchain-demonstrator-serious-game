const BeerGame = (() => {
    const init = () => {
        //TODO: move gameId out of function parameters and into configMap
    }

    const configMap = {
        baseUrl: "https://localhost:44393"
    }
    
    const getGame = () => {
        return  $.ajax({
            url: `${configMap.baseUrl}/BeerGame/GetGame`,
            type: "POST"
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
    
    return{
        getGame: getGame,
        updateGame: updateGame,
        sendOrders: sendOrders
    }
})();
 

