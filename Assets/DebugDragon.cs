using UnityEngine;
using System.Collections;

public class DebugDragon : MonoBehaviour {

    // Use this for initialization
    void Start() {
       
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(transform.position.x + " " + transform.position.y);
    }
}
