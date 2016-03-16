using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BacktoScene : MonoBehaviour {

	public void onClick(){

		//move back to the scene menu
		Debug.Log("Going back to the Scene Menu.");
		SceneManager.LoadScene ("Acts");
	}
}
