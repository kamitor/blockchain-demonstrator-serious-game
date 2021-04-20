const BeerGame = (() => {
    
    const configMap = {
        baseUrl: "https://localhost:5001"
    }
    
    const getGame = () => {
        return  $.ajax({
            url: `${configMap.baseUrl}/BeerGame/GetGame`,
            type: "GET"
        })
    }
    
    const updateGame = () => {
        getGame().then(result => {
            let game = result;

            $("#section-{game.Player.Farmer}")
            
        })
    }
    
    return{
        getGame: getGame,
        updateGame: updateGame
    }
})();
 

