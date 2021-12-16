using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    public Sprite originalSprite;
    public Sprite hurtSprite;

    public int healthValue = 2;

    public GameObject boomEffect;
    
    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
