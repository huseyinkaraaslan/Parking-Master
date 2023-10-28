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
        loadScreen.SetActive(true);
        loadingBar.value = 0;
        
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

        float progressValue = 0;
        while (!async.isDone)
        {
            progressValue = Mathf.MoveTowards(progressValue, async.progress, Time.deltaTime);
            loadingBar.value = progressValue;

            if(progressValue >= .9f)
                loadingBar.value = 1;

            yield return null;
        }

        loadScreen.SetActive(false);
    }
}
