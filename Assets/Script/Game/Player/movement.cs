using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class movement : MonoBehaviour
{
    private CharacterController characterController;
    private float JumpPower = 20.0f;//ジャンプ力
    private float flyPower = 2.0f;//飛ぶパワー
    private float MoveSpeed = 25.0f;//移動速度
    private float g = 15.0f;//重力
    private bool flying = false;//飛行チェック

    //ダブルクリック用の間隔、チェック用タイム
    float doubleClickTime = 0.25f, doubleClickTimeCheck;

    private float horizontal;//回転用
    //ジャンプ用
    private Vector3 move3;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        horizontal = transform.eulerAngles.y;
        doubleClickTimeCheck = Time.time;
    }

    // Update is called once per frame
    void Update()
    {


        //キャラをカメラ方向に向く
        var mouseHorizontal = Input.GetAxis("Mouse X");
        horizontal = (horizontal + mouseHorizontal * 3) % 360f;
        transform.rotation = Quaternion.AngleAxis(horizontal, Vector3.up);
        //移動
        //wasd移動
        if (Input.GetKey(KeyCode.LeftShift))
        {
            MoveSpeed = 40.0f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            MoveSpeed = 25.0f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            characterController.Move(this.gameObject.transform.forward * MoveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.S))
        {
            characterController.Move(this.gameObject.transform.forward * -1f * MoveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            characterController.Move(this.gameObject.transform.right * -1 * MoveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            characterController.Move(this.gameObject.transform.right * MoveSpeed * Time.deltaTime);
        }
        //重力制御
        characterController.Move(move3 * Time.deltaTime);
        //ジャンプ
        if (characterController.isGrounded)//地面タッチ中
        {
            //重力リセット
            g = 15.0f;
            if (flying)//飛行状態で地面タッチで飛行解除
            {
                flying = false;
            }
            if (Input.GetKeyDown(KeyCode.Space))//ジャンプ
            {
                //ジャンプ時、地面タッチまで重力加速（じゃないとジャンプ時のみ落ちる遅い）
                g *= 2;
                move3.y = JumpPower;
            }

        }
        else//滞空中
        {
            if (Input.GetKeyDown(KeyCode.Space))//空中でスペースキーダブルクリックで飛行の起動・解除
            {
                double interval = Time.time - doubleClickTimeCheck;//間隔保存

                move3.y = 0;
                if (interval < doubleClickTime)//間隔が値以下でダブルクリック発生
                {
                    //飛行のon/off
                    if (flying)
                    {
                        flying = false;
                    }
                    else
                    {
                        flying = true;
                    }
                }
                doubleClickTimeCheck = Time.time;//タイム保存
            }

        }
        //飛行
        if (flying)
        {
            move3.y -= 0.1f * Time.deltaTime;//重力落下
            if (Input.GetKey(KeyCode.Space))//スベースキーで上昇
            {
                move3.y += flyPower;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                move3.y = 0;
            }
            if (Input.GetKey(KeyCode.C))//cキーで降下
            {
                move3.y -= flyPower;
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                move3.y = 0;
            }

        }
        else
        {
            move3.y -= g * Time.deltaTime;//重力落下
        }
    }

}
