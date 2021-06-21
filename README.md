[![Continuous Integration and Deployment](https://github.com/Hogeschool-Windesheim/Beer-Game/actions/workflows/Continuous%20Integration%20and%20Deployment.yml/badge.svg)](https://github.com/Hogeschool-Windesheim/Beer-Game/actions/workflows/Continuous%20Integration%20and%20Deployment.yml)
# Beer-Game
Code for the Blockchain Demonstrator Lab

##Installation guide

###Step 1:
Setup GitHub actions runner on your server. Go to settings > actions > runners and click add runner. 
Follow the steps provided by Github

###Step 2: 
Go to the Actions tab in our repository. Then select the Continuous Integration and Deployment workflow. 
Then click on Run workflow located at the right side of your screen. Then just click the green ***Run workflow*** 
button. Then the workflow should start running on your server.

###Step 3
Since the last step created two new images on your server webapp:latest and api:latest.
Create a docker container for each project.\
This can be done by running these commands: 
`sudo docker run --name api -d -p 0.0.0.0:5002:80 api:latest` and 
`sudo docker run --name webapp -d -p 0.0.0.0:5000:80 webapp:latest`

###Step 4
With the containers running in their own environment, we just need to set up a proxy pass. 
We coupled our server port 80 to 0.0.0.0:5000. When the proxy is set up this will redirect all http traffic
to the docker container containing the web application. For the REST API we couple port 8080 to 0.0.0.0:5002. 
This will redirect all traffic towards the server using port 8080 to the REST API.

We used Nginx to handle the redirections. Our /etc/nginx/sites-available/default file looks like this.\

    server {
    listen 80 default_server;\
    listen [::]:80 default_server;

        root /home/BlockchainDemonstrator/Webapp;

        index index.html index.htm index.nginx-debian.html;

        server_name _;

        location / {

                proxy_pass         http://0.0.0.0:5000/;
                proxy_http_version 1.1;
                proxy_set_header   Upgrade $http_upgrade;
                proxy_set_header   Connection keep-alive;
                proxy_set_header   Host $host;
                proxy_cache_bypass $http_upgrade;
                proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
                proxy_set_header   X-Forwarded-Proto $scheme;
        }
    }

    server {
    listen 8080;
    listen [::]:8080;

        server_name _;

        root /home/BlockchainDemonstrator/Api;

        location / {
        proxy_pass         http://0.0.0.0:5002/;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
        }
    }