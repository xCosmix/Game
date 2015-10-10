using UnityEngine;
using System.Collections;

public class Weapon_Item : MonoBehaviour {
    public Weapon.weaponEnum weapon;
    private new Collider2D collider;
    private Weapon weaponInstance;
    private bool collisioning;
	// Use this for initialization
	void Start () {
        collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        SetWeapon();
	}
	private void SetWeapon ()
    {
        switch (weapon)
        {
            case (Weapon.weaponEnum.sword):
                weaponInstance = new Sword();
                break;
        }
    }
	// Update is called once per frame
	void Update () {
        if (collisioning && Input.GetButtonDown("X"))
            pickUp();
	}
    private void pickUp ()
    {
        weaponInstance.Equip();
        DestroyImmediate(gameObject);
    }
    void OnTriggerEnter2D (Collider2D coll)
    {
        Player player = coll.GetComponent<Player>();
        if (player == null) return;
        collisioning = true;
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        Player player = coll.GetComponent<Player>();
        if (player == null) return;
        collisioning = false;
    }
}
