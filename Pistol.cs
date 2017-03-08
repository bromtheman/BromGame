using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    // public GameObject ammotype;
    public float damage = 1f;
    private bool playerreload = false; // Is the player trying to reload?
    public int clipsize = 1; // How many bullets you can shoot
    private float rememberreload; // remember reload
    private int rememberclipsize;
    public float reloadtime = 10f; // how long it takes to reload
    public AudioSource shootsound; // audio file 1 name
    public AudioSource reloadsound; // audio file 2 name
    private bool allowfire = true;
    private float nextFire;
    private bool rayhit;
    public float fireRate = 1f;
    public Camera ourcamera;
    public WeaponParticlesController weaponParticlesController;
    void Start()
    {
     //   ourcamera = GetComponent<Camera>();
        rememberclipsize = clipsize;
        rememberreload = reloadtime; // Allows us to reset the reload time later on.

    }
    void Update()
    {

        if (Input.GetButtonDown("Fire1") && clipsize != 0 && Time.time > nextFire && allowfire == true) // if we're firing and haven't shot 10 bullets yet nor reached fire rate and aren't reloading
        {
            nextFire = Time.time + fireRate;
            Fire(); // shoot
            clipsize--; // subtract from current clip size
        }
        if (clipsize == 0)
        { // if we've shot 10 bullets
            reload();
        }
        if (Input.GetButtonDown("Reload"))
        {
            playerreload = true; // If the player press R set Player Reload to true to allow the reload to continue
        }
        if (playerreload == true) // Starts the reload and keeps it on to finish the timer
        {
            reload();
        }
    }
    void Fire()
    {
        weaponParticlesController.Play();
        shootsound.Play();
        Debug.Log("Fired");
        RaycastHit hit;
        Ray bulletCheck = ourcamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(bulletCheck, out hit, Mathf.Infinity)) //If our ray "Direction" hits something at a length of infinite.
        {
            hit.collider.SendMessageUpwards("Damage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }
    void reload()
    {
        allowfire = false; // can't shoot while reloading
        reloadsound.Play(); // play sound
        reloadtime -= Time.deltaTime; // Count down reloading by Time.deltaTime
        if (reloadtime <= 0.0f) // If we're done reloading set clipsize to rememberclipsize and reset reload time.
        {
            clipsize = rememberclipsize; // reset values
            reloadtime = rememberreload;
            playerreload = false;
            allowfire = true; // can shoot again
            return;
        }
    }



}




/*   void Fire()
   {
       RaycastHit hit; // our hit detector
       Vector3 rayout = new Vector3(0, 1, 0);
       // draws a vector pointing forward that will later add rotation
       rayout = Bullet_Emitter.transform.rotation * rayout; 
       // adding rotation so that it stays facing forward from gun (local and not global).
       Ray direction = new Ray(Bullet_Emitter.transform.position, rayout);
       // Direction of the ray is the position of the Bullet emitter going forward as defined with our Vector 3 above
       // with the name rayout. Which stays locally defined with line 34.
       shootsound.Play();
       if (Physics.Raycast(direction, out hit, Mathf.Infinity)) //If our ray "Direction" hits something at a length of INFINITY *le gasp*
       {

           Debug.DrawRay(Bullet_Emitter.transform.position, rayout, Color.green, 10f); // draws the ray so we can see it
           Debug.Log("Ray has VALLLLUUUU"); // testing purposes :P
           hit.collider.SendMessageUpwards("Damage", damageAmount, SendMessageOptions.DontRequireReceiver); //Send the damage
       }

 */

// GameObject Bullet_Handler; // shooting
// Bullet_Handler = Instantiate(AmmoType, BulletSpawner.transform.position, BulletSpawner.transform.rotation);