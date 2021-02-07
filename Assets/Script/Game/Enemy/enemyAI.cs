using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{
    public enum EnemyAiState
    {
        CHARGING,            //攻撃のチャージ
        MOVE,            //移動
        ATTACK,        //停止して攻撃
        MOVEANDATTACK,    //移動しながら攻撃
        STAN,            //待機
        AVOID,        //回避
    }
    public EnemyAiState aiState = EnemyAiState.CHARGING;


    public Vector3[] wayPoints = new Vector3[3];
    public float HP = 100000;
    //[SerializeField]
    //private Transform flyObj;
    private Transform target;
    private float Atkdir = 50.0f;
    private NavMeshAgent agent;//NavMeshAgentの情報を取得するためのNavmeshagent型の変数
    float aiTime = 0;

    Rigidbody rb;
    bool knockBack;

    Vector3 direction;
    [SerializeField]
    GameObject chargingPar, beamPar,head;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();//NavMeshAgentの情報をagentに代入
        target = findTarget();
        aiState = EnemyAiState.MOVE;
        chargingPar.SetActive(false);
        beamPar.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        //基本的にタイマーで行動する
        aiTime -= 1 * Time.deltaTime;
        UpdateAI();
        //頭をターゲットに向ける
        head.transform.LookAt(target);

    }
    //AI行動変更処理
    protected virtual void UpdateAI()
    {
        //SetAi();
        switch (aiState)
        { 
            case EnemyAiState.CHARGING:
                Charging();
                break;
            case EnemyAiState.MOVE:
                Move();
                break;
            case EnemyAiState.ATTACK:
                Attack();
                break;
            case EnemyAiState.MOVEANDATTACK:
                MoveAndAttack();
                break;
            case EnemyAiState.STAN:
                Stan();
                break;
            case EnemyAiState.AVOID:
                Avoid();
                break;
        }
    }

    //private void SetAi()
    //{
    //}
    //攻撃のチャージ
    private void Charging()
    {
        
        if (aiTime >= 0)
        {
            chargingPar.transform.localScale = Vector3.one;
            chargingPar.SetActive(true);
            if (aiTime <= 1)
            {
                chargingPar.transform.localScale= Vector3.one*2;
            }
        }
        else
        {
            chargingPar.SetActive(false);
            NextAiState(EnemyAiState.ATTACK);
        }

    }
    //移動
    private void Move()
    {
        if (target == null)
        {
            //ターゲットいない場合ターゲットを探す
            target = findTarget();
            //ターゲット全滅でプレイヤー追跡
            if (target == null)
            {
                wayPoints[0] = GameObject.FindGameObjectWithTag("Player").transform.position;
                wayPoints[0].y = 0;
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }


        }
        else
        {

            float dir = Vector3.Distance(transform.position, target.position);
            agent.stoppingDistance = Atkdir;

            if (dir > Atkdir)
            {
                agent.SetDestination(target.position);
            }
            else
            {
                if (target != GameObject.FindGameObjectWithTag("Player").transform)
                {
                    Debug.Log(target.gameObject.name);

                    NextAiState(EnemyAiState.CHARGING);
                }
            }

        }
        if (agent.isStopped)
        {
            Debug.Log("yes");
        }
    }
    //攻撃
    private void Attack()
    {
        if (aiTime >= 0)
        {
            beamPar.SetActive(true);
        }
        else
        {
            beamPar.SetActive(false);
            NextAiState(EnemyAiState.STAN);
        }
    }

    private void MoveAndAttack()
    {
    }

    private void Stan()
    {
        if (aiTime >= 0)
        {
            if (knockBack)
            {
                agent.velocity = direction * 10;
                StartCoroutine(knockBacking());
            }
        }
        else
        {
            NextAiState(EnemyAiState.MOVE);
        }
    }

    private void Avoid()
    {
    }

    Transform findTarget()
    {
        GameObject[] building;
        building = GameObject.FindGameObjectsWithTag("building");
        if (building.Length <= 0)
        {
            return GameObject.FindGameObjectWithTag("Player").transform;
        }
        Transform t = building[UnityEngine.Random.Range(0, building.Length)].transform;
        //Debug.Log(building.Length);
        return t;
    }

    void NextAiState(EnemyAiState enemyAiState, float t = 10)
    {
        aiState = enemyAiState;
        aiTime = t;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "bullet")
        {
            other.GetComponent<Collider>().enabled = false;
            HP -= other.GetComponent<Bullet>().damage;
            if (aiState == EnemyAiState.CHARGING)
            {
                direction = other.transform.forward;
                chargingPar.SetActive(false);
                NextAiState(EnemyAiState.STAN,0.2f);
                knockBack = true;
                
            }
            Debug.Log(HP);
        }
    }
    //ノックバック処理
    IEnumerator knockBacking() 
    {
        Debug.Log("knoked");
        agent.speed = 5;
        agent.angularSpeed = 1;
        agent.acceleration = 1;
        yield return new WaitForSeconds(0.2f);

        knockBack = false;
        agent.speed = 10;
        agent.angularSpeed = 360;
        agent.acceleration = 10;
    }


}
