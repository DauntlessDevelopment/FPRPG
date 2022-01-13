using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            if (GetComponent<NavMeshAgent>().enabled)
            {
                GetComponent<NavMeshAgent>().SetDestination(player.transform.position);

            }
        }


        RaycastHit hit;
        Vector3 start_ray_direction = Quaternion.AngleAxis(-45, transform.up) * transform.forward;

        for(int i = 0; i < 30; i++)
        {
            Vector3 ray_direction = Quaternion.AngleAxis(3 * i, transform.up) * start_ray_direction;
            if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), ray_direction, out hit, 10f))
            {
                if (hit.transform.GetComponent<Player>() != null)
                {
                    player = hit.transform.gameObject;
                }
            }
            Debug.DrawLine(transform.position + new Vector3(0, 1, 0), transform.position + new Vector3(0, 1, 0) + ray_direction * 10f, Color.green);
        }
        

        if(GetComponentInChildren<Animator>() != null && GetComponent<NavMeshAgent>().enabled)
        {
            GetComponentInChildren<Animator>().SetFloat("speed", GetComponent<NavMeshAgent>().velocity.magnitude);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>() != null)
        {
            player = other.gameObject;
        }
    }
}
