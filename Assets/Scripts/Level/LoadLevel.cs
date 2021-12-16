using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    public GameObject[] background;
    public Sprite[] bgSprite;
    
    public GameObject[] ground;
    public Sprite[] groundSprite;

    public GameObject[] grass;
    public Sprite[] grassSprite;

    private void Awake()
    {
        var stage = PlayerPrefs.GetString("nowStage", null);
        int index = int.Parse(stage.Remove(0, 5));
        foreach (var bg in background)
        {
            bg.GetComponent<SpriteRenderer>().sprite = bgSprite[index - 1];
        }
        foreach (var g in ground)
        {
            g.GetComponent<SpriteRenderer>().sprite = groundSprite[index - 1];
        }
        foreach (var gr in grass)
        {
            if (index == 1 || index == 2)
            {
                gr.GetComponent<SpriteRenderer>().sprite = grassSprite[index - 1];
                continue;
            }
            gr.SetActive(false);
        }
        
        Instantiate(Resources.Load(
            stage + "/" +
            PlayerPrefs.GetString("nowLevel", null), typeof(GameObject))
        );
    }
}
