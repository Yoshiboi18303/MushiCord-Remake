@echo off

cls

echo Installing Lavalink, this may take a few minutes depending on your internet connection...

echo DO NOT EXIT OR DISCONNECT FROM THE INTERNET DURING THIS PROCESS.

powershell -Command "Invoke-WebRequest https://github.com/freyacodes/Lavalink/releases/download/3.6.2/Lavalink.jar -OutFile Lavalink.jar"