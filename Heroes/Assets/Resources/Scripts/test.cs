using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
	public string mensaje= "Hola, estoy vivo";
	public string mensaje2= "Estoy ejecutando";

	public GameObject miCam;

	public float velocidad = 1.0f;
	public float delay = 2.0f;
	public int life = 3;
	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
		// :::::::::::::::MOVE KNIFE:::::::::::::::::::::::::::

		if (Input.GetKey(KeyCode.W))
		{
			Vector3 pos = transform.position;
			pos.y += velocidad * Time.deltaTime;
			transform.position = pos;
		}
		if (Input.GetKey(KeyCode.S))
		{
			Vector3 pos = transform.position;
			pos.y -= velocidad * Time.deltaTime;
			transform.position = pos;
		}
		if (Input.GetKey(KeyCode.D))
		{
			Vector3 pos = transform.position;
			pos.x += velocidad * Time.deltaTime;
			transform.position = pos;
		}
		if (Input.GetKey(KeyCode.A))
		{
			Vector3 pos = transform.position;
			pos.x -= velocidad * Time.deltaTime;
			transform.position = pos;
		}
		// ::::::::::::::::::::::::::::::::::::::::::::::::

		// :::::::::::::::CAMERA:::::::::::::::::::::::::::
		
		//miCam.transform.position = transform.position;
		Vector3 camps = miCam.transform.position;
		camps += (transform.position-camps)*Time.deltaTime*delay;
		camps.z = transform.position.z - 1.0f;
		miCam.transform.position = camps;
		// ::::::::::::::::::::::::::::::::::::::::::::::::


		// :::::::::::::::DIE:::::::::::::::::::::::::::
		if (life<=0) 
		{ 
			Destroy(gameObject);
		}
		// ::::::::::::::::::::::::::::::::::::::::::::::::


	}
}
	
