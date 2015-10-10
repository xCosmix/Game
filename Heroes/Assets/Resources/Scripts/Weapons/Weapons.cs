using UnityEngine;
using System.Collections;

public class Sword : Weapon
{
    public Sword ()
    {
        name = "Sword";
        damage = 1;
        coldDown = 0.3f;
        energy = 6;
        weight = 1.0f;
        graphic = Resources.Load<Sprite>("Sprites/Weapon_Sprites/Sword_sprite");
        action_A = new Slash();
        action_B = new Slash();
    }
    public override void Equip()
    {
        base.Equip();
    }
    public class Slash : WeaponAction
    {
        public static float duration = 0.5f;
        private static int myFrames;
        private static int frame_hitEvent;
        public override bool Use(Weapon weapon)
        {
            if (!base.Use(weapon)) return false;
            myFrames = Mathf.RoundToInt(duration / Time.fixedDeltaTime);
            frame_hitEvent = Mathf.RoundToInt(0.2f / Time.fixedDeltaTime);
            Create(Player.player.gameObject, "Sword_Attack", "Combat", 1, myFrames);
            return true;
        }
        public override void OnStart()
        {
             
        }
        public override void OnUpdate()
        {
            if (AtFrame(frame_hitEvent)) {
                //Debug.Log("ñaña");
                Player.my_rigidbody.AddForce(Player.pad_aim.normalized, ForceMode2D.Impulse);
            }
            if (AfterFrame(frame_hitEvent) || AtFrame(frame_hitEvent))
            {
                Collider2D[] colls = Physics2D.OverlapCircleAll(Player.player.transform.position, 10.0f);
            }
        }
        public override void OnEnd()
        {
            base.OnEnd();
        }
        public override bool Goal()
        {
            return false;
        }
        public override bool Break()
        {
            return false;
        }
    }
}
