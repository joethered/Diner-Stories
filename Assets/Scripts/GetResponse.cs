using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetResponse : MonoBehaviour {

	public static int awkpts = 0;
	public static int forpts = 0;
	public static int witpts = 0;

	void start(){

		awkpts = PlayerPrefs.GetInt ("awkward");
		forpts = PlayerPrefs.GetInt ("formal");
		witpts = PlayerPrefs.GetInt ("witty");
	}

	public void onClick(RectTransform text){

		Debug.Log ("You clicked a button.");

		Debug.Log ("You made the " + text.name + " choice.");

		switch(text.name){

		case "Witty":
			witpts += 5;
			forpts -= 3;
			awkpts -= 2;
			Debug.Log (witpts);
			break;
		
		case "Formal":
			forpts += 5;
			awkpts -= 3;
			witpts -= 2;
			Debug.Log (forpts);
			break;

		case "Awkward":
			awkpts += 5;
			witpts -= 3;
			forpts -= 2;
			Debug.Log (awkpts);
			break;
		}

		PlayerPrefs.SetInt ("awkward", awkpts);
		PlayerPrefs.SetInt ("formal", forpts);
		PlayerPrefs.SetInt ("witty", witpts);
	}
}
