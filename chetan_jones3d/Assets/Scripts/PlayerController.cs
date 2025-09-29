using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Vector3 respawnPoint;
    Camera PlayerCam;

    Rigidbody rb;
    Ray jumpRay;
    Ray interactRay;
    RaycastHit interactHit;
    GameObject pickupObj;

    public PlayerInput input;
    public Transform weaponSlot;
    public Weapon currentWeapon;

    float verticalMove;
    float horizontalMove;

    public float speed = 5f;
    public float jumpHeight = 10f;
    public float groundDetectLength = 1f;
    public float interactDistance = 1f;

    public int health = 5;
    public int maxHealth = 5;

    
    public bool attacking = false;



    public void Start()
    {
        input = GetComponent<PlayerInput>();
        jumpRay = new Ray(transform.position, transform.forward);
        interactRay = new Ray(transform.position, transform.forward);
        rb = GetComponent<Rigidbody>();
        PlayerCam = Camera.main;
        weaponSlot = PlayerCam.transform.GetChild(0);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        // Camera Rotation System
        Quaternion playerRotation = PlayerCam.transform.rotation;
        playerRotation.x = 0;
        playerRotation.z = 0;
        transform.rotation = playerRotation;



        // Movement System
        Vector3 temp = rb.linearVelocity;

        temp.x = verticalMove * speed;
        temp.z = horizontalMove * speed;

        jumpRay.origin = transform.position;
        jumpRay.direction = -transform.up;

        interactRay.origin = PlayerCam.transform.position;
        interactRay.direction = PlayerCam.transform.forward;

        if (Physics.Raycast(interactRay, out interactHit, interactDistance))
        {
            if (interactHit.collider.gameObject.tag == "weapon")
            {
                pickupObj = interactHit.collider.gameObject;
            }
        }
        else
            pickupObj = null;

        if (currentWeapon)
            if (currentWeapon.holdToAttack && attacking)
            currentWeapon.fire();
        

        rb.linearVelocity = (temp.x * transform.forward) +
                            (temp.y * transform.up) +
                            (temp.z * transform.right);
    }
    public void fireModeSwitch()
    {
        if (currentWeapon.weaponID == 1)
        {
            currentWeapon.GetComponent<Rifle>().toggleFireMode();
        }
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (currentWeapon)
        {
            if (currentWeapon.holdToAttack)
            {
                if (context.ReadValueAsButton())
                    attacking = true;
                else
                    attacking = false;
            }
            else
                if (context.ReadValueAsButton())
                currentWeapon.fire();
        }
    }  
    public void reload()
    {
        if (currentWeapon)
            if (!currentWeapon.reloading)
                currentWeapon.reload();
    }    
    public void Interact ()
    {
        if (pickupObj)
        {
            if (pickupObj.tag == "weapon")
                pickupObj.GetComponent<Weapon>().equip(this);

            pickupObj = null;
        }
        else
            reload();
    }
     public void DropWeapon()
    {
        if (currentWeapon)
        {
            currentWeapon.GetComponent<Weapon>().unequip();
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 inputAxis = context.ReadValue<Vector2>();

        verticalMove = inputAxis.y;
        horizontalMove = inputAxis.x;
    }


    public void Jump()
    {
        if (Physics.Raycast(jumpRay, groundDetectLength))
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Killzone")
            health = 0;

        if ((other.tag == "Health") && (health < maxHealth))
        {
            health++;
            other.gameObject.SetActive(false);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "hazard")
            health--;
    }

}

