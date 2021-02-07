
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class HeroAI : MonoBehaviour
{

    bool onGround;
    //弾の描画
    LineRenderer line;
    //射撃する弾
    [SerializeField]
    GameObject bullet;
    //射撃の初期位置
    [SerializeField]
    Transform shootPoint;
    float shootTime = -2, reshootTime = 0.3f;//間隔
    public enum HeroAiState
    {
        CHARGING,            //攻撃のチャージ
        MOVE,            //移動
        ATTACK,        //停止して攻撃
        MOVEANDATTACK,    //移動しながら攻撃
        IDLE,            //待機
        AVOID,        //回避
        ULT,//アルティメット
    }
    //初期ステータス
    public HeroAiState heroAiState = HeroAiState.IDLE;

    //怪獣の位置
    GameObject enemyObj;

    // Start is called before the first frame update
    void Start()
    {
        onGround = false;
        enemyObj = GameObject.FindGameObjectWithTag("enemy");
        line = GetComponent<LineRenderer>();
        heroAiState = HeroAiState.MOVEANDATTACK;
    }

    // Update is called once per frame
    void Update()
    {
        lookAtMonster();
        UpdateAI();
    }

    protected virtual void UpdateAI()
    {
        //SetAi();
        switch (heroAiState)
        {
            case HeroAiState.CHARGING:
                Charging();
                break;
            case HeroAiState.MOVE:
                Move();
                break;
            case HeroAiState.ATTACK:
                Attack();
                break;
            case HeroAiState.MOVEANDATTACK:
                MoveAndAttack();
                break;
            case HeroAiState.IDLE:
                Idle();
                break;
            case HeroAiState.AVOID:
                Avoid();
                break;

        }
    }



    private void Charging()
    {
    }

    private void Move()
    {
    }

    private void Attack()
    {
        //リセット間隔
        if (shootTime < -1)
        {
            shootTime = 0.3f;
        }
        //時間経過
        shootTime -= 1 * Time.deltaTime;
        //時間が0になったら射撃
        if (shootTime < 0)
        {
            Shoot();
        }
    }

    private void MoveAndAttack()
    {
        if (onGround)
        {
            Jump();
        }
        
        //リセット間隔
        if (shootTime < -1)
        {
            shootTime =Random.Range(0.3f,2f);
        }
        //時間経過
        shootTime -= 1 * Time.deltaTime;
        //時間が0になったら射撃
        if (shootTime < 0)
        {
           
            Shoot();
        }


    }

    private void Idle()
    {
    }

    private void Avoid()
    {
    }
    #region 参照関数
    void Shoot()
    {
        RaycastHit hit;
        //敵に向かって射撃
        Vector3 dir = enemyObj.transform.position - shootPoint.position;
        var ray = new Ray(shootPoint.position,dir);

        Debug.DrawRay(shootPoint.position, dir, Color.red);
        line.SetPosition(0, shootPoint.position);
        //射撃
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "enemy")
            {
                enemyObj.GetComponent<enemyAI>().HP -= 500;
                GameObject bulletClone = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
                bulletClone.transform.position = hit.point;
                line.enabled = true;
                //当たり位置と射撃位置の間に射線を引く
                line.SetPosition(1, hit.point);
                //弾痕
                // bulletClone.transform.parent = hit.collider.transform;
                Destroy(bulletClone, 1);
            }
            else { if(heroAiState != HeroAiState.MOVEANDATTACK) heroAiState = HeroAiState.MOVEANDATTACK; }
            
        }
        //射線0.1秒後にを消す
        Invoke("lineReset", 0.2f);
        shootTime = reshootTime;
    }
    void lineReset()
    {
        line.enabled = false;
    }

    Transform findBuilding()
    {
        GameObject[] building;
        building = GameObject.FindGameObjectsWithTag("building");
        if (building.Length <= 0)
        {
            return GameObject.Find("Plane").transform;
        }
        Transform t = building[UnityEngine.Random.Range(0, building.Length)].transform;
        return t;
    }

    void Jump()
    {
        onGround = false;
        // 標的の座標
        Vector3 targetPosition = findBuilding().position;
        while (Vector3.Distance(targetPosition, transform.position) > 30)
        {
            targetPosition=findBuilding().position;
        }
        targetPosition.y *= 2;
        targetPosition.y += 1;
        
        // 射出角度
        float angle = 60f;

        // 射出速度を算出
        Vector3 velocity = CalculateVelocity(this.transform.position, targetPosition, angle);

        // 射出
        Rigidbody rid = this.GetComponent<Rigidbody>();
        rid.AddForce(velocity * rid.mass, ForceMode.Impulse);
    }
    /// <summary>
    /// 標的に命中する射出速度の計算
    /// </summary>
    /// <param name="pointA">射出開始座標</param>
    /// <param name="pointB">標的の座標</param>
    /// <returns>射出速度</returns>
    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        // 射出角をラジアンに変換
        float rad = angle * Mathf.PI / 180;

        // 水平方向の距離x
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

        // 垂直方向の距離y
        float y = pointA.y - pointB.y;

        // 斜方投射の公式を初速度について解く
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            // 条件を満たす初速を算出できなければVector3.zeroを返す
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }
    
    void lookAtMonster()
    {
        var direction = enemyObj.transform.position - transform.position;
        direction.y = 0;

        var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);
    }
    #endregion
    private void OnCollisionEnter(Collision collision)
    {
        onGround = true;   
    }
    private void OnCollisionExit(Collision collision)
    {
        onGround = false;
    }
}
