using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHit : MonoBehaviour
{
    public BallBouncerAgent agent;
    public Ball ball;


    public void Update()
    {
        //Debug.Log("Agent Distance:" + agent.HoopDistancePenalty());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            agent.AddReward(-0.5f);
            //agent.AddReward(agent.HoopDistancePenalty());
            //agent.AddReward(ball.HoopDistancePenality());

            agent.EndEpisode();


        }
    }
}
