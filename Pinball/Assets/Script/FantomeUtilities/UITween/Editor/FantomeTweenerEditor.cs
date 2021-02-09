
namespace FantomePI {
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(FantomeTweener), true)]
    public class FantomeTweenerEditor : Editor {

        private bool eventRegistered = false;
        private FantomeTweener tw;

        private bool toggled = true;

        private bool playF = false;
        private bool playB = false;

        private double editorDeltaChecker = 0f;
        private double minDelta = 0.0166f;

        public override void OnInspectorGUI() {
            tw = target as FantomeTweener;

            EditorGUIUtility.labelWidth = 130f;

            toggled = FantomeEditorScriptor.DrawFoldHeader(toggled, "Tweener Properties");
            if (toggled) {
                FantomeEditorScriptor.BeginContentBox();

                GUI.changed = false;

                FantomeTweener.PlayStyle st = (FantomeTweener.PlayStyle)EditorGUILayout.EnumPopup("PlayStyle", tw.playStyle);

                AnimationCurve curve = EditorGUILayout.CurveField("Animation Curve", tw.curve, GUILayout.Width(190f), GUILayout.Height(62f));

                GUILayout.BeginHorizontal();
                float duration = EditorGUILayout.FloatField("Duration", tw.duration, GUILayout.Width(190f));
                GUILayout.Label("seconds");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                float delay = EditorGUILayout.FloatField("Start Delay", tw.delay, GUILayout.Width(190f));
                GUILayout.Label("seconds");
                GUILayout.EndHorizontal();

                bool ignoreTimeScale = EditorGUILayout.Toggle("Ignore Time Scale", tw.ignoreTimeScale);
                bool fixedUpdate = EditorGUILayout.Toggle("Use Fixed Update", tw.useFixedUpdate);
                bool playOnEnable = EditorGUILayout.Toggle("Play On Object Enable", tw.playOnObjEnable);
                bool onOff = EditorGUILayout.Toggle("Disable Object On Reverse", tw.onOffOnReverseEnd);

                if (GUI.changed) {
                    FantomeEditorScriptor.RegisterUndo("Tween Change", tw);
                    tw.playStyle = st;
                    tw.duration = duration;
                    tw.delay = delay;
                    tw.ignoreTimeScale = ignoreTimeScale;
                    tw.useFixedUpdate = fixedUpdate;
                    tw.playOnObjEnable = playOnEnable;
                    tw.onOffOnReverseEnd = onOff;
                    tw.curve = curve;
                    tw.ResetTween();
                    FantomeEditorScriptor.SetDirty(tw);
                }

                FantomeEditorScriptor.EndContentBox();
            }

            GUILayout.Space(10f);

            EditorGUILayout.BeginHorizontal();

            playB = GUILayout.Toggle(playB, "◀", GUI.skin.button);
            bool stop = GUILayout.Button("■");
            playF = GUILayout.Toggle(playF, "▶", GUI.skin.button);

            if (playF) {
                if (tw.started && !tw.forward && !tw.playStyle.Equals(FantomeTweener.PlayStyle.PingPong)) {
                    playB = false;
                    tw.started = false;
                    UnregisterEditorUpdate();
                }
                if (!tw.started) {
                    tw.onFinish.AddListener(UnregisterEditorUpdate);
                    tw.PlayForward(true);
                }
                RegisterEditorUpdate();
            }

            if (playB) {
                if (tw.started && tw.forward && !tw.playStyle.Equals(FantomeTweener.PlayStyle.PingPong)) {
                    playF = false;
                    tw.started = false;
                    UnregisterEditorUpdate();
                }
                if (!tw.started) {
                    tw.onFinish.AddListener(UnregisterEditorUpdate);
                    tw.PlayReverse(true);
                }
                RegisterEditorUpdate();
            }

            if (stop) {
                if (playF) playF = false;
                if (playB) playB = false;

                if (tw.started) {
                    tw.started = false;
                    UnregisterEditorUpdate();
                }
                tw.ResetTween();
                EditorUtility.SetDirty(tw);
            }

            EditorGUILayout.EndHorizontal();

            if (tw.playStyle == FantomeTweener.PlayStyle.Once) {
                GUILayout.Space(10f);

                SerializedProperty ev = serializedObject.FindProperty("onFinish");
                EditorGUILayout.PropertyField(ev);
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void EditorUpdate() {
            float delta = (float)(EditorApplication.timeSinceStartup - editorDeltaChecker);
            if (delta < minDelta) return;
            editorDeltaChecker = EditorApplication.timeSinceStartup;
            tw.TickTween(delta);
            EditorUtility.SetDirty(tw);
        }

        private void RegisterEditorUpdate() {
            if (!eventRegistered) {
                eventRegistered = true;
                editorDeltaChecker = EditorApplication.timeSinceStartup;
                EditorApplication.update += EditorUpdate;
            }
        }

        private void UnregisterEditorUpdate() {
            playF = false;
            playB = false;
            if (eventRegistered) {
                EditorApplication.update -= EditorUpdate;
                eventRegistered = false;
            }
            tw.onFinish.RemoveListener(UnregisterEditorUpdate);
        }

    }
}
