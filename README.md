[![.github/workflows/Build & Test project.yml](https://github.com/Hogeschool-Windesheim/Beer-Game/actions/workflows/Build%20&%20Test%20project.yml/badge.svg)](https://github.com/Hogeschool-Windesheim/Beer-Game/actions/workflows/Build%20&%20Test%20project.yml)
[![Continuous Integration and Deployment](https://github.com/Hogeschool-Windesheim/Beer-Game/actions/workflows/Continuous%20Integration%20and%20Deployment.yml/badge.svg)](https://github.com/Hogeschool-Windesheim/Beer-Game/actions/workflows/Continuous%20Integration%20and%20Deployment.yml)
# Beer-Game
Code for the Blockchain Demonstrator Lab

# Installation guide
There are two methods of installation. The first method uses an installation script which has to be 
manually executed. The second method uses GitHub actions, this method also has the benefit of enabling CI/CD 
on your server. 
## Installation script
The steps below will take you through the installation process using an installation script.
### Prerequisite 
You have downloaded the installation script from the GitHub repository. Don't mind the Build & Test project.yml & Continious Intergration and Deployment failing as that does not matter for this installation process.
<br>
<br>
### Step 1
After the virtual machine has been set up, you should first make sure the following ports are open: 80 and 8080. Because 
these are needed for the web application to work

### Step 2

The second step is to get the project code on the new virtual machine. 
To download the project code on the virtual machine, use the following command.

`git clone https://github.com/Hogeschool-Windesheim/blockchain-demonstrator-serious-game.git`

### Step 3

After the project code is on the virtual machine, it next to needs to be installed. 
In order to install the project code, the installation script requires permission to be able to be run
This can be given with the following command.

`chmod a+x install.sh`

### Step 4

Finally, to install the project code, use the following command.

`sudo ./install.sh`

It is important to both use sudo and the ./ otherwise the script will not work.
Step 3 and 4 can also be performed on the start and exit script.
