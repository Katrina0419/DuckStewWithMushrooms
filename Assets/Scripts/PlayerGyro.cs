using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using Cinemachine;
using Random = UnityEngine.Random;

using Image = UnityEngine.UI.Image;
using Unity.VisualScripting;

//玩家控制
public class PlayerGyro : MonoBehaviour
{
    Rigidbody2D rb;
    float dirX, dirY, dirZ;
    public float moveSpeed;

    public GameObject body, head;

    public Vector2 playerTF;

    //跟随煤球数
    public GameObject follow;
    public Transform fishes;
    public float followDistance; // 道具之间的跟随距离
    public List<GameObject> items = new List<GameObject>();
    public Transform props;

    //获取标题名
    public GameObject duckRoom;

    //虚拟相机
    public CinemachineVirtualCamera CVCame;

    //是否分裂
    public bool isSnake = false;
    public Image btnSP;
    public List<Sprite> sp = new List<Sprite>();

    public GameObject goodEnd, goodImag, badImag;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTF = transform.position;
    }

    //重新开始
    public void GameStart()
    {
        this.transform.localScale = Vector3.one;

        foreach (Transform p in fishes)
        {
            Destroy(p.gameObject);
        }
        items.Clear();

        this.transform.position = new Vector3(-510, 386, 0);
        foreach (Transform p in props)
        {
            p.gameObject.SetActive(true);
        }

        goodEnd.GetComponent<BoxCollider2D>().enabled = true;
        goodEnd.GetComponent<BoxCollider2D>().isTrigger = false;

        goodImag.SetActive(false);
        badImag.SetActive(false);

        goodEnd.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        duckRoom.GetComponent<SpriteRenderer>().color = Color.white;

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
            //float d = LinearInterpolation(transform.position.x, playerTF.x, duckRoom.transform.position.x, 150, 140);
            //CVCame.m_Lens.FieldOfView = d;
        }


        dirX = Input.acceleration.x * moveSpeed;
        dirY = Input.acceleration.y * moveSpeed;
        dirZ = (-Input.acceleration.z - 0.5f) * moveSpeed;

        if (dirZ < 100) //上
        {
            rb.velocityY = dirY + 70f;
        }
        else //下
        {
            rb.velocityY = -(dirZ - 50f) * 1.7f;
        }

        if (dirX < -20) //左
        {
            rb.velocityX = dirX * 1.7f;

            transform.DOScaleX(-1, 0f);
            transform.rotation = Quaternion.EulerRotation(0, 0, -Input.acceleration.y);
            //transform.rotation = Quaternion.Euler(0, 0, Input.acceleration.z * 70 + 15);
        }
        else //右
        {
            rb.velocityX = dirX * 1.7f;

            transform.DOScaleX(1, 0f);
            transform.rotation = Quaternion.EulerRotation(0, 0, Input.acceleration.y);
            //transform.rotation = Quaternion.Euler(0, 0, -Input.acceleration.z * 70 - 15);
        }       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Fish") //加随从道具
        {
            collision.gameObject.SetActive(false);

            float angle = Random.Range(0f, 360f);
            float distance = Random.Range(5f, 10f);//周围随机
            Vector3 randomPosition = new Vector3(fishes.position.x + Mathf.Cos(angle) * distance, fishes.position.y + Mathf.Sin(angle) * distance, 0);
            GameObject item = Instantiate(follow, randomPosition, Quaternion.Euler(0, 0, 0), fishes);

            items.Add(item);
            item.GetComponent<NavFollowAi>().ball.material = item.GetComponent<NavFollowAi>().ballimage[items.Count]; //随从赋材质球

            if(items.Count >18)//触发结局
            {
                goodEnd.GetComponent<BoxCollider2D>().isTrigger = true;
            }
            else
            {
                goodEnd.GetComponent<BoxCollider2D>().isTrigger = false;
            }

            //角色大一点
            body.transform.DOScale(body.transform.localScale * 1.1f, 1f);
        }
        
        if (collision.gameObject.name == "record") //reset重置点
        {
            playerTF = collision.gameObject.transform.position;

            btnSP.enabled = true; 
        }

        if (collision.gameObject.name == "good")
        {
            goodEnd.GetComponent<BoxCollider2D>().enabled = false;//碰撞体消失
            collision.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 1f);
            Invoke("GoodEnd", 3f);
            //Invoke("GameStart", 3f);

        }
        if (collision.gameObject.name == "bad")
        {
            Invoke("BadEnd", 3f);
            //Invoke("GameStart", 3f);
        }
    }

    public void BadEnd()
    {
        badImag.SetActive(true);
    }
    public void GoodEnd()
    {
        goodImag.SetActive(true);
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
        var nowScale = body.transform.localScale; //储存当下大小

        if (isSnake) //合并
        {
            body.transform.DOScale(Vector3.one, 1f); //鸭子变大

            btnSP.sprite = sp[1];

            for (int i = 0; i < items.Count; i++)
            {
                items[i].GetComponent<NavFollowAi>().agent.enabled = false;

                float angle = Random.Range(0f, 360f);
                float distance = Random.Range(5f, 10f);
                Vector3 randomPosition = new Vector3(fishes.localPosition.x + Mathf.Cos(angle) * distance, fishes.localPosition.y + Mathf.Sin(angle) * distance, 0);

                items[i].transform.DOLocalMove(randomPosition, 1f);
                items[i].transform.localRotation = Quaternion.EulerAngles(0, 0, 0);
            }
        }
        else //列队
        {
            body.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 1f); //鸭子变小

            btnSP.sprite = sp[0];

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
