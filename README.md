**The University of Melbourne**

# COMP30019 – Graphics and Interaction

## Teamwork plan/summary

<!-- [[StartTeamworkPlan]] PLEASE LEAVE THIS LINE UNTOUCHED -->

Sprint 1 (Week 9):
Thomas - Start floor shader
Dylan - Player and piece movement
Kelsey - First floor map

Sprint 2 (Mid-Sem Break):
Thomas - Continue work on floor shader
Dylan - Key / door / trapdoor logic and animation
Kelsey - floor procedural generation

Sprint 3 (Week 10):
Thomas - work on wrinkle floor shader
Dylan - Menu and saving functionality and Asset sourcing
Kelsey - Asset sourcing and floor procedural generation

Sprint 4 (Week 11):
Testing 1
Evaluation
Make game play video
Thomas - dust particle system
Dylan - sound effect sourcing and first floor decoration with functions
Kelsey - floor procedural generation and lighting

Sprint 5 (Week 12):
Make changes from evaluation
Make game play video
Thomas - Post-processing and Title screen
Dylan - Title screen
Kelsey - Environment decoration (using procedural generation)

Sprint 6 (SWOT VAC):
Testing 2
Report

<!-- [[EndTeamworkPlan]] PLEASE LEAVE THIS LINE UNTOUCHED -->

## Final report

Read the specification for details on what needs to be covered in this report...

Remember that _"this document"_ should be `well written` and formatted **appropriately**.
Below are examples of markdown features available on GitHub that might be useful in your report.
For more details you can find a guide [here](https://docs.github.com/en/github/writing-on-github).

### Table of contents

- [Game Summary](#game-summary)
- [Technologies](#technologies)
- [Using Images](#using-images)
- [Code Snipets](#code-snippets)

### Game Summary

Reminiscent of other first-person puzzle-based horror games such as Amneisa, Captured takes place within a world ruled by chess pieces, who subjugate pawns and treat them as slaves. Taking place within a prison, the protagonist, one such pawn, is one day given the opportunity to attempt escape, and must do so by traversing the prison and reaching the palace to assassinate the king, while avoiding recapture by enemy chess pieces and solving light puzzles. The logic of the enemy pieces is supposed to approximate their in-game movement capabilities: Rook, the least agile piece in chess, is restricted to oscillating movement in a single direction; Bishop, a piece unpredictable due to its ability to quickly traverse in an unorthodox direction, has highly predictable movement and bounces across rooms; and Knight, the most agile piece in chess, can lock onto the protagonist and charge in one direction. Other game enemies include the Queen, who chases the player at key game moments, and the King, whose underwhelming weakness and lack of agility satirises the ineffectiveness of a monarchy. While the story is largely seen as less important than the core gameplay, the world is littered with journals detailing additional lore for players interested enough to seek them out.

### Technologies

Project is created with:

- Unity 2022.1.9f1
- Ipsum version: 2.33
- Ament library version: 999

### How to play the game

**\*\*\*\***Captured**\*\*\*\*** has rather simple controls, providing little more than simple walking, running and looking around. A large portion of gameplay concerns itself with interacting with objects in the environment, such as keys, doors, levers and locked safes. Gameplay in partitioned into floors, each of which is restarted following player death (which occurs whenever a piece makes contact with the player). The first floor is highly puzzled-based with few pieces, in order to introduce the player to the simple game mechanics; the second floor is a procedurally-generated labyrinth with various enemies, the third floor is story-heavy boss-battle with a Queen, and the final level is a simple confrontation with the key.

### Gameplay Related Design Decisions

The initial game concept was highly ambitious, with the intention that each floor was a large labyrinth of chess-themed **\***Zelda**\***esque puzzles with various room filled with enemies to dodge in the style of **********\***********World’s Hardest Game**********\***********. It was soon decided, however, that time limitations, in addition to a new desire to introduce procedural generation in the creation of levels, resulted in the majority of custom-floor creation resulting in the development of the first floor - which is quite puzzled-based but not to the extent as was initially desired - in addition to Queen and King boss battles. Although the latter was to be included from the game’s first conception, the former was originally envisioned as a chase sequence through an elaborate custom-created labyrinth, time limitations, once again, resulted in such an idea being simplified somewhat. Furthermore, the original idea itself, a chess-themed horror game, was born out of a combination of the creator’s interest in chess, the notion of creating a puzzle-based game, and the perceived ease of developing a dynamic first-person camera as opposed to a third-person one. Following from the conceptualisation of such ideas, it was proposed due to the perceived novelty of combining chess with horror elements, in addition to wanting to allude to games such as the aforementioned **\*\***Amnesia**\*\***. The journals were a direct result of such inspiration, as a means of delivering lore to intrigued players in an otherwise gameplay-heavy experience.

### Designs and Graphical Assets

Assets were entirely sourced from third-party repositories, predominately the Unity Asset Store, due to the game’s large ambition prohibition the creation of custom assets.

### Procedural Generation Technique

To make this game can be replayed using seed in the future, for all the randomly selecting function below, Math.Random is used.

1. Place rooms

   For future extendability, we set a few public variable for generator, including floor size, max room size, room count (not actual room count). Using those input, random size room bounded by max size is placed down at random place within the floor range. Overlapped rooms will be removed.

   The put down room game object is from a room prefab input. Therefore floor style can easily be changed in the future.

2. Place hallway

   To avoid long hallways crossing the whole map, Delaunay Triangulation is used to find shortest path between two neighbours by finding biggest-triangle-filled polygon mesh from a room made node map. This is Delaunay Triangulation process is implemented by Bowyer-Watson Algorithm.

   After a node map is made, next step is to find the shortest path between any two nodes (AFK rooms). So a minimum spanning tree is implemented using Prim’s algorithm. However, a minimum spanning tree doesn’t contain any loop. In order to make player to reach each room faster and have a better experience exploring map, a few edges are randomly selected to add loops to the map.

   After edges are selected (means that we found every two rooms that we need to connect) , next step is to put actually hallway prefab game object on to the map. The hallway prefab is always a cube, so making sharp diagonal hallway will decrease player game experience a lot. A path finding algorithm called A star is used here to finding a non-diagonal path between two rooms. To make each hallway shorter, the algorithm is also adjust to go thorough as many rooms as possible.

3. Place Decoration

   To make each room in the floor to have a different view, a set of mini-room decorations are made to randomly put on top of each procedurally generated rooms depending on the size. The aim is to make a few different decorations of each possible size of the room so it can be randomly allocated, but due to the very short game development time. we made one mini-room decoration per size for now.

4. Place Door and Key pair

   The goal of the game is to escape each floor then kill the chess king. There has to be a key-door pair each floor including the procedurally generated ones. So a room is randomly selected in the map to place down the key and another room is randomly selected to put down the exit door.

5. Place Enemy

   After the map and decoration is down, the step is spawning random enemy to appear in the floor. Numbers of enemy can be put in the generator through unity to control the difficulty of procedurally generated floors. We didn’t get to fully test this part’s functionality so in submitted game version we put enemy number as zero.

### Particle System Technique

For the particle system, we created a fire particle effect for the torches placed throughout the scenes. This particle effect can be found in the location “Assets/Procedural/Main Torch.prefab”. A child of this prefab is the “Fire” particle effect, composed of the child particle effect “Ember”.

The parent effect shows the smoky flames, emitted from the shape of a cone. Each particle is a smoky image that is made to resemble a collection of smoke rising. The particles are randomised in many ways, including size, rotation, and lifetime to make the fire look more realistic. To replicate real torches, the particles change over their lifetime. The particles initially show blue flames around the tip of the torch, before transitioning to white, yellow, and finally red as it fades out. The size initially increases quickly before dissipating to zero as the smoke image rotates over its lifetime.

Alongside the smoky particle effects, embers are emitted at the same time. The embers are simpler, but nevertheless, help to simulate a real torch. The embers are simply stretched, red particles, that swirl upwards with random accelerations added to their velocity.

### Querying , Observational Methods and Changes Documentation

We had three bug-fix and feature adjustment sprints after each feedback collection.

**************\*\*\*\***************Querying Method**************\*\*\*\***************

A google form is set up for participants to fill in after game without any developer around because google form can be set completely anonymous and good visual summary on feedback. There are six people filled in the form during different time of development. Most of them are male, 50% percent of the participant is 21-25 years old, and two-third has previous gaming experience and enjoying horror media. Most people sees story as good plotted and love the visual effects. They describe the gaming emotions as excited and scared mainly, some thinks it is humoured also.

**Observational Methods**

There are seven player observed during playing. They are all male and have some game experience, love horror genre and have confidence in game techinique.

Notes were taken during each participants game process by one of the three developers in team. Participants express their confuse during the process and developer also captures bugs during the play. By observing player first time understanding, developer realised that players are not playing the game most of time as we expected and can find some interesting pathway of the game that developed accidentally by bugs.

**Feedbacks and Changes Documentation**

During the first feedback collection sprint, there were only one player played this game and found lighting is too dark for the environment but with the sound effect, it gives a decent scary atmosphere. and a few bugs is found in this time and most of them of fix before the next feedback sprint. People see story as good plotted. and also gamers points about that the innstruction wasn’t clear enough so we added an instruction menu in the paise menu.

In the second sprint, we showed our improvement and people are saying it is too difficult so we adjust to decrease difficulty and fixing bugs we observed.

We mainly recieved complements on the third feedback and also some required us to add fire sound effect so we added.

### References and External Resources

- Unity Asset Store - Graphical assets
- Freesound - Auditory assets
- [https://github.com/vazgriz/DungeonGenerator](https://github.com/vazgriz/DungeonGenerator) - Map generation inspiration
- [https://www.youtube.com/watch?v=R6D1b7zZHHA&t=504s](https://www.youtube.com/watch?v=R6D1b7zZHHA&t=504s) - Smoke generation inspiration
- [https://www.youtube.com/watch?v=5Mw6NpSEb2o](https://www.youtube.com/watch?v=5Mw6NpSEb2o) - Fire effect inspiration
- [https://www.youtube.com/watch?v=kWhOMJMihC0](https://www.youtube.com/watch?v=kWhOMJMihC0) - Mini-map inspiration[https://gorillasun.de/blog/bowyer-watson-algorithm-for-Delaunator-triangulation](https://gorillasun.de/blog/bowyer-watson-algorithm-for-Delaunator-triangulation) - Bowyer-Watson Algorithm for Delaunator Triangulation
- [https://dotnetcoretutorials.com/2020/07/25/a-search-pathfinding-algorithm-in-c/](https://dotnetcoretutorials.com/2020/07/25/a-search-pathfinding-algorithm-in-c/) — a star path
- [https://www.geeksforgeeks.org/prims-algorithm-using-priority_queue-stl/](https://www.geeksforgeeks.org/prims-algorithm-using-priority_queue-stl/) - Prim's algorithm for minimum spanning tree
- COMP30019 Workshops & Unity Learn - Unity knowledge

### Shaders

The first of our two shaders is a wave effect that can be found at “Assets/Art/Materials/FloorShader.shader”. This wave effect has two main functions, permanent rippling from an assigned “\_RippleOrigin”. Firstly the shader uses the standard lighting model provided by Unity with the custom vertex manipulation function “vert”, and using “fullforwardshadows” making shadows possible on the surfaces.

The amplitude of this wave can be assigned through the global parameter “\_Amplitude”, and the dissipation of the wave over distance can be changed by “\_Spread”. This shader makes it such that the floor is constantly rippling out from the Queen during the QueenFloor scene which can be seen in “Assets/Scenes/QueenFloor.unity”. This adds an element of stress to the player as the queen approaches.

In conjunction with this, the shader uses “\_LandTime” and “\_LandOrigin”. The land time starts when the queen lands, and produces large ripples coming out from the origin as defined. These values are defined in the same scene and add to an initial jump scare for the character. Both these shaders manipulate vertices, this takes place before the lighting effects in the graphics pipeline to distort the uv mapping of the assigned texture.

The second shader is a pixel shader which works as a postprocessing effect for cameras. The shader can be found at “Assets/Art/Materials/PixelShader.shader”. The effect has two components to it. Firstly it is able to grayscale the pixels of the camera, this is a trivial effect that simply changes the colour of the pixels. The second function darkens the screen around the borders and slowly fades into the centre of this screen which requires handling the proximity of the player to the closest piece to determine the exaggeration of this effect. The closest piece is defined externally in the Camera’s script, updated every time the “Update” function is called. This being a postprocessing effect happens after the vertices, lighting, and all other sections of the graphical pipeline have been performed. The effect can be observed in the scene “floor1” on approaching any chess piece.
