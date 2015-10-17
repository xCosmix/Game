using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GIUTes : MonoBehaviour {
	public test player;
	public Image[] life = new Image[3];
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		int invisibles = 3 - player.life;
		int index = life.Length;
		for (int i = invisibles; i>0; i--)
		{
			index--;
			life[index].gameObject.SetActive(false);
		}
	}
}
