using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class RedUnitCtrl : LivingCtrl
{
    public enum UnitStatus
    {
        IDLE, ATTACK, TRACE, DIE
    };

    private NavMeshAgent nav;
    private Animator anim;
    //상대 기지 
    //콜라이더에 들어오는 유닛
    public GameObject Mob = null;
    //무기
    public BoxCollider spear;
    //현재 State
    private UnitStatus m_CurState = UnitStatus.TRACE;

    //범위안에 들어온 유닛과의 거리
    float dist_enemy = 0;
    //공격 사정거리
    private float attack_dist = 3.0f;
    private float trace_dist = 5.0f;
    private float tower_dist = 7.0f;
    private bool tower_attack = false;

    //public GameObject Target;
    public float damage;

    protected override void Start()
    {
        base.Start();
        nav = this.gameObject.GetComponent<NavMeshAgent>();
        anim = this.gameObject.GetComponent<Animator>();
       // Target = GameObject.Find("EnemyTower");
        onDeath += OnDie;
        StartCoroutine(UnitState());
        StartCoroutine(UnitDist());
        //nav.SetDestination(Target.gameObject.transform.position);
        InvokeRepeating("UnitSkill", 2.0f,2.0f);
        Destroy(this.gameObject,6.0f);

    }

    private void Update()
    {

    }

    IEnumerator UnitState()
    {
        while (!dead)
        {
            switch (m_CurState)
            {
                case UnitStatus.IDLE:
                    anim.SetBool("IsWalk", false);
                    break;
                case UnitStatus.ATTACK:
                    anim.SetBool("IsWalk", false);
                    anim.SetTrigger("IsAttack");
                    spear.enabled = true;
                    yield return new WaitForSeconds(1.0f);
                    spear.enabled = false;
                    break;
                case UnitStatus.TRACE:
                    anim.SetBool("IsWalk", true);
                    break;
                case UnitStatus.DIE:
                    break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator UnitDist()
    {

        while (!dead)
        {
            if (Mob == null) // 이미 타겟유닛이 있다면 다시 구할 필요가 없음.
            {
                Collider[] coll = Physics.OverlapSphere(this.gameObject.transform.position, 5.0f);
                foreach (Collider coll_ in coll)
                {
                    if (coll_.CompareTag("BLUE"))
                    {//스피어 안에 여러명의 적이 들어왔을 때 거리 비교 
                        if (dist_enemy == 0)
                        {
                            dist_enemy = Vector3.Distance(transform.position, coll_.transform.position);
                            Mob = coll_.gameObject;
                        }
                        else
                        {
                            if (dist_enemy > Vector3.Distance(transform.position, coll_.transform.position))
                            {
                                dist_enemy = Vector3.Distance(transform.position, coll_.transform.position);
                                Mob = coll_.gameObject;
                            }
                        }
                    }
                }
                //float _dist = Vector3.Distance(transform.position, Target.transform.position);
                //if (_dist <= tower_dist)
                //{
                //    Debug.Log("Tower");
                //    Mob = Target;
                //    tower_attack = true;
                //    nav.SetDestination(transform.position);
                //    m_CurState = UnitStatus.ATTACK;
                //    transform.LookAt(Mob.transform);
                //}
            }
            else
            {
                if (!tower_attack)
                {
                    dist_enemy = Vector3.Distance(this.gameObject.transform.position, Mob.transform.position);
                    if (dist_enemy > attack_dist)//공격 사정거리 외
                    {
                        m_CurState = UnitStatus.TRACE;
                        nav.SetDestination(Mob.transform.position);
                    }
                    else if (dist_enemy <= attack_dist)//공격사정거리 내
                    {
                        nav.SetDestination(transform.position);
                        m_CurState = UnitStatus.ATTACK;
                        transform.LookAt(Mob.transform);
                    }
                    if (Mob.GetComponent<EnemyCtrl>().dead)//몹이 죽었을 경우
                    {
                        Debug.Log("적을죽임");
                        Mob = null;
                        m_CurState = UnitStatus.IDLE;

                        //nav.SetDestination(Target.transform.position);
                        // m_CurState = UnitStatus.TRACE;
                    }
                }
                //타겟(타워 가 범위 안에 있을 때)
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BLUE_SPEAR")//창에 맞을 경우
        {
            TakeHit(other.gameObject.GetComponentInParent<EnemyCtrl>().damage);
        }
    }

    void OnDie()
    {
        if (nav)
        {
            Destroy(nav);
        }
        anim.SetTrigger("IsDie");
    }

    void UnitSkill()
    {
        Collider[] coll = Physics.OverlapSphere(this.gameObject.transform.position, 1000.0f);
        foreach (Collider coll_ in coll)
        {
            Debug.Log("skill");

            if (coll_.CompareTag("BLUE"))
            {
                Debug.Log("skill Hit");

                EnemyCtrl enemy = coll_.GetComponent<EnemyCtrl>();
                enemy.TakeHit(100);
            }
        }
    }
}
