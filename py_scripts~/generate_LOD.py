import bpy
import sys

sys.path.insert(0, './py_scripts~')

from import_export import import_scene, export_scene
from value_getters import get_int, get_float

import_scene()

objs = bpy.context.scene.objects[:]

number_of_LOD = min(get_int("number_of_LOD"), 8)
least_detail_percent = get_float("least_detail_percent") / 100

for obj in objs:
  if obj.type != 'MESH':
    continue
  bpy.ops.object.select_all(action='DESELECT')
  obj.select_set(True)
  bpy.context.view_layer.objects.active = obj
  obj.name += "_LOD"
  bpy.ops.object.modifier_add(type='DECIMATE')
  for j in range(number_of_LOD - 1):
    bpy.ops.object.duplicate_move()
    bpy.context.object.name = obj.name + str(j + 1)
    bpy.context.object.modifiers["Decimate"].ratio = 1 - (1 - least_detail_percent) / (number_of_LOD - 1) * (j + 1)
  obj.name += "0"

export_scene()