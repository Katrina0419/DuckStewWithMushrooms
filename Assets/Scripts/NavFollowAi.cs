using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//����ú��
public class NavFollowAi : MonoBehaviour
{
    public GameObject player;
    public SpriteRenderer ball;
    //NavMeshAgent agent;

    public List<Sprite> ballimage;


    private void Start()
    {
        //agent = GetComponent<NavMeshAgent>();

        int num = ballimage.Count; //�������һ��ú�򣬲���listɾ��
        ball.GetComponent<SpriteRenderer>().sprite = ballimage[Random.Range(0, num)];
    }

    [System.Obsolete]
    private void Update()
    {
        //agent.destination = player.transform.position;
        //transform.rotation = Quaternion.EulerRotation(Vector3.zero);

        //transform.DOScaleX(player.transform.localScale.x, 1f);

        //if(Mathf.Abs(transform.position.x - player.transform.position.x) > 20)
        //{
        //    transform.DOMove(player.transform.position, 0.3f);
        //}
    }
}
