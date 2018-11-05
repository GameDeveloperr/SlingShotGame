using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum e_BoomType
{
    None,Boom, BigBoom
};

public class BigBoomCtrl : MonoBehaviour
{

    public Transform Explosion;
    private PlayerCtrl player;
    private float damage = 100.0f;
    //포물선
    float fv_x;      // x축으로 속도
    float fv_y;      // y축으로 속도
    float fv_z;      // z축으로 속도

    float fg;        // y축으로의 중력 가속도
    float fEndTime;  // 도착지점도달시간
    float fMaxHeight = 10.0f;   //최대 높이
    float fHeight;   //최대 높이의 y - 시작높이의 y
    float fEndHeight;   //도착지점높이 y - 시작지점 높이 y
    public float fTime = 0.0f; // 흐르는 시간
    float fmaxTime = 0.5f; // 최대 높이 까지 가는 시간

    Vector3 StartPos;
    //Vector3 EndPos = Vector3.zero;
    public bool IsShoot = false;
    //
    public LineRenderer line;


    //폭탄 범위 설정
    public float BoomRange = 5.0f;
    public e_BoomType m_CurType;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
        line = GetComponent<LineRenderer>();
        line.startWidth = 0.3f;
        line.endWidth = 0.3f;
        line.startColor = Color.red;
        line.endColor = Color.red;
        line.positionCount = 2;
        line.enabled = false;

        switch (m_CurType)
        {
            case e_BoomType.Boom:
                BoomRange = 5.0f;
                break;
            case e_BoomType.BigBoom:
                BoomRange = 10.0f;
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (IsShoot)
        {
            fTime += Time.deltaTime;

            transform.position = new Vector3(StartPos.x + fv_x * fTime, StartPos.y + (fv_y * fTime) - (0.5f * fg * fTime * fTime), StartPos.z + fv_z * fTime);
            //Camera.main.GetComponent<CameraMove>().FollowBoom();
        }
    }
    public void StartDraw()
    {
        StartCoroutine(LineDraw());
    }
    IEnumerator LineDraw()
    {
        line.enabled = true;
        while (!IsShoot)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, player.SetEndPos());

            yield return null;
        }
        line.enabled = false;
    }

    public void Shoot(Vector3 vStartPos, Vector3 vEndPos)
    {
        StartPos = vStartPos;
        //EndPos = vEndPos;
        fEndHeight = vEndPos.y - vStartPos.y;
        fHeight = fMaxHeight - vStartPos.y;

        fg = 2 * fHeight / (fmaxTime * fmaxTime);
        fv_y = Mathf.Sqrt(2 * fg * fHeight);
        float a = fg;
        float b = -2 * fv_y;
        float c = 2 * fEndHeight;

        fEndTime = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);

        fv_x = -(vStartPos.x - vEndPos.x) / fEndTime;
        fv_z = -(vStartPos.z - vEndPos.z) / fEndTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.collider.name);
        ////5.0f공격 범위 안의 Collider
        Collider[] coll = Physics.OverlapSphere(this.gameObject.transform.position, BoomRange);
        foreach (Collider coll_ in coll)
        {
            // Debug.Log(this.gameObject.name+"target");

            if (coll_.CompareTag("BLUE"))
            {
                EnemyCtrl enemy = coll_.GetComponent<EnemyCtrl>();
                enemy.TakeHit(damage);
            }

            if (coll_.CompareTag("EnemyTower"))
            {
                EnemyTowerCtrl enemytower = coll_.GetComponent<EnemyTowerCtrl>();
                enemytower.TakeHit(damage);
                GameManager.Instance.UIManager.EnemyTower_Hpbar.value -= damage;
            }
        }

        Transform ex = Instantiate(Explosion, transform.position, Quaternion.identity);
        GameManager.Instance.m_cplayerCtrl.BallSelct((int)e_BallPattern.None);
        GameManager.Instance.m_cplayerCtrl.SelectOk = false;
        GameManager.Instance.UIManager.m_CbuttonCtrl.gameObject.SetActive(true);
        Destroy(gameObject);
        Destroy(ex.gameObject, 0.5f);
    }

    public void BoomType(int type)
    {
        m_CurType = (e_BoomType)type;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawSphere(this.gameObject.transform.position, 5.0f);
    //}
}
