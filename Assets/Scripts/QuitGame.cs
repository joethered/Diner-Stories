using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour {

	// Use this for initialization
	public void onClick(){

		//close game
		Debug.Log ("Quitting Game Now!");
		Application.Quit ();
	}
}
