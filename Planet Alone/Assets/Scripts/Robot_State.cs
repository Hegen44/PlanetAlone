﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Robot_State : MonoBehaviour
{

    public Frustration frustration;
    public Comfort comfort;
    public Saddness saddness;
    public Anxiety anxiety;
    public Bored bored;
    public Playfullness playfullness;

    public World_State world_state;
    private GameObject my_head;
    public ControllerVelocity controller_velocity;
    public GameObject vr_player;
    private float velocity;
    float time_player_not_insight;
    float Last_time_speaking = 0;
    float FriendOrFoe = 0;// x < -0.2 = Foe , -0.2<x<0.2 = Neurtual, 0.2<x = Friend
    List<Emotion> emotions;

    // Use this for initialization
    void Start()
    {
        Emotion comforts = new Emotion();
        Emotion frustrution = new Emotion();
        emotions.Add(comforts);
        emotions.Add(frustrution);
    }

    // Update is called once per frame
    void Update()
    {
        check_head();
        velocity = checkVelocity();
        is_vr_player_in_field_of_view_of_robot();
        //Debug.Log(velocity);
    }

    void check_head()
    {
        my_head = world_state.getRobotObj();
    }

    public float checkVelocity()
    { //UPDATE: anxiety, comfort, frustation
        float localVel = GetComponent<Rigidbody>().velocity.magnitude;


        if (world_state.rightHandItem != null && world_state.rightHandItem.CompareTag("Robot_Head"))
        {
            localVel = controller_velocity.getVelocity();
        }
        else if (world_state.leftHandItem != null && world_state.leftHandItem.CompareTag("Robot_Head"))
        {
            localVel = controller_velocity.getVelocity();
        }

        return localVel; //.magnitude;
    }

    public bool is_vr_player_in_field_of_view_of_robot()
    {
        RaycastHit hit;
        Vector3 rayDirection = vr_player.transform.position - transform.position;
        float fieldOfViewRange = 50;
        float rayRange = 10;
        float minPlaterDetectDistance = 3;

        var distanceToPlayer = Vector3.Distance(transform.position, vr_player.transform.position);
       
        if (Physics.Raycast(transform.position, rayDirection, out hit))
        { // If the player is very close behind the player and in view the enemy will detect the player
            if ((hit.transform.tag == "MainCamera") && (distanceToPlayer <= minPlaterDetectDistance))
            {
                Debug.Log("Can see player");
                time_player_not_insight = Time.time;
                return true;
            }
        }

        if ((Vector3.Angle(rayDirection, transform.forward)) <= fieldOfViewRange)
        { // Detect if player is within the field of view
            if (Physics.Raycast(transform.position, rayDirection, out hit, rayRange))
            {
                if (hit.transform.tag == "MainCamera")
                {
                    Debug.Log("Can see player");
                   time_player_not_insight = Time.time;
                    return true;
                }
                else
                {
                    Debug.Log("Can not see player");
                    return false;
                }
            }
        }
        Debug.Log("END OF FUNC");
        return false;
    }

    public float getlastSpeakTime() {
        return Last_time_speaking;
    }

    public float getTimeOutOfSight()
    {
        return time_player_not_insight;
    }


    public string FriendNFoe() {
        if (FriendOrFoe > 0.2) {
            return "Friend";
        } else if (FriendOrFoe < -0.2) {
            return "Foe";
        } else {
            return "Neutral";
        }
    }
}
