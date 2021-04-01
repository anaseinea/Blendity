using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Blendity
{
	public class Utils : MonoBehaviour
	{
		static readonly string[] validExtensions = new string[] { ".fbx", ".obj", ".x3d", "gltf" };
		static readonly Func<string, bool> IsValidExtension = fileName => validExtensions.Any((extension) => fileName.EndsWith(extension));

		public static string GetActiveFileName() => AssetDatabase.GetAssetPath(Selection.activeInstanceID);
		private static string[] GetSelectedFileNames() => Array.ConvertAll(Selection.objects, obj => AssetDatabase.GetAssetPath(obj.GetInstanceID()));
		public static bool IsValidImports()
		{
			string[] fileNames = GetSelectedFileNames();
			return fileNames.Any(IsValidExtension);
		}

		public static List<string> GetValidImports()
		{
			string[] fileNames = GetSelectedFileNames();
			return fileNames.Aggregate(new List<string>(), (a, b) => { if (IsValidExtension(b)) a.Add(b); return a; });
		}

		public static string GetWindowsRelativePath(string path, string suffix = "")
		{
			string[] pathPieces = path.Replace("Assets", "..").Split('/');

			string[] fileNamePieces = pathPieces.Last().Split('.');
			string extension = "";

			if (fileNamePieces.Length > 1)
			{
				extension = fileNamePieces.Last();
				pathPieces[pathPieces.Length - 1] = string.Join(".", fileNamePieces.Take(fileNamePieces.Length - 1));
			}

			string output = $@"{string.Join(@"\", pathPieces)}{suffix}" + (extension.Length > 0 ? $".{extension}" : "");
			return output;
		}

		// https://github.com/Unity-Technologies/UnityCsReference/blob/61f92bd79ae862c4465d35270f9d1d57befd1761/Editor/Mono/Prefabs/PrefabUtility.cs#L131
		public static void ExtractMaterialsFromAsset(AssetImporter importer, string destinationPath)
		{
			var assetsToReload = new HashSet<string>();
			var materials = AssetDatabase.LoadAllAssetsAtPath(importer.assetPath).Where(x => x.GetType() == typeof(Material)).ToArray();

			foreach (var material in materials)
			{
				var newAssetPath = destinationPath + "/" + material.name + ".mat";
				newAssetPath = AssetDatabase.GenerateUniqueAssetPath(newAssetPath);

				var error = AssetDatabase.ExtractAsset(material, newAssetPath);
				if (string.IsNullOrEmpty(error))
				{
					assetsToReload.Add(importer.assetPath);
				}
			}

			foreach (var path in assetsToReload)
			{
				AssetDatabase.WriteImportSettingsIfDirty(path);
				AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
			}
		}

		public static void ExtractTexturesAndMaterials(string assetPath)
		{
			assetPath = assetPath.Replace("..", "");
			string path = "Assets/" + assetPath.Replace("\\", "/");
			AssetImporter assetImporter = AssetImporter.GetAtPath(path);
			ModelImporter modelImporter = assetImporter as ModelImporter;
			string[] outputFilePieces = assetPath.Split('\\');
			string outputDir = "Assets/" + string.Join("/", outputFilePieces.Take(outputFilePieces.Length - 1));

			modelImporter.ExtractTextures(outputDir);
			AssetDatabase.Refresh();
			ExtractMaterialsFromAsset(assetImporter, outputDir);
		}
	}
}
