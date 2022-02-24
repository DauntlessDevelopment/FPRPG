using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// SETUP ADAPTIVE CROSSHAIR
/// </summary>



public class RaycastWeapon : MonoBehaviour
{
    public Transform hip_xform;
    public Transform ADS_xform;
    public Transform gun_end;
    public GameObject muzzle_flash;
    public List<Texture> muzzles_flash_sprites = new List<Texture>();
    private int ammo_count;
    private int max_ammo = 10;

    public Image crosshair;

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
        GetComponentInChildren<Animator>().SetTrigger("Shoot");
        MuzzleFlash();

    }

    private void MuzzleFlash()
    {
        muzzle_flash.GetComponent<MeshRenderer>().material.mainTexture = muzzles_flash_sprites[Random.Range(0, muzzles_flash_sprites.Count)];
        muzzle_flash.SetActive(true);
        StartCoroutine(DeativateObjectAfterDelay(0.1f, muzzle_flash));
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        if(Physics.Raycast(gun_end.transform.position, transform.forward, out hit))
        {
            Vector2 crosshair_screen_position = Camera.main.WorldToScreenPoint(hit.point);
            crosshair.rectTransform.position = crosshair_screen_position;
            float dist_scale = Mathf.Min(Mathf.Max(hit.distance, 0.1f), 101);
            dist_scale /= 100;
            crosshair.transform.localScale = new Vector3(1,1,1) * (1 - dist_scale);
        }
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

    public void ToggleADS()
    {
        if(transform.position == hip_xform.position)
        {
            transform.position = ADS_xform.position;
        }
        else
        {
            transform.position = hip_xform.position;
        }
        
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

    IEnumerator DeativateObjectAfterDelay(float delay, GameObject o)
    {
        yield return new WaitForSeconds(delay);
        o.SetActive(false);
    }
}
