﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Blendity
{
	public class Spaceship : MonoBehaviour
	{
		[MenuItem("Assets/Blendity/Generate Spaceship", true)]
		public static bool GenerateSpaceshipValid()
		{
			string activeName = Utils.GetActiveFileName();
			return !activeName.EndsWith("Assets") && Directory.Exists(activeName);
		}

		[MenuItem("Assets/Blendity/Generate Spaceship")]
		public static void GenerateSpaceship()
		{
			ParamsModal modal = ScriptableObject.CreateInstance<ParamsModal>();
			string[,] defaultVariables = {
				{ "Number of Spaceships", "1"},
				{ "num_hull_segments_min", "3" },
				{ "num_hull_segments_max", "6" },
				{ "create_asymmetry_segments", "True" },
				{ "num_asymmetry_segments_min", "1" },
				{ "num_asymmetry_segments_max", "5" },
				{ "create_face_detail", "True" },
				{ "allow_horizontal_symmetry", "False" },
				{ "allow_vertical_symmetry", "False" },
				{ "apply_bevel_modifier", "True" },
				{ "assign_materials", "True" },
			};
			modal.defaultVariables = defaultVariables;
			modal.OnStart = (List<KeyValue> variables) =>
			{
				EditorUtility.DisplayProgressBar("Creating Spaceships !", "Generating Spaceships", .1f);

				int numOfShips = int.Parse(variables[0].value);
				variables.RemoveAt(0);

				Func<string, int, Dictionary<string, string>> EnvCreator = (string path, int threadSeed) =>
				{
					int seed = (int)Stopwatch.GetTimestamp() + threadSeed;
					string spaceshipName = $"spaceship_{seed}";
					string output = $@"{path}\{spaceshipName}\{spaceshipName}.fbx";
					Dictionary<string, string> envVars = new Dictionary<string, string>{
					{"output",$"{output}"}
					};
					variables.ForEach((variable) => envVars.Add(variable.key, variable.value));
					return envVars;
				};

				List<CommandOutput> procOutputs = Core.RunCommandTimesN(
					$@"-b -P py_scripts~\generate_spaceship.py",
					numOfShips,
					EnvCreator
				 );
				EditorUtility.DisplayProgressBar("Creating Spaceships !", "Importing Models", .5f);

				float progressPerLoop = 0.4f / procOutputs.Count;
				float progress = 0.55f;
				int i = 1;

				AssetDatabase.Refresh();
				procOutputs.ForEach(procOutput =>
				{
					progress += progressPerLoop;
					EditorUtility.DisplayProgressBar("Creating Spaceships !", "Importing Materials and Textures #" + i++, progress);
					Utils.ExtractTexturesAndMaterials(procOutput.outputFile);
				});
				AssetDatabase.Refresh();
				EditorUtility.ClearProgressBar();
			};
			modal.ShowModalUtility();
		}

	}
}