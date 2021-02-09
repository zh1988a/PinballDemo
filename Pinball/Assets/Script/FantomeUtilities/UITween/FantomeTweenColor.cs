namespace FantomePI {
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("Fantome/Tween/TweenColor")]
    public class FantomeTweenColor : FantomeTweener {

        public Color from;
        public Color to;

        private bool _cached = false;
        private Graphic _graphic;
        private SpriteRenderer _sRenderer;
        private Material _mat;
        private Light _light;

        public Color value {
            get {
                if (!cached) {
                    //FTLog.LogErrorFormat("Tween Color on {0} cannot be played!", this.gameObject.name);
                    return Color.black;
                }
                if (_graphic != null) return _graphic.color;
                if (_sRenderer != null) return _sRenderer.color;
                if (_mat != null) return _mat.color;
                if (_light != null) return _light.color;
                return Color.black;
            } set {
                if (!cached) return;
                if (_graphic != null) _graphic.color = value;
                else if (_sRenderer != null) _sRenderer.color = value;
                else if (_mat != null) _mat.color = value;
                else if (_light != null) _light.color = value;
            }
        }


        protected override void OnUpdateValue(float amount, bool isFinished) {
            value = Color.Lerp(from, to, amount);
            if (isFinished) value = forward ? to : from;
        }

        public bool cached {
            get {
                if (_cached) return true;
               
                _graphic = this.GetComponent<Graphic>();
                if (_graphic != null) {
                    _cached = true;
                    return true;
                }

                _sRenderer = this.GetComponent<SpriteRenderer>();
                if (_sRenderer != null) {
                    _cached = true;
                    return true;
                }

                Renderer renderer = this.GetComponent<Renderer>();
                if (renderer != null) {
                    _mat = renderer.material;
                    _cached = true;
                    return true;
                }

                _light = this.GetComponent<Light>();
                if (_light) {
                    _cached = true;
                    return true;
                }

                return _cached;
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