using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody rb;
    public BallBouncerAgent agent;
    public GameObject hoop;

    public float startDistToHoop = 10f;


    public void Update()
    {
        //Debug.Log("Ball Distance:" + HoopDistancePenality());

        //Don't allow dropping the ball outside the area
        if (this.transform.position.y < -10f)
        {
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Don't allow dropping the ball
        if (collision.gameObject.tag == "Ground")
        {
            EndEpisode();
        } else if (collision.gameObject.tag == "agent")
        {
            rb.AddForce(collision.GetContact(0).normal * 10f * rb.mass, ForceMode.Impulse);
        }
    }

    private void EndEpisode()
    {
        //Remove points for failing the task
        // by either dropping the ball on the floor or outside the area
        agent.AddReward(-0.5f);

        //Give more points the closer the ball gets to the hoop
        //To incentivise making the ball go in the correct direction
        //agent.AddReward(HoopDistancePenality());
        //agent.AddReward(agent.HoopDistancePenalty());

        agent.EndEpisode();
    }

    public float HoopDistancePenality()
    {
        return ((startDistToHoop - DistanceToHoop()) / startDistToHoop) * 0.5f;
    }

    public float DistanceToHoop()
    {
        return Mathf.Abs(hoop.transform.position.x - transform.position.x);
    }

    public float DistanceToHoopNormalized()
    {
        return DistanceToHoop() / startDistToHoop;
    }

}
