using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
   

    protected static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
                if (instance == null)
                {
                    Debug.LogError("Error Creating Intance.");
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public CameraMove CameraPos;
    public UIManager UIManager;
    public EnemyTowerCtrl m_cEnemyTower;
    public PlayerCtrl m_cplayerCtrl;
    public GameObject QuaterView;
    public GameObject TopView;
}
