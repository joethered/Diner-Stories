using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Acts : MonoBehaviour {

	public void onClick(){

		//move to the multiple scenes
		Debug.Log("Let's begin.  Moving to the Scenes.");
		SceneManager.LoadScene("Acts");
	}
}
