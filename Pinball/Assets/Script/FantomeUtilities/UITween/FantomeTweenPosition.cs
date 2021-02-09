namespace FantomePI {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("Fantome/Tween/TweenPosition")]
    public class FantomeTweenPosition : FantomeTweener {
        private Transform _t;

        private Transform t {
            get {
                if (_t == null) _t = this.transform;
                return _t;
            }
        }

        public Vector3 from;
        public Vector3 to;

        protected override void OnUpdateValue(float amount, bool isFinished) {
            t.localPosition = from * (1 - amount) + to * amount;

            if (isFinished) t.localPosition = base.forward ? to : from;
        }

        [ContextMenu("Set 'From' to current value")]
        public override void SetFromToCurrentValue() {
            from = t.localPosition;
        }

        [ContextMenu("Set 'To' to current value")]
        public override void SetToToCurrentValue() {
            to = t.localPosition;
        }

        public override bool ShouldPlay(bool forward) {
            return forward ? !t.localPosition.Equals(to) : !t.localPosition.Equals(from);
        }
    }
}