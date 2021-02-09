namespace FantomePI {

    using UnityEngine;

    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("Fantome/Tween/TweenHeight")]
    public class FantomeTweenHeight : FantomeTweener {
        private RectTransform _rt;
        private RectTransform rt {
            get {
                if (!_rt) _rt = this.GetComponent<RectTransform>();
                return _rt;
            }
        }

        public float from;
        public float to;

        protected override void OnUpdateValue(float amount, bool isFinished) {
            rt.SetHeight(from * (1 - amount) + to * amount);

            if (isFinished) rt.SetHeight(base.forward ? to : from);
        }

        [ContextMenu("Set 'From' to current value")]
        public override void SetFromToCurrentValue() {
            from = rt.sizeDelta.y;
        }

        [ContextMenu("Set 'To' to current value")]
        public override void SetToToCurrentValue() {
            to = rt.sizeDelta.y;
        }

        public override bool ShouldPlay(bool forward) {
            return forward ? !rt.sizeDelta.y.Equals(to) : !rt.sizeDelta.y.Equals(from);
        }
    }

}