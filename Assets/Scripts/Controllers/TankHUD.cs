using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TankHUD : MonoBehaviour
{
    [SerializeField] private GameObject HealthBar;
    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        HealthBar.transform.localScale = new Vector3(currentHealth / maxHealth,1f,1f);
    }
}
