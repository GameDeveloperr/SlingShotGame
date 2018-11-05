using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingCtrl : MonoBehaviour {

    public float startingHealth;
    public float health;
    public bool dead;

    public event System.Action onDeath;

    protected virtual void Start()
    {
        dead = false;
        startingHealth = health;
    }

    public void TakeHit(float damage)
    {
        health -= damage;
        Debug.Log(this.gameObject.name + " hit" + health);

        if (health <= 0 && dead == false)
        {
            Debug.Log(this.gameObject.name+" dead"+health);
            Die();
        }
    }

    protected void Die()
    {
        dead = true;

        //이렇게 조건을 달면 일반 함수처럼 델리게이를 사용하듯이 사용할수 있다.
        if (onDeath != null)
        {
            onDeath();
        }
        if(GetComponent<CapsuleCollider>())
        {
            Destroy(GetComponent<CapsuleCollider>());
        }
        if(GetComponent<BoxCollider>())
        {
            Destroy(GetComponent<BoxCollider>());
        }
        if(GetComponent<Rigidbody>())
        {
            Destroy(GetComponent<Rigidbody>());
        }
        if(GetComponent<SphereCollider>())
        {
            Destroy(GetComponent<SphereCollider>());
        }
        GameObject.Destroy(gameObject, 1.5f);
    }

}
