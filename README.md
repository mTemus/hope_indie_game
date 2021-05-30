# Hope
Hope [temporary name] was born when feeling of the _feeling_ of **Kingdom** has met strategic approach of **Stronghold**. Somewhere these two met Rimworld, Regions of Ruin, Rise to Ruins and other guys. 

In this game player is the Hope. The main goal will be to create a safe place for inhabitants of world that is dying and of course, figure out why it is doing so. By controlling a common, indistinct character, player will need to create a village
that will be a colony for his people and all the new immigrants, which also will be safe enough and self-sufficient to live in. Village will need to be defended from mysterious monsters, bandits or even other human beings.

Player will be able to explore the game world and interact with it by increasing authority of his society, expanding outposts and meeting the unexpected.

Game is in deep prototyping stage and everything can be changed. It is made only in free time, but it will be hopefully published someday.

### Table of contents

[1. World](#world)  
[2. Player](#player)
 * [Tools System](#player-tools-system)
   
[3. Systems](#systems)  
* [Building](#building-system)  

[4. Villagers](#villagers)  
* [Professions](#professions)  
* [Villagers AI](#villagers-ai)  

[5. Used Assets](#assets-used-in-project)  
[6. Video updates](#updates-on-youtube)  
[7. License](#license)

---

## World
For now, there is only one simple test map divided on various **areas**. Each of it is a distinct biome and contains different stuff. Areas are based on 2D grid which helps placing object and transfer data between game objects and world entities.

**Future plans**:
There will be a map of whole game world, probably created randomly, but with some had-made regions that will be key to game story. Each of them will be explorable, player will be able to build outpost and defend them going fahrer and fahrer to

---

## Player
Player, as mentioned before, will be controlling a character, his avatar in the game world, that will allow him to interact with it. 

### Player Tools System
![Tools][Tools]

All interactions will be done with tools, that player is be able to select from UI radial menu.
Currently implemented tools:
* __Villagers book__ - it is used to show villager properties and change its profession
* __Hammer__ - it is used to create constructions of buildings

---

## Systems
### Building System
![Building system][building system]
Building system allows on selecting (currently not much) a building and place its construction close to player avatar. Each building has resources that are required to build it.
If there are not enough resources to build, player can't place construction.

---

## Villagers
It's an entity living in village which player can manage. Villagers are one of main pillars of the game and every of them should be unique. They will live and work in the village.

Currently, villager has statistics: __strength, dexterity, intelligence__, which are considered in his profession.

**Future plans**:
* Villagers _will be learning_. Learning will arise new behaviours which will speed up villagers daily life and responsibilities. They also should feel like being "real".

* Villagers _will be aging_. Start of being born, they will age from being a child to being an elder. Their age will affect on their life, if older they will be then weaker they will become, like humans.

* Villagers _will make relations_. They will make friends, lovers or enemies as each others. Relations should affect on their daily routines, making them better or worse. After getting a lover, they will make a child to populate village.

### Professions
Villagers have professions to perform specific works in the village (or whole kingdom) for which they acquire daily gold. They will be able to spent it in the village for food and entertainment. 

Each profession has statistic requirements which will define how villager will fit this profession (not really implemented).

Currently implemented:
* **Unemployed** - Villager without job is mean to wander close to Town Hall (currently not implemented).
* **Builder** - Villager, who delivery  resources from a warehouse to the construction and build constructions when they have enough resources.
* **Lumberjack** - Villager, who is gathering wood.
* **Workplace Hauler** - a villager, who is handling _resource carrying_ tasks, in his workplace.
* **Global Hauler** - a villager hired in warehouse - it is helping workplaces that have no hired haulers and transform resources between workplace and warehouse.

#### Villagers AI
![Villager AI][villager]
Villager will be a really complex thing. As object, it has own brain which contains layers that handle specific behaviour.

It's AI behaviour is handled by two mechanism: a _Behaviour Tree_ and a Finite States Machine - _tasks_. 

##### Behaviour Trees

For now, there is only one BT for villager, a Worker BT, which defines behaviour of villager whe he is performing his profession. There will be other BTs, for behaviour while not working, for events behaviours, etc.
![Worker behaviour tree][worker-BT]
Worker BT was implemented by hand-coded BT framework, but it was buggy and inefficient, so it was changed to NodeCanvas Framework:
![Worker behaviour tree implemented on NodeCanvas][worker-BT-nodeCanvas]

##### Tasks
Task is a *finite states machine* that controls villager-worker directly. It has own nested states which every of them is a part of an algorithm for executing actions related with certain profession.

Tasks are created in workplaces due to an order of events or signals.

Tasks can be _single_ or _automatic_: 
* Single tasks has a goal to achieve, for example, deliver a resource from warehouse to construction.
* Automatic tasks works as a while loop and never end or works until some conditions will not be met. For example, gathering tasks has a start, but is executed until villager can perform his profession.

Every villager-worker with profession has a collection which contain tasks that he can do. Tasks are executed in _Work_ workerBT node.

**Currently implemented tasks**: 
* **Building task** - It tells the villager that there is a construction which should be transformed into a building. Building always comes with Resource Carrying Task, one for each resource type. It is dependent on them - resources must be delivered first to start constructing the building.
* **Resource Carrying Task** - It is the most commonly occurring task. It tells the villager that some resources need to be carried from point A to point B.
* **Gathering Task** - It is used to instruct villager about gathering raw material to get resource and transport it to the workplace.
* **Resource Pickup Task** - When villager is carrying resource and mind about his own business, something can interrupt his routine - he will throw the resource on ground, so someone need to pickup it and transport to closest warehouse.

--- 

## Assets used in project

[1. Lean Tween](https://assetstore.unity.com/packages/tools/animation/leantween-3595)  
[2. Node Canvas](https://assetstore.unity.com/packages/tools/visual-scripting/nodecanvas-14914)

_All visual assets are currently also from asset store._

---

## Updates on youtube
* [First update - tools and buildings](https://www.youtube.com/watch?v=laCbKncUxlc)
* [Building selecting while building UI](https://www.youtube.com/watch?v=eGZ8UUqtYbY)
* [Villager selection and profession changing UI](https://www.youtube.com/watch?v=rstPntP1JzQ)
* [Workplace Hauler profession implementation](https://www.youtube.com/watch?v=pYRBg31MNkk)
* [Developer console implementation](https://www.youtube.com/watch?v=r-kNdXYLJPA)
* [Resource Gathering - wood](https://www.youtube.com/watch?v=hXsnhwjksXg)
* [Resources on ground](https://www.youtube.com/watch?v=osgqHBOloio)

---

## License
Below project was published only for demonstration purposes. I do not allow copying, modifying and making money from it.





[tools]: https://imgur.com/Q88VK3p.png
[building system]: https://imgur.com/P0PRc5b.png
[villager]: https://imgur.com/Mxx59ax.png
[worker-BT]: https://imgur.com/RSEfWRo.png
[worker-BT-nodeCanvas]: https://imgur.com/vduUbTp.png