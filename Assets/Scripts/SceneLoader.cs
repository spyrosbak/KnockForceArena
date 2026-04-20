using UnityEngine;
using PurrNet;
using PurrNet.Modules;
using UnityEngine.SceneManagement;

public class SceneLoader : NetworkBehaviour
{
    [PurrScene] public string sceneToLoad;

    public void ChangeScene()
    {
        PurrSceneSettings settings = new()
        {
            isPublic = true,
            mode = LoadSceneMode.Single
        };

        networkManager.sceneModule.LoadSceneAsync(sceneToLoad, settings);
    }

    public void NextScence()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}