using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//机关管理
public class Organ : MonoBehaviour
{
    public GameObject player;

    private void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player" && collision.gameObject.GetComponent<PlayerGyro>().isSnake == true)
        {
            this.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && collision.gameObject.GetComponent<PlayerGyro>().isSnake == false)
        {
            this.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
}
