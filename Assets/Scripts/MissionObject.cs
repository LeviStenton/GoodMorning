﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObject : MonoBehaviour {

    [System.Serializable]
    public struct ObjectInfo
    {
        public Animator anim;
        public AnimationClip animClip;
        public AudioSource[] audioSource;
        public float[] delay;
        public string name;
        public bool interactType; // True for pick up, false to interact 
        public bool held;
        public GameObject objToHide;
        public GameObject obj;
        public float timer;
    }

    public ObjectInfo objectInfo;
    public int[] missionProg = new int[2];

    private void Start()
    {
        objectInfo.obj = this.gameObject;
        objectInfo.anim = this.GetComponent<Animator>();
    }
}
