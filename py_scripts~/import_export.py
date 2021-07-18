import bpy
from os import environ, path, makedirs


def import_scene():
  inputPath = environ.get('input')
  import_scene = bpy.ops.import_scene
  if inputPath.endswith("fbx"):
    import_scene.fbx(filepath=inputPath)
  elif inputPath.endswith("obj"):
    import_scene.obj(filepath=inputPath)
  elif inputPath.endswith("x3d"):
    import_scene.x3d(filepath=inputPath)
  elif inputPath.endswith("gltf"):
    import_scene.gltf(filepath=inputPath)


def export_scene(save_blend=False):
  outputPath = environ.get('output')
  directory = '\\'.join(outputPath.split('\\')[:-1])
  if not path.isdir(directory):
    makedirs(directory)

  export_scene = bpy.ops.export_scene
  if outputPath.endswith("fbx"):
    export_scene.fbx(filepath=outputPath, path_mode='COPY', embed_textures=True)
  elif outputPath.endswith("obj"):
    export_scene.obj(filepath=outputPath)
  elif outputPath.endswith("x3d"):
    export_scene.x3d(filepath=outputPath)
  elif outputPath.endswith("gltf"):
    export_scene.gltf(filepath=outputPath)
  if save_blend:
    bpy.ops.wm.save_as_mainfile(filepath=outputPath + ".blend")
