using UnityEngine;
using System.Collections;

public class Weapon : System.Object {
    public enum weaponEnum { sword }
    public string name;
    public byte damage;
    public float weight;
    public int energy;
    public float coldDown;
    public Sprite graphic;
    public GameObject instance;
    public SpriteRenderer spriteRender;
    public Vector2 size;
    public WeaponAction action_A;
    public WeaponAction action_B;
    public float usedTime;
    public virtual void Equip ()
    {
        instance = new GameObject(name + "_Instance");
        instance.transform.position = Player.player.transform.position;
        instance.transform.SetParent(Player.player.transform);
        spriteRender = instance.AddComponent<SpriteRenderer>();
        spriteRender.sortingLayerName = "Weapons";
        spriteRender.sprite = graphic;
        size = new Vector2(graphic.rect.width / 100.0f, graphic.rect.height / 100.0f);
        Player.weapon = this;
    }
    public void Attack () { action_A.Use(this); }
    public void Special () { action_B.Use(this); }
    public class WeaponAction : Action
    {
        private Weapon myWeapon;
        public virtual bool Use(Weapon weapon)
        {
            myWeapon = weapon;
            if ((Time.time - myWeapon.usedTime) < myWeapon.coldDown || myWeapon.energy > Player.currentEnergy) return false;
            return true;
        }
        public override void OnEnd()
        {
            myWeapon.usedTime = Time.time;
            Player.DrainEnergy(myWeapon.energy);
        }
        //stuff polymorph
    }
}
