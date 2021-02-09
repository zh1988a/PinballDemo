namespace FantomePI {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(FantomeTweenWidth))]
	public class FantomeTweenWidthEditor : FantomeTweenerEditor {

		public override void OnInspectorGUI() {
			GUILayout.Space(6f);
			EditorGUIUtility.labelWidth = 120f;

			FantomeTweenWidth tw = target as FantomeTweenWidth;

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