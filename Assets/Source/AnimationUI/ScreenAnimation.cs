using UnityEngine;

public class ScreenAnimation : MonoBehaviour
{
    [SerializeField] private AnimationUI[] _animationUIs;

    public void Open()
    {
        foreach (var animation in _animationUIs)
        {
            animation.OpenAnimation();
        }
    }

    public void Close()
    {
        foreach (var animation in _animationUIs)
        {
            animation.CloseAnimation();
        }
    }
}
