using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ActManager : MonoBehaviour {

	public Button back;
	public static int act = 0;
	public Button button1;
	public Button button2;
	public Button button3;
	public static int x = 0;
	public static int y = 0;
	public static int z = 0;


	void start(){
		act = PlayerPrefs.GetInt ("scene");
		x = PlayerPrefs.GetInt ("spdate");
		y = PlayerPrefs.GetInt ("rob");
		z = PlayerPrefs.GetInt ("fun");
		if (act >= 1) {
			back.interactable = false;
		} else if (act >= 3) {
			back.interactable = false;
			Debug.Log ("Thanks for playing.");
			Application.Quit ();
		}
		if (x >= 1) {
			button1.interactable = false;
		}
		if (y >= 1) {
			button2.interactable = false;
		}
		if (z >= 1) {
			button3.interactable = false;
		}
	}


	public void onClick(RectTransform leftclick){

		back.interactable = false;
		act++;
		Debug.Log ("Act " + act);

		PlayerPrefs.SetInt ("scene", act);

		switch (leftclick.name){

		//choose an act
		case "Speed Date":
			Debug.Log ("Moving to Speed Date.");
			button1.interactable = false;
			PlayerPrefs.SetInt ("spdate", 1);
			x = 1;
			SceneManager.LoadScene ("Scene UI");
			break;

		case "Robbery":
			Debug.Log ("Moving to Robbery.");
			button2.interactable = false;
			PlayerPrefs.SetInt ("rob", 1);
			y = 1;
			SceneManager.LoadScene ("Scene UI");
			break;

		case "Funeral":
			Debug.Log ("Moving to Funeral.");
			button3.interactable = false;
			PlayerPrefs.SetInt ("fun", 1);
			z = 1;
			SceneManager.LoadScene ("Scene UI");
			break;

		}
	}
}
