using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class Enemy : MonoBehaviour
{
    GameObject player;
    private int view_cone_stage = 0;
    private float last_attack = 0f;
    private float attack_rate = 2f;
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

                if(Vector3.Distance(transform.position, player.transform.position) < 3f && Time.timeSinceLevelLoad > last_attack + 1/attack_rate)
                {
                    player.GetComponent<Player>().TakeDamage(10);
                    last_attack = Time.timeSinceLevelLoad;
                }
            }
        }


        ViewCone(3,90,10,10);


        if (GetComponentInChildren<Animator>() != null && GetComponent<NavMeshAgent>().enabled)
        {
            GetComponentInChildren<Animator>().SetFloat("speed", GetComponent<NavMeshAgent>().velocity.magnitude);
        }
        else if(GetComponentInChildren<Animator>() != null)
        {
            GetComponentInChildren<Animator>().SetFloat("speed", 0);

        }

    }

    private void ViewCone(int stages = 3, float FOV = 90f, int rays_per_frame = 10, float range = 10f)
    {
        Light light = GetComponentInChildren<Light>();
        light.spotAngle = FOV;
        light.range = range;

        float ray_space = FOV / rays_per_frame;
        RaycastHit hit;
        float ray_space_offset = ray_space / stages;
        Vector3 start_ray_direction = Quaternion.AngleAxis(-FOV/2 + view_cone_stage * ray_space_offset, transform.up) * transform.forward;

        int rays = rays_per_frame;
        if(view_cone_stage != 0)
        {
            rays = rays_per_frame - 1;
        }
        for (int i = 0; i <= rays; i++)
        {
            Vector3 ray_direction = Quaternion.AngleAxis(ray_space * i, transform.up) * start_ray_direction;
            if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), ray_direction, out hit, range))
            {
                if (hit.transform.GetComponent<Player>() != null)
                {
                    player = hit.transform.gameObject;
                    StopAllCoroutines();
                    break;
                }
                else
                {
                    StartCoroutine(LostSightOfPlayer());
                }
            }
            else
            {
                StartCoroutine(LostSightOfPlayer());
            }
            Debug.DrawLine(transform.position + new Vector3(0, 1, 0), transform.position + new Vector3(0, 1, 0) + ray_direction * range, Color.green);
            view_cone_stage++;
            if(view_cone_stage >= stages)
            {
                view_cone_stage = 0;
            }
        }
    }

    public void HearPlayer(Player p)
    {
        player = p.gameObject;
    }





    IEnumerator LostSightOfPlayer()
    {
        yield return new WaitForSeconds(2);
        player = null;
    }
}
