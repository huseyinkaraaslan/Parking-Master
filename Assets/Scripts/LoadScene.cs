using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public static LoadScene Instance;

    public GameObject loadScreen;
    public Slider loadingBar;

    private void Start()
    {
        Instance = this;
        loadScreen.SetActive(false);
    }

    public void LoadNextScene(string sceneName)
    {
        StartCoroutine(StartLoading(sceneName));
    }

    IEnumerator StartLoading(string sceneName)
    {
        loadingBar.value = 0;
        loadScreen.SetActive(true);
        
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        float progressValue = 0;
        while (!async.isDone)
        {
            progressValue = Mathf.MoveTowards(progressValue, async.progress, Time.deltaTime);
            loadingBar.value = progressValue;
            if(progressValue >= .9f)
            {
                loadingBar.value = 1;
                async.allowSceneActivation = true;
            }
            yield return null;
        }

        loadScreen.SetActive(false);
    }
}
