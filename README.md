# Blendity
Unity Asset for running Blender 3D processes/scripts from a context menu

## Features
* Procedural tree generation with 9 types of trees 2K or 5K triangles (via Sapling Tree add-on) [Video Showcase](https://www.youtube.com/watch?v=pQSfgwxG9tQ)
* Generate procedural spaceships in parallel (via [a1studmuffin's SpaceshipGenerator](https://github.com/a1studmuffin/SpaceshipGenerator))
* Fracture multiple files in parallel (mesh has to be closed, no holes no loose parts) [Video Showcase](https://www.youtube.com/watch?v=tlss-FGgKHs)
* LOD creation with custom number of levels and reduction ratio
* Unwrap UVs using one of Blenders methods (smart, lightmap, sphere projection, capsule projection or cube projection)
* Import/Export of fbx, obj, 3dx and gltf models

## Usage
* You can download a tarball file from [here](https://aetuts.itch.io/blendity) then install it from Window > Package Manager then click the "+" icon and choose "Add package from tarball ..." and select the com.ae.blendity-X.X.X.tgz file that you downloaded
or
* You can clone this repo then install it from Window > Package Manager then click the "+" icon and choose "Add package from disk ..." then navigate to the "package.json" file
or 
* Install it from Window > Package Manager then click the "+" icon and choose "Add package from git url ..." and put https://github.com/anaseinea/Blendity.git
Note: you have to have git and git lfs installed if you want to clone or install from git url.

Commands are found in the context menu in the "Project" window under "Blendity",
 Fracture is enabled if you have at least one supported file chosen and Spaceships generator is enabled if you select any folder other than "Assets"

You can select multiple objects and execute a command on all at once, this will run multiple instances of Blender at the same time.

## Compatibility
This package works on Windows only and tested on Unity 2019 (but might work on older versions)

## How does it work ?
"Core.cs" has functions for running Blender processes, passing -b would lunch it in headless mode,
 passing -P allows to pass a python script which would get executed then the process will be done.
For each new feature there is a C# script that adds context menu and make use of the ParamsModal which allows to customize variables needed in Blender side,
 then run a process and passes a python script to it,
 and a python script that execute Blender side commands,
 parameters are loaded as environment variables to the process.


