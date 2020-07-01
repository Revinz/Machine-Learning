using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoop : MonoBehaviour
{
    public BallBouncerAgent agent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ball")
        {
            Debug.Log("Goal!");
            agent.SetReward(2f);
            agent.EndEpisode();
        }
    }
}
