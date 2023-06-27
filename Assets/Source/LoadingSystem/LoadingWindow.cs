using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingWindow : MonoBehaviour
{
    [SerializeField] private Transform _loadingPanel;
    [SerializeField] private Image _loadingBar;

    private static LoadingWindow _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static LoadingWindow Instance
    {
        get { return _instance; }
    }

    public void ShowLoadingWindow(float value)
    {
        StartCoroutine(LoadingRoutine(value));
    }

    private IEnumerator LoadingRoutine(float value)
    {
        _loadingPanel.gameObject.SetActive(true);

        float elapsedTime = 0f;
        float duration = value;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;
            _loadingBar.fillAmount = progress;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _loadingBar.fillAmount = 1f;

        yield return new WaitForSeconds(0.5f);

        _loadingPanel.gameObject.SetActive(false);
    }
}
