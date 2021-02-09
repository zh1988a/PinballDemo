namespace FantomePI {
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("Fantome/Tween/TweenAlpha")]
    public class FantomeTweenAlpha : FantomeTweener {

        public float from;
        public float to;

        private CanvasGroup cv;
        private Graphic gr;
        private SpriteRenderer sr;

        protected override void OnUpdateValue(float amount, bool isFinished) {
            value = Mathf.Lerp(from, to, amount);
            if (isFinished) value = forward ? to : from;
        }

        private bool _cached = false;

        public bool cached {
            get {
                if (!_cached) {
                    cv = this.GetComponent<CanvasGroup>();
                    if (cv != null) {
                        _cached = true;
                        return _cached;
                    }

                    gr = this.GetComponent<Graphic>();
                    if (gr != null) {
                        _cached = true;
                        return _cached;
                    }

                    sr = this.GetComponent<SpriteRenderer>();
                    if (sr != null) {
                        _cached = true;
                        return _cached;
                    }
                }
                return _cached;
            }
        }

        private float value {
            get {
                if (cached) {
                    if (cv != null) return cv.alpha;
                    if (gr != null) return gr.color.a;
                    if (sr != null) return sr.color.a;
                    return 0f;
                }
                return 0f;
            } set {
                if (!cached) return;
                if (cv != null) {
                    cv.alpha = value;
                } else if (gr != null) {
                    Color c_gr = gr.color;
                    c_gr.a = value;
                    gr.color = c_gr;
                } else if (sr != null) {
                    Color c_sr = sr.color;
                    c_sr.a = value;
                    sr.color = c_sr;
                }
            }
        }

        [ContextMenu("Set 'From' to current value")]
        public override void SetFromToCurrentValue() {
            from = value;
        }

        [ContextMenu("Set 'To' to current value")]
        public override void SetToToCurrentValue() {
            to = value;
        }

        [ContextMenu("Check Target Again")]
        public override void CheckCache() {
            _cached = false;
        }

        public override bool ShouldPlay(bool forward) {
            return forward ? !value.Equals(to) : !value.Equals(from);
        }
    }
}