using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace Blendity
{
  public class Unwrapper : Editor
  {
    [MenuItem("Assets/Blendity/Unwrap", true)]
    public static bool UnwrapperValid()
    {
      return Utils.IsValidImports();
    }

    [MenuItem("Assets/Blendity/Unwrap")]
    public static void UnwrapperFn()
    {
      ParamsModal modal = ScriptableObject.CreateInstance<ParamsModal>();
      string[,] defaultVariables = {
        { "unwrap_mode", "smart_project","dropdown:smart_project,lightmap_pack,cube_project,cylinder_project,sphere_project" }
      };
      modal.defaultVariables = defaultVariables;
      modal.OnStart = (List<KeyValueConfig> variables) =>
      {
        EditorUtility.DisplayProgressBar("Looking at your Mesh !", "Unwraping ...", .1f);

        Func<string, int, Dictionary<string, string>> EnvCreator = (string path, int threadSeed) =>
        {
          string output = Utils.GetWindowsPath(path, "-unwrapped");
          Dictionary<string, string> envVars = new Dictionary<string, string>{
            {"input",$"{path}"},
            {"output",$"{output}"}
          };
          variables.ForEach((variable) => envVars.Add(variable.key, variable.value));
          return envVars;
        };

        List<CommandOutput> procOutputs = Core.RunCommandOnSelected(
          $@"-b -P py_scripts~\unwrap.py",
          EnvCreator
        );
        EditorUtility.DisplayProgressBar("Looking at your Mesh !", "Importing Models", .5f);

        procOutputs.ForEach(UnityEngine.Debug.Log);
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
      };
      modal.ShowModalUtility();
    }
  }
}
