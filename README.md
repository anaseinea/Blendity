# Blendity
Unity Asset for running Blender 3D processes/scripts from a context menu

## Features
* Import/Export of fbx, obj, 3dx and gltf models
* Fracture multiple files in parallel
* Generate multiple procedural spaceships in parallel (via [a1studmuffin's SpaceshipGenerator](https://github.com/a1studmuffin/SpaceshipGenerator))

## Usage
* You can download a tarball file from [here](https://aetuts.itch.io/blendity) then install it from Window > Package Manager then click the "+" icon and choose "Add package from tarball ..." and select the com.ae.blendity-X.X.X.tgz file that you downloaded
or
* You can download this repo as zip and unpack it or clone it then install it from Window > Package Manager then click the "+" icon and choose "Add package from disk ..." then navigate to the "package.json" file
or 
* Install it from Window > Package Manager then click the "+" icon and choose "Add package from git url ..." and put https://github.com/anaseinea/Blendity.git
Note: cloning or "Add package from git url ..." requires you to have git and git lfs installed

Commands are found in the context menu in the "Project" window under "Blendity",
 Fracture is enabled if you have at least one supported file chosen and Spaceships generator is enabled if you select any folder other than "Assets"

## Compatibility
This package works on Windows only and tested on Unity 2019 (but might work on older versions)

## How does it work ?
"Core.cs" has functions for running Blender processes, passing -b would lunch it in headless mode,
 passing -P allows to pass a python script which would get executed then the process will be done.
For each new feature there is a C# script that adds context menu and make use of the ParamsModal which allows to customize variables needed in Blender side,
 then run a process and passes a python script to it,
 and a python script that execute Blender side commands,
 parameters are loaded as environment variables to the process.


