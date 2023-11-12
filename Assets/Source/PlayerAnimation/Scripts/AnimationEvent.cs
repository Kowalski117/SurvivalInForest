using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    [SerializeField] private PlayerAudioHandler _playerAudioHandler;

    private PlayerInteraction _playerInteraction;

    private void Awake()
    {
        _playerInteraction = GetComponentInParent<PlayerInteraction>();
    }

    public void InteractResourceEvent()
    {
        _playerInteraction.InteractResource();
    }

    public void HitEvent()
    {
        _playerInteraction.Hit();
    }

    public void HitInAir()
    {
        _playerAudioHandler.PlayHitInAirClip();
    }
}
