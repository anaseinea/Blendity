using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Blendity
{
	public class KeyValue
	{
		public string key, value;
		public bool userCreated = false;
	}

	public class ParamsModal : EditorWindow
	{
		private string newVariableKey = "";
		public string[,] defaultVariables;
		private List<KeyValue> variables;
		private ReorderableList list;
		private void InitializeList()
		{
			variables = new List<KeyValue>();
			for (int i = 0; i < defaultVariables.Length / 2; i++)
				variables.Add(new KeyValue { key = defaultVariables[i, 0], value = defaultVariables[i, 1] });
			list = new ReorderableList(variables, typeof(Dictionary<string, string>), false, false, true, true);
			list.drawElementCallback =
				(Rect rect, int index, bool isActive, bool isFocused) =>
				{
					EditorGUIUtility.labelWidth = 200f;
					variables[index].value = EditorGUI.TextField(
						new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight)
						, variables[index].key, variables[index].value
					);
				};


			list.onCanRemoveCallback = (ReorderableList mlist) =>
			{
				return variables[mlist.index].userCreated;
			};

			list.onCanAddCallback = (ReorderableList mlist) =>
			{
				return newVariableKey.Length > 0 && variables.All((variable) => variable.key != newVariableKey);
			};

			list.onAddCallback = (ReorderableList l) =>
			{
				variables.Add(new KeyValue { key = newVariableKey, userCreated = true });
				newVariableKey = "";
				focusControl = () =>
				{
					GUI.FocusControl("New Variable Key");
				};
			};
		}

		Action focusControl;

		private void OnGUI()
		{
			if (list == null)
			{
				InitializeList();
				position = new Rect(Event.current.mousePosition - new Vector2(400, 400), new Vector2(400, 600));
			}
			GUILayout.BeginVertical(GUILayout.MinHeight(position.height - EditorGUIUtility.singleLineHeight * 3));
			list.DoLayoutList();
			GUILayout.EndVertical();
			GUI.SetNextControlName("New Variable Key");
			newVariableKey = EditorGUILayout.TextField("New Variable Key", newVariableKey);
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Close"))
				Close();
			if (GUILayout.Button("Start"))
			{
				OnStart(variables);
				Close();
			}
			EditorGUILayout.EndHorizontal();

			if (focusControl != null)
			{
				focusControl();
				focusControl = null;
			}
		}

		public Action<List<KeyValue>> OnStart;
		private void OnInspectorUpdate()
		{
			Repaint();
		}
	}
}