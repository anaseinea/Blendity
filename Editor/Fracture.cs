using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace Blendity
{
  public class Fracture : Editor
  {
    [MenuItem("Assets/Blendity/Fracture", true)]
    public static bool CreateFracValid()
    {
      return Utils.IsValidImports();
    }

    [MenuItem("Assets/Blendity/Fracture")]
    public static void CreateFrac()
    {
      ParamsModal modal = ScriptableObject.CreateInstance<ParamsModal>();
      string[,] defaultVariables = {
      { "numOfPieces", "20", "int:2,500" },
      { "noise", "0", "float:0,1" },
      { "scaleX", "1", "float:0,1" },
      { "scaleY", "1", "float:0,1" },
      { "scaleZ", "1", "float:0,1" },
      { "smoothFaces", "False" , "bool"},
      { "sharpEdges", "True", "bool" },
      { "margin", "0" , "float:0,1"},
      { "recenterOrigin", "True", "bool" },
    };
      modal.defaultVariables = defaultVariables;
      modal.OnStart = (List<KeyValueConfig> variables) =>
      {
        EditorUtility.DisplayProgressBar("Shattering Your Mesh !", "Generating Pieces", .1f);

        Func<string, int, Dictionary<string, string>> EnvCreator = (string fileName, int threadSeed) =>
        {
          int seed = (int)Stopwatch.GetTimestamp() + threadSeed;
          string output = Utils.GetWindowsPath(fileName, "-frac");
          Dictionary<string, string> envVars = new Dictionary<string, string>{
          {"input",$"{fileName}"},
          {"output",$"{output}"},
          {"seed",$"{seed}"},
          };
          variables.ForEach((variable) => envVars.Add(variable.key, variable.value));
          return envVars;
        };

        List<CommandOutput> procOutputs = Core.RunCommandOnSelected(
          $@"-b -P py_scripts~\fracture.py",
          EnvCreator
         );
        procOutputs.ForEach(UnityEngine.Debug.Log);
        EditorUtility.DisplayProgressBar("Shattering Your Mesh !", "Importing Models", .8f);
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
      };
      modal.ShowModalUtility();
    }
  }
}