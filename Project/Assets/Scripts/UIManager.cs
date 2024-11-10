using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public void PlayerReset(GameObject player)
    {
        player.transform.position = player.GetComponent<PlayerGyro>().playerTF;
    }
}
