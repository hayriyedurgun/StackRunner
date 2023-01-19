# StackRunner

It is runner game for GameGuru case.

Character tries to reach finish platform while platform pieces coming from left and right sequentially. Platform creating from platform pieces with user's tap like Ketchapp's "Stack" game.

When the player successfully completes the level, player makes a dance animation and orbit camera looks at the character.
After completing the level, other levels are added to the end of the final platform of the previous level.

Gameplay settings can change via /Resources/GameplaySettings scriptable object.
Gameplay settings contains:

       TileSpeed: Speed of moving platform tiles.
       TileSpawnXPos: Moving platform tile's initial x position.
       TileCutThreshold: Threshold value for tile can cut or not.
        
#### Used assets: ####
* Cinemachine
* Zenject
* DOTween


![Alt Text](https://github.com/hayriyedurgun/StackRunner/blob/main/Animation.gif)
