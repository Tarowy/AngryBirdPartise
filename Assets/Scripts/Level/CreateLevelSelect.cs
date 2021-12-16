using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateLevelSelect : MonoBehaviour
{
    public GameObject level;
    public GameObject stage;
    public Sprite[] starSprite;
    public Sprite lockSprite;
    public Sprite[] bgSprite;

    private bool _lastIsPassed = true;

    public void EnterLevelChooser(string stageName)
    {
        PlayerPrefs.SetString("nowStage",stageName);
        //根据场景切换关卡图标的背景
        var index = int.Parse(stageName.Remove(0, 5));
        if (index > bgSprite.Length)
        {
            index = bgSprite.Length - 1;
        }

        level.GetComponent<Image>().sprite = bgSprite[index-1];
        stage.SetActive(false);
        gameObject.SetActive(true);
        
        for (int i = 1; i <= 10; i++)
        {
            var instantiate = Instantiate(level, transform.position, Quaternion.identity, transform);
            instantiate.name = "Level" + i;
            
            if (_lastIsPassed)
            {
                var starNum = PlayerPrefs.GetInt(stageName+"-"+instantiate.name,0);
                // Debug.Log(stageName+"-"+instantiate.name+"得分为: "+starNum);
                //这一关没有分数说明未通过，无法解锁下一关
                if (starNum == 0 || i >= 4)
                {
                    _lastIsPassed = false;
                }
                //如果当前是第一关或者上一关得到了分数就能解锁这一关
                instantiate.transform.GetChild(0).GetComponent<Text>().text = i.ToString();
                instantiate.transform.GetChild(1).GetComponent<Image>().sprite = starSprite[starNum];
                instantiate.GetComponent<Button>().onClick.AddListener(delegate { this.LoadScene(instantiate.name); });
                continue;
            }
            
            //否则这一关将会被锁上
            instantiate.GetComponent<Button>().enabled = false;
            instantiate.transform.GetChild(0).gameObject.SetActive(false);
            instantiate.transform.GetChild(1).gameObject.SetActive(false);
            instantiate.GetComponent<Image>().sprite = lockSprite;
        }
    }
    
    public void LoadScene(String levelName)
    {
        PlayerPrefs.SetString("nowLevel",levelName);
        SceneManager.LoadScene("02-Game");
    }
}
