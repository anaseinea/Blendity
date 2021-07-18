using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Blendity
{
  public class RockGen : Editor
  {
    [MenuItem("Assets/Blendity/Generate/Rocks", true)]
    public static bool GenerateRockValid()
    {
      string activeName = Utils.GetActiveFileName();
      return !activeName.EndsWith("Assets") && Directory.Exists(activeName);
    }

    [MenuItem("Assets/Blendity/Generate/Rocks")]
    public static void GenerateRock()
    {
      ParamsModal modal = ScriptableObject.CreateInstance<ParamsModal>();
      string[,] defaultVariables = {
        { "Number of Rocks", "1","int:1,30" },
        { "Stone Type", "Default","dropdown:Default,River Rock,Asteroid,Sandstone,Ice,Fake Ocean"  },
        { "scale_X", "1","float:0,100" },
        { "scale_Y", "1","float:0,100" },
        { "scale_Z", "1","float:0,100" },
        { "Mesh Density", "2","int:1,4" },
        { "Smooth", "True","bool" },
        { "Smoothness Factor", "1.2","float:0,2.5" }
      };
      modal.defaultVariables = defaultVariables;
      modal.OnStart = (List<KeyValueConfig> variables) =>
      {
        EditorUtility.DisplayProgressBar("Creating Rocks !", "Generating Rocks", .1f);

        int numOfRocks = int.Parse(variables[0].value);
        variables.RemoveAt(0);

        Func<string, int, Dictionary<string, string>> EnvCreator = (string path, int threadSeed) =>
        {
          int seed = Mathf.Abs((int)Stopwatch.GetTimestamp() + threadSeed) % 1048576;
          string rockName = $"{variables[0].value}_{seed}";
          string output = $@"{path}\{rockName}.fbx";
          Dictionary<string, string> envVars = new Dictionary<string, string>{
            {"seed",$"{seed}"},
            {"output",$"{output}"}
          };
          variables.ForEach((variable) => envVars.Add(variable.key, variable.value));
          return envVars;
        };

        List<CommandOutput> procOutputs = Core.RunCommandTimesN(
          $@"-b -P py_scripts~\generate_rock.py",
          numOfRocks,
          EnvCreator
        );
        EditorUtility.DisplayProgressBar("Creating Rocks !", "Importing Models", .7f);

        AssetDatabase.Refresh();
        procOutputs.ForEach(output => output.Print());

        EditorUtility.ClearProgressBar();
      };
      modal.ShowModalUtility();
    }
  }
}