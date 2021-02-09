namespace FantomePI {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [AddComponentMenu("Fantome/Tween/TweenRotation")]
    public class FantomeTweenRotation : FantomeTweener {
        public Vector3 from;
        public Vector3 to;
        public bool useQuaternion = false;

        private Transform _tr = null;

        private Transform tr {
            get {
                if (_tr == null) _tr = this.transform;
                return _tr;
            }
        }

        protected override void OnUpdateValue(float amount, bool isFinished) {
            tr.localRotation = useQuaternion ? Quaternion.Slerp(Quaternion.Euler(from), Quaternion.Euler(to), amount) :
            Quaternion.Euler(new Vector3(
            Mathf.Lerp(from.x, to.x, amount),
            Mathf.Lerp(from.y, to.y, amount),
            Mathf.Lerp(from.z, to.z, amount)));

            if (isFinished) tr.localRotation = forward ? Quaternion.Euler(to) : Quaternion.Euler(from);
        }

        [ContextMenu("Set 'From' to current value")]
        public override void SetFromToCurrentValue() { from = tr.localRotation.eulerAngles; }

        [ContextMenu("Set 'To' to current value")]
        public override void SetToToCurrentValue() { to = tr.localRotation.eulerAngles; }

        public override bool ShouldPlay(bool forward) {
            return forward ? !tr.localRotation.eulerAngles.Equals(to) : !tr.localRotation.eulerAngles.Equals(from);
        }
    }
}