#!/bin/bash
echo "Warning, use with caution (This script does not work if a server port has been changed!)"
if [[ $(lsof -ti:5000) ]]; then
        kill $(lsof -ti:5000)
else echo "The Web App server is already offline"
fi
if [[ $(lsof -ti:5002) ]]; then
        kill $(lsof -ti:5002)
else echo "The REST App server is already offline"
fi
