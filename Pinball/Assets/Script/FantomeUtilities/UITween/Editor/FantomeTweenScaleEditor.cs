namespace FantomePI {
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(FantomeTweenScale))]
    public class FantomeTweenScaleEditor : FantomeTweenerEditor {
        public override void OnInspectorGUI() {
			GUILayout.Space(6f);
			EditorGUIUtility.labelWidth = 120f;

			FantomeTweenScale tw = target as FantomeTweenScale;
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