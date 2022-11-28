# This file checks for Lavalink and installs it if it doesn't exist in the same directory

from time import sleep
from os.path import exists
from os import system


system("cls")

print("Lavalink Runner and Installer\n\n")

print("Please note that this script is not official and any damage to your operating system is your fault and your fault only.\n\n")

continue_running = input("Are you sure you want to continue to run this script? [y/N] ").lower()

if continue_running == "y" or continue_running == "yes":
    system("cls")
    i = 0
    time = 0
    progressStuff = ["/", "-", "\\", "-"]
    print(f"Checking if Lavalink is in the current directory... {progressStuff[i]}")
    system("cls")
    while time < 20:
        if i > len(progressStuff) - 1:
            i = 0
        print(f"Checking if Lavalink is in the current directory... {progressStuff[i]}")
        sleep(0.5)
        system("cls")
        i += 1
        time += 1
    if exists("Lavalink.jar"):
        print("Lavalink is installed and put in this directory, press enter to run the jar file.")
        system("cls")
        system("java -jar Lavalink.jar")
    else:
        install_lavalink = input("Lavalink was not found in the current directory, do you want to install it? [Y/n] ").lower()
        if install_lavalink == "" or install_lavalink == "y" or install_lavalink == "yes":
            successful_installation = system("installLavalink.cmd")
            system("cls")
            i = 0
            time = 0
            while time < 20:
                if i > len(progressStuff) - 1:
                    i = 0
                print(f"Checking if installation was successful... {progressStuff[i]}")
                sleep(0.5)
                system("cls")
                i += 1
                time += 1
            
            # 0 for Successful, -1 for Unsuccessful
            if successful_installation < 0:
                if exists("Lavalink.jar"):
                    system("del Lavalink.jar")
                print("ERROR: Lavalink was not installed correctly, please install it manually.\n\nThis script will now exit.")
            else:
                input("SUCCESS: Lavalink was successfully installed to the current directory, press enter to run the jar file!")
                system("cls")
                system("java -jar Lavalink.jar")
        else:
            print("This script cannot run without Lavalink installed, please install it either through the use of this script or manually.\n\nThis script will now exit.")
else:
    print("Script cancelled.")