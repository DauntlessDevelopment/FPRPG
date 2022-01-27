using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class RaycastWeapon : MonoBehaviour
{
    public Transform gun_end;

    private int ammo_count;
    private int max_ammo = 10;

    private int total_ammo = 0;

    public int GetAmmoCount() { return ammo_count; }
    public int GetMaxAmmo() { return max_ammo; }
    public int GetTotalAmmo() { return total_ammo; }

    private void Awake()
    {
        ammo_count = max_ammo;
    }

    public void ShootRay()
    {
        StopAllCoroutines();
        if(ammo_count <= 0) 
        { 
            Reload();
            return; 
        }
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
        ammo_count--;
        Debug.DrawLine(gun_end.position, gun_end.position + transform.forward * 10f, Color.red, 2f);

    }

    public void Reload()
    {
        //ammo_count = max_ammo;
        StopAllCoroutines();
        StartCoroutine(ReloadWithDelay(0.2f));
    }

    public void PickupAmmo(int amount)
    {
        total_ammo += amount;
    }

    IEnumerator ReloadWithDelay(float delay)
    {
        while(ammo_count < max_ammo && total_ammo > 0)
        {
            yield return new WaitForSeconds(delay);
            ammo_count++;
            total_ammo--;
        }
    }
}
