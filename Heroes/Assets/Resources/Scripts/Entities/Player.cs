using UnityEngine;
using System.Collections;

public class Player : Entity {
    [Range(0.5f, 20.0f)]
    public float speed;
    [Range(0.1f, 2.0f)]
    public float camSpeed = 1.0f;
    public int energy = 100;
    public int energyPerSecond = 2;
    //StatsPlayer
    public static int currentEnergy;
    //AnimatorInfo------------
    public static bool running;
    public static int dir_running;
    public static int dir_aiming;
    //AnimatorInfo------------
    private Vector2 camTarget;
    public static Camera playerCam;
    public static Vector2 pad_left;
    public static Vector2 pad_right;
    public static Vector2 pad_aim;
    public static Weapon weapon;
    //Components---------------
    public static Player player;
    //Components---------------
    
    public override void My_Start () {
        player = GameObject.FindObjectOfType<Player>() as Player;
        playerCam = Camera.main;
        currentEnergy = energy;
        StartCoroutine(EnergyController());
        new Idle(this.gameObject);
        new Aim(this.gameObject);
	}
	public override void My_Update () {
        InputController();
        CameraController();
        WeaponController();
	}
    //INPUT::::::::::::::::::::::::::::::+
    private void InputController ()
    {
        Pads();
        if (pad_left.sqrMagnitude > 1.0f) {
            new Run(this.gameObject);
        }
        if (Input.GetButtonDown("RB") && weapon != null)
            weapon.Attack();
        animator.SetInteger("dir", dir_aiming);
       // animator.SetBool("running", running);
    }
    private void Pads ()
    {
        pad_left = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        pad_right = new Vector2(Input.GetAxis("Right_Horizontal"), Input.GetAxis("Right_Vertical"));
        pad_aim = pad_right.sqrMagnitude > 1.0f ? pad_right : Dir(dir_aiming);
    }
    //INPUT::::::::::::::::::::::::::::::+
    //Camera:::::::::::::::::::
    private Vector2 MainTarget ()
    {
        Vector2 output = pad_right.sqrMagnitude > 1.0f ? (Vector2)transform.position + pad_right * 0.5f : (Vector2)transform.position;
        return output;
    }
    private void CameraController ()
    {
        camTarget = MainTarget();
        Vector3 plus = ((Vector3)camTarget-playerCam.transform.position) * Time.deltaTime * camSpeed;
        plus.z = 0.0f;
        playerCam.transform.position += plus;
    }
    //Camera:::::::::::::::::::
    //weapon::::::::::::::::::
    private void WeaponController()
    {
        if (weapon == null) return;
        Transform weaponTransform = weapon.instance.transform;
        Vector2 rot_dir = pad_aim;
        Quaternion target = Quaternion.LookRotation(Vector3.forward , rot_dir);
        weaponTransform.rotation = Quaternion.Slerp(weaponTransform.rotation, target, 10.0f * Time.deltaTime);
        weaponTransform.position = handPos(0) + ((Vector2)weaponTransform.up * weapon.size.y * 0.5f);
    }
    private Vector2 handPos (int hand)
    {
        Vector2 plus = hand == 0 ? transform.right : transform.right * -1;
        return (Vector2)transform.position;
    }
    //weapon::::::::::::::::::
    private IEnumerator EnergyController ()
    {
        float rate = 1.0f / (float)energyPerSecond;
        while (true) {
            yield return new WaitForSeconds(rate);
            if (currentEnergy < energy)
                currentEnergy++;
        }
    }
    public static void DrainEnergy (int val)
    {
        currentEnergy -= val;
        if (currentEnergy < 0) currentEnergy = 0;
    }
    public class Idle : Action
    {
        public Idle (GameObject user)
        {
            Create(user, "Idle", "Default", 1);
        }
        public override void OnStart()
        {
          //  Debug.Log("IdleStart");
        }
        public override void OnUpdate()
        {
            Player.my_rigidbody.AddForce(Vector2.zero);
            running = false;
        }
        public override bool Goal ()
        {
            return false;
        }
        public override bool Break ()
        {
            return false;
        }
    }
    public class Run : Action
    {
        public Run(GameObject user)
        {
            Create(user, "Run", "Default", 2, 0);
        }
        public override void OnStart()
        {
        //    Debug.Log("runningStart");
        }
        public override void OnUpdate()
        {
            Player.my_rigidbody.AddForce(pad_left.normalized*player.speed);
            dir_running = Player.Dir(pad_left);
            running = true;
        }
        public override bool Goal()
        {
            return false;
        }
        public override bool Break()
        {
            return pad_left.sqrMagnitude <= 1.0f;
        }
    }
    public class Aim : Action
    {
        public Aim (GameObject user)
        {
            Create(user, "Aim", "Default", 0);
        }
        public override void OnStart()
        {
            //    Debug.Log("runningStart");
        }
        public override void OnUpdate()
        {
            if (pad_right.sqrMagnitude > 1.0f)
                dir_aiming = Player.Dir(pad_right.normalized);
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
    public static int Dir (Vector2 dir)
    {
        return Mathf.Abs(dir.normalized.x) > Mathf.Abs(dir.normalized.y) ? dir.normalized.x > 0.0f ? 3 : 1 : dir.normalized.y > 0.0f ? 2 : 0;
    } 
    public static Vector2 Dir (int dir)
    {
        return dir == 0 ? new Vector2(0.0f, -1.0f) : dir == 1 ? new Vector2(-1.0f, 0.0f) : dir == 2 ? new Vector2(0.0f, 1.0f) : new Vector2(1.0f, 0.0f);
    }
}
