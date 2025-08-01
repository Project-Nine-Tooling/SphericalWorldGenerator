# Spherical World Map Generator

Date: 2025-07-26
Version: 1.1

Based on Jon's [WorldGeneratorFinal](https://github.com/jongallant/WorldGeneratorFinal), as per its article on [procedural world generation](https://www.jgallant.com/procedurally-generating-wrapping-world-maps-in-unity-csharp-part-4/). Organized and stripped Unity related logic. Proceedings from this will likely be used in The Matrix.

Notice his algorithm is very similar to how Minecraft works, especially for the climate part.

Technical notes:

* (20250727) Per the computer science of terrain generation - looks like this implementation uses agent based approach?

## TODO

Improvements:

- [ ] Baseline multithreading performance
- [ ] Improve river path finding 
- [ ] Implement height control e.g. control with global height map similar to HDPlanet
- [ ] Integrate erosion

Refactoring:

- [ ] Modularize components as static helpers for customized compositional use

Optimization:

- [ ] Memory usage for large maps: 16000*16000

## Mapings

### Spherical

![Example](http://www.jgallant.com/wp-content/uploads/2019/05/worldgen1.png "Example")

### Wrapping

![Example](http://www.jgallant.com/wp-content/uploads/2019/05/worldgenwrap.png "Example")

## Resources

Assets:

* Sphere mesh taken from: https://www.binpress.com/creating-octahedron-sphere-unity/

Additional algorithms:

* This cloud gen has erosion: https://github.com/search?q=repo%3ASebLague%2FClouds++language%3AC%23&type=code