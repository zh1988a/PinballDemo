namespace FantomePI {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class FantomeGroupTweener : MonoBehaviour {

        [SerializeField]
        public List<TweenTarget> targets = new List<TweenTarget>();


        [Serializable]
        public struct TweenTarget {
            public FantomeTweener tween;
            public bool forward;
        }

        public void PlayForward(bool isForce=true) {
            foreach (var target in targets) {
                if (target.forward) {
                    target.tween.PlayForward(isForce);
                } else {
                    target.tween.PlayReverse(isForce);
                }
            }
        }

        public void PlayReverse(bool isForce = true) {
            foreach (var target in targets) {
                if (target.forward) {
                    target.tween.PlayReverse(isForce);
                } else {
                    target.tween.PlayForward(isForce);
                }
            }
        }

        public void PlayForwardIfShould(bool isForce = true) {
            foreach (var target in targets) {
                if (target.tween.ShouldPlay(target.forward)) {
                    if (target.forward) {
                        if (!target.tween.gameObject.activeInHierarchy) target.tween.SetValueToForwardEnd();
                        else target.tween.PlayForward(isForce);
                    } else {
                        if (!target.tween.gameObject.activeInHierarchy) target.tween.SetValueToReverseEnd();
                        else target.tween.PlayReverse(isForce);
                    }
                }
            }
        }

        public void PlayReverseIfShould(bool isForce = true) {
            foreach (var target in targets) {
                if (target.tween.ShouldPlay(!target.forward)) {
                    if (target.forward) {
                        if (!target.tween.gameObject.activeInHierarchy) target.tween.SetValueToReverseEnd();
                        else target.tween.PlayReverse(isForce);
                    } else {
                        if (!target.tween.gameObject.activeInHierarchy) target.tween.SetValueToForwardEnd();
                        else target.tween.PlayForward(isForce);
                    }
                }
            }
        }
    }
}