from os import environ

def get_float(field_name):
  return float(environ.get(field_name))

def get_int(field_name):
  return int(get_float(field_name))

def get_str(field_name):
  return environ.get(field_name)

def get_vec(field_name_x, field_name_y, field_name_z):
  return (get_float(field_name_x),get_float(field_name_y),get_float(field_name_z))

def get_bool(field_name):
  value = environ.get(field_name)
  return value == 'True' or value == 'true' or value == "T" or value == 't'