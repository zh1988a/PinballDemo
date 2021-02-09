using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FantomeEditorScriptor {

    private static Color disabledButWhite = new Color(1, 1, 1, 2);

    public static void BeginContentBox() {
        EditorGUILayout.BeginHorizontal("TextArea", GUILayout.MinHeight(10f));
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space(2f);
    }

    public static void EndContentBox() {
        EditorGUILayout.Space(3f);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    public static bool DrawFoldHeader(bool expanded, string text) {
        text = string.Format("{0} <b><size=11>{1}</size></b>", expanded ? "\u25BC" : "\u25BA", text);

        GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
        GUILayout.BeginHorizontal();
        //GUI.changed = false;

        expanded = GUILayout.Toggle(expanded, text, "dragtab", GUILayout.MinWidth(20f));

        GUILayout.Space(2f);
        GUILayout.EndHorizontal();

        GUI.backgroundColor = Color.white;

        return expanded;
    }

    public static T ImmutableObjectField<T>(T obj, params GUILayoutOption[] options) where T : Object {
        return ImmutableObjectField(obj, false, options);
    }

    public static T ImmutableObjectField<T>(T obj, bool allowSceneObjects, params GUILayoutOption[] options) where T : Object {
        GUI.enabled = false;
        GUI.color = disabledButWhite;
        T res = EditorGUILayout.ObjectField(obj, obj.GetType(), allowSceneObjects, options) as T;
        GUI.color = Color.white;
        GUI.enabled = true;
        return res;
    }

    public static T ObjectField<T>(T obj, params GUILayoutOption[] options) where T : Object {
        return ObjectField(obj, false, options);
    }

    public static T ObjectField<T>(T obj, bool allowSceneObjects, params GUILayoutOption[] options) where T : Object {
        return EditorGUILayout.ObjectField(obj, typeof(T), allowSceneObjects, options) as T;
    }

    public static void RegisterUndo(UnityEngine.Object obj, string undoName) {
        UnityEditor.Undo.RecordObject(obj, undoName);
        SetDirty(obj);
    }

    public static void SetDirty(UnityEngine.Object obj, string undoName = "last change") {
        #if UNITY_EDITOR
        #if UNITY_2018_3_OR_NEWER
        if (obj) {
            UnityEditor.EditorUtility.SetDirty(obj);

            if (!UnityEditor.AssetDatabase.Contains(obj) && !Application.isPlaying) {
                if (obj is Component) {
                    var component = (Component)obj;
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(component.gameObject.scene);
                } else if (!(obj is UnityEditor.EditorWindow || obj is ScriptableObject)) {
                    UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
                }
            }
        }
        #else
		if (obj) UnityEditor.EditorUtility.SetDirty(obj);
        #endif
        #endif
    }

    public static void RegisterUndo(string name, Object obj) { if (obj != null) UnityEditor.Undo.RecordObject(obj, name); }

    public static void RegisterUndo(string name, params Object[] objects) { 
        if (objects != null && objects.Length > 0) UnityEditor.Undo.RecordObjects(objects, name); 
    }

    public static SerializedProperty DrawProperty(string label, SerializedObject serializedObject, string property, params GUILayoutOption[] options) {
        return DrawProperty(label, serializedObject, property, false, options);
    }

    public static SerializedProperty DrawProperty(string label, SerializedObject serializedObject, string property, bool padding, params GUILayoutOption[] options) {
        SerializedProperty sp = serializedObject.FindProperty(property);

        if (sp != null) {

            if (padding) EditorGUILayout.BeginHorizontal();

            if (label != null) EditorGUILayout.PropertyField(sp, new GUIContent(label), options);
            else EditorGUILayout.PropertyField(sp, options);

            if (padding) {
                GUILayout.Space(18f);
                EditorGUILayout.EndHorizontal();
            }
        } else {
            Debug.LogWarning("Unable to find property " + property);
        }

        return sp;
    }
}
