using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider _healthBar;

    public void SetHealth(float health, float maxHealth)
    {
        if(_healthBar == null)
        {
            Debug.Log("Missing reference: No healthbar to modify");
            return;
        }

        float newHealth = health/maxHealth;
        _healthBar.SetValueWithoutNotify(newHealth);
    }
}
