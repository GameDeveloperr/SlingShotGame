using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCtrl : MonoBehaviour {

    bool controllmode = true;
    public void FootMatBall()
    {
        GameManager.Instance.m_cplayerCtrl.BallSelct((int)e_BallPattern.first);
        this.gameObject.SetActive(false);
    }

    public void BoomBall()
    {
        GameManager.Instance.m_cplayerCtrl.BallSelct((int)e_BallPattern.second);
        this.gameObject.SetActive(false);
    }

    public void BigBoomBall()
    {
        GameManager.Instance.m_cplayerCtrl.BallSelct((int)e_BallPattern.third);
        this.gameObject.SetActive(false);
    }

    public void ControllMode()
    {
        if (controllmode) {
            GameManager.Instance.UIManager.canvas.gameObject.SetActive(false);
            GameManager.Instance.UIManager.ControllBtn.SetActive(true);
            controllmode = false;
        }
        else
        {
            GameManager.Instance.UIManager.canvas.gameObject.SetActive(true);
            GameManager.Instance.UIManager.ControllBtn.SetActive(false);
            controllmode = true;
        }

    }
    

    public void Quater()
    {
        GameManager.Instance.QuaterView.SetActive(true);
        GameManager.Instance.TopView.SetActive(false);

    }

    public void Top()
    {
        GameManager.Instance.QuaterView.SetActive(false);
        GameManager.Instance.TopView.SetActive(true);
    }
}
