import bpy


def make_LODs(number_of_LOD=4, least_detail_percent=0.2):
  objs = bpy.context.scene.objects[:]
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
      bpy.context.object.modifiers["Decimate"].ratio = 1 - (
          1 - least_detail_percent) / (number_of_LOD - 1) * (j + 1)
    obj.name += "0"
