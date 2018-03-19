using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShell : MonoBehaviour {


    public int shellDamage = 10;
    public float speed = 5f;
            
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 forward = new Vector2(0, speed) * Time.deltaTime;
        transform.Translate(forward);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {

        print("I hit: " + collision.gameObject.name);
        SendMessageUpwards("base.TakeDamage", shellDamage, SendMessageOptions.DontRequireReceiver);


    }


}
