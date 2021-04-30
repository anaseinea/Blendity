import sys

sys.path.insert(0, './py_scripts~')

from import_export import import_scene, export_scene
from value_getters import get_int, get_float
from LOD_maker import make_LODs

import_scene()

number_of_LOD = min(get_int("number_of_LOD"), 8)
least_detail_percent = get_float("least_detail_percent") / 100

make_LODs(number_of_LOD, least_detail_percent)

export_scene()
