using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAsync : MonoBehaviour
{
    public void Start()
    {
        Screen.SetResolution(960, 500, false);
        SceneManager.LoadSceneAsync(1);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
