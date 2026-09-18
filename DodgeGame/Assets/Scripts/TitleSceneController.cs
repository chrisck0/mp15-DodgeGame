using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneController : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    private void OnEnable() => BindButtonEvents();
    private void OnDisable() => UnbindButtonEvents();

    private void BindButtonEvents()
    {
        _startButton.onClick.AddListener(LoadGameScene);
    }

    private void UnbindButtonEvents()
    {
        _startButton.onClick.RemoveListener(LoadGameScene);
    }

    public void LoadGameScene()
    {
        StartCoroutine(LoadGameSceneRoutine());
    }

    private IEnumerator LoadGameSceneRoutine()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);
        if (!asyncOperation.isDone)
        {
            Debug.Log($"Loading GameScene... {asyncOperation.progress * 100}%");
            yield return null;
        }
    }
}
