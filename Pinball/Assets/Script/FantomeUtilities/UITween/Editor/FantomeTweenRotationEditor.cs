namespace FantomePI {
    using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(FantomeTweenRotation))]
	public class FantomeTweenRotationEditor : FantomeTweenerEditor {
		public override void OnInspectorGUI() {
			GUILayout.Space(6f);
			EditorGUIUtility.labelWidth = 120f;

			FantomeTweenRotation tw = target as FantomeTweenRotation;

			GUI.changed = false;

			Vector3 from = EditorGUILayout.Vector3Field("From", tw.from);
			Vector3 to = EditorGUILayout.Vector3Field("To", tw.to);
			bool useQ = EditorGUILayout.Toggle("Use Quaternion", tw.useQuaternion);

			if (GUI.changed) {
				FantomeEditorScriptor.RegisterUndo("Tween Change", tw);
				tw.from = from;
				tw.to = to;
				tw.useQuaternion = useQ;
				FantomeEditorScriptor.SetDirty(tw);
			}

			base.OnInspectorGUI();
		}
	}
}