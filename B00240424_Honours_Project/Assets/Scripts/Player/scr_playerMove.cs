using UnityEngine;
using System.Collections;

public class scr_playerMove : MonoBehaviour {
    //Get the character controller component of the player object
    CharacterController charCont;
    //PlayerMovementSpeed
    float playerSpeed = 5;
    //Set the sensitivity of the mouse for moving the camera
    float mouseSensitivity = 5;
    //The vertical rotation of the camera
    float rotVert = 0;
    //Set the maximum range the camera can move to vertically
    float upDownRange = 60;
    //Player position
    Vector3 updatePlayer;
    //Set the jump height
    float jumpHeight = 4;

    // Use this for initialization
    void Start () {
        //Get the character controller component of the player object
        charCont = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        //MovePlayer
        playerMovement();
        //RotatePlayerCamera
        cameraRot();
        //PlayerJump
        playerJumping();
    }

    //Player jump
    void playerJumping(){
        //Ensure player is on the ground
        if (charCont.isGrounded){
            //Check for jump button input
            if (Input.GetKeyDown(KeyCode.Space)){
                
            }
        }
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
            float vertInput = Input.GetAxisRaw("Vertical") * playerSpeed;
            //Move player side to side by getting Unity's horizontal input which will be between -1 - 1
            float hortInput = Input.GetAxisRaw("Horizontal") * playerSpeed;
            //Create a vector and add in the values use to update the players position
            updatePlayer = new Vector3(hortInput, 0, vertInput);
            //Update the players position by checking the rotation of the player
            updatePlayer = transform.rotation * updatePlayer;
            //Check for spacebar down to let player jump
            if(Input.GetKeyDown(KeyCode.Space)){
                //set the Y position of the player to the value of how heigh the player can jump
                updatePlayer.y = jumpHeight;
            } 
        }
        //Add gravity to the player
        updatePlayer.y += Physics.gravity.y * Time.deltaTime;
        //Run the move function passing it the values to be added to the player position
        charCont.Move(updatePlayer * Time.deltaTime);
    }
}
