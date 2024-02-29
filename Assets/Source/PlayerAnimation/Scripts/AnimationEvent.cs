using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField] private PlayerAudioHandler _playerAudioHandler;

    private TargetInteractionHandler _playerInteraction;

    private void Awake()
    {
        _playerInteraction = GetComponentInParent<TargetInteractionHandler>();
    }

    public void InteractResource()
    {
        _playerInteraction.InteractResource();
    }

    public void Hit()
    {
        _playerInteraction.Hit();
    }

    public void HitInAir()
    {
        _playerAudioHandler.PlayHitInAirClip();
    }
}
