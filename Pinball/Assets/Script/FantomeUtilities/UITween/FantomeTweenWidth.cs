namespace FantomePI {

    using UnityEngine;

    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("Fantome/Tween/TweenWidth")]
    public class FantomeTweenWidth : FantomeTweener {
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
            rt.SetWidth(from * (1 - amount) + to * amount);

            if (isFinished) rt.SetWidth(base.forward ? to : from);
        }

        [ContextMenu("Set 'From' to current value")]
        public override void SetFromToCurrentValue() {
            from = rt.sizeDelta.x;
        }

        [ContextMenu("Set 'To' to current value")]
        public override void SetToToCurrentValue() {
            to = rt.sizeDelta.x;
        }

        public override bool ShouldPlay(bool forward) {
            return forward ? !rt.sizeDelta.x.Equals(to) : !rt.sizeDelta.x.Equals(from);
        }
    }

}