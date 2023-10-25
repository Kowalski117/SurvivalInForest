using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
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
}
