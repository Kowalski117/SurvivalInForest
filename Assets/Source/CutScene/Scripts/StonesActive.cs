using UnityEngine;

public class StonesActive : MonoBehaviour
{
    [SerializeField] private GameObject _stones;

    private bool _isPlaying = false;

    public void SetActiveStones()
    {
        _stones.SetActive(true);
        _isPlaying = true;
    }

    private void OnApplicationFocus(bool focus)
    {
        if(_isPlaying)
            _stones.SetActive(focus);
    }
}
