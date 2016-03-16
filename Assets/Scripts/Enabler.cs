using UnityEngine;
using System.Collections;

public class Enabler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gc.flipEnabled();
	}
	
}
