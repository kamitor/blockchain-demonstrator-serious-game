#!/bin/bash

#Run database
echo "V--------------------Running database--------------------V"
docker start database

#Run REST API and web app
echo "V--------------------Running REST API and web app--------------------V"
url="demonstrator.sparklivinglab.nl"
echo "Warning the URL is currently set to $url\nmake sure this address is correct!" 

(cd ~/blockchain-demonstrator-serious-game/BlockchainDemonstratorApi/bin/Debug/netcoreapp3.1/publish/
dotnet BlockchainDemonstratorApi.dll $url) &
(
cd ~/blockchain-demonstrator-serious-game/BlockchainDemonstratorWebApp/bin/Debug/netcoreapp3.1/publish
dotnet BlockchainDemonstratorWebApp.dll $url)
