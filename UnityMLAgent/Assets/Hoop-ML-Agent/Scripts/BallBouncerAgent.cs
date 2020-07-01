using MLAgents;
using MLAgents.Sensors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBouncerAgent : Agent
{
    public float speed = 3000f;
    public float turnSpeed = 50f;


    public Ball ball;
    public GameObject hoop;
    public GameObject innerHoop;
    public Rigidbody rb;
    public HoopArea area;

    public float startDistToHoop = 10;

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void OnEpisodeBegin()
    {
        area.ResetArea();
        //Update ball and agent distance to hoop
        ball.startDistToHoop = ball.DistanceToHoop();
        startDistToHoop = HoopDistance();

        base.OnEpisodeBegin();
    }

    /// <summary>
    ///     Used for manual testing the actions of the agent
    /// </summary>
    /// <returns></returns>
    public override float[] Heuristic()
    {
        float[] action = new float[2];

        //Horizontal movement
        action[0] = Input.GetAxis("Horizontal");

        //Tilting
        action[1] = Input.GetAxis("Rotation"); //Custom Axis created that uses Q and E to tilt the agent

        return action;
    }

    /// <summary>
    /// What observations we want to use to determine the best action
    /// </summary>
    /// <param name="sensor"></param>
    public override void CollectObservations(VectorSensor sensor)
    {
        /// All observations should be in normalized units

        //The ball's velocity (vector3 = 3 values)
        sensor.AddObservation(ball.rb.velocity / 20);

        //The ball's distance to the agent
        sensor.AddObservation((ball.transform.position - this.transform.position) /100);

        //Ball's position vector 3 = 3 values
        sensor.AddObservation((area.gameObject.transform.position - ball.transform.position) / 100);

        //Distance to the hoop (float = 1 value)
        sensor.AddObservation(HoopDistanceNormalized());

        //Balls distance to hoop (float = 1 value)
        sensor.AddObservation(ball.DistanceToHoopNormalized());

        //Direction to ball (vector3 = 3 values)
        sensor.AddObservation((ball.transform.position - this.transform.position).normalized);

        //Own rotation
        Quaternion rotation = transform.rotation;
        Vector3 normalized = rotation.eulerAngles / 180.0f - Vector3.one;
        sensor.AddObservation(normalized);

        //Own velocity (vector3 = 3 values)
        sensor.AddObservation(rb.velocity / 20);

        //Own rotation speed
        sensor.AddObservation(rb.angularVelocity / rb.maxAngularVelocity);

        //Own position vector3 = 3 values
        sensor.AddObservation((area.gameObject.transform.position - transform.position) / 100);

        //Total (12) values
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        //Makes the agent try to perform the task as possible
        // By discouraging using too many actions
        AddReward(-1f / maxStep);

        //Horizontal Movement
        rb.AddForce(Vector3.right * vectorAction[0] * speed * Time.fixedDeltaTime * rb.mass, ForceMode.Force);

        //Tilting
        rb.AddTorque(Vector3.forward * -vectorAction[1] * turnSpeed * Time.fixedDeltaTime * rb.mass, ForceMode.Force);

    }

    public void FixedUpdate()
    {
        //Request a new decision on every fixed update
        RequestDecision();
    }

    public float HoopDistancePenalty()
    {
        return ((startDistToHoop - HoopDistance()) / startDistToHoop) * 0.3f;
    }

    public float HoopDistance()
    {
        return Mathf.Abs(innerHoop.transform.position.x - transform.position.x);
    }

    public float HoopDistanceNormalized()
    {
        return HoopDistance() / startDistToHoop;
    }


}
