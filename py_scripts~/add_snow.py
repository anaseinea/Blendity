import bpy
import sys

sys.path.insert(0, './py_scripts~')

from import_export import import_scene, export_scene
from value_getters import get_float

import_scene()

bpy.context.scene.snow.coverage = get_float('coverage %')
bpy.context.scene.snow.height = get_float('height')

bpy.ops.object.select_all(action='SELECT')
bpy.ops.snow.create()

objs = bpy.context.selected_objects[:]
bpy.ops.object.select_all(action='DESELECT')

for obj in objs:
  obj.select_set(True)
  bpy.context.view_layer.objects.active = obj
  bpy.ops.object.modifier_remove(modifier="Subdiv")
  bpy.context.object.modifiers["Decimate"].ratio = 1 - get_float(
      'mesh reduction')

export_scene()
