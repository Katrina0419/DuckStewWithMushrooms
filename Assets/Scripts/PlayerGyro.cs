using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using Cinemachine;
using Random = UnityEngine.Random;
using static UnityEditor.Progress;

//玩家控制
public class PlayerGyro : MonoBehaviour
{
    Rigidbody2D rb;
    float dirX, dirY, dirZ;
    public float moveSpeed;

    public GameObject body, head, fishes;

    public Vector2 playerTF;

    //跟随煤球数
    public GameObject follow;
    public float followDistance; // 道具之间的跟随距离
    public List<GameObject> items = new List<GameObject>();

    //获取标题名
    public GameObject duckRoom;

    //虚拟相机
    public CinemachineVirtualCamera CVCame;

    //是否分裂
    public bool isSnake = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTF = transform.position;
    }


    [System.Obsolete]
    private void FixedUpdate()
    {
        if (transform.position.x < duckRoom.transform.position.x)
        {
            //开头标题隐藏
            float b = LinearInterpolation(transform.position.x, playerTF.x, duckRoom.transform.position.x, 1, 0);
            duckRoom.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, b);

            //镜头
            float d = LinearInterpolation(transform.position.x, playerTF.x, duckRoom.transform.position.x, 150, 140);
            CVCame.m_Lens.FieldOfView = d;
        }


        dirX = Input.acceleration.x * moveSpeed;
        //dirY = Input.acceleration.y * moveSpeed;
        dirZ = (-Input.acceleration.z - 0.5f) * moveSpeed;

        if (dirZ < 100) //上
        {
            rb.velocityY = dirZ - 30f;
        }
        else //下
        {
            rb.velocityY = -(dirZ - 50f) * 1.7f;
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

            float angle = Random.Range(0f, 360f);
            float distance = Random.Range(10f, 15f);//周围随机
            Vector3 randomPosition = new Vector3(transform.position.x + Mathf.Cos(angle) * distance, transform.position.y + Mathf.Sin(angle) * distance, 0);
            GameObject item = Instantiate(follow, randomPosition, Quaternion.Euler(0, 0, 0), fishes.transform);

            items.Add(item);
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

    public void isSnakeNow()
    {
        if (isSnake) //分裂
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].GetComponent<NavFollowAi>().agent.enabled = false;

                float angle = Random.Range(0f, 360f);
                float distance = Random.Range(5f, 10f);
                Vector3 randomPosition = new Vector3(transform.position.x + Mathf.Cos(angle) * distance, transform.position.y + Mathf.Sin(angle) * distance, 0);

                items[i].transform.DOMove(randomPosition, 1f);
                items[i].transform.localRotation = Quaternion.EulerAngles(0, 0, 0);
            }
        }
        else //列队
        {
            for (int i = 0; i < items.Count; i++)
            {
                Vector3 offset = new Vector3(-followDistance * (i + 1), 0, 0);
                items[i].transform.DOMove(transform.position + offset, 1f);

                items[i].GetComponent<NavFollowAi>().agent.enabled = true;
                items[0].GetComponent<NavFollowAi>().followOne = this.gameObject; //One By One
                if(i > 0)
                {
                    items[i].GetComponent<NavFollowAi>().followOne = items[i - 1].GetComponent<NavFollowAi>().gameObject;
                }

                items[i].transform.localRotation = Quaternion.EulerAngles(0, 0, 0);
            }
        }

        isSnake = !isSnake;
    }

    /// <summary> 线性插值（计算a区间内任意b值）</summary>
    public virtual float LinearInterpolation(float a, float startA, float endA, float startB, float endB)
    {
        float b = startB + (a - startA) / (endA - startA) * (endB - startB);
        return b;
    }
}
