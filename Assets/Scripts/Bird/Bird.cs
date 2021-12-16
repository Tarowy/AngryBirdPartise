using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bird : Destructible
{
    public bool isFlying;
    public AudioClip birdCollision;
    public GameObject whatAreYouDo;

    protected virtual void Update()
    {
        if (isFlying && Time.timeScale != 0)
        {
            Skill();
        }
    }
    
    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        AudioSource.PlayClipAtPoint(birdCollision,transform.position);
        isFlying = false;
        spriteRenderer.sprite = hurtSprite;
    }

    public void CountTimeToDie()
    {
        Invoke("Die",5f);
    } 

    public virtual void Die()
    {
        GameManager.Instance.ChangeBird(gameObject);
        Instantiate(boomEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public virtual void Skill()
    {
        Debug.Log("不同鸟的技能");
    }


    public void ClickToChangeBird()
    {
        Debug.Log("更换到:"+gameObject.name);
    }
}
