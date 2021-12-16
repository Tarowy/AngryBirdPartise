using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageCreator : MonoBehaviour
{
    public GameObject stagePrefab;
    public Sprite[] bgSprirtes;
    public CreateLevelSelect levelSelect;

    private int _stageStars;
    private int _totalStars;

    private void Awake()
    {
        // PlayerPrefs.DeleteAll();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            _stageStars = 0;
            var stage = Instantiate(stagePrefab, transform.position, Quaternion.identity,transform);
            //替换背景、名称以及解锁所需要的星星数量的文本
            stage.name = "Stage" + (i + 1);
            stage.GetComponent<Image>().sprite = bgSprirtes[i];
            stage.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = i*(6+i) + "";

            if (i == 4)
            {
                stage.transform.GetChild(0).gameObject.SetActive(false);
                stage.transform.GetChild(1).gameObject.SetActive(false);
                continue;
            }
            
            //如果星星总数大于这一关所需要的总数就解锁，否则上锁
            if (_totalStars < i*(6+i) || i>=3)
            {
                continue;
            }
            
            //计算这一关的星星总数
            for (int j = 1; j <= 10; j++)
            {
                _stageStars+= PlayerPrefs.GetInt(stage.name + "-" + "Level" + j,0);
            }
            
            //解锁
            stage.transform.GetChild(0).gameObject.SetActive(false);
            stage.transform.GetChild(1).gameObject.SetActive(true);

            _totalStars += _stageStars;
            stage.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = _stageStars + "/30";

            stage.GetComponent<Button>().enabled = true;
            stage.GetComponent<Button>().onClick.AddListener(delegate { levelSelect.EnterLevelChooser(stage.name); });
        }
    }

    public void Return()
    {
        SceneManager.LoadScene(0);
    }
}
