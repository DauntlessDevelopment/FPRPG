using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Spawner : MonoBehaviour
{
    public GameObject spawnable;
    [Range(1,10)]
    public float radius = 1;
    private SphereCollider sphere;
    private float last_spawn = 0f;
    private float spawn_rate = 0.5f;

    public bool contiuous_spawn = false;
    // Start is called before the first frame update
    void Start()
    {
        sphere = GetComponent<SphereCollider>();
        sphere.isTrigger = true;
        sphere.radius = radius;
        Spawn();
    }

    private void Spawn()
    {
        Instantiate(spawnable, transform.position, transform.rotation);
    }

    private void FixedUpdate()
    {
        if(contiuous_spawn)
        {
            if(Time.timeSinceLevelLoad > last_spawn + 1 / spawn_rate)
            {
                Spawn();
                last_spawn = Time.timeSinceLevelLoad;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.name.Contains(spawnable.name) && Time.timeSinceLevelLoad > last_spawn + 1/spawn_rate && !contiuous_spawn)
        {
            Spawn();
            last_spawn = Time.timeSinceLevelLoad;
        }
    }



}
