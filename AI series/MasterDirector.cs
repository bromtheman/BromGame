using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterDirector : MonoBehaviour
{
    public static List<GameObject> Players;


    // Use this for initialization
    void Start()
    {
        Players = new List<GameObject>();
        Players.AddRange(GameObject.FindGameObjectsWithTag("Target"));

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayerDeath()
    {
    }
}