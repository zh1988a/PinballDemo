namespace FantomePI {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(FantomeTweenHeight))]
	public class FantomeTweenHeighEditor : FantomeTweenerEditor {

		public override void OnInspectorGUI() {
			GUILayout.Space(6f);
			EditorGUIUtility.labelWidth = 120f;

			FantomeTweenHeight tw = target as FantomeTweenHeight;

			if (!tw.transform == null) {
				EditorGUILayout.HelpBox("Cannot cache the target object. Please check again", MessageType.Error);
				tw.enabled = false;
				return;
			}

			GUI.changed = false;

			float from = EditorGUILayout.FloatField("From", tw.from);
			float to = EditorGUILayout.FloatField("To", tw.to);

			if (GUI.changed) {
				FantomeEditorScriptor.RegisterUndo("Tween Change", tw);
				tw.from = from;
				tw.to = to;
				FantomeEditorScriptor.SetDirty(tw);
			}


			base.OnInspectorGUI();
		}
	}
}