using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Linq;

public class AI : MonoBehaviour
{
    private NavMeshAgent agent;
    private UserPlayer UserPlayer;
    private List<Vector3> selection;
    private MasterDirector MasterDirector;
    float smallestDistance = -1;
    int closestPlayer = -1;
    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < MasterDirector.Players.Count; i++)
        {
            float distance = Vector3.Distance(MasterDirector.Players[i].transform.position, transform.position);
            if (distance < smallestDistance | smallestDistance == -1)
            {
                smallestDistance = distance;
                closestPlayer = i;
            }
        }

        if (closestPlayer != -1)
        {
            var TheClosestPlayer = MasterDirector.Players[closestPlayer];
            agent.SetDestination(TheClosestPlayer.gameObject.transform.position);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            for (int i = 0; i < MasterDirector.Players.Count; i++)
            {
                print(MasterDirector.Players[i]);
            }

        }
    }
}