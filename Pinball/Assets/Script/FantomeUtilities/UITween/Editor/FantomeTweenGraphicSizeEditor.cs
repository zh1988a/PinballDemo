namespace FantomePI {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

	[CustomEditor(typeof(FantomeTweenGraphicSize))]
    public class FantomeTweenGraphicSizeEditor : FantomeTweenerEditor {

        public override void OnInspectorGUI() {
			GUILayout.Space(6f);
			EditorGUIUtility.labelWidth = 120f;

			FantomeTweenGraphicSize tw = target as FantomeTweenGraphicSize;

			if (!tw.transform == null) {
				EditorGUILayout.HelpBox("Cannot cache the target object. Please check again", MessageType.Error);
				tw.enabled = false;
				return;
			}

			GUI.changed = false;

			Vector2 from = EditorGUILayout.Vector2Field("From", tw.from);
			Vector2 to = EditorGUILayout.Vector2Field("To", tw.to);

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