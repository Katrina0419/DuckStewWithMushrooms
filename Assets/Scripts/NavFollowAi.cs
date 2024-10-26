using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//����ú��
public class NavFollowAi : MonoBehaviour
{
    public GameObject followOne;
    public SpriteRenderer ball;
    public NavMeshAgent agent;

    public List<Sprite> ballimage;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        int num = ballimage.Count; //�������һ��ú�򣬲���listɾ��
        ball.GetComponent<SpriteRenderer>().sprite = ballimage[Random.Range(0, num)];
    }

    [System.Obsolete]
    private void Update()
    {
        transform.localRotation = Quaternion.EulerAngles(transform.localRotation.x, transform.localRotation.y, 0);

        if (agent.enabled == true)
        {
            agent.destination = followOne.transform.position;
            transform.rotation = Quaternion.EulerRotation(Vector3.zero);

            if (Mathf.Abs(transform.position.x - followOne.transform.position.x) > 20) //�������
            {
                transform.DOMove(followOne.transform.position, 0.3f);
            }
        }

    }
}
