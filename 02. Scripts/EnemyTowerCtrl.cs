using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTowerCtrl : LivingCtrl {

    public Transform[] CreatePos;
    public Transform Enemy;

    public Transform DieEffect;

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.UIManager.E_TowerHPset();
        onDeath += Death;
        StartCoroutine(Action());
    }

    void Death()
    {
        for(int i = 0; i < 5; i++)
        {
            float x_pos = Random.Range(-3.0f, 3.0f);
            float y_pos = Random.Range(-3.0f, 3.0f);

            Vector3 c_pos = transform.position;
            c_pos.x += x_pos;
            c_pos.y += y_pos;

            Transform ex = Instantiate(DieEffect, c_pos, Quaternion.identity);
            Destroy(ex.gameObject, 2.0f);
        }
        StopCoroutine(Action());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RED_SPEAR")//창에 맞을 경우
        {
            TakeHit(other.gameObject.GetComponentInParent<RedUnitCtrl>().damage);
            GameManager.Instance.UIManager.E_TowerDamage(other.gameObject.GetComponentInParent<RedUnitCtrl>().damage);
        }
    }

    IEnumerator Action()
    {
        while (true)
        {
            int Count = Random.Range(1, 5);
            ResponEnemy(Count);

            yield return new WaitForSeconds(5.0f);
        }
    }

    public void ResponEnemy(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int pos_num = Random.Range(0, 3);

            float x_pos = Random.Range(-3.0f, 3.0f);
            float z_pos = Random.Range(-3.0f, 3.0f);

            Vector3 c_pos = CreatePos[pos_num].position;
            c_pos.x += x_pos;
            c_pos.z += z_pos;
            Instantiate(Enemy, c_pos, CreatePos[pos_num].rotation);
        }
    }
}
