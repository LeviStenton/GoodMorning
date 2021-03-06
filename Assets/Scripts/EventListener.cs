﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Event : UnityEvent<MissionObject.ObjectInfo, GameObject, Vector3>
{
    
}

public class EventListener : MonoBehaviour {

    public int[] missionProg;
    public int[] needsMilk = { 50, 50, 50, 50, 50 };
	public float[] missionTimers = {0,0,0,0,0};
    public Event m_event;
    [Range(-3, 3)]
    public float xPickupModifier, yPickupModifier, zPickupModifier;
    [Range(-180, 180)]
    public float xRotationModifier, yRotationModifier, zRotationModifier;
    public GameObject heldObject;
    public float maxReach = 1.0f;
    public AudioSource _audioSource;
    bool holdingMilk = false;
    float milkTimer = 0;
    float gameTimer = 0;

    bool startedGame = false;

	// Use this for initialization
	void Start () {
        if (m_event == null)
            m_event = new Event();
	}
	
	// Update is called once per frame
	/*void Update () {

        for (int index = 0; index < missionTimers.Length; ++index)
        {
            if (missionTimers[index] > 0)
                missionTimers[index] -= Time.deltaTime;
        }

        gameTimer += Time.deltaTime;

        if (milkTimer > 0)
            milkTimer -= Time.deltaTime;

        if (missionProg[6] == 36)
        {
            SceneManager.LoadScene("Menu");
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Input.GetAxis("Fire1") > 0 && m_event != null && Physics.Raycast(ray, out hitInfo, maxReach))
        {
            if (hitInfo.collider.gameObject.GetComponent<MissionObject>())
            {
                MissionObject hitObject = hitInfo.collider.gameObject.GetComponent<MissionObject>();

                m_event.AddListener(hitObject.objectListener);

                int missionIndex = hitObject.missionProg[0];
                int missionProgress = hitObject.missionProg[1];

                for (int mIndex = 0; mIndex < missionProg.Length; ++mIndex)
                {
                    if (missionIndex == mIndex && missionProgress == missionProg[missionIndex] && ((holdingMilk && missionProgress == needsMilk[missionIndex]) || (!holdingMilk && missionProgress != needsMilk[missionIndex])))
                    {
                        if (!(heldObject && hitObject.objectInfo.interactType))
                        {
                            if (missionTimers[missionIndex] <= 0)
                            {
                                if (heldObject)
                                {
                                    Destroy(heldObject);
                                    heldObject = null;
                                    /*if (hitObject.GetComponent<MeshRenderer>())
                                    {
                                        hitObject.GetComponent<MeshRenderer>().enabled = true;
                                        Color colour = hitObject.GetComponent<MeshRenderer>().material.color;
                                        hitObject.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(colour.r, colour.g, colour.b, 1));
                                    }

                                    if (hitObject.GetComponentInChildren<MeshRenderer>())
                                        foreach (MeshRenderer rend in hitObject.GetComponentsInChildren<MeshRenderer>())
                                        {
                                            rend.enabled = true;
                                            Color colour = rend.material.color;
                                            rend.material.SetColor("_Color", new Color(colour.r, colour.g, colour.b, 1));
                                        }
                                        
                                }
                                ++missionProg[missionIndex];
                                missionTimers[missionIndex] = hitObject.objectInfo.timer;
                                m_event.Invoke(hitObject.objectInfo, heldObject, new Vector3(xRotationModifier, yRotationModifier, zRotationModifier));
                                ++missionProg[6];

                                if (!startedGame && missionProg[4] > 0)
                                {
                                    startedGame = true;
                                    Analytics.CustomEvent("First Object", new Dictionary<string, object>
                                {
                                    {"Cereal", missionProg[0]},
                                    {"Coffee", missionProg[1]},
                                    {"Shower", missionProg[2]},
                                    {"Toilet", missionProg[3]},
                                    {"Alarm", missionProg[4]}
                                });
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("Analytics/first_mission.txt", true)) {
                                        string mission = "";
                                        if (missionProg[0] > 0)
                                            mission = "Cereal";
                                        else if (missionProg[1] > 0)
                                            mission = "Coffee";
                                        else if (missionProg[2] > 0)
                                            mission = "Shower";
                                        else if (missionProg[3] > 0)
                                            mission = "Toilet";
                                        else
                                            mission = "Alarm";
                                        file.WriteLine(mission);
                                    }
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("Analytics/mission_rec.txt", true)) {
                                        file.WriteLine("_________________________");
                                        file.WriteLine("Cereal    Coffee    Shower    Toilet    Alarm    Time");
                                    }
                                }
                                using (System.IO.StreamWriter file = new System.IO.StreamWriter("Analytics/mission_rec.txt", true)) {
                                    file.WriteLine(missionProg[0] + "         " + missionProg[1] + "         " + missionProg[2] + "         " + missionProg[3] + "         " + missionProg[4] + "         " + gameTimer);
                                }
                            }
                        }
                    }
                }
            }
            else if (hitInfo.collider.gameObject.GetComponent<Milk>())
            {
                if (!holdingMilk && milkTimer <= 0 && !heldObject)
                {
                    hitInfo.collider.gameObject.GetComponent<Milk>().grabbingMilk = true;
                    milkTimer = 0.5f;
                }
            }
            else if (hitInfo.collider && heldObject && heldObject.GetComponent<Milk>() && milkTimer <= 0)
            {
                holdingMilk = false;
                heldObject.transform.rotation = Quaternion.identity;
                heldObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + heldObject.GetComponent<BoxCollider>().bounds.size.y / 2, hitInfo.point.z);
                heldObject.transform.parent = null;
                heldObject = null;
                milkTimer = 0.5f;
            }
            else if (hitInfo.collider.GetComponent<Door>() && !hitInfo.collider.GetComponent<Door>().animPlayed)
            {
                if (hitInfo.collider.GetComponent<Door>().anim && hitInfo.collider.GetComponent<Door>().animClip)
                {
                    hitInfo.collider.GetComponent<Door>().anim.enabled = true;
                }
                if (hitInfo.collider.GetComponent<Door>().anima)
                    hitInfo.collider.GetComponent<Door>().anima.Play();
                hitInfo.collider.GetComponent<Door>().animPlayed = true;
            }
        }
        if (heldObject)
        {
            heldObject.transform.localPosition = Vector3.Lerp(heldObject.transform.localPosition, new Vector3(xPickupModifier, yPickupModifier, zPickupModifier), Time.deltaTime * 4);
        }
    }*/
}
