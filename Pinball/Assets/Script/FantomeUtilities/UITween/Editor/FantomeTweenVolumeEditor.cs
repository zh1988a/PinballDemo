namespace FantomePI {
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(FantomeTweenVolume))]
    public class FantomeTweenVolumeEditor : FantomeTweenerEditor {
		public override void OnInspectorGUI() {
			GUILayout.Space(6f);
			EditorGUIUtility.labelWidth = 120f;

			FantomeTweenVolume tw = target as FantomeTweenVolume;

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