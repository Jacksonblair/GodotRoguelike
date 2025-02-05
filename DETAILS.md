## Skills system structure

- GameManager
  - Talks to SceneManager, LevelManager to manage game
- GameSetupManager
  - Talks to GameManager to setup game

GameSetup tells GameManager to load last save
GameManager makes sure the level managers are set up before running the level

# NEXT STEPS
- Make enemies attack
  - Logic in enemy for attacking
  - Apply hits to player (knockback?)
  - Player health
  - Player death handling

- Create base enemy type, standardize applying damage/force/etc
- Create a weapon based skill
- Create items

## Enemies
What defines them? 
- Behaviour
  - They generally want to run at the player and attack them.
  - But all actors have some behaviour, so this is a class?

Behaviour
  - Detection range (only attack you if nearby)
  - Activation (only attack if 'activated' by some collision)

## Allies
- Behaviour
  - They generally want to run at the enemy and attack them

^ Same as above

These two are both actors. They should both receive input, just like the player. 
That way we can possess them later.

Input -> Actor * Behaviour == Activity


## GAME IDEAS
- There are different paths, which lead to different enemy types. 
- Different enemy types require different setups:
  - Ex: A big fast insectoid insect is unkillable up close without some super reflexes/counters
- When it rains, NPC's go for cover 
- Each level has an associated infinite landscape. (duh?)
- After cutscene, receive Ability or Item. Should be impactful.
  - Certain abilities/items should open up progression in infinite landscapes. 

## MULTIPLAYER
https://www.youtube.com/watch?v=n8D3vEx7NAE
- Establish authorities ?
- MultiplayerSynchronizer to each player.
  - Sync Position
  - Sync sprites
  - Sync projectiles
  - Sync Health, Enemy Health, Enemy Position, etc... 



```mermaid
graph TD;
    subgraph A
        od>Odd shape]-- Two line<br/>edge comment --> ro
        di{Diamond with <br/> line break} -.-> ro(Rounded<br>square<br>shape)
        di==>ro2(Rounded square shape)
    end
```