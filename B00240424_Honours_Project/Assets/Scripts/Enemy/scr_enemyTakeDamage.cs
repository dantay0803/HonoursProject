using UnityEngine;
using System.Collections;

public class scr_enemyTakeDamage : MonoBehaviour {

    //Set the enemy health
    int health = 100;
    //Set the damage of the player
    int playerDamage = 10;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Run when enemy has been hit by the players raycast
    void detectHit(RaycastHit hit){
        //Apply damage to object after being shot
        applyDamage();
    }

    //Reduce enemy health when shot
    void applyDamage(){
        //Reduce the enemy objects health
        health -= playerDamage;

        //Destroy object when health is 0 or less
        if(health <= 0){
            Destroy(gameObject);
        }
    }
}
