#!/bin/bash
echo "V--------------------Updating VM--------------------V"
sudo apt-get update

#Get code from repository
echo "V--------------------Getting code from repository--------------------V"
[ -d "~/Beer-Game" ] && sudo rm -r ~/Beer-Game
git clone https://github.com/Hogeschool-Windesheim/Beer-Game.git ~/Beer-Game

#Change appsettings.json connection string
sudo sed -i 's/Server=(localdb)\\\\mssqllocaldb;Database=BeerGameContext;Trusted_Connection=True;MultipleActiveResultSets=true/Server=172.17.0.2;Database=BeerGameContext;Trusted_Connection=True;MultipleActiveResultSets=true;User id=sa;Password=B33rgam3;Integrated Security=false/g' ~/Beer-Game/BlockchainDemonstratorApi/appsettings.json

#Install dependencies
echo "V--------------------Installing dependencies--------------------V"
sudo snap install docker
sudo snap install dotnet-sdk --classic --channel=3.1
sudo apt install nginx -y

#Setup database
echo "V--------------------Setting up database--------------------V"
if [[ $(docker ps -a | grep 'Exited.*database') ]]; then
        sudo docker start database
elif ! [[ $(docker ps -a | grep 'database') ]]; then
   sudo docker pull mcr.microsoft.com/mssql/server:2019-latest
   sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=B33rgam3" \
      -p 1433:1433 --name database -h database \
      -d mcr.microsoft.com/mssql/server:2019-latest
else
	echo "Database had already been succesfully set up"
fi

#Setup nginx
echo "server {
listen 80 default_server;
listen [::]:80 default_server;

    root ~/Beer-Game/BlockchainDemonstratorWebApp;

    index index.html index.htm index.nginx-debian.html;

    server_name _;

    location / {

            proxy_pass         http://0.0.0.0:5000/;
            proxy_http_version 1.1;
            proxy_set_header   Upgrade \$http_upgrade;
            proxy_set_header   Connection keep-alive;
            proxy_set_header   Host \$host;
            proxy_cache_bypass \$http_upgrade;
            proxy_set_header   X-Forwarded-For \$proxy_add_x_forwarded_for;
            proxy_set_header   X-Forwarded-Proto \$scheme;
    }
}

server {
listen 8080;
listen [::]:8080;

    server_name _;

    root ~/Beer-Game/BlockchainDemonstratorApi;

    location / {
    proxy_pass         http://0.0.0.0:5002/;
    proxy_http_version 1.1;
    proxy_set_header   Upgrade \$http_upgrade;
    proxy_set_header   Connection keep-alive;
    proxy_set_header   Host \$host;
    proxy_cache_bypass \$http_upgrade;
    proxy_set_header   X-Forwarded-For \$proxy_add_x_forwarded_for;
    proxy_set_header   X-Forwarded-Proto \$scheme;
    }
}
" > /etc/nginx/sites-available/default
sudo service nginx restart

#Build REST API and web app
echo "V--------------------Building REST API and web app--------------------V"
dotnet publish ~/Beer-Game/BlockchainDemonstratorApi
dotnet publish ~/Beer-Game/BlockchainDemonstratorWebApp

#Run REST API and web app
echo "V--------------------Running REST API and web app--------------------V"
url=$(dig +short myip.opendns.com @resolver1.opendns.com)

(cd ~/Beer-Game/BlockchainDemonstratorApi/bin/Debug/netcoreapp3.1/publish/
dotnet BlockchainDemonstratorApi.dll $url) &
(
cd ~/Beer-Game/BlockchainDemonstratorWebApp/bin/Debug/netcoreapp3.1/publish
dotnet BlockchainDemonstratorWebApp.dll $url)
