using UnityEngine;
using System.Collections;

public class EnemyT : MonoBehaviour {
	public float delay = 0.5f;

	public GameObject player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		// :::::::::::::::FOLLOW:::::::::::::::::::::::::::
		
		if (player == null) return;
		Vector3 eneps = transform.position;
		eneps += (player.transform.position-eneps)*Time.deltaTime*delay;
		eneps.z = transform.position.z;
		transform.position = eneps;
		// ::::::::::::::::::::::::::::::::::::::::::::::::


	}

	void OnTriggerEnter2D (Collider2D coll)
	{

		if (coll.tag=="Sword")
		{
			Destroy(gameObject);
		}
		if (coll.tag=="Player")
		{
			test jugador = coll.gameObject.GetComponent<test>();
			jugador.life -= 1;
		}

	}
}
