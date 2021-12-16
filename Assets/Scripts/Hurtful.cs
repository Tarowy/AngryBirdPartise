using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtful : Destructible
{
    public float deadSpeed = 10f;
    public float hurtSpeed = 4f;

    public GameObject scoreEffect;
    private GameObject _lastGameObject;

    public AudioClip hurtClip;
    public AudioClip deadClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        var tagName = other.gameObject.tag;
        //一只鸟只能对同一只猪产生一次伤害
        if (tagName.Equals("Player"))
        {
            if (_lastGameObject != null && other.gameObject.name.Equals(_lastGameObject.name))
            {
                Debug.Log("不判定");
                return;
            }
            _lastGameObject = other.gameObject;
            Debug.Log("last: "+_lastGameObject.name);
        }
        //可破坏物体受到的相对碰撞速度大于死亡速度则直接死亡
        if (other.relativeVelocity.magnitude > deadSpeed)
        {
            Die();
            AudioSource.PlayClipAtPoint(deadClip,transform.position);
            return;
        }
        //否则生命值-1
        if (other.relativeVelocity.magnitude > hurtSpeed)
        {
            //防止鸟撞击猪导致猪与地的相对速度过高造成死亡
            if (tagName.Equals("Ground") && Vector3.Distance(other.gameObject.transform.position,gameObject.transform.position) > 1 )
            {
                Debug.Log("禁用伤害");
                return;
            }
            healthValue -= 1;
            AudioSource.PlayClipAtPoint(hurtClip,transform.position);
            Debug.Log(other.gameObject.tag+"造成了伤害");
            spriteRenderer.sprite = hurtSprite;
            if (healthValue <= 0)
            {
                Die();
            }
        }
    }

    public virtual void Die()
    {
        Instantiate(boomEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        var score = Instantiate(scoreEffect, transform.position+new Vector3(0,0.65f,0), Quaternion.identity);
        GameManager.Instance.AddScore(scoreEffect.GetComponent<ScoreValue>().value);
        Destroy(score,1.5f);
    }
}
