import bpy
import sys
from os import environ

sys.path.insert(0, './py_scripts~')
from value_getters import get_int, get_str, get_bool
from import_export import export_scene
from uv_tools import rotate_uv, unwrap_each_face
from LOD_maker import make_LODs

bpy.ops.sapling.importdata(filename=get_str('tree_type'))
bpy.ops.curve.tree_add(limitImport=False, seed=get_int('seed'))

obj = bpy.context.scene.objects["tree"]
obj.select_set(True)
bpy.context.view_layer.objects.active = obj
bpy.ops.object.convert(target='MESH')

bark_mat = bpy.data.materials.new("bark material")
obj.data.materials.append(bark_mat)

# Rotate tree uvs
rotate_uv()

# unwrap tree leaves
if "leaves" in bpy.context.scene.objects:
  obj = bpy.context.scene.objects["leaves"]

  leaves_mat = bpy.data.materials.new("leaves material")
  obj.data.materials.append(leaves_mat)

  bpy.context.view_layer.objects.active = obj
  unwrap_each_face()
  obj.select_set(True)
  rotate_uv()

if get_bool("generate_LODs"):
  make_LODs(least_detail_percent=0.3)

environ['output'] = get_str('output') + get_str('tree_type')[:-3] + "_" + get_str('seed') + ".fbx"
export_scene()
