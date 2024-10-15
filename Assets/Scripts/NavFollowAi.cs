using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//¸úËæÃºÇò
public class NavFollowAi : MonoBehaviour
{
    public GameObject player;
    public SpriteRenderer ball;
    NavMeshAgent agent;

    public List<Sprite> ballimage;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        ball.GetComponent<SpriteRenderer>().sprite = ballimage[Random.Range(0, ballimage.Count)];
    }

    [System.Obsolete]
    private void Update()
    {
        agent.destination = player.transform.position;
        transform.rotation = Quaternion.EulerRotation(Vector3.zero);

        transform.DOScaleX(player.transform.localScale.x, 1f);
    }
}
