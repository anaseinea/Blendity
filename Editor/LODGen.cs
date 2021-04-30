using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Blendity
{
  public class LODGen : Editor
  {
    [MenuItem("Assets/Blendity/Make LOD", true)]
    public static bool GenerateLODValid()
    {
      return Utils.IsValidImports();
    }

    [MenuItem("Assets/Blendity/Make LOD")]
    public static void GenerateLOD()
    {
      ParamsModal modal = ScriptableObject.CreateInstance<ParamsModal>();
      string[,] defaultVariables = {
        { "number_of_LOD", "4","int:1,8" },
        { "least_detail_percent", "20","float:0,99" }
      };
      modal.defaultVariables = defaultVariables;
      modal.OnStart = (List<KeyValueConfig> variables) =>
      {
        EditorUtility.DisplayProgressBar("Decimating Your Mesh !", "Generating LOD clones", .2f);

        Func<string, int, Dictionary<string, string>> EnvCreator = (string fileName, int threadSeed) =>
        {
          string input = Utils.GetWindowsPath(fileName);
          string output = Utils.GetWindowsPath(fileName, "-LOD");
          Dictionary<string, string> envVars = new Dictionary<string, string>{
            {"input",$"{input}"},
            {"output",$"{output}"}
          };
          variables.ForEach((variable) => envVars.Add(variable.key, variable.value));
          return envVars;
        };

        List<CommandOutput> procOutputs = Core.RunCommandOnSelected(
          $@"-b -P py_scripts~\generate_LOD.py",
          EnvCreator
         );
        procOutputs.ForEach(UnityEngine.Debug.Log);
        EditorUtility.DisplayProgressBar("Decimating Your Mesh !", "Importing Models", .8f);
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
      };
      modal.ShowModalUtility();
    }
  }
}
