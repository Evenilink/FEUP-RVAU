# V-RARez

V-RARez is a **Mixed Reality** video game for mobile phones, taking inspiration from [Fez](http://fezgame.com/).

There are 3 marks required to play the game: a cuboid object and 2 images. The cuboid object marks the actual playable scene, one of the images displays the current score and a button to restart the game, while the last image is an inventory the player can look to see if there's any available power up.

V-RARez requires the player to move around an object that works as an **AR** mark, in order for him to be able to see his character and conquer the challenges laid before him.

The game also uses **VR**, in which the phone's camera is used. This way, the player can still see his environments, while not requiring him to hold the phone in his hands.

Regarding input, that is done using a Wireless Controller connected to the mobile phone through Bluetooth, and also using a very known mobile VR interface known as the gazer, in which the player is required to look to a specific object for a few seconds in order to trigger it.

The weather is used as an external factor. Depending on the physical location of the player, if it is raining/storming, clouds can be seen in the level, while if the day is clear, nothing is spawned. Pooling is being used to handle the cloud system.

## Gameplay
Starting the application, the player must align himself with the model object, in order to see the main menu, which has a "play game" and "exit" buttons. The player interacts with those buttons through the gazer.

After starting a new game, the player must use the controller to move the character. Using the thumbstick, he can move left or right, and by pressing the 'X' button on the Dual Shock 4 (or the 'A' button on the XBOX Remote Controller) he can make the character jump. The score increases by how high the player can climb.

There are 2 types of enemies:
* **Goomba**, moves laterally, kills the player by touching him, and is killed by being jumped on;
* **Shooter Shell**, jumps most of the time while shooting bullets, kills the player by touching him or by hitting him with bullets, and is killed by being jumped on.

Power Ups can be used by pressing the square button on the Dual Shock 4 (or 'X' on the XBOX Remote Controller) to increase player's speed for a few seconds (requiring a cooldown to be activated again), or the circle button on the Dual Shock 4 (or 'B' on the XBOX Remote Controller) to increase player's jump height, also requiring a cooldown.

## Technologies
This project was created using **Unity3D** as the game engine, with the **Vuforia Framework** for every Augmented Reality feature needed, and the **Google Cardboard SDK** for Virtual Reality.

**Mixamo** was also used for characters and animations, and the **OpenWeather API** for retriving the current weather at a specific location. 

**MagikaVoxel** was also used to design and model the levels.
