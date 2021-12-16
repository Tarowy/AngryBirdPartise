using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBird : Bird
{
    private Animator _animator;
    protected override void Awake()
    {
        _animator = GetComponent<Animator>();
        base.Awake();
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        if (isFlying)
        {
            _animator.enabled = false;
        }
        base.OnCollisionEnter2D(other);
    }

    public override void Skill()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _animator.SetBool("useSkill",true);
            var velocity = GetComponent<Rigidbody2D>().velocity;
            velocity.x *= -1;
            GetComponent<Rigidbody2D>().velocity = velocity;
        }
    }
}
