namespace FantomePI {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    [AddComponentMenu("Fantome/Tween/TweenScale")]
    public class FantomeTweenScale : FantomeTweener {
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
            t.localScale = from * (1 - amount) + to * amount;
        }

        [ContextMenu("Set 'From' to current value")]
        public override void SetFromToCurrentValue() {
            from = t.localScale;
        }

        [ContextMenu("Set 'To' to current value")]
        public override void SetToToCurrentValue() {
            to = t.localScale;
        }

        public override bool ShouldPlay(bool forward) {
            return forward ? !t.localScale.Equals(to) : !t.localScale.Equals(from);
        }
    }
}