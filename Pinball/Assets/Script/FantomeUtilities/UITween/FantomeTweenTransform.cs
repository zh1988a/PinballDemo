namespace FantomePI {
    using UnityEngine;

    [AddComponentMenu("Fantome/Tween/TweenTransform")]
    public class FantomeTweenTransform : FantomeTweener {
        private Transform _t;
        private Transform t {
            get {
                if (_t == null) _t = this.transform;
                return _t;
            }
        }

        public Transform from;
        public Transform to;

        protected override void OnUpdateValue(float amount, bool isFinished) {
            t.localPosition = from.localPosition * (1 - amount) + to.localPosition * amount;
            t.localScale = from.localScale * (1 - amount) + to.localScale * amount;
            t.localRotation = Quaternion.Slerp(from.localRotation, to.localRotation, amount);

            if (isFinished) {
                if (base.forward) {
                    t.SetPositionAndRotation(to);
                    t.localScale = to.localScale;
                } else {
                    t.SetPositionAndRotation(from);
                    t.localScale = from.localScale;
                }
            }
        }

        public override bool ShouldPlay(bool forward) {
            if (forward) {
                return !t.localPosition.Equals(to.localPosition) || !t.localRotation.Equals(to.localRotation) || !t.localScale.Equals(to.localScale);
            } else {
                return !t.localPosition.Equals(from.localPosition) || !t.localRotation.Equals(from.localRotation) || !t.localScale.Equals(from.localScale);
            }
        }
    }
}