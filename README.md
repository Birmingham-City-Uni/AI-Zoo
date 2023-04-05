# AI Zoo
milestones-MattLWatts created by GitHub Classroom

This project is for CMP6206 Artificial Intelligence for Games


For this project I am aiming to create a Zoo in the form of a 3D navigatable game world with different enclosures. In each a different type of animal with different AI behaviour

Enclosure 1: https://www.youtube.com/watch?v=EtrbfGRbWG4&feature=youtu.be (Right Side)
  - Chicken is using A* Pathfinding to find a deer
  - Enclosure is mapped with a point based graph
  - Chicken uses a stack based FSM, transitioning between IDLE -> CreatePath -> Seek. Calculating a new path once it has found the deer
  - Uses sensor to determine when it has caught the deer
  - A standalone node generator, generates a randomised list of locations where the deer will spawn at
  - The list is used to detemine the next location of the deer. This same list is used in enclosure 3 on an identical graph to compare BFS to A*
  - Enclosure has animations

Enclosure 2: https://www.youtube.com/watch?v=uafVkEVISro&feature=youtu.be
  - There are 3 Pandas in this enclosure
  - Pandas have 3 statuses (Hunger, Thirstiness and Tiredness)
  - They have 3 tasks avaliable to them (Eating, Drinking, Sleeping)
  - These pandas are (Intelligent, Semi-Intelligent and Unintelligent)
    - Intelligent: Panda goes to the task it needs to do based off of its current needs
    - Semi-Intelligent: Panda weights its tasks to make it more likely to go to a task that it needs too.
    - Unintelligent: Panda goes to any task randomly
  - Enclosure uses a nav mesh for the pandas to move around
  - Enclosure has animations
    
Enclosure 3: https://www.youtube.com/watch?v=EtrbfGRbWG4&feature=youtu.be

  - Chicken uses Breadth First Search Pathfinfing to find the deer
  - Enclosure 3 is mapped with an identical point graph to enclosure 1
  - Chicken uses a sensor to determine when it has caught the deer
  - A standalone node generator, generates a randomised list of locations where the deer will spawn at
  - The list is used to detemine the next location of the deer. This same list is used in enclosure 1 on an identical graph to compare BFS to A*
  - Enclosure has animations
  
Other:

  - Boids fly above the zoo, flocking to 27 different points above the player moving between them at a time interval
    - https://www.youtube.com/watch?v=K4C1M1JhyIc&feature=youtu.be
  - Floating UIs have been added to all the enclosures in order to show a better idea of what is happening
  - Debug gizmos have been added to all enclosures to show in edit mode the calculations
    - Note in this video https://www.youtube.com/watch?v=EtrbfGRbWG4&feature=youtu.be
    - The generated path is indicated with cyan circles
    - The edges that were traversed through the path calculation are shown in red
    - The vector calculations are shown in red and green as the chicken moves 
  - Scene now has terrain, skybox and textures
  
Branches:
Development: 	All work currently ongoing or to be tested
Master: 			Finished work

To expand on testing done in Milestone 2. I have added UI debugging to all enclosures as a better way to test more dynamic behaviours, Such as pathfinding and Behaviour Trees. There are several visual aids to see how a path is being found in enclosure 1 & 3 and the occurence of a specific task being carried out in enclosure 2

Kanban board and Sprint information can be found in the following sheet:
https://docs.google.com/spreadsheets/d/1Plv213kFlPjUJ5UY8fgSjopg8qkljMKAB_ckzxIADRY/edit?usp=sharing
Added new colour system to start to prioritise tasks

Assets used:
Fence Assets: 	    https://assetstore.unity.com/packages/3d/props/exterior/low-poly-fence-pack-61661
Animals:      	    https://assetstore.unity.com/packages/3d/characters/animals/kubikos-22-animated-cube-mini-animals-100696
Skybox:             https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633
Terrain Materials:  https://assetstore.unity.com/packages/2d/textures-materials/terrain-textures-pack-free-139542
Chicken Sound:      https://www.fesliyanstudios.com/royalty-free-sound-effects-download/rooster-chicken-259
Background Music:   https://assetstore.unity.com/packages/audio/music/orchestral/ultimate-game-music-collection-37351

