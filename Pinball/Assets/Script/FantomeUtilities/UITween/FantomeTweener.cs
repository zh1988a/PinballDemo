namespace FantomePI {
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;

    public abstract class FantomeTweener : MonoBehaviour {
        public static FantomeTweener current;
        public PlayStyle playStyle = PlayStyle.Once;
        public AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
        public bool ignoreTimeScale = true;
        public float delay = 0f;
        public float duration = 1f;
        private float factorPerSec = 0f;
        private float delayElapsed = 0f;
        public bool useFixedUpdate = false;
        public bool forward = false;
        public bool started = false;
        public float factor = 0f;

        public bool playOnObjEnable = false;
        public bool onOffOnReverseEnd = false;

        public UnityEvent onFinish = new UnityEvent();

        private Coroutine tweenEnumerator = null;

        private void Reset() {
            CheckCache();
            SetFromToCurrentValue();
            SetToToCurrentValue();
        }

        protected void OnEnable() {
            if (playStyle != PlayStyle.Once || playOnObjEnable) {
                forward = true;
                ResetTween();
                Play();
            }
        }

        protected virtual void OnDisable() {
            StopAllCoroutines();
        }

        private IEnumerator PlayTween() {
            while (started) {
                if (useFixedUpdate) {
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                } else {
                    yield return YieldInstructionCache.WaitForEndOfFrame;
                }
                TickTween(ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime);
            }
        }

        public void TickTween(float delta) {
            if (!started) return;

            if (delayElapsed < delay) {
                delayElapsed += delta;
                return;
            }

            factor += factorPerSec * delta * (forward ? 1 : -1);

            bool finished = false;
            
            switch (playStyle) {
                case PlayStyle.Once:
                    finished = forward ? factor >= 1f : factor <= 0f;
                    if (finished) factor = forward ? 1f : 0f;
                    break;
                case PlayStyle.Loop:
                    if (forward && factor >= 1f) factor = 0f;
                    else if (!forward && factor <= 0f) factor = 1f;
                    break;
                case PlayStyle.PingPong:
                    if (forward && factor >= 1f) {
                        forward = false;
                    } else if (!forward && factor <= 0f) {
                        forward = true;
                    }
                    break;
            }

            OnUpdateValue(curve.Evaluate(Mathf.Clamp01(factor)), finished);

            if (finished) {
                started = false;
                onFinish?.Invoke();
                if (onOffOnReverseEnd && !forward && factor <= 0f) this.gameObject.SetActive(false);
                //if (this is FantomeButton) return;
                if (Application.isPlaying && !playOnObjEnable) this.enabled = false;
            }
        }

        protected abstract void OnUpdateValue(float amount, bool isFinished);

        public enum PlayStyle : int {
            Once = 0,
            Loop,
            PingPong
        }

        #region PUBLIC METHODS

        public void ResetTween() {
            factor = forward ? 0f : 1f;
            started = false;
            delayElapsed = 0f;
            factorPerSec = 1 / duration;
            OnUpdateValue(curve.Evaluate(factor), false);
        }

        public void SetValueToForwardEnd() {
            forward = true;

            factor = 1f;
            started = false;
            delayElapsed = 0f;
            factorPerSec = 1 / duration;

            OnUpdateValue(curve.Evaluate(factor), true);
        }

        public void SetValueToReverseEnd() {
            forward = false;

            factor = 0f;
            started = false;
            delayElapsed = 0f;
            factorPerSec = 1 / duration;

            OnUpdateValue(curve.Evaluate(factor), true);
        }

        private void Play() {
            if (forward && !gameObject.activeSelf) gameObject.SetActive(true);
            if (!started) started = true;
            if (!this.enabled) this.enabled = true;
            if (!gameObject.activeInHierarchy) return;
            #if UNITY_EDITOR
            if (Application.isPlaying){
                if (tweenEnumerator != null) StopCoroutine(tweenEnumerator);
                tweenEnumerator = StartCoroutine(PlayTween());
            }
            #else
            StartCoroutine(PlayTween());
            #endif
        }

        public void PlayForward(bool force = false) {
            forward = true;
            if (force) ResetTween();
            Play();
        }

        public void PlayReverse(bool force = false) {
            forward = false;
            //if (factor.Equals(0)) factor = 1;
            if (force) ResetTween();
            Play();
        }

        public void ResetAndPlayForward() {
            PlayForward(true);
        }

        public void ResetAndPlayReverse() {
            PlayReverse(true);
        }

        public void ReverseDirectionAndPlay(bool force = false) {
            forward = !forward;
            if (force) ResetTween();
            Play();
        }

        public void PlayForce(bool isForward) {
            forward = isForward;
            ResetTween();
            Play();
        }

        #endregion

        #region VIRTUAL METHODS

        public virtual void SetFromToCurrentValue() { }

        public virtual void SetToToCurrentValue() { }

        public virtual void CheckCache() { }

        public abstract bool ShouldPlay(bool forward);

        #endregion

    }

}