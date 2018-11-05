using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum e_BallPattern{
    None,first,second,third
};

public class PlayerCtrl : MonoBehaviour {

    public Transform FootnamBoom;
    public Transform Boom;
    public Transform BigBoom;

    public CameraMove m_cCamera;

    private Transform CurrentBoom = null;
    //
    private float maxX = 70.0f;
    private float sensX = 100.0f;
    private float rotationX = 0.0f;

    private float sensY = 1.5f;
    private float Dist = 0.0f;

    private float Power = 20.0f;
    //
    public bool CanShoot = false;
    public bool SelectOk = false;

    public e_BallPattern m_CurPattern = e_BallPattern.None;

    private void Start()
    {
        //CreateBoom();
       // ShootReady();
    }
    private void Update()
    {
        if (m_CurPattern != e_BallPattern.None)
        {
            if (!CurrentBoom)
            {
                CreateBoom((int)m_CurPattern);
                CanShoot = true;
                //Invoke("ShootReady", 1.0f);
                //m_CurPattern = e_BallPattern.None;
            }
            if (CurrentBoom)
            {

                if (CanShoot)
                {
                    //Debug.DrawRay(CurrentBoom.position, CurrentBoom.forward * 100.0f, Color.red);
                    if (Input.GetMouseButtonDown(0))
                    {
                        switch (m_CurPattern)
                        {
                            case e_BallPattern.first:
                                CurrentBoom.GetComponent<BoomCtrl>().StartDraw();
                                break;
                             case e_BallPattern.second:
                                CurrentBoom.GetComponent<BigBoomCtrl>().StartDraw();
                                break;
                             case e_BallPattern.third:
                                CurrentBoom.GetComponent<BigBoomCtrl>().StartDraw();
                                break;
                        }
                
                        //CurrentBoom.GetComponent<BoomCtrl>().StartDraw();
                        //Debug.Log("왼쪽 버튼 꾹");
                        SelectOk = true;
                    }
                    if (Input.GetMouseButton(0))
                    {//회전
                    Debug.Log("왼쪽 버튼 꾹누른채");

                        rotationX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
                        rotationX = Mathf.Clamp(rotationX, -maxX, maxX);

                        transform.localEulerAngles = new Vector3(0, -rotationX, 0);
                        //m_cCamera.SetPosition();//카메라 이동
                                                //폭탄 이동
                        Dist += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
                        Dist = Mathf.Clamp(Dist, -1.5f, 0.0f);
                        CurrentBoom.rotation = Quaternion.Lerp(CurrentBoom.rotation, transform.rotation, sensX * Time.deltaTime);
                        CurrentBoom.position = transform.position + (transform.forward * Dist);
                        //
                        // 폭탄 궤적
                    }
                    else if (Input.GetMouseButtonUp(0) && SelectOk == true)
                    {
                        Debug.Log("왼쪽 버튼 놧다");

                        //폭탄 발사
                        //m_cCamera.SetTarget(CurrentBoom);
                        //도착지점
                        Vector3 endpos = SetEndPos();
                        //

                        switch (m_CurPattern)
                        {
                            case e_BallPattern.first:
                                CurrentBoom.GetComponent<BoomCtrl>().fTime = 0.0f;
                                CurrentBoom.GetComponent<BoomCtrl>().Shoot(CurrentBoom.position, new Vector3(endpos.x, 0, endpos.z));
                                CurrentBoom.GetComponent<BoomCtrl>().IsShoot = true;
                                break;
                            case e_BallPattern.second:
                                CurrentBoom.GetComponent<BigBoomCtrl>().fTime = 0.0f;
                                CurrentBoom.GetComponent<BigBoomCtrl>().Shoot(CurrentBoom.position, new Vector3(endpos.x, 0, endpos.z));
                                CurrentBoom.GetComponent<BigBoomCtrl>().IsShoot = true;
                                break;
                            case e_BallPattern.third:
                                CurrentBoom.GetComponent<BigBoomCtrl>().fTime = 0.0f;
                                CurrentBoom.GetComponent<BigBoomCtrl>().Shoot(CurrentBoom.position, new Vector3(endpos.x, 0, endpos.z));
                                CurrentBoom.GetComponent<BigBoomCtrl>().IsShoot = true;
                                break;
                        }
                        //CurrentBoom.GetComponent<BoomCtrl>().fTime = 0.0f;
                        //CurrentBoom.GetComponent<BoomCtrl>().Shoot(CurrentBoom.position, new Vector3(endpos.x, 0, endpos.z));
                        //CurrentBoom.GetComponent<BoomCtrl>().IsShoot = true;

                        Dist = 0.0f;
                        CanShoot = false;
                    }
                }
            }
        }
    }
    public void CreateBoom(int number)
    {
        Transform boom =null;
        switch (number) {
            case (int)e_BallPattern.first:
                boom = Instantiate(FootnamBoom, transform.position, transform.rotation);
                break;
            case (int)e_BallPattern.second:
                boom = Instantiate(Boom, transform.position, transform.rotation);
                break;
            case (int)e_BallPattern.third:
                boom = Instantiate(BigBoom, transform.position, transform.rotation);
                break;
        }
        CurrentBoom = boom;
        //Transform boom = Instantiate(Boom, transform.position, transform.rotation);
        //CurrentBoom = boom;
    }
    //public void ShootReady()
    //{
    //    m_cCamera.SetTarget(transform);
    //    m_cCamera.SetPosition();
    //    CanShoot = true;
    //}

    public Vector3 SetEndPos()
    {
        float power = -Dist * Power;
        //폭탄 발사
        //도착지점
        Vector3 endpos = CurrentBoom.forward * power;
        endpos.x *= 2;
        if (CurrentBoom.eulerAngles.y <= 70 && CurrentBoom.eulerAngles.y >= 0) { endpos.z -= 30 * CurrentBoom.forward.x; }
        else { endpos.z += 30 * CurrentBoom.forward.x; }

        return endpos;
    }

    public void BallSelct(int number)
    {
        m_CurPattern = (e_BallPattern)number;
    }
    //private void OnGUI()
    //{
    //    GUIStyle a = new GUIStyle();
    //    a.fontSize = 50;

    //    float rot = CurrentBoom.localEulerAngles.y;

    //    float cos = Mathf.Cos(rot * Mathf.Deg2Rad);
    //    float sin = Mathf.Sin(rot * Mathf.Deg2Rad);
    //    float height = sin * -Dist * Power;
    //    float dist = cos * -Dist * Power;
    //    float x = CurrentBoom.position.x + height * 2;
    //    float y = CurrentBoom.position.y + dist * 2;
    //    Vector3 endpos = new Vector3(x, 0, y);

    //    Vector3 Boom_Pos = CurrentBoom.forward * (-Dist * Power);
    //    GUI.TextField(new Rect(10, 10, 200, 100), Boom_Pos.ToString(), a);
    //    GUI.TextField(new Rect(10, 50, 200, 100), endpos.ToString(), a);

    //    GUI.TextField(new Rect(10, 90, 200, 100), CurrentBoom.right.ToString(), a);
    //    GUI.TextField(new Rect(10, 140, 200, 100), CurrentBoom.forward.ToString(), a);

    //}
}
