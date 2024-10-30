using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using Cinemachine;
using Random = UnityEngine.Random;

using Image = UnityEngine.UI.Image;
using Unity.VisualScripting;

//��ҿ���
public class PlayerGyro : MonoBehaviour
{
    Rigidbody2D rb;
    float dirX, dirY, dirZ;
    public float moveSpeed;

    public GameObject body, head;

    public Vector2 playerTF;

    //����ú����
    public GameObject follow;
    public Transform fishes;
    public float followDistance; // ����֮��ĸ������
    public List<GameObject> items = new List<GameObject>();
    public Transform props;

    //��ȡ������
    public GameObject duckRoom;

    //�������
    public CinemachineVirtualCamera CVCame;

    //�Ƿ����
    public bool isSnake = false;
    public Image btnSP;
    public List<Sprite> sp = new List<Sprite>();

    public GameObject goodEnd, goodImag, badImag;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTF = transform.position;
    }

    //���¿�ʼ
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
            //��ͷ��������
            float b = LinearInterpolation(transform.position.x, playerTF.x, duckRoom.transform.position.x, 1, 0);
            duckRoom.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, b);

            //��ͷ
            //float d = LinearInterpolation(transform.position.x, playerTF.x, duckRoom.transform.position.x, 150, 140);
            //CVCame.m_Lens.FieldOfView = d;
        }


        dirX = Input.acceleration.x * moveSpeed;
        dirY = Input.acceleration.y * moveSpeed;
        dirZ = (-Input.acceleration.z - 0.5f) * moveSpeed;

        if (dirZ < 100) //��
        {
            rb.velocityY = dirY + 70f;
        }
        else //��
        {
            rb.velocityY = -(dirZ - 50f) * 1.7f;
        }

        if (dirX < -20) //��
        {
            rb.velocityX = dirX * 1.7f;

            transform.DOScaleX(-1, 0f);
            transform.rotation = Quaternion.EulerRotation(0, 0, -Input.acceleration.y);
            //transform.rotation = Quaternion.Euler(0, 0, Input.acceleration.z * 70 + 15);
        }
        else //��
        {
            rb.velocityX = dirX * 1.7f;

            transform.DOScaleX(1, 0f);
            transform.rotation = Quaternion.EulerRotation(0, 0, Input.acceleration.y);
            //transform.rotation = Quaternion.Euler(0, 0, -Input.acceleration.z * 70 - 15);
        }       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Fish") //����ӵ���
        {
            collision.gameObject.SetActive(false);

            float angle = Random.Range(0f, 360f);
            float distance = Random.Range(5f, 10f);//��Χ���
            Vector3 randomPosition = new Vector3(fishes.position.x + Mathf.Cos(angle) * distance, fishes.position.y + Mathf.Sin(angle) * distance, 0);
            GameObject item = Instantiate(follow, randomPosition, Quaternion.Euler(0, 0, 0), fishes);

            items.Add(item);
            item.GetComponent<NavFollowAi>().ball.material = item.GetComponent<NavFollowAi>().ballimage[items.Count]; //��Ӹ�������

            if(items.Count >18)//�������
            {
                goodEnd.GetComponent<BoxCollider2D>().isTrigger = true;
            }
            else
            {
                goodEnd.GetComponent<BoxCollider2D>().isTrigger = false;
            }

            //��ɫ��һ��
            body.transform.DOScale(body.transform.localScale * 1.1f, 1f);
        }
        
        if (collision.gameObject.name == "record") //reset���õ�
        {
            playerTF = collision.gameObject.transform.position;

            btnSP.enabled = true; 
        }

        if (collision.gameObject.name == "good")
        {
            goodEnd.GetComponent<BoxCollider2D>().enabled = false;//��ײ����ʧ
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
        if (collision.gameObject.name == "CameraColl") //����ͼ����
        {
            transform.position = playerTF;
        }
    }


    public void isSnakeNow()
    {
        var nowScale = body.transform.localScale; //���浱�´�С

        if (isSnake) //�ϲ�
        {
            body.transform.DOScale(Vector3.one, 1f); //Ѽ�ӱ��

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
        else //�ж�
        {
            body.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 1f); //Ѽ�ӱ�С

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

    /// <summary> ���Բ�ֵ������a����������bֵ��</summary>
    public virtual float LinearInterpolation(float a, float startA, float endA, float startB, float endB)
    {
        float b = startB + (a - startA) / (endA - startA) * (endB - startB);
        return b;
    }
}
