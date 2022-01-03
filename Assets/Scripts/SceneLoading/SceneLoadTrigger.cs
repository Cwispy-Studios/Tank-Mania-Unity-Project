using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CwispyStudios.TankMania.SceneLoading
{
    public class SceneLoadTrigger : MonoBehaviour
    {
        public void LoadScene(string targetScene)
        {
            if (targetScene != "")
            {
                LoadingData.sceneToLoad = targetScene; 
            }
            
            SceneManager.LoadScene("LoadingScene");
        }
    }
}

