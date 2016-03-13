using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BacktoStart : MonoBehaviour {

	public void onClick(){

		//move back to the start menu
		Debug.Log("Going back to the Start Menu.");
		SceneManager.LoadScene ("Start Menu");
	}
}
