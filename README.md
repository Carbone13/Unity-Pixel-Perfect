# Unity Pixel Perfect
### A pixel perfect physic & collisions solution for Unity 2D, inspired from Celeste

# Features
* Entities move 1 pixel per 1 pixel.
* Collision detection is optimized.
* Use a custom made collider class, which work with Pixel Size.

# How to use
<img src="Readme/ppu.jpg" align="right" />
First, one pixel of your sprites should equal 1 unit in Unity. To do so you need to use a PPU (Pixel Per Unit) of 1 in your sprites import settings.
Then, any geometry/level in your scene should use the component 'Entity' this will give them a collision box, and they will be tracked by the system.

#
For anything that will move, you have 2 choices

* --> If it need to move & collide (i.e a Player), you need to use a class which inherit from Actor2D (see Demo Scripts)
* --> If it need to move without colliding, but still pushing Actor2D in its path (i.e a Moving platform), you need to use a class which inherit from Solid2D (see Demo Scripts)

# Project Structure
<img src="Readme/system_structure.jpg" align="center" />
