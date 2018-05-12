using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public Animator animator;
    private Ray jumpCheck;
    public int jumpForce = 50;
    public float pickupRadius = 5f;
    public float movementSpeed = 2.5f;
    public Actor actor;
    private Rigidbody rb;
    public bool canJump = true;
    public bool lockscreen = false;
    private void OnDrawGizmos()
    {
        // Draw's the interaction circle you see on the player in the editor.
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.gameObject.transform.position, pickupRadius);
        

    }

    private void Awake()
    {
        actor = this.gameObject.GetComponentInChildren<Actor>();
        rb = this.gameObject.GetComponent<Rigidbody>();
        if (lockscreen)
        {
            print("Screen is unlocked");
            UnlockScreen();
            lockscreen = false;
        }
        else
        {
            print("Screen is locked");
            LockScreen();
            lockscreen = true;
        }
    }



    void Update () {
   //     Cursor.lockState = CursorLockMode.Locked;
   //     Cursor.visible = false;
        float rotX = Input.GetAxis("Mouse X");
        transform.Rotate(0, rotX, 0);
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("isRunning", true);

        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        if (Input.GetKeyDown("space") && canJump)
        {
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse); // Allows us to jump using our rigidbody
        }



        float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
        float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed; // Basic movement script. Nothing complex
        Vector3 speed = new Vector3(sideSpeed, 0, forwardSpeed);
        speed = transform.rotation * speed;
        transform.Translate(speed, Space.World);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            actor.DropItem(); // Makes us drop our currently equipped item.
        }

        if (Input.GetMouseButton(0))
        {
            animator.SetBool("isAttacking", true);
            if (this.actor.equippedItem == null) return;
            WeaponContainer wc = this.actor.equippedItem.GetComponent<WeaponContainer>();   // Make this only run once when an item/weapon is picked up and then cache the result to make it more effecient. 
            if (wc != null) wc.Use();
            ItemContainer ic = this.actor.equippedItem.GetComponent<ItemContainer>();
            if (ic != null) ic.Use();
        }
        else
        {
            animator.SetBool("isAttacking", false );
        }




/*
        RaycastHit hit;
        jumpCheck = new Ray(transform.position, Vector3.down); // drawing a new ray down from the player position
        if (Physics.Raycast(jumpCheck, out hit, .9f)) // If the vector pointing down hits something at a distance of .9f
        {                                             // then we can jump as we're on the ground, if not. We aren't on the ground
            canJump = true;                           // and can't jump.
        }
        else
        {
            canJump = false;
        }
*/

 
        Collider[] nearbyInteractable = Physics.OverlapSphere(this.gameObject.transform.position, pickupRadius);
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (Collider item in nearbyInteractable)                               // Draws a sphere around the player that
            {                                                                           // gathers nearby colliders, and if they
               ItemContainer gameItem = item.gameObject.GetComponent<ItemContainer>();  // contain the ItemContainer component
                if (gameItem == null)                                                   // then they will be picked up.
                {                                                                       // if not, they're ignored. :(
                    // print(item.gameObject.name + " isn't an item");
                    // Unsure if this should return, maybe?
                    
                }
                else
                {
                    // print("I could grab " + item.gameObject.name);
                    if (gameItem.isOut == false)
                    {
                        actor.PickupItem(item.gameObject);
                        return;
                    }
                }     
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
           if (lockscreen)
            {
                print("Screen is unlocked");
                UnlockScreen();
                lockscreen = false;
            }
           else
            {
                print("Screen is locked");
                LockScreen();
                    lockscreen = true;
            }
        }
    }
    void OnCollisionExit(Collision collision)
    {
        canJump = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        canJump = true;
    }

    void LockScreen()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void UnlockScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }




}
