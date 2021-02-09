namespace FantomePI {
    using UnityEngine;
    using UnityEditor;

	[CustomEditor(typeof(FantomeTweenColor))]
    public class FantomeTweenColorEditor : FantomeTweenerEditor {
        public override void OnInspectorGUI() {
			GUILayout.Space(6f);
			EditorGUIUtility.labelWidth = 120f;

			FantomeTweenColor tw = target as FantomeTweenColor;

			if (!tw.cached) {
                EditorGUILayout.HelpBox("Tween Color Cannot cache the target object. Please check again", MessageType.Error);
				tw.enabled = false;
				return;
            }

			GUI.changed = false;

			Color from = EditorGUILayout.ColorField("From", tw.from);
			Color to = EditorGUILayout.ColorField("To", tw.to);

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