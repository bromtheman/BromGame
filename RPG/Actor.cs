using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actor : MonoBehaviour {
    // The basic script of all game character, contains health and basic stats that we can use for manipulating what the
    // that we can use for manipulating what the gameobject holds.
    public Slider healthBar;
    public int health = 100;
    public int maxHealth;
    public int strength = 5;
    public int intelligence = 5;
    public int agility = 5;
    public bool isPlayer = false;
    public int luck = 5;
    Inventory inventory; // Our inventory if we have one.
    WeaponContainer weaponContainer; // Used for visual representation of debugging. REMOVE LATER
    public GameObject equippedItem; // What item do we currently have out? 
    public GameObject itemHolder; // This is used for positioning the equippedItem and it should be an empty attached to the 
                                  // parent object.
    public int team = 0; //0=neutral, 1 = player, -1 = enemy
    void Awake()
    {
        maxHealth = health;
        if (itemHolder == null) // This is just in-case I accidently forget to set the ItemHolder in the inspector,
        {                      // If it is still attached, then this will try to find it. Otherwise it reports an error.
            try
            {
                itemHolder = this.gameObject.transform.Find("ItemHolder").gameObject;
            }
            catch
            {
                print("An ItemHolder hasn't been assigned! This will cause issues later on! - " + gameObject.name);
            }

        }
        if (isPlayer && healthBar != null)
        { 
            healthBar.maxValue = maxHealth;
            healthBar.value = health;
        }
        inventory = this.gameObject.GetComponent<Inventory>();
        gameObject.layer = 9;
    }
    public void DropItem()
    {
        if (equippedItem == null) return; // we have nothing equipped so we can't drop it.
        MeshCollider itemCollider = equippedItem.GetComponent<MeshCollider>(); // grabbing the MeshCollider of object to drop
        ItemContainer equippedContainer = equippedItem.GetComponent<ItemContainer>(); // grabbing the ItemContainer script
        itemCollider.isTrigger = false; // Turning off the isTrigger on the equipped item's collider so it wont hurt us anymore
        this.equippedItem.transform.parent = null; // Removing the player as the parent of the item. 
        equippedContainer.isOut = false; // Returning this value to its default, as we aren't holding it out anymore
        equippedItem.AddComponent<Rigidbody>(); // Adding a rigidbody to give the object gravity and physics.
        equippedItem = null; // We're reseting our equipped item to nothing so we can pick up things again.   
    }
    public Weapon GetWeapon()
    {
        Weapon w = null;
        WeaponContainer wc = (WeaponContainer)equippedItem.GetComponent<ItemContainer>();
        w = wc.weapon;
        return w;
    }
    public void PickupItem(GameObject item) // item is the object we're picking up.
    {
        if (equippedItem == null) // If we don't already have something. If we do, we'll add it to inventory instead.
        {

            MeshCollider itemCollider = item.GetComponent<MeshCollider>(); 
            if (itemCollider != null) // if the object does have a MeshCollider
            {
                ItemContainer gameItem = item.gameObject.GetComponent<ItemContainer>();
                if (gameItem.isOut) return;
                gameItem.holder = this;
                gameItem.isOut = true; // Going to pick up the object, so it will be out.
                equippedItem = item;   // Setting equippeditem to the item we're picking up
                Destroy(item.GetComponent<Rigidbody>()); // Getting rid of the object's gravity.
                itemCollider.isTrigger = true; // Making the object a trigger so that it can do damage.
                item.transform.SetPositionAndRotation(itemHolder.transform.position, itemHolder.transform.rotation);
                // Set the position of the object we're picking up to the ItemHolder
                item.transform.parent = itemHolder.transform.parent.transform; // Make the object move with the player.  
            }
            else
            {
                print(item.gameObject.name + " doesn't contain a mesh collider"); // If the item doesn't have a mesh collider
            }                                                                     // we'll get errors later on, so we check
        }                                                                         // before picking it up.
        else
        {
            // add to inventory later!
            print("Adding to my fake inventory.");
            inventory.AddToInventory(item.GetComponent<ItemContainer>()); // A very temporary fix. Later on make PickupItem use the item script as a condition rather than the gameobject.
            return;
        }
    }
    public void CheckHealth()  // Anytime we change health we need to check this method.
    {
        if (health > maxHealth) health = maxHealth;
        if (isPlayer && healthBar != null) healthBar.value = health;
        if (health <= 0)
        {
            DropItem();
            var thisParent = this.gameObject.transform.parent.gameObject;
            if (thisParent != null)
            {
                Destroy(thisParent);
               
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
    public void RestoreHealth(int healthRestored)
    {
        health += healthRestored;
        CheckHealth();
    }
	public void TakeDamge(int damageTaken)
    {
        health -= damageTaken;
        CheckHealth();
    }
}
