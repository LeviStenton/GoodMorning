﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public Animator anim;
    public AnimationClip animClip;
    bool animPlayed = false;
    public float maxReach = 1.0f;

	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Interact") > 0 && !animPlayed)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, maxReach);
            if (hitInfo.collider.gameObject.GetComponent<Door>())
            {
                if (anim && animClip)
                {
                    anim.enabled = true;
                }
            }
        }
	}
}
