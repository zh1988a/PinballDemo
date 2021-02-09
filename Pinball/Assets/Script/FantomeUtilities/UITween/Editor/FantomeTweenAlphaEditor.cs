namespace FantomePI {
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(FantomeTweenAlpha))]
    public class FantomeTweenAlphaEditor : FantomeTweenerEditor {
		public override void OnInspectorGUI() {
			GUILayout.Space(6f);
			EditorGUIUtility.labelWidth = 120f;

			FantomeTweenAlpha tw = target as FantomeTweenAlpha;

			if (!tw.cached) {
				EditorGUILayout.HelpBox("Tween Alpha Cannot cache the target object. Please check again", MessageType.Error);
				tw.enabled = false;
				return;
			}

			GUI.changed = false;

			float from = EditorGUILayout.Slider("From", tw.from, 0f, 1f);
			float to = EditorGUILayout.Slider("To", tw.to, 0f, 1f);

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