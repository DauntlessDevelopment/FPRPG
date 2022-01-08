using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    public Transform gun_end;
    public void ShootRay()
    {
        RaycastHit hit;
        if(Physics.Raycast(gun_end.position, transform.forward, out hit, 10f))
        {
            hit.transform.GetComponent<Rigidbody>().AddForceAtPosition(transform.forward * 20f, hit.point);

        }
        Debug.DrawLine(gun_end.position, gun_end.position + transform.forward * 10f, Color.red, 2f);

    }
}
