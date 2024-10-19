using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;

//玩家控制
public class PlayerGyro : MonoBehaviour
{
    Rigidbody2D rb;
    float dirX, dirY;
    public float moveSpeed;

    public GameObject body, head;

    public Vector2 playerTF;

    //跟随煤球数
    float followCount;

    //或许标题名
    public GameObject duckRoom;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTF = transform.position;
    }

    [System.Obsolete]
    private void Update()
    {
        if (SystemInfo.supportsGyroscope)
        {
            dirX = Input.acceleration.x * moveSpeed;
            dirY = Input.acceleration.y * moveSpeed;
        }
        else
        {
        }

        if(transform.position.x <= duckRoom.transform.position.x) //开头标题隐藏
        {
            float b = LinearInterpolation(transform.position.x, playerTF.x, duckRoom.transform.position.x, 1, 0);
            duckRoom.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, b);
        }
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        if (rb.velocity.y < -20) //下
        {
            rb.velocity = new Vector2(dirX, dirY * 0.5f);
        }
        else //上
        {
            rb.velocity = new Vector2(dirX * 1.5f, dirY * 1.5f);
        }

        if (rb.velocity.x < -20) //左
        {
            transform.DOScaleX(-1f, 1f);

            body.transform.rotation = Quaternion.EulerRotation(0, 0, -Input.acceleration.y * 0.7f);
            head.transform.rotation = Quaternion.EulerRotation(0, 0, -Mathf.Clamp(Input.acceleration.y, -40f, 10f));
        }
        else //左
        {
            transform.DOScaleX(1f, 1f);

            body.transform.rotation = Quaternion.EulerRotation(0, 0, Input.acceleration.y * 0.7f);
            head.transform.rotation = Quaternion.EulerRotation(0, 0, Mathf.Clamp(Input.acceleration.y, -40f, 10f));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Circle") //道具球
        {
            collision.gameObject.SetActive(false);

            var obj = Instantiate(Resources.Load("Follow"), transform.position, Quaternion.Euler(0, 0, 0));
            obj.GetComponent<NavFollowAi>().player = this.gameObject;

            followCount++;
        }
        
        if (collision.gameObject.name == "record") //reset重置点
        {
            playerTF = collision.gameObject.transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "JiGuan_0")//重力机关
        {

        }     
    }


    /// <summary>
    /// 线性插值（计算a区间内任意b值）
    /// </summary>
    public virtual float LinearInterpolation(float a, float startA, float endA, float startB, float endB)
    {
        float b = startB + (a - startA) / (endA - startA) * (endB - startB);
        return b;
    }
}
