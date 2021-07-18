import bpy
import sys

sys.path.insert(0, './py_scripts~')
from import_export import export_scene
from value_getters import get_int, get_str, get_float, get_bool

preset = [
    'Default', 'River Rock', 'Asteroid', 'Sandstone', 'Ice', 'Fake Ocean'
].index(get_str('Stone Type'))

x = get_float('scale_X')
y = get_float('scale_Y')
z = get_float('scale_Z')

bpy.ops.mesh.add_mesh_rock(preset_values=str(preset),
                           scale_X=(x, x),
                           scale_Y=(z, z),
                           scale_Z=(y, y),
                           display_detail=get_int('Mesh Density'),
                           user_seed=get_int('seed'),
                           use_random_seed=False)

if get_bool('Smooth'):
  bpy.ops.object.modifier_add(type='SMOOTH')
  bpy.context.object.modifiers["Smooth"].factor = get_float('Smoothness Factor')
  bpy.context.object.modifiers["Smooth"].iterations = 3

bpy.ops.object.convert(target='MESH')
bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)

export_scene()
