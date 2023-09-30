namespace IL3DN
{
    using UnityEngine;
    /// <summary>
    /// Override player sound when walking in different environments
    /// Attach this to a trigger
    /// </summary>
    public class IL3DN_ChangeWalkingSound : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _footStepsOverride;
        [SerializeField] private AudioClip _jumpSound;
        [SerializeField] private AudioClip _landSound;

        public AudioClip[] FootStepsOverride => _footStepsOverride;
        public AudioClip JumpSound => _jumpSound;
        public AudioClip LandSound => _landSound;
    }
}
