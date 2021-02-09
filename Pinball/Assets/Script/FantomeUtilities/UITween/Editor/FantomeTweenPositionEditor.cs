namespace FantomePI {
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(FantomeTweenPosition))]
    public class FantomeTweenPositionEditor : FantomeTweenerEditor {
        public override void OnInspectorGUI() {
			GUILayout.Space(6f);
			EditorGUIUtility.labelWidth = 120f;

			FantomeTweenPosition tw = target as FantomeTweenPosition;
			GUI.changed = false;

			Vector3 from = EditorGUILayout.Vector3Field("From", tw.from);
			Vector3 to = EditorGUILayout.Vector3Field("To", tw.to);

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