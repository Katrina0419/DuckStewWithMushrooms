using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//¸úËæÃºÇò
public class NavFollowAi : MonoBehaviour
{
    public GameObject followOne;
    public SpriteRenderer ball;
    public NavMeshAgent agent;

    public List<Material> ballimage;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        //int num = ballimage.Count;
        //ball.GetComponent<SpriteRenderer>().material = ballimage[Random.Range(0, num)];
    }

    [System.Obsolete]
    private void Update()
    {
        transform.localRotation = Quaternion.EulerAngles(transform.localRotation.x, transform.localRotation.y, 0);

        if (agent.enabled == true)
        {
            agent.destination = followOne.transform.position;
            transform.rotation = Quaternion.EulerRotation(Vector3.zero);

            if (Mathf.Abs(transform.position.x - followOne.transform.position.x) > 20) //ÍÑÀë¸úËæ
            {
                transform.DOMove(followOne.transform.position, 0.3f);
            }
        }

    }
}
