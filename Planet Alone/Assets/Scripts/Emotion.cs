﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class Emotion{
    AudioClip[] sources;
    AssetDatabase asd;
    Dialogue data;
    const int friend = 0;
    const int foe = 1;

    public float rating = 0;
    //public List<Tuple> dialogue_options = new List<Tuple>();
    public Dictionary<int, List<Tuple>> dialogue = new Dictionary<int, List<Tuple>>()
    {
        { friend, new List<Tuple>() },
        { foe, new List<Tuple>() }
    };


    public Emotion(int emotion_id) {
        data = GameObject.FindGameObjectWithTag("Robot_Head").GetComponent<Dialogue>();
        for (int i = 0; i < data.database.Count; ++i)
        {
            if (data.database[i].Emotion == emotion_id && data.database[i].Action_tag == "Idle")
            {
                dialogue[data.database[i].FOF].Add(new Tuple(data.database[i].Sources, 0));
            }
        }
    }
}


