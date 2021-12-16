using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<GameObject> birdList = new List<GameObject>();
    public List<GameObject> pigList = new List<GameObject>();

    public DragControl dragControl;
    public int score;

    public GameObject winInterface;
    public GameObject loseInterface;
    public GameObject[] stars;

    private int _birdCountAll;
    private void Awake()
    {
        Time.timeScale = 1;
        Application.targetFrameRate = 144;
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        birdList = GameObject.FindGameObjectsWithTag("Player").ToList();
        _birdCountAll = birdList.Count;
        pigList = GameObject.FindGameObjectsWithTag("Pig").ToList();
        ChangeBird(null);
    }

    /// <summary>
    /// 移除已经弹射出去的鸟并将下一只鸟转移到弹弓上,并判断鸟是不是都死了
    /// </summary>
    /// <param name="bird"></param>
    public void ChangeBird(GameObject bird)
    {
        if (bird != null)
        {
            birdList.Remove(bird);
        }

        if (birdList.Count == 0 && pigList.Count != 0)
        {
            ShowInterface(loseInterface,0);
            Debug.Log("你输了");
        }

        for (int i = 0; i < birdList.Count; i++)
        {
            if (birdList[i] != null)
            {
                dragControl.InitComponet(birdList[i]);
                return;
            }
        }
    }

    /// <summary>
    /// 移除已死亡的猪并判断是否猪已全部阵亡
    /// </summary>
    /// <param name="pig"></param>
    public void PigDie(GameObject pig)
    {
        if (pig != null)
        {
            pigList.Remove(pig);
        }

        if (pigList.Count == 0)
        {
            var starNum = 2;
            if (_birdCountAll - (birdList.Count-1) <= (int)Math.Ceiling(_birdCountAll / 3f))
            {
                starNum = 3;
            }
            else if (birdList.Count == 1)
            {
                starNum = 1;
            }
            ShowInterface(winInterface, starNum);
            Debug.Log("你赢了");
        }
    }

    public void AddScore(int value)
    {
        this.score += value;
        Debug.Log("当前分数:"+score);
    }

    public void ShowInterface(GameObject @interface,int starNum)
    {
        @interface.SetActive(true);
        if (starNum > 0)
        {
            var levelName = PlayerPrefs.GetString("nowStage", null) + "-" +
                            PlayerPrefs.GetString("nowLevel", null);
            var levelStars = PlayerPrefs.GetInt(levelName);
            
            Debug.Log("GameManager:" + levelName+"  "+levelStars);
            if (levelStars < starNum)
            {
                PlayerPrefs.SetInt(levelName,starNum);
            }
            
            StartCoroutine(ShowStars(starNum));
        }
    }

    IEnumerator ShowStars(int starNum)
    {
        for (int i = 0; i < starNum; i++)
        {
            yield return new WaitForSeconds(0.5f);
            stars[i].SetActive(true);
        }
    }
}
