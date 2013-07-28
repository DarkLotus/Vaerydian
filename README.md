# Vaerydian

Vaerydian is an in-work Action RPG written in C# utilizing:
* [MonoGame](http://github.com/mono/MonoGame) - Graphics Framework
* [BehaviorLibrary](http://github.com/NetGnome/BehaviorLibrary) - AI Framework
* [ECSFramework](http://github.com/NetGnome/ECSFramework) - Entity-Component-System based Game Framework
* [AgentComponentBus](http://github.com/NetGnome/AgentComponentBus) - Asynchronous processing framework for ECSFramework
* [Glimpse](http://github.com/NetGnome/Glimpse) - UI Framework for MonoGame & ECSFramework
* [fastJSON](http://fastjson.codeplex.com) - A Very fast JSON parser, used for driving the game with data
* [LibNoise](http://libnoisedotnet.codeplex.com) - a C# implementation of the famous libnoise noise framework, used for procedural content

Vaerydian can can compile under both Windows (MonoGame WindowsGL) and Linux(MonoGame Linux).

When built, copy the Content directory to the build output directory and run the game.

## Controls

* WASD - movement
* R - target closest enemy
* RightClick - fire bolts at target
* LeftClick - swing melee weapon in direction of reticle
* Enter - descend to next level based on current map position (i.e., generates the next map based on current map & position)
* F12 - go back to previous map
* F6 - dump current map and player configuration to .v files
* ESC - return to start menu / exit game

## Data

All data that drives the game is stored in
	Content/json/

All files are the format of
	filename.v

At this time I have no guidance for editing the files, so do so at your own risk. Nothing will permanently break anything, but you may want to back these up in case you forget to undo all your changes.

Enjoy!
