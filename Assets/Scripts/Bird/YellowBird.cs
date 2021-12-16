using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowBird : Bird
{
    public override void Skill()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("jiasu");
            GetComponent<Rigidbody2D>().velocity *= 2;
            isFlying = false;
        }
    }
}
