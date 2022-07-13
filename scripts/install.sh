#!/bin/bash
echo "V--------------------Updating VM--------------------V"
sudo apt-get update
sudo install git -y
sudo apt-get install dnsutils -y

#Change appsettings.json connection string
sudo sed -i 's/Server=(localdb)\\\\mssqllocaldb;Database=BeerGameContext;Trusted_Connection=True;MultipleActiveResultSets=true/Server=172.17.0.2;Database=BeerGameContext;Trusted_Connection=True;MultipleActiveResultSets=true;User id=sa;Password=B33rgam3;Integrated Security=false/g' ~/blockchain-demonstrator-serious-game/BlockchainDemonstratorApi/appsettings.json

#Install dependencies
echo "V--------------------Installing dependencies--------------------V"
sudo apt install snapd -y
sudo snap install docker
sudo snap install dotnet-sdk --classic --channel=3.1
sudo apt install nginx -y

#Setup database
echo "V--------------------Setting up dependencies--------------------V"
if [[ $(docker ps -a | grep 'database') ]]; then
	sudo docker rm -f database
	echo "Removed old database"
fi
sudo docker pull mcr.microsoft.com/mssql/server:2019-latest
sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=B33rgam3" \
      -p 1433:1433 --name database -h database \
      --restart=always \
      -d mcr.microsoft.com/mssql/server:2019-latest
echo "Created new database"
echo "Waiting 30 extra seconds for database startup"
sleep 30

#Setup nginx
echo "server {
        listen 80;
        server_name _;
        return 301 https://\$host\$request_uri;
}

server {
listen 443 ssl http2;
listen [::]:443;

    root ~/blockchain-demonstrator-serious-game/BlockchainDemonstratorWebApp;

    index index.html index.htm index.nginx-debian.html;

    server_name _;

    server_tokens off;

    ssl_certificate             /etc/ssl/certs/blockchain-demonstrator.crt;
    ssl_certificate_key         /etc/ssl/private/blockchain-demonstrator.key;
    ssl_session_cache           builtin:1000 shared:SSL:10m;
    ssl_protocols               TLSv1.3;
    ssl_prefer_server_ciphers on;

    location / {
            proxy_pass         http://0.0.0.0:5000/;
            proxy_set_header   Host \$host;
            proxy_set_header   X-Forwarded-For \$proxy_add_x_forwarded_for;
            proxy_set_header   X-Forwarded-Proto \$scheme;
    }
}

server {
listen 8080;
listen [::]:8080;

    server_name _;

    root ~/blockchain-demonstrator-serious-game/BlockchainDemonstratorApi;

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
dotnet publish ~/blockchain-demonstrator-serious-game/BlockchainDemonstratorApi
dotnet publish ~/blockchain-demonstrator-serious-game/BlockchainDemonstratorWebApp

#Run REST API and web app
echo "V--------------------Running REST API and web app--------------------V"
url="demonstrator.sparklivinglab.nl"
echo "Warning the URL is currently set to $url make sure this address is correct!" 

(cd ~/blockchain-demonstrator-serious-game/BlockchainDemonstratorApi/bin/Debug/netcoreapp3.1/publish/
dotnet BlockchainDemonstratorApi.dll $url) &
(
cd ~/blockchain-demonstrator-serious-game/BlockchainDemonstratorWebApp/bin/Debug/netcoreapp3.1/publish
dotnet BlockchainDemonstratorWebApp.dll $url)
