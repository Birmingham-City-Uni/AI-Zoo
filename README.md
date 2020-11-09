# milestones-MattLWatts
milestones-MattLWatts created by GitHub Classroom

This project is for CMP6206 Artificial Intelligence for Games


For this project I am aiming to create a Zoo in the form of a 3D navigatable game world with different enclosures. In each a different type of animal with different AI behaviour

Enclosure 1: https://www.youtube.com/watch?v=uJQ2bztwhbE&feature=youtu.be
  - Chicken is chasing the Deer
  - Enclosure is mapped with a point based graph
  - Chicken uses a stack based FSM, transitioning between IDLE -> CreatePath -> Seek. Calculating a new path once the cached node position is different to the deers closest node
  - Uses sensor to determine when it has caught the deer
  - Deer also uses pathfinding to randomly move around the enclosure (Effectively moving constantly around the enclosure)

Enclosure 2: https://www.youtube.com/watch?v=ru8qnMXAJ34&feature=youtu.be
  - Panda travells around between "tasks" using behaviour tree
  - Tasks are eating, drinking and sleeping
  - Panda determines its next task based of an array of weights
  - Each weight is changed depending on the pandas need for that set task
  - I.e if food is running low compared to tiredness and thirstiness. Panda will prioritise travelling to go and sleep

Branches:
Development: 	All work currently ongoing or to be tested
Master: 			Finished work

Unit testing begun. Primarily on movement scripts. Still needs to be expanded to other functionality

Kanban board and Sprint information can be found in the following sheet:
https://docs.google.com/spreadsheets/d/1Plv213kFlPjUJ5UY8fgSjopg8qkljMKAB_ckzxIADRY/edit?usp=sharing

Assets used:
Fence Assets: 	https://assetstore.unity.com/packages/3d/props/exterior/low-poly-fence-pack-61661
Animals:      	https://assetstore.unity.com/packages/3d/characters/animals/kubikos-22-animated-cube-mini-animals-100696
