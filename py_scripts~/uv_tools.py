import bpy
import bmesh
from math import cos, sin, radians


def make_rotation_transformation(angle, origin=(0, 0)):
  cos_theta, sin_theta = cos(angle), sin(angle)
  x0, y0 = origin

  def xform(point):
    x, y = point[0] - x0, point[1] - y0
    return (x * cos_theta - y * sin_theta + x0,
            x * sin_theta + y * cos_theta + y0)

  return xform


def rotate_uv(degrees=90):
  # rotation logic
  obj = bpy.context.object
  obj_data = obj.data
  obj.select_set(True)
  bpy.ops.object.editmode_toggle()
  bm = bmesh.from_edit_mesh(obj_data)

  uv_layer = bm.loops.layers.uv.verify()

  rad = radians(degrees)
  anchor = (0.5, 0.5)

  rot = make_rotation_transformation(rad, anchor)

  # adjust uv coordinates
  for face in bm.faces:
    for loop in face.loops:
      loop_uv = loop[uv_layer]
      # use xy position of the vertex as a uv coordinate
      loop_uv.uv = rot(loop_uv.uv)

  bmesh.update_edit_mesh(obj_data)
  bpy.ops.object.editmode_toggle()
  obj.select_set(False)


def unwrap_each_face():
  obj = bpy.context.object
  obj.select_set(True)
  bpy.ops.object.mode_set(mode='EDIT')
  bpy.ops.mesh.select_mode(type='FACE')
  bpy.ops.mesh.select_all(action='DESELECT')
  bm = bmesh.from_edit_mesh(obj.data)
  for face in bm.faces:
    face.select = True
    bm.faces.active = face
    bpy.ops.uv.reset()
    face.select = False
  bpy.ops.object.mode_set(mode='OBJECT')
  obj.select_set(False)
  bm.free()
