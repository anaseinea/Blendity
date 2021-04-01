import bpy
import sys
from os import makedirs, environ

sys.path.insert(0, './py_scripts~')
from import_export import export_scene
from value_getters import get_int, get_bool, get_str

bpy.ops.mesh.generate_spaceship(
  num_hull_segments_min=get_int("num_hull_segments_min"),
  num_hull_segments_max=get_int("num_hull_segments_max"),
  create_asymmetry_segments=get_bool("create_asymmetry_segments"),
  num_asymmetry_segments_min=get_int("num_asymmetry_segments_min"),
  num_asymmetry_segments_max=get_int("num_asymmetry_segments_max"),
  create_face_detail=get_bool("create_face_detail"),
  allow_horizontal_symmetry=get_bool("allow_horizontal_symmetry"),
  allow_vertical_symmetry=get_bool("allow_vertical_symmetry"),
  apply_bevel_modifier=get_bool("apply_bevel_modifier"),
  assign_materials=get_bool("assign_materials")
)

bpy.ops.object.editmode_toggle()
bpy.ops.mesh.select_all(action='SELECT')
bpy.ops.uv.smart_project()
bpy.ops.object.editmode_toggle()

output_path = get_str('output')
makedirs('\\'.join(output_path.split('\\')[:-1]))
export_scene()