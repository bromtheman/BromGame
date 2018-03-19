using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBlocks : MonoBehaviour
{
    public int Health = 0;
    public int Mass = 0;    
    private int MaxHealth = 0;
    private int StartingMass;
    

    public void Start()
    {

        MaxHealth = Health;
        StartingMass = Mass;
    }



    protected void TakeDamage(int Damage)
    {
        print(Health);
        Health = Health - Damage;
        CheckForDeath();
    }
    void CheckForDeath()
    {
        if(Health <= 0)
        {
            Destroy(this.gameObject);
        }


    }

}



