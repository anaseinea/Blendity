# UNUSED
import bpy
import ast
import os


def getPresetpaths():
  """Return paths for both local and user preset folders"""
  userDir = os.path.join(bpy.utils.script_path_user(), 'presets', 'operator',
                         'add_curve_sapling')
  if os.path.isdir(userDir):
    pass
  else:
    os.makedirs(userDir)

  script_file = os.path.realpath(__file__)
  directory = os.path.dirname(script_file)
  localDir = os.path.join(directory, "presets")
  return (localDir, userDir)


def get_dict_from_file(filename):
  # Read the preset data into the global settings
  try:
    f = open(os.path.join(getPresetpaths()[0], filename), 'r')
  except (FileNotFoundError, IOError):
    f = open(os.path.join(getPresetpaths()[1], filename), 'r')
  settings = f.readline()
  f.close()
  return ast.literal_eval(settings)
