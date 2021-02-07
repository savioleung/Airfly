using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRot : MonoBehaviour
{
    [SerializeField]
    private Transform camTran;    //カメラ
    [SerializeField]
    private Transform pivot;    //追跡点

    private const float Y_ANGLE_MIN = -50.0f;//Y最小角度
    private const float Y_ANGLE_MAX = 75.0f;//Y最大角度

    public float distance = 4f;//目標との距離
    //デフォルト角度
    private float currentX = 0.0f;
    private float currentY = 45.0f;
    void Start()
    {//マウス固定・見えないようにする
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (pivot == null)
            pivot = transform.parent;

        camTran = transform;
    }
    private void Update()
    {
        //マウスの入力をゲット
        currentX += Input.GetAxis("Mouse X");
        currentY -= Input.GetAxis("Mouse Y");
        //Yの活動範囲を規制
        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    private void LateUpdate()
    {//回転
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX*3, 0);
        camTran.position = pivot.position + rotation * dir;
        camTran.LookAt(pivot.position);
    }

}
