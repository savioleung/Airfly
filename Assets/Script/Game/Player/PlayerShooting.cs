using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    float pokeForce;
    LineRenderer line;
    //射撃する弾
    [SerializeField]
    GameObject bullet;
    //射撃の初期位置
    [SerializeField]
    Transform p;
    //射撃のデフォルト命中位置（当たらない場合用）
    [SerializeField]
    Transform shootView;
    float shootTime = -2, reshootTime = 0.25f;//間隔
    //弾がデカくなる為
    float beBig = 0.2f, bulletChargeMax = 15;

    [SerializeField]
    Slider beBigSlider;
    // Speed in units per sec.
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //チャージショットの値をUIに表示
        beBigSlider.value = beBig;
        if (Input.GetMouseButton(0))
        {
            //リセット間隔
            if (shootTime < -1)
            {
                shootTime = 0.1f;
            }
            //時間経過
            shootTime -= 1 * Time.deltaTime;
            //時間が0になったら射撃
            if (shootTime < 0)
            {
                Shoot();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {//タイムが狂わないようにタイム一旦設定する
            shootTime = -2;
        }
        else if (Input.GetMouseButton(1))
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("down");
                beBig = 0.2f;
            }
            beBig += 10 * Time.deltaTime;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("Up");
            //最大値を固定
            if (beBig >= bulletChargeMax) { beBig = bulletChargeMax; }
            Shoot(beBig);

        }
    }
    void Shoot(float big = 0.2f)
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        line.SetPosition(0, p.position);
        //射撃
        if (Physics.Raycast(ray, out hit))
        {
            GameObject bulletClone = Instantiate(bullet, p.position, p.rotation);
            bulletClone.transform.localScale = Vector3.one * big;
            bulletClone.transform.position = hit.point;
            line.enabled = true;
            //当たり位置と射撃位置の間に射線を引く
            line.SetPosition(1, hit.point);
            //弾痕
            // bulletClone.transform.parent = hit.collider.transform;
            Destroy(bulletClone, 3);
        }else
        {
            //射線を直線的に描画
            line.SetPosition(1,shootView.position);
        }
        //射線0.1秒後にを消す
        Invoke("lineReset", 0.1f);
        shootTime = reshootTime;
    }
    //射撃の後、射線のリセット
    void lineReset()
    {
        line.enabled = false;
    }

}
