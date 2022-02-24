using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float health = 100f;
    private float move_speed = 10f;
    private float turn_speed = 360f;
    private bool can_jump = false;

    public bool is_loud = false;

    public Text ammo_text;
    public Text ammo_reserve_text;
    public Text health_text;

    public RaycastWeapon laser;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(UpdateUI());
    }

    // Update is called once per frame
    void Update()
    {
        if(health == 0)
        {
            return;
        }
        HandleInput(); 
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            health = 0;
            Die();
        }
        health_text.text = "Health : " + health;
    }    
    private void Die()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
    private void HandleInput()
    {
        if(Input.GetAxisRaw("Vertical") != 0)
        {
            transform.Translate(Input.GetAxis("Vertical") * transform.forward * move_speed * Time.deltaTime, Space.World);
            transform.GetComponent<Rigidbody>().velocity = new Vector3(0, transform.GetComponent<Rigidbody>().velocity.y, 0);
        }
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            transform.Translate(Input.GetAxis("Horizontal") * transform.right * move_speed * Time.deltaTime, Space.World);
            transform.GetComponent<Rigidbody>().velocity = new Vector3(0, transform.GetComponent<Rigidbody>().velocity.y, 0);

        }

        if (Input.GetAxisRaw("Mouse X") != 0)
        {
            transform.Rotate(transform.up, Input.GetAxis("Mouse X") * turn_speed * Time.deltaTime);
        }
        if(Input.GetAxisRaw("Mouse Y") != 0)
        {
            Camera.main.transform.Rotate(Vector3.left, Input.GetAxis("Mouse Y") * turn_speed * Time.deltaTime);
        }

        if(Input.GetButtonDown("Jump") && Physics.Raycast(transform.position - new Vector3(0,0.9f,0), Vector3.down, 0.2f))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * 200f);
            
        }
        
        if(Input.GetButtonDown("Fire1"))
        {
            laser.ShootRay();
        }

        if(Input.GetButtonDown("Fire2"))
        {
            laser.ToggleADS();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            laser.Reload();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(is_loud)
        {
            if(other.GetComponent<Enemy>() != null)
            {
                other.GetComponent<Enemy>().HearPlayer(this);
            }
        }
    }

    IEnumerator UpdateUI()
    {
        while(true)
        {
            yield return new WaitForFixedUpdate();
            ammo_text.text = $"Ammo : {laser.GetAmmoCount()}/{laser.GetMaxAmmo()}";
            ammo_reserve_text.text = $"Reserve : {laser.GetTotalAmmo()}";
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<AmmoCollectable>() != null)
        {
            laser.PickupAmmo(collision.gameObject.GetComponent<AmmoCollectable>().amount);
            Destroy(collision.gameObject);
        }
    }
}
