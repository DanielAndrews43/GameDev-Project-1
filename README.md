# Berkeley Gamedev Introductory Project Specification and Directions

## Overview

In this project, you will extend our simple 2D platformer with new abilities, items, and enemies!
	
## Project Setup

1. Make a new directory and create a new git repo: ``git init``
2. Add the course remote: ``git remote add course https://github.berkeley.edu/berkeley-gamedev/Project-1.git``  
3. Pull from the remote to get the skeleton code: ``git pull course master``
4. Pull often for bug fixes!
5. Stage the skeleton code in your local repository: ``git add -A``
6. Commit the skeleton code: ``git commit -m "Initial commit"``
7. Create a repository in the **berkeley-gamedev** organization
8. Add your project partner as as collaborator
9. Add your remote repository as the origin: ``git remote add origin "your-repo-address-here"``
10. Push the skeleton code to the origin: ``git push origin master``
11. You're ready to go!

## Game Documentation
	
### The Mario GameObject

The scripts and components that make up the player character in our game are all encapsulated in the **MarioHolder**. This shows the capability of GameObjects to simply act as organizational tools for the objects in your game. More than just organizational, however, the parent GameObject serves another important purpose: to keep all Mario objects in the same location - as the position of a child GameObject is relative to the position of the parent GameObject. In this way, we can move the player character by moving MarioHolder's Transform but keeping the Transforms of **Little Mario**, **Super Mario**, and **Ducking Mario** set to (0, 0, 0).

Super Mario, Little Mario, and Ducking Mario correspond to the different forms that Mario can take on in the game. Little Mario will begin the game, and upon eating a Magic Mushroom will turn into Super Mario. When Super Mario ducks, he will become Ducking Mario. In order to switch between these different Mario bodies, we use the logic in the Player Controller to [activate](https://unity3d.com/learn/tutorials/topics/scripting/activating-gameobjects) the new Mario GameObject, and deactivate the old Mario GameObject with ``gameObject.SetActive(Boolean bool)``. When a GameObject is deactivated, it is temporarily removed from the scene. 

Each Mario GameObject has a Transform, a Sprite Renderer, an Animator, several geometric Colliders, and a Script. If you click on one of these GameObjects, you can see that there is a disconnect between the sprite and the collider: they do not fill the exact same space. It is possible to have a collider that automatically contours to the exact form of your sprite by using the **Polygon Collider**. However, using the Polygon Collider would create many more small surfaces for collisions, which introduces many more opportunities for performance hits and bugs. In our case, it is best to keep it simple with the pill-shaped collider system we use. 

### PlayerController

We've covered the basics of scripting in class. The ``Start()`` function is called on initialization, ``Update()`` is called once per frame and is best for catching input, and ``FixedUpdate()`` is called on a fixed interval and is best for phyiscs updates. The Player Controller uses a [State Machine](http://gameprogrammingpatterns.com/state.html) to intercept the input and carry out actions based on what state Mario is in, so the ``Update`` function simply has to get the input and delegate it to the state. The ``FixedUpdate`` function handles flipping the player character left and right, delegates to the current state, and handles the transitions between states.

A State Machine is a "machine" that can be in one state at a time. It transitions between states in response to external inputs: in our case either Mario's movements in the game, or input from the player. 

#### States

The PlayerState interface dictates the functions that a state must implement:  

* ``Enter`` to be called in ``PlayerController.FixedUpdate`` when the player character enters the state  
* ``FixedUpdate`` to be called by ``PlayerController.FixedUpdate`` on a fixed timescale for physics updates  
* ``Update`` to be called by ``PlayerController.Update`` per frame  
* ``Exit`` to called by ``PlayerController.FixedUpdate`` when the player character leaves the state  
* ``HandleInput`` to be called by ``PlayerController.Update`` when an input is received and to return the next state that the current state that Mario will transition to  

##### Grounded

On entering, the ``Grounded`` state will set the animator to play the walking animation by using the boolean flag "Grounded". In its ``Update`` and ``FixedUpdate`` functions, the state will move Mario along the ground based on input, duck Mario based on input, and check for exiting the state: when Mario begins falling or Mario jumps. On exit, the ``Exit`` function will check if Mario is jumping or falling, and play the correct animation (for falling, this just means pausing the current animation).

##### InAir

The ``InAir`` state does many of the same things as the Grounded state. In its ``FixedUpdate`` function, it applies the jumping force to Mario (as long as ``jumpingTime`` is greater than zero) and continuously checks if Mario has reached the ground - at which point it flags ``stateEnded`` as true, which signals to ``PlayerController.FixedUpdate`` that Mario should transition to the ``Grounded`` state which is returned in ``HandleInput``.

### World 1-1

The GameObject for World 1-1 is organized in much the same way as the GameObject for the player character: with parent and child GameObjects. Again, this serves to orient all child GameObjects to the parent. In our case, the world map as the top-level parent ensures that all GameObjects will be oriented with the bottom left of the world map at the origin even though their actual position in space is different.

### Basic Blocks

The **Basic Block** object is created from the Basic Block prefab, indicated by the blue color of the object's name in the hierarchy. You can view the prefab by navigating to Assets/Prefabs/ in the Project folder. All blocks, items, and enemies are prefabs. The bold values in the transform of each of the block GameObjects indicate that the default prefab values are being overridden. 

The Basic Block prefab parent object only has a Transform so that all the child GameObjects are in the same position. The **Top_Collider** object holds the top collider of the block, and the **Bottom_Collider** holds the bottom collider of the block and the Block script. The reason for splitting the colliders between two separate GameObjects is that it is easier to check if Mario is walking on the top collider or hitting the bottom collider by checking for different GameObjects than it is to check the location of the collision, or even to differentiate between collider components on the same GameObject. The **Sprite** object simply holds the sprite, but its transform gives it a 0.5 offset from its parent. This ensures that the sprite itself is lined up with its parent GameObject: the bottom left corner of the sprite is the origin of its parent GameObject. All blocks, items, and enemies with the **SpriteRenderer** component on a separate GameObject use this technique. 

#### Script

``Block`` defines the basic behavior for a block. Since ``Top_Collider`` has no special behavior other than being a floor, the script is attached to the ``Bottom_Collider`` so that the appropriate collision method ``OnCollisionEnter2D`` is called when the player hits the bottom collider. The main behavior of a Block is to bounce up and down when hit by a player, or break if hit by Super Mario. In this case, the bouncing behavior is implemented by using a [Coroutine](https://docs.unity3d.com/Manual/Coroutines.html). Essentially, the ``MoveUpAndDown`` function is called once per frame; the function does not run to completion when it is called. The same behavior could also have been implemented using the ``Update`` function or an Animator component. 

### Mystery Blocks

The **Mystery Block** object is functionally identical to the Basic Block.

#### Script

``MysteryBlock`` inherits from the ``Block`` class so it has many of the same behaviors. However, after a Mystery Block is bounced up and down by the player it becomes unbreakable. This is implemented with a coroutine that simply calls the ``MoveUpAndDown`` coroutine that it inherits and then changes the sprite and makes the block unbreakable on completion.

### Goombas

The **Goomba** is an agent in the world and therefore has a ``Rigidbody`` component. The colliders are once again separated into different GameObjects so we can more easily check whether Mario has bounced on top of the Goomba to kill it, or the Goomba has hit Mario on the side. 

#### Script

``Goomba`` inherits from the ``Enemy`` class, an abstract class that specifies the "contract" terms that Goomba must implement in order to be an Enemy:  

* ``GetScore`` - called by the ``UIManager`` to get the score value of the Enemy when it is killed by Mario  
* ``HitByPlayer`` - called by the player when the enemy is HIT BY the player  
* ``HitPlayer`` - called by the player when the enemy HITS the player  

The Goomba ``HitByPlayer`` function kills the Goomba, and the ``HitPlayer`` function shrinks the player. However, these methods are called from ``PlayerController.OnCollisionEnter``, which detects whether the player has hit the side or the top of the Goomba. 

### Magic Mushrooms

The **Magic Mushroom** is also an agent in the world and therefore has its own ``RigidBody`` along with a singular collider since we do not need to detect where the player hits the item. However, this collider is a [Trigger](https://unity3d.com/learn/tutorials/topics/physics/colliders-triggers). This simply means that the collider will not register collisions but rather send trigger events to its associated scripts. We use this for activation: we want the Magic Mushroom to begin rising out of its block at a controlled speed when Mario enters its trigger zone, rather than being bumped out by him. 

#### Script

``MagicMushroom`` inherits from the ``Item`` class, an abstract class like ``Enemy``. ``Item`` defines the following methods:  

* ``GetScore`` - called by the ``UIManager`` to get the score value of the Item when it is picked up by Mario  
* ``PickUpIten`` - called by the player character when the item is hit (triggered) by the player  
* ``ItemBehavior`` - an alias for ``FixedUpdate`` that is called by ``FixedUpdate`` on every time step  

The Magic Mushroom must also implement other important behaviors: it must rise out of the block it is behind when triggered by the player (Coroutine ``Activate``) and it must become visible as it rises (Coroutine ``ShowAndHide``). After the Magic Mushroom has been activated, ``myCollider.isTrigger`` is set to false and it takes on the normal behavior of a collider so that it can move across the ground and register collision with the player correctly. As in the case of the Goomba, collision is handled by the player controller, which calls ``PickUpItem`` on collision with the Item. 

### Sprites

Sprites are 16x16 pixels, with the larger sprites - such as Super Mario - being 16x32 pixels. They use Bilinear filtering and have their pivot in the **Center**. Although we could accomplish the same functionality by setting the pivot to **Bottom Left**, it is easier and introduces less bugs to simply use the GameObject-offset method as outlined above. 

### UI Manager

The UI Manager is a [Singleton](http://gameprogrammingpatterns.com/singleton.html), which essentially means that there is only **one** instance of it that can ever exist, and that all scripts can access it through its own static member ``uiManager``. 

The UI Manager keeps track of important UI and game functions such as keeping score, keeping time, and keeping track of the player's lives. It uses a coroutine to keep time - but one that waits for one second between calls rather than between fixed time steps of the phyiscs engine. Loading the game scene (switching between the Main Menu and the Game Scene) would normally be implemented with [``SceneManager.LoadScene("Main Scene")``](https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadScene.html) but we've abstracted it with ``UIManager.LoadScene`` to make sure that some time is spent on the Loading Screen by using a coroutine timer. The Menu Scene can be loaded normally. ``LoadOnClick`` calls this method in the ``LoadOnClick.LoadScene`` function, which, rather than being a Script component on a GameObject, is tied to the UI through the **Button** component on the **1Player** GameObject, which registers the **On Click** event. 

## Project Specification

### Part 1

Add a new enemy and a new item!

### Part 2

Make your item transition Mario into a new state!

### Part 3 

Add a UI element to keep track of the player's lives.
1. ``UIManager.TakeLife()``
2. ``PlayerController.Shrink()``

## Project Submission

1. Commit your changes
2. Create your submission branch: ``git checkout -b submit``
3. Push both branches to origin: ``git push origin master submit``
