using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentLevelLoader : MonoBehaviour
{
    private void Start()
    {
        var currentLevel = SceneManager.GetActiveScene().buildIndex;
        var savedLevel = SaveDataBase.GetCurrentLevelIndex();

        if (currentLevel != savedLevel)
            SceneManager.LoadScene(savedLevel);
    }
}
