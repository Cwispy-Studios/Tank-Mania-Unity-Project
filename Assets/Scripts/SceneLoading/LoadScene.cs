using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CwispyStudios.TankMania.SceneLoading
{
    public class LoadScene : MonoBehaviour
    {
        public GameObject loadingScreen;
        public Slider loadingBar;
        public TMP_Text loadingText, sceneText;

        private AsyncOperation loadingOperation;

        private void Start()
        {
            sceneText.text = LoadingData.sceneToLoad;
            Invoke("StartLoad", 2f);
        }

        private void Update()
        {
            if (loadingOperation != null)
            {
                if (!loadingOperation.isDone)
                {
                    float progressValue = loadingOperation.progress / 0.9f;
                    loadingBar.value = Mathf.Clamp01(progressValue);
                    loadingText.text = Mathf.Round(progressValue * 100) + "%";
                } 
            }
        }

        private void StartLoad()
        {
            loadingOperation = SceneManager.LoadSceneAsync(LoadingData.sceneToLoad);
            loadingScreen.SetActive(true);
        }
    }
}
