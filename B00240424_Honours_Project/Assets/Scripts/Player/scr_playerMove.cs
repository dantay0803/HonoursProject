using UnityEngine;
using System.Collections;

public class scr_playerMove : MonoBehaviour {
    //Get the character controller component of the player object
    CharacterController charCont;
    //PlayerMovementSpeed
    const float playerWalkSpeed = 5;
    //Player sprint speed
    const float playerSprintSpeed = 9;
    //set the current movement speed of the player
    float movementSpeed = 0;
    //Set the sensitivity of the mouse for moving the camera
    const float mouseSensitivity = 5;
    //The vertical rotation of the camera
    float rotVert = 0;
    //Set the maximum range the camera can move to vertically
    const float upDownRange = 60;
    //Player position
    Vector3 updatePlayer;
    //Set the jump height
    const float jumpHeight = 4;


    bool isCrouching = false;


    // Use this for initialization
    void Start () {
        //Get the character controller component of the player object
        charCont = GetComponent<CharacterController>();
        //Initilise the player speed as walking
        movementSpeed = playerWalkSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        //MovePlayer
        playerMovement();
        //RotatePlayerCamera
        cameraRot();
        //Check if player wants to sprint
        playerSprint();
        //Allow player to switch between standing and crouching
        toggleCrouch();
    }

    //Rotate camera with mouse movement
    void cameraRot(){
        //Get the raw mouse x movement
        float rotHor = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        //Update the y rotation of the player 
        transform.Rotate(0, rotHor, 0);
        //Get the mouse vertical movement for moving the camera up and down
        rotVert -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        //Limit how far the camera can rotate up and down
        rotVert = Mathf.Clamp(rotVert, -upDownRange, upDownRange);
        //Update the rotational values of the camera
        Camera.main.transform.localRotation = Quaternion.Euler(rotVert, rotHor, 0);
    }

    //CheckForPlayerMovement
    void playerMovement(){
        //Check player is on the ground 
        if(charCont.isGrounded){
            //Move player forwards or backwards by getting Unity's vertical input which will be between -1 - 1
            float vertInput = Input.GetAxisRaw("Vertical") * movementSpeed;
            //Move player side to side by getting Unity's horizontal input which will be between -1 - 1
            float hortInput = Input.GetAxisRaw("Horizontal") * movementSpeed;
            //Create a vector and add in the values use to update the players position
            updatePlayer = new Vector3(hortInput, 0, vertInput);
            //Update the players position by checking the rotation of the player
            updatePlayer = transform.rotation * updatePlayer;
            //Check for spacebar down to let player jump
            if(Input.GetButtonDown("Jump")){
                //If player is not crouching allow them to jump
                if (!isCrouching){
                    //set the Y position of the player to the value of how heigh the player can jump
                    updatePlayer.y = jumpHeight;
                }
                //If the player is crouching make them stand
                else if(isCrouching){
                    isCrouching = false;
                    updatePlayerCrouching();
                }
            } 
        }
        //Add gravity to the player
        updatePlayer.y += Physics.gravity.y * Time.deltaTime;
        //Run the move function passing it the values to be added to the player position
        charCont.Move(updatePlayer * Time.deltaTime);
    }

    //Check for the player sprint input
    void playerSprint(){
        //Ensure the player is not shooting or aiming down their sights or crouching to allow them to sprint
        if(Input.GetButtonDown("Fire1") || Input.GetAxis("Fire1") > 0 || Input.GetButtonDown("ADS") || Input.GetAxis("ADS") > 0 || isCrouching){
            //If the player is aiming down sights or shooting only allow them to shoot
            if(movementSpeed != playerWalkSpeed){
                movementSpeed = playerWalkSpeed;
            }    
        }
        //If the player is not aiming down sights or shooting allow them to sprint
        else{
            //If the player is holding the sprint button set their speed to the sprinting speed
            if(Input.GetButton("Sprint") && movementSpeed != playerSprintSpeed){
                movementSpeed = playerSprintSpeed;
            }
            //If not set their speed to the walking speed
            else if(!Input.GetButton("Sprint") && movementSpeed != playerWalkSpeed){
                movementSpeed = playerWalkSpeed;
            }
        }
    }

    //Allow player to switch between standing and crouching
    void toggleCrouch(){
        //If the player is not crouching and the crouch button is pressed set the player as crouching
        if (Input.GetButtonDown("Crouch") && !isCrouching){
            isCrouching = true;
            updatePlayerCrouching();
        }
        //if the player is crouching and the crouch button is pressed make the player stand.
        else if (Input.GetButtonDown("Crouch") && isCrouching){
            isCrouching = false;
            updatePlayerCrouching();
        }
    }

    //Toggle the player between standing and crouching
    void updatePlayerCrouching(){
        //Move the camera down 1 unit to look like the player is crouching
        if (isCrouching){
            Vector3 playerPos = Camera.main.transform.position;
            playerPos.y -= 1;
            Camera.main.transform.position = playerPos;
        }
        //Move the camera up 1 unit to look like the player has stood up
        else if (!isCrouching){
            Vector3 playerPos = Camera.main.transform.position;
            playerPos.y += 1;
            Camera.main.transform.position = playerPos;
        }
    }

}
