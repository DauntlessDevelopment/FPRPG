using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Teleporter linked_teleporter;
    public GameObject teleported_object;

    public bool random = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != teleported_object)
        {
            if(random)
            {
                var teleporters = FindObjectsOfType<Teleporter>();
      
                Teleporter selected = this;
                do
                {
                    selected = teleporters[Random.Range(0, teleporters.Length)];
                } while (selected == this && teleporters.Length > 1);

                other.gameObject.transform.position = selected.transform.position + new Vector3(0, 1, 0);
                selected.teleported_object = other.gameObject;

            }
            else
            {
                other.gameObject.transform.position = linked_teleporter.transform.position + new Vector3(0, 1, 0);
                linked_teleporter.teleported_object = other.gameObject;
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == teleported_object)
        {
            teleported_object = null;
        }
    }
}
