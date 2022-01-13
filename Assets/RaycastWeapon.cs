using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class RaycastWeapon : MonoBehaviour
{
    public Transform gun_end;
    public void ShootRay()
    {
        RaycastHit hit;
        if(Physics.Raycast(gun_end.position, transform.forward, out hit, 10f))
        {
            Debug.Log("Hit Something!");

            if (hit.transform.GetComponent<Rigidbody>() != null)
            {
                Debug.Log("Hit Something with RigidBody!");

                if (hit.transform.GetComponent<NavMeshAgent>() != null)
                {
                    hit.transform.GetComponent<NavMeshAgent>().enabled = false;
                    hit.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                    Debug.Log("Hit Something with and NavMeshAgent!");
                }
                hit.transform.GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * 20f, hit.point);

            }

        }
        Debug.DrawLine(gun_end.position, gun_end.position + transform.forward * 10f, Color.red, 2f);

    }

    //Changes
}
