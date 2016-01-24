using UnityEngine;
using System.Collections;

public class scr_global : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Lock cursor to screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);
    }
	
	// Update is called once per frame
	void Update () {
        //UnlockPlayerMouse
        unlockMouse();
    }

    //Allow player to unlock their mouse
    void unlockMouse(){
        if (Input.GetKeyDown(KeyCode.Escape)){
            //Lock cursor to screen
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = (true);
        }
    }
}
