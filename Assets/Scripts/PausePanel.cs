using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public GameObject pauseBtn;
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    //按钮事件
    
    public void StartTime()
    {
        Time.timeScale = 1;
        _animator.SetBool("isPausing",false);
    }
    
    public void PauseBtn()
    {
        pauseBtn.SetActive(false);
        gameObject.SetActive(true);
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("02-Game");
    }

    public void BackToLevel()
    {
        SceneManager.LoadScene("01-Level");
        // GameObject.Find("Grid").GetComponent<CreateLevelSelect>().EnterLevelChooser(PlayerPrefs.GetString("nowStage"));
    }

    public void NextLevel()
    {
        var nowlevel = PlayerPrefs.GetString("nowLevel"); //当前level
        int index = int.Parse(nowlevel.Remove(0, 5)); //当前level+1
        if (index + 1 >= 10)
        {
            return;
        }
        PlayerPrefs.SetString("nowLevel", "Level" + (index + 1));
        SceneManager.LoadScene(2);
    }
    
    //动画事件
    
    public void PauseTime()
    {
        Time.timeScale = 0;
    }
    
    public void HidePanel()
    {
        gameObject.SetActive(false);
        pauseBtn.SetActive(true);
    }
}
