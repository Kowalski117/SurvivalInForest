using System.Collections;
using UnityEngine;

public class StartAnimationUI : MonoBehaviour
{
    [SerializeField] AnimationUI _animationUI;
    [SerializeField] private float _delay;

    private WaitForSeconds _waitForSeconds;

    private void Awake()
    {
        _animationUI.CloseAnimation();
    }


    private IEnumerator Start()
    {
        yield return _waitForSeconds = new WaitForSeconds(_delay);
        _animationUI.OpenAnimation();
    }
}
