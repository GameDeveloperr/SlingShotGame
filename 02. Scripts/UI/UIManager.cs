using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Slider EnemyTower_Hpbar;

    public EnemyTowerCtrl EnemyTower;
    public ButtonCtrl m_CbuttonCtrl;
    public Canvas canvas;
    public GameObject ControllBtn;


    public void E_TowerHPset()
    {
        EnemyTower_Hpbar.maxValue = EnemyTower.startingHealth;
        EnemyTower_Hpbar.value = EnemyTower.health;
    }
    public void E_TowerDamage(float damage)
    {
        EnemyTower_Hpbar.value -= damage;
    }
}
