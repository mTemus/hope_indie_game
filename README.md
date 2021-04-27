# Hope
Hope [TBD] is an side-scroll strategy management game similar to Kingdom, but with more advanced village/castle managing system like in Stronghold. Game is in deep prototyping stage and it is made only in free time, but it will be hopefully published someday.

## Current content
Game world is divided on areas which every of them has an own 2D grid. Game has not much to offer yet, but all new ideas are changed into new systems in continous updates.

### Player Tools System
Player can choose different tools from a radial UI menu to manage his village. Radial menu is based on an youtube tutorial, but it has been modified to match game standards, also the tutorial was not covering all the topics, so I filled it with my own code.

### Building System
Building system is based currently on a 2D grid, which helps to snap buildings on certains positions. Grid also holds building data in its tiles, what helps to find specific structures.

### Villagers System
#### Villagers AI
Villagers AI is based currently on a Behaviour tree. There is only one behaviour tree. It was created for all working villagers independly on their current profession. The worker is wandering close to its workplace and it's trying to get a task of type specified to his profession.
Whole tree is shown [HERE](https://drive.google.com/file/d/1rWhAOH2TzJkVR03YYvBP-NTEdC9Znux7/) - yellow rectangles are implemented, white ones are planned.

#### Professions
Currently, there are only three professions implemented: Unemployed, Builder and Lumberjack.
*Unemployed - Villager without job is mean to wander close to Town Hall (currently not implemented).
*Builder - Villager who delivery  resources from a warehouse to construction site and build constructions when they have enough resources.
*Lumberjack - Villager gathering wood (currently not implemented)

Profession can be changed through villager selecting UI, which can be displayed using a Vilalgers Book tool from Tools Radial Menu.

#### Tasks system
Tasks are instructions for each profession. Most of them will be specific for a certain professions, but a Resource Delivery task is generics and will be used by all professions. At crrent moment, there are two tasks types: Building and Resource Delivery.

## Updates on youtube
* [First update - tools and buildings](https://www.youtube.com/watch?v=laCbKncUxlc)
* [Building selecting while building UI](https://www.youtube.com/watch?v=eGZ8UUqtYbY)
* [Villager selection and profession changing UI](https://www.youtube.com/watch?v=rstPntP1JzQ)
* [Workplace Hauler profession implementation](https://www.youtube.com/watch?v=pYRBg31MNkk)
* [Developer console implementation](https://www.youtube.com/watch?v=r-kNdXYLJPA)
