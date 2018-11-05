using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTower : MonoBehaviour {

    public GameObject Camera;

    public Transform DieEffect;

    private void Start()
    {
        Camera.SetActive(false);
    }

    private void CameraOn()
    {
        Camera.SetActive(true);
    }
    private void CameraOff()
    {
        Camera.SetActive(false);
    }

    public void Fire()
    {
        CameraOn();
        Collider[] coll = Physics.OverlapSphere(this.gameObject.transform.position, 10.0f);
        foreach (Collider coll_ in coll)
        {
            if (coll_.CompareTag("Enemy"))
            {
                EnemyCtrl enemy = coll_.GetComponent<EnemyCtrl>();
                enemy.TakeHit(enemy.health);
                Transform ex = Instantiate(DieEffect, coll_.gameObject.transform.position, Quaternion.identity);
                Destroy(ex.gameObject, 1.5f);
            }
        }
        Invoke("CameraOff", 1.0f);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawSphere(this.gameObject.transform.position, 10.0f);
    //}
}
