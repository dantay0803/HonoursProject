using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class scr_singleShot : MonoBehaviour {

    //Set the starting ammo for the magazineCount
    public int maxAmmo = 12;
    //Set how much ammo the guns magazine has
    int magazineCount;
    //Add a delay to the shots when firing
    public float fireRateDelay = 0.1f;
    //Keep track of the delay between shots
    float currentFireRateDelay = 0;
    //Hit raycast to detect collision between objects
    RaycastHit hit;
    //Get textfield used to display ammo in magazine
    public Text ammoDisplay;
    //Get audio source to play gun shot
    public AudioSource sfxSource;
    //Gun shot SFX
    public AudioClip gunShot;
    //Reload SFX
    public AudioClip reloadSFX;
    //Bool to track if the gun is currently being reloaded
    bool reloading = false;
    //How long the reload pause should last
    public float reloadTimer = 2.5f;
    //Check if player is aiming down sights
    bool isADS = false;
    //Crosshair UI object
    GameObject crosshair;
    //Store the gun and arm objects animator component
    Animation anim;


    bool playerIsCrouching = false;

    // Use this for initialization
    void Start(){
        //Set the amount of ammo in the magazine
        magazineCount = maxAmmo;
        //Display ammo currently in the magazine
        updateAmmoDisplay();
        //Get the crosshair UI object
        crosshair = GameObject.Find("crosshair");
        //Get the gun and arm objects animator component    
        anim = this.gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update(){
        //Player shooting
        shooting();
        //Reload gun
        reload();
        //Set the player to aim down sights of the gun
        playerADS();
        //Play gun sprint animation
        playSprintAnimation();
        //Check if the crouch button has been pressed
        checkForCrouchInput();
    }

    //Play SFX clip
    public void playSFXClip(AudioClip clip){
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    //Player shooting
    void shooting(){
        //Check for left mouse button down and the player has ammo and the current in game time is more than the delay timer
        if ((Input.GetButtonDown("Fire1") || Input.GetAxis("Fire1") > 0) && (magazineCount > 0) && (Time.realtimeSinceStartup > currentFireRateDelay) && !reloading){
            //Spawn a raycast from the center of the player camera
            Ray raycast = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            //Spawn hit raycast along the raycast being shot from the center of the camera
            if (Physics.Raycast(raycast, out hit)){
                //On collision with objects send the method to run from each object and do not require the objects to contain a recevier
                hit.collider.gameObject.SendMessage("detectHit", hit, SendMessageOptions.DontRequireReceiver);
            }
            //Check if the player is aiming down sights or not
            if(isADS){
                //If the player is aiming down sight play the aim down sight shoot animation
                this.gameObject.GetComponent<playAnimations>().playADSFire();
            }
            else{
                //If not play the normal shoot animation
                this.gameObject.GetComponent<playAnimations>().playFire();
            }
            //Reduce the ammo the player has in the magazineCount
            magazineCount--;
            //Update the ammo display
            updateAmmoDisplay();
            //PlayShotSound
            playSFXClip(gunShot);
            //If the player can shoot set the value of the fire delay for a slight break inbetween shots
            currentFireRateDelay = Time.realtimeSinceStartup + fireRateDelay;
        }
        //Automatically reload the gun if no ammo in magazine
        else if ((Input.GetButtonDown("Fire1") || Input.GetAxis("Fire1") > 0) && magazineCount <= 0 && Time.realtimeSinceStartup > currentFireRateDelay){
            //ReloadGun
            reloading = true;
            //PlayReloadSound
            playSFXClip(reloadSFX);
            //Play reload animation
            this.gameObject.GetComponent<playAnimations>().playReload();
        }
    }

    //Reload the magazine count
    void reload(){
        //If the player presses R reset the magazine count to 30 
        if ((Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton2)) && magazineCount < maxAmmo){
            //Play reload animation
            this.gameObject.GetComponent<playAnimations>().playReload();
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
                magazineCount = maxAmmo;
                //Update the ammo display
                updateAmmoDisplay();
                //Set player aiming down sights whilst reloading to false
                isADS = false;
                //Update the crosshair display
                toggleCrosshair();
            }
        }
    }

    //Display ammo currently in the magazine
    void updateAmmoDisplay(){
        //Display the amount of ammo in the magazine to the ammo text display
        ammoDisplay.text = magazineCount.ToString();
    }

    //Allow player to aim down sights of gun
    void playerADS(){
        if ((Input.GetButtonDown("ADS") || Input.GetAxis("ADS") > 0) && !reloading && !isADS){
            //Play gun ADS animations
            this.gameObject.GetComponent<playAnimations>().playADSIdle();
            //Set the player as aiming down sights
            isADS = true;
            //Update the crosshair display
            toggleCrosshair();

        }
        else if((Input.GetButtonUp("ADS") || Input.GetAxis("ADS") <= 0) && !reloading && isADS){
            //Play gun shooting animations
            this.gameObject.GetComponent<playAnimations>().playIdle();
            //Set the player as not aiming down sights
            isADS = false;
            //Update the crosshair display
            toggleCrosshair();
        }
    }

    //Check if the crosshair should be displayed or not
    void toggleCrosshair(){
        //If the player is aiming down sights hide the corsshair
        if(isADS){
            crosshair.SetActive(false);
        }
        //If they are not aiming down sight show crosshair 
        else{
            crosshair.SetActive(true);
        }
    }

    //Play sprint animation
    void playSprintAnimation(){
        //Ensure the player is not shooting or aiming down their sights or crouching to allow them to sprint
        if (!Input.GetButtonDown("Fire1") && Input.GetAxis("Fire1") <= 0 && !Input.GetButtonDown("ADS") && Input.GetAxis("ADS") <= 0 && !playerIsCrouching){
            //If the player is holding the sprint button set their speed to the sprinting speed
            if (Input.GetButton("Sprint") && (!anim.IsPlaying("M9-Reload") && !anim.IsPlaying("M9-Fire") && !anim.IsPlaying("M9-ADS-Fire") && !anim.IsPlaying("M9-ADS-Idle"))){
                this.gameObject.GetComponent<playAnimations>().playSprint();
            }
            //If not set their speed to the walking speed
            else if (!Input.GetButton("Sprint") && (!anim.IsPlaying("M9-Reload") && !anim.IsPlaying("M9-Fire") && !anim.IsPlaying("M9-ADS-Fire") && !anim.IsPlaying("M9-ADS-Idle"))){
                this.gameObject.GetComponent<playAnimations>().playIdle();
            }
        }
    }

    //Check if the crouch button has been pressed
    void checkForCrouchInput(){
        //If the crouch button is pressed and the player is not currently crouching set the player as crouching
        if(Input.GetButtonDown("Crouch") && !playerIsCrouching){
            playerIsCrouching = true;
        }
        //If the crouch button is pressed and the player is currently crouching set the player as standing
        else if (Input.GetButtonDown("Crouch") && playerIsCrouching){
            playerIsCrouching = false;
        }
        //If the player is crouching and the jump button is pressed set the player to standing
        if(playerIsCrouching && Input.GetButtonDown("Jump")){
            playerIsCrouching = false;
        }
    }

}
