# XR Final Project

This repository is a portfolio copy of a collaborative VR game project originally developed in a private team repository.

Original repository owner: `saturnguo`  
Project type: Group VR / XR game  

## Overview

This project is a VR puzzle game inspired by the classic chicken-and-fox river crossing problem. The player must transport chickens and foxes across a river using a rowboat while following the central rule of the puzzle: chickens cannot be left on either side of the river if they are outnumbered by foxes.

The game uses VR hand interactions to let the player physically grab animals, place them into the boat, and row across the river. The goal is to safely move all required animals to the opposite side without triggering a losing condition.

## Gameplay

Players interact with the world in VR by using their hands to:

- Pick up and move chickens and foxes
- Place animals into the boat
- Row the boat across the river
- Manage the number of chickens and foxes on each side
- Solve the puzzle without allowing foxes to outnumber chickens

The game combines a logic puzzle with physical VR interaction, making the player actively participate in transporting the animals instead of simply selecting moves from a menu.

## My Contributions

My main contributions focused on the boat physics and core game systems.

### Boat Physics

I worked on the physics system for the rowboat, especially the rowing interaction. The player rows using their VR hands, so the boat needed to respond naturally to hand movement and physical input. This involved tuning and implementing the behavior that allows the boat to move across the river through player interaction.

### Game System

I also worked on the overall game system, including the logic that supports the chicken-and-fox puzzle structure. This included helping manage the rules of the game, tracking the state of animals across the two sides of the river, and supporting the win/loss conditions based on the number of chickens and foxes.

## Technologies Used

- Unity
- C#
- VR / XR interaction systems
- Physics-based gameplay
- 3D game environment and assets

## Team Project Notice

This was not a solo project. It was built collaboratively as a group VR game project. I am including this repository for application and portfolio purposes to demonstrate the parts of the project that I personally contributed to, especially the boat physics and game system logic.
