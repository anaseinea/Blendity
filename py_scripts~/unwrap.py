import bpy
import sys

sys.path.insert(0, './py_scripts~')

from import_export import import_scene, export_scene
from value_getters import get_str

import_scene()

unwrap_mode = get_str('unwrap_mode')
# "smart_project", "lightmap_pack", "cube_project","cylinder_project","sphere_project"

objs = bpy.context.scene.objects[:]
for obj in objs:
  if obj.type != 'MESH':
    continue
  bpy.ops.object.select_all(action='DESELECT')
  obj.select_set(True)
  bpy.context.view_layer.objects.active = obj
  bpy.ops.object.editmode_toggle()
  getattr(bpy.ops.uv, unwrap_mode)()
  bpy.ops.uv.select_all(action='SELECT')
  bpy.ops.uv.pack_islands(margin=0.001)
  bpy.ops.object.editmode_toggle()

export_scene()
