namespace FantomePI {
    using UnityEngine;

    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("Fantome/Tween/TweenGraphicSize")]
    public class FantomeTweenGraphicSize : FantomeTweener {

        private RectTransform _rt;
        private RectTransform rt {
            get {
                if (!_rt) _rt = this.GetComponent<RectTransform>();
                return _rt;
            }
        }

        public Vector2 from;
        public Vector2 to;

        protected override void OnUpdateValue(float amount, bool isFinished) {
            rt.sizeDelta = from * (1 - amount) + to * amount;

            if (isFinished) rt.sizeDelta = base.forward ? to : from;
        }

        [ContextMenu("Set 'From' to current value")]
        public override void SetFromToCurrentValue() {
            from = rt.sizeDelta;
        }

        [ContextMenu("Set 'To' to current value")]
        public override void SetToToCurrentValue() {
            to = rt.sizeDelta;
        }

        public override bool ShouldPlay(bool forward) {
            return forward ? !rt.sizeDelta.Equals(to) : !rt.sizeDelta.Equals(from);
        }
    }
}