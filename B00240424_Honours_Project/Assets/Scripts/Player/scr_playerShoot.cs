using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scr_playerShoot : MonoBehaviour {

    //Set the starting ammo for the magazineCount
    public int magazineCount = 30;
    //Add a delay to the shots when firing
    public const float fireRateDelay = 0.1f;
    //Keep track of the delay between shots
    float currentFireRateDelay = 0;
    //Hit raycast to detect collision between objects
    RaycastHit hit;
    //Get textfield used to display ammo in magazine
    public Text ammoDisplay;
    //Get audio source to play gun shot
    public AudioSource sfxSource;
    //Assualt Rifle SFX
    public AudioClip gunShot;
    //Reload SFX
    public AudioClip reloadSFX;
    //Bool to track if the gun is currently being reloaded
    bool reloading = false;
    //How long the reload pause should last
    float reloadTimer = 2.5f;

	// Use this for initialization
	void Start () {
        //Display ammo currently in the magazine
        updateAmmoDisplay();
    }
	
	// Update is called once per frame
	void Update () {
        //Player shooting
        shooting();
        //Reload gun
        reload();
    }

    //Play SFX clip
    public void playSFXClip(AudioClip clip){
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    //Player shooting
    void shooting(){
        //Check for left mouse button down and the player has ammo and the current in game time is more than the delay timer
        if (Input.GetMouseButton(0) && magazineCount > 0 && Time.realtimeSinceStartup > currentFireRateDelay){
            //If the player can shoot set the value of the fire delay for a slight break inbetween shots
            currentFireRateDelay = Time.realtimeSinceStartup + fireRateDelay;
            //Reduce the ammo the player has in the magazineCount
            magazineCount--;
            //Update the ammo display
            updateAmmoDisplay();
            //Spawn a raycast from the center of the player camera
            Ray raycast = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            //Spawn hit raycast along the raycast being shot from the center of the camera
            if (Physics.Raycast(raycast, out hit)){
                //On collision with objects send the method to run from each object and do not require the objects to contain a recevier
                hit.collider.gameObject.SendMessage("detectHit", hit, SendMessageOptions.DontRequireReceiver);
            }
            //PlayShotSound
            playSFXClip(gunShot);
        }
        //Automatically reload the gun if no ammo in magazine
        else if (Input.GetMouseButton(0) && magazineCount <= 0 && Time.realtimeSinceStartup > currentFireRateDelay){
            magazineCount = 30;
            //Update the ammo display
            updateAmmoDisplay();
        }
    }

    //Reload the magazine count
    void reload(){
        //If the player presses R reset the magazine count to 30 
        if(Input.GetKeyDown(KeyCode.R) && magazineCount < 30){
            //PlayReloadSound
            playSFXClip(reloadSFX);
            //Set reloading bool to true in order to play reload animation
            reloading = true;
        }
        //If the gun is being reloaded start timer to add delay for reload animation and ammo display update
        if (reloading && reloadTimer > 0){
            //Reduce timer
            reloadTimer -= Time.deltaTime;
            if (reloadTimer <= 0){
                //Set gun as not reloading
                reloading = false;
                //Reset reload timer
                reloadTimer = 3;
                //Update the ammo count value
                magazineCount = 30;
                //Update the ammo display
                updateAmmoDisplay();
            }
        }
    }

    //Display ammo currently in the magazine
    void updateAmmoDisplay(){
        //Display the amount of ammo in the magazine to the ammo text display
        ammoDisplay.text = magazineCount.ToString();
    }
}
