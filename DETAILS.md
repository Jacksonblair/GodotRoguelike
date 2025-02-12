## Skills system structure



Howdy. Does anyone have a good resource for queueing up things in an ability system?

Say for instance i want my player to attack something. I play like an attack animation
, and then i set it up so that halfway through the animation i actually apply the hit.

Or i set up a timer, and when it finishes i shoot a projectile.

If in the first case, my dude is interrupted before the animation gets halfway through,
i dont want to apply that hit anymore.

Or if i get inerrupted in the second cast, i want to cancel the timer, or 
just not shoot the projectile

Well animations can be updated in the Actor

But id rather everything was singularly located, easier to debug.


'Ability' is interruptable?

Enemy aims hitbox towards player.
Play animation
Halfway through anim, apply hit







Right now, i have SKills in their own little world.
- THe skill creates a projectile, or plays an animation towards a certain direction.
- Im trying to make them decoupled so that i can:
  - Easy add any skill.


If im trying to add a charge and slash skill to the player AND the enemy

The player needs to play a particular animation, and so does the enemy. 
But the skill cant know about both of these can it?

And also, the player/enemy cant know if the skill requires a particular animation,
if its generic, can it?

The skill can specify that its chargeable, or has charges and stages, but thats it.

That means the onus on determining the animation to use resides with the actor.

Say i have: Charging fire slash, and a player-only skill called Jimmys Charging fire slash.

For the second one, there is a particular actor it is associated with, and only they have
the animations for it.

For the first one, both player and enemies can use it.

But the underlying mechanics of charging/executing/resource consumption the skill remain the same.

Lets assume that player characters all have their own set of animations for:
- Charging
- Slashing

The attack is executed. It contains internal state about its own use. It can create
its own hitboxes and attack effects, but it cannot play an animation on the actor.

Or can it? Can it emit an event to the actor to say "im charging, use a charge animation"

Then the actor can look at that, and determine what animation to play.

What if its an enemy. I can hardcode in the abilities, and have exact references to their types, avoiding any difficuly with generic interfaces/etc.

But the player has a customizable set of skills. So i can check in the player what skill exactly is charging, and determine the animation there. 

-> Actor -> SkillScenes


Examples:

GoblinActor
- GoblinSwipe
- GoblinStomp

PlayerActor
- Skill1
- Skill2
    
Global state needs to know what skills are equipped. So UI can access them.

Skills should be children of actors, makes positioning easy. 
And it makes it easy to preview them in editor.

What about skill bits and bobs?
- Cooldowns
- Charges
- Charging state
- Modifiers

They should probably all be included in the skill scene automatically.




My issue is this:

I have a Skill base class, which needs a SkillData property to implement its functionality.

I have a FIreball class, which extends from Skill, and a FireballSkillData class which extends from SkillData.

I want the base methods of Skill to use the FireballSkillData in place of SKill.

And i also want the Fireball class to know that its SkillData property is of type FireballSkillData

Do i need to use an interface i here somewhere?







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