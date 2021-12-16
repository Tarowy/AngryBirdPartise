using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBird : Bird
{
    public float boomForce = 2f;
    public HashSet<GameObject> blockList = new HashSet<GameObject>();
    public GameObject bigBoom;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ground"))
        {
            blockList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Ground") && blockList.Contains(other.gameObject))
        {
            blockList.Remove(other.gameObject);
        }
    }

    public override void Skill()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<Rigidbody2D>().gravityScale = 0;
            spriteRenderer.enabled = false;
            GetComponent<TrailRenderer>().enabled = false;
            Instantiate(bigBoom, transform.position, Quaternion.identity);
            foreach (var block in blockList)
            {
                Debug.Log("block:"+block.name);
                var direct = (block.transform.position - transform.position).normalized;
                block.GetComponent<Rigidbody2D>().AddForce(new Vector2(direct.x,direct.y)*boomForce,ForceMode2D.Impulse);
            }
        }
    }

    public override void Die()
    {
        GameManager.Instance.ChangeBird(gameObject);
        Destroy(gameObject);
    }
}
