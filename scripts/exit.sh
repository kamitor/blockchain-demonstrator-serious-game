#!/bin/bash
echo "Warning, use with caution (This script does not work if a server port has been changed!)"
if [[ $(lsof -ti:5000) ]]; then
        kill $(lsof -ti:5000)
        echo "Killed the Web App server"
else echo "The Web App server is already offline"
fi
if [[ $(lsof -ti:5002) ]]; then
        kill $(lsof -ti:5002)
        echo "Killed the REST API server"
else echo "The REST API server is already offline"
fi
