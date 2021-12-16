using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Pig : Hurtful
{
    public override void Die()
    {
        GameManager.Instance.PigDie(gameObject);
        base.Die();
    }
}
