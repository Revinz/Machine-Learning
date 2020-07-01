using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopArea : MonoBehaviour
{

    public GameObject ball;

    public GameObject FrontEdgeHoop;
    public GameObject BackEdgeHoop;

    public GameObject agent;
    public GameObject hoop;
    Vector3 ballStartPos;
    Vector3 agentStartPos;

    float distance = 6;
    float size = 2;

    public void Start()
    {
        ballStartPos = ball.transform.position;
        agentStartPos = agent.transform.position;
    }

    public void ResetArea()
    {

        /*   TRAINING
        //Get current curriculum's distance
        distance = Academy.Instance.FloatProperties.GetPropertyWithDefault("HoopDistance", 12);
        size = Academy.Instance.FloatProperties.GetPropertyWithDefault("HoopSize", 2);



        hoop.transform.localScale = new Vector3(size, 1, size);
        //FrontEdgeHoop.transform.localScale = new Vector3(FrontEdgeHoop.transform.localScale.x, FrontEdgeHoop.transform.localScale.y,  1/ size);
        //BackEdgeHoop.transform.localScale = new Vector3(BackEdgeHoop.transform.localScale.x, BackEdgeHoop.transform.localScale.y, 1 / size);

        */

        distance = Random.Range(10, 25f); //For inference

        //Reset ball and agent to start position
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        agent.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.transform.position = ballStartPos;
        agent.transform.rotation = Quaternion.identity;
        agent.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        agent.transform.position = agentStartPos;

        //Hoop position
        //hoop.transform.localPosition = new Vector3(Random.Range(10, 50), 2.75f, 1.2f); //Random position
        hoop.transform.localPosition = new Vector3(distance, 2.75f, 1.2f); //Static position for training
    }
}
