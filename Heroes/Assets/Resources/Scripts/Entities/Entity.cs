using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {
    //Stats
    public int life;
    //Stats
    public static Rigidbody2D my_rigidbody;
    public static Animator animator;
    // Use this for initialization
    void Start () {
        my_rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        My_Start();
    }
	public virtual void My_Start ()
    {

    }
	void Update () {
        My_Update();
	}
    public virtual void My_Update ()
    {

    }
    public void TakeDamage (int damage, float weight, Transform dealer)
    {
        life -= damage;
        life = life > 0 ? life : 0;
        if (life == 0) Die(dealer.position, weight); else Push(dealer.position, weight);
    }
    public void Push (Vector2 origin, float weight)
    {
        Vector2 delta = (Vector2)transform.position - origin;
        my_rigidbody.AddForce(delta.normalized * weight, ForceMode2D.Impulse);
    }
    public void Die (Vector2 origin, float weight)
    {
        Vector2 delta = (Vector2)transform.position - origin;
        my_rigidbody.AddForce(delta.normalized * weight * 2.0f, ForceMode2D.Impulse);
    }
    public void Blood(Vector2 dir, float mount)
    {

    }
    public void Blood (Vector2 dir, float mount, Color color)
    {

    }
}
