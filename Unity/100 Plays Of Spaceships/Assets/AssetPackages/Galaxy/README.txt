Galaxy Generator


Overview
========

Thank you for downloading this project! Your support is appreciated, as is any feedback you might have. Please get in touch via the Asset Store contact info should you ave question, concerns, or ideas!

The Galaxy Generator is intended to act as a quick and easy filler for space based games and experiences. Using The Galaxy Generator you can quickly and easily populate your scene with very customisable spiral galaxies. If you're looking for a comprehensive map builder this may not be what you need, but if you want aesthetic fly throughs or background visual effects this asset should suit your needs.


Getting Started
===============

To get started, open up one of the demo scenes, or drag the GalaxyGenerator prefab into your scene.


GalaxyGenerator Prefab
======================

There are many parameters in the Galaxy Generator prefab to allow for broad customisation, so read on if you're a little overwhelmed at first.


General Settings
----------------

- Spiral Arm Template: prefab that calculates the shape of a spiral
- Gas Particle Template: prefab containing nebulous gas cloud particles
- Star Particle Template: prefab containing particle system for star-like points of light
- Play On Awake: Should the galaxy be created as soom as the gameObject becomes active?
- Auto Update: Setting that is good for experimenting with look - as you change parameters in the editor the galaxy with automatically update. Note that it can take a few seconds for the galaxy to settle into its new parameters.
- Generate Galaxy Trigger: public bool to trigger galaxy generator. If you wish to activate a galaxy ia script this is what you can use to do that. Automatically turns false again once fired.
- Randomize: public bool to randomise the look of the galaxy. Recommended only for experimenting with looks - values can become quite erratic.

Spiral Settings
---------------

The maths for a spiral is similar to that of a circle. Imagine drawing a circle, but as you sweep across the arc of the circle over time the distance fom the center of your circle gets bigger. The result is a spiral.

In this section variables starting with T_ refer to settings for that time-based sweep. T_ variables will change the way an invdividual spiral looks.

- Number of Spirals: How many spiral arms the galaxy will have.
- Number of Points: How many Vector3 points make up each spiral.
- Fidelity: Distance between sections. Lower values result in tighter, more compact galaxies.
- T_scale: How big the spiral is relative to the gameObject.
- T_multiplier: Affects the overall curve of the spiral (higher values result in more curve).
- T_exponent: Similar to the idea of falloff; how sharply the spiral turns. Lower values result in more compact outer edges.

Gas and Star Particle Settings
------------------------------

These settings define the min and max values for the (randomised) particle settings that create the nebulous gas clouds and the starry points of light.

- Max Particles: As particles fade away, the Galaxy Generator will ensure that the particle system maintains this amount of particles. As particles die, new ones will birth to replace them.
- Position Offset: Min/Max values for how far away from the defined spiral a particle can appear. Low values result in a strongly defines spiral galaxy.
- Velocity: Should you wish for your galaxy to have a shimmering, slightly fluid appearance, set these above zero.
- Size: Min/Max values for size of particles.
- Life: Min/Max values for how long the particles remain on screen. Note that higher lifetimes can result in galaxies updating to new parameters more slowly.
- Colours: Define the two colour of the galaxy.

Visibility Settings
-------------------

Turn off/on the gas particles, star particles, and spiral lines.

Galaxy Generator Generator
==========================

One quick way to get a sense of various galactic possibilities is to use the generator generator. This tool, found in the Galaxy_Randomised scene, creates a number of randomised galaxies. The look of these is not super controllable at this point, so this is more of an experimentation tool than something to use in a scene. 

That said, the structure of the Generator Generator could be a good starting point should you wish to create your own procedural galaxy cluster.