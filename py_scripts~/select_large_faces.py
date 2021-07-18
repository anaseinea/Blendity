import bpy
import bmesh


def select_large_faces(factor=20):
  ob = bpy.context.edit_object
  me = ob.data
  bm = bmesh.from_edit_mesh(me)
  avg_face_area = sum(f.calc_area() for f in bm.faces) / len(bm.faces)

  # for f in bm.faces:
  #   f.select = f.calc_area() > factor
  for f in bm.faces:
    f.select = len(f.verts) > 4  #  or f.calc_area() > factor * avg_face_area

  bmesh.update_edit_mesh(me)
