using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float move_speed = 10f;
    private float turn_speed = 360f;
    private bool can_jump = false;

    public bool is_loud = false;

    public RaycastWeapon laser;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput(); 
    }

    private void HandleInput()
    {
        if(Input.GetAxisRaw("Vertical") != 0)
        {
            transform.Translate(Input.GetAxis("Vertical") * transform.forward * move_speed * Time.deltaTime, Space.World);
        }
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            transform.Translate(Input.GetAxis("Horizontal") * transform.right * move_speed * Time.deltaTime, Space.World);
        }

        if(Input.GetAxisRaw("Mouse X") != 0)
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
        
        if(Input.GetButton("Fire1"))
        {
            laser.ShootRay();
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
}
