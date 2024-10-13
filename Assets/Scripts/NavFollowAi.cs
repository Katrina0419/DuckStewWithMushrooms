using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavFollowAi : MonoBehaviour
{
    public GameObject player;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    [System.Obsolete]
    private void Update()
    {
        agent.destination = player.transform.position;
        transform.rotation = Quaternion.EulerRotation(Vector3.zero);
    }
}
