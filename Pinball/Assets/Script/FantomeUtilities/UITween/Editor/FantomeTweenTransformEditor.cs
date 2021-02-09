namespace FantomePI {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(FantomeTweenTransform))]
    public class FantomeTweenTransformEditor : FantomeTweenerEditor {

        public override void OnInspectorGUI() {
            GUILayout.Space(6f);
            EditorGUIUtility.labelWidth = 120f;

            FantomeTweenTransform tw = target as FantomeTweenTransform;
            GUI.changed = false;

            Transform from = EditorGUILayout.ObjectField("From", tw.from, typeof(Transform), true) as Transform;
            Transform to = EditorGUILayout.ObjectField("To", tw.to, typeof(Transform), true) as Transform;

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