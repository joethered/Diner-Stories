using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ActManager : MonoBehaviour {

	public Button back;
	public int act = 0;
	public Button buttonname;

	public void onClick(){

		back.interactable = false;
		act++;
		/*
		//choose an act
		if (buttonname.GetComponentInChildren<Text>().text = "Speed Date") {
			Debug.Log ("Moving to Speed Date.");
			SceneManager.LoadScene ("prototype");
			buttonname.interactable = false;

		} else if (buttonname.GetComponentInChildren<Text>().text = "Robbery") {
			Debug.Log ("Moving to Robbery.");
			SceneManager.LoadScene ("prototype");
			buttonname.interactable = false;

		} else if (buttonname.GetComponentInChildren<Text>().text = "Funeral") {
			Debug.Log ("Moving to Funeral.");
			SceneManager.LoadScene ("prototype");
			buttonname.interactable = false;
		
		}*/

		//temp code until transitions made proper
		Debug.Log ("Moving to the next scene.");
		SceneManager.LoadScene ("prototype");
		buttonname.interactable = false;
	}
}
