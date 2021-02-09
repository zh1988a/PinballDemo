namespace FantomePI {
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    [AddComponentMenu("Fantome/Tween/TweenVolume")]
    public class FantomeTweenVolume : FantomeTweener {
        [Range(0f, 1f)] public float from = 1f;
        [Range(0f, 1f)] public float to = 1f;

        private AudioSource _audioSource;

        private AudioSource audioSource {
            get {
                if (_audioSource == null) _audioSource = this.GetComponent<AudioSource>();
                return _audioSource;
            }
        }

        protected override void OnUpdateValue(float amount, bool isFinished) {
            audioSource.volume = audioSource == null ? 0f : (from * (1 - amount) + to * amount);
            audioSource.enabled = audioSource?.volume > 0.01f;
        }

        [ContextMenu("Set 'From' to current value")]
        public override void SetFromToCurrentValue() {
            from = audioSource.volume;
        }

        [ContextMenu("Set 'To' to current value")]
        public override void SetToToCurrentValue() {
            to = audioSource.volume;
        }

        public override bool ShouldPlay(bool forward) {
            return forward ? !audioSource.volume.Equals(to) : audioSource.volume.Equals(from);
        }
    }
}