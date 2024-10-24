using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using Cinemachine;

//玩家控制
public class PlayerGyro : MonoBehaviour
{
    Rigidbody2D rb;
    float dirX, dirY;
    public float moveSpeed;

    public GameObject body, head, fishes;

    public Vector2 playerTF;

    //跟随煤球数
    float followCount;

    //获取标题名
    public GameObject duckRoom;

    //虚拟相机
    public CinemachineVirtualCamera CVCame;

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

        if(transform.position.x <= duckRoom.transform.position.x) 
        {
            //开头标题隐藏
            float b = LinearInterpolation(transform.position.x, playerTF.x, duckRoom.transform.position.x, 1, 0);
            duckRoom.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, b);

            //镜头拉升
            float d = LinearInterpolation(transform.position.x, playerTF.x, duckRoom.transform.position.x, 130, 140);
            CVCame.m_Lens.FieldOfView = d;
        }
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        if (dirY < -20) //下
        {
            rb.velocityY = dirY;
        }
        else //上
        {
            rb.velocityY = dirY * 1.2f;
        }

        if (dirX < -20) //左
        {
            rb.velocityX = dirX * 1.7f;
            transform.DOScaleX(-1f, 0f);

            transform.rotation = Quaternion.EulerRotation(0, 0, -Input.acceleration.y * 0.7f);
            //body.transform.rotation = Quaternion.EulerRotation(0, 0, -Input.acceleration.y * 0.7f);
            //head.transform.rotation = Quaternion.EulerRotation(0, 0, -Mathf.Clamp(Input.acceleration.y, -40f, 10f));
        }
        else //右
        {
            rb.velocityX = dirX * 1.7f;
            transform.DOScaleX(1f, 0f);

            transform.rotation = Quaternion.EulerRotation(0, 0, Input.acceleration.y * 0.7f);
            //body.transform.rotation = Quaternion.EulerRotation(0, 0, Input.acceleration.y * 0.7f);
            //head.transform.rotation = Quaternion.EulerRotation(0, 0, Mathf.Clamp(Input.acceleration.y, -40f, 10f));
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Fish") //加随从道具
        {
            collision.gameObject.SetActive(false);

            var obj = Instantiate(Resources.Load("Follow"), fishes.transform.position, Quaternion.Euler(0, 0, 0), fishes.transform);
            //obj.GetComponent<NavFollowAi>().player = this.gameObject;
            obj.GetComponent<AudioSource>().Play();

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "CameraColl") //出地图重置
        {
            transform.position = playerTF;
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
