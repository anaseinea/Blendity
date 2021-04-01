import argparse

def get():
  parser = argparse.ArgumentParser()
 
  # get all script args
  _, all_arguments = parser.parse_known_args()
  double_dash_index = all_arguments.index('--')
  script_args = all_arguments[double_dash_index + 1: ]
 
  # add parser rules
  parser.add_argument('--number', help="number of cubes")
  parser.add_argument('--output', help="output file")
  parser.add_argument('--input', help="input file")
  parser.add_argument('--seed', help="seed")
  parsed_script_args, _ = parser.parse_known_args(script_args)
  return parsed_script_args