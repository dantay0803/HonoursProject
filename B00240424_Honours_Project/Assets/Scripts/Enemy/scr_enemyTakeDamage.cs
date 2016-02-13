using UnityEngine;
using System.Collections;

public class scr_enemyTakeDamage : MonoBehaviour {

    //Set the enemy health
    int health = 100;
    //Set the damage of the player
    int playerDamage = 10;

    //Blood splatter particle array
    public GameObject bloodSplatterParticle;
    //Create and array of objects to hold the blood particles 
    GameObject[] bloodSplatter;
    //The currently selected blood splatter object in the array
    int currentBloodSplatterObject = 0;
    //How many blood splatter objects to create 
    int maxBloodSplatterObjects = 6;

    // Use this for initialization
    void Start () {
        //Create a pool of blood splatter particle systems
        createBloodSplatterPool();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //Run when enemy has been hit by the players raycast
    void detectHit(RaycastHit hit){
        //Apply damage to object after being shot
        applyDamage();
        //Play the blood splatter particle effect where the enemy gets shot
        playBloodSplatterParticleEffect(hit);
    }

    //Reduce enemy health when shot
    void applyDamage(){
        //Reduce the enemy objects health
        health -= playerDamage;
        Debug.Log("Health: " + health);
        //Destroy object when health is 0 or less
        if(health <= 0){
            Destroy(gameObject);
        }
    }

    //Create a pool of blood splatter particle systems
    void createBloodSplatterPool(){
        //Create a new game object array the size of the max blood splatter object value
        bloodSplatter = new GameObject[maxBloodSplatterObjects];
        //Set each of the objects in the array as the blood splatter particle prefab
        for (int i = 0; i < maxBloodSplatterObjects; i++){
            bloodSplatter[i] = (GameObject)Instantiate(bloodSplatterParticle);
        }
    }

    //Play the blood splatter particle effect where the enemy gets shot
    void playBloodSplatterParticleEffect(RaycastHit hit){
        //Position the blood splatter particle object to where the players raycast hits the enemy
        bloodSplatter[currentBloodSplatterObject].transform.position = hit.point;
        //Play the particle system animation
        bloodSplatter[currentBloodSplatterObject].GetComponent<ParticleSystem>().Play();
        //Select the next particle object and when the next object is the last in the array restart the selection at the begining of the array. 
        if (++currentBloodSplatterObject >= maxBloodSplatterObjects){
            currentBloodSplatterObject = 0;
        }
    }
}
