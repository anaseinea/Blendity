using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Blendity
{
  public class TreeGen : Editor
  {
    [MenuItem("Assets/Blendity/Generate/Trees", true)]
    public static bool GenerateTreeValid()
    {
      string activeName = Utils.GetActiveFileName();
      return !activeName.EndsWith("Assets") && Directory.Exists(activeName);
    }

    [MenuItem("Assets/Blendity/Generate/Trees")]
    public static void GenerateTree()
    {
      string presetsPath = $@"{Utils.GetPackagePath()}\blender~\2.92\scripts\presets\operator\add_curve_sapling\";
      string[] presets = Array.ConvertAll(Directory.GetFiles(presetsPath), (path) => path.Split('\\').Last());
      string options = string.Join(",", presets);

      ParamsModal modal = ScriptableObject.CreateInstance<ParamsModal>();
      string[,] defaultVariables = {
        { "Number of Trees", "1","int:1,20" },
        { "tree_type", presets[0], $"dropdown:{options}"},
        { "generate_LODs", "True", "bool" },
      };
      modal.defaultVariables = defaultVariables;
      modal.OnStart = (List<KeyValueConfig> variables) =>
      {
        EditorUtility.DisplayProgressBar("Creating Trees !", "Generating Trees", .1f);

        int numOfShips = int.Parse(variables[0].value);
        variables.RemoveAt(0);

        Func<string, int, Dictionary<string, string>> EnvCreator = (string path, int threadSeed) =>
        {
          int seed = (int)Stopwatch.GetTimestamp() + threadSeed;
          string output = $@"{path}\";
          Dictionary<string, string> envVars = new Dictionary<string, string>{
            {"seed",$"{seed}"},
            {"output",$"{output}"}
          };
          variables.ForEach((variable) => envVars.Add(variable.key, variable.value));
          return envVars;
        };

        List<CommandOutput> procOutputs = Core.RunCommandTimesN(
          $@"-b -P py_scripts~\generate_tree.py",
          numOfShips,
          EnvCreator
        );
        EditorUtility.DisplayProgressBar("Creating Trees !", "Importing Models", .5f);

        float progressPerLoop = 0.4f / procOutputs.Count;
        float progress = 0.55f;
        int i = 1;

        AssetDatabase.Refresh();

        string materialDir = procOutputs[0].outputFile + "materials";
        Directory.CreateDirectory(materialDir);

        string barkMaterial = materialDir + @"\bark material.mat";
        Utils.CreateMaterial(barkMaterial);
        string leavesMaterial = materialDir + @"\leaves material.mat";
        Utils.CreateMaterial(leavesMaterial);
        
        procOutputs.ForEach((procOutput) =>
        {
          UnityEngine.Debug.Log(procOutput);
          string s = Array.Find(procOutput.result.Split('\n'), (str) => str.StartsWith("FBX export starting... '")).Split('\'')[1].Replace(@"\\", @"\");
          Utils.SearchAndRemapMaterials(s);

          progress += progressPerLoop;
          EditorUtility.DisplayProgressBar("Creating Trees !", "Searching for Materials #" + i++, progress);
        });
        EditorUtility.ClearProgressBar();
      };
      modal.ShowModalUtility();
    }
  }
}