using System.Collections;
using System.Collections.Generic;
using AttributeSystem.Authoring;
using UnityEngine;
using UnityEngine.UI;

public class HealthObserver : AbstractAttributeObserver
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

        float newHealth = health / maxHealth;

        Debug.Log($"{gameObject.name}: {newHealth}");

        _healthBar.SetValueWithoutNotify(newHealth);
    }

    public override void HandleChange(AttributeScriptableObject attribute, float prevValue, float currentValue, float minValue, float maxValue)
    {
        base.HandleChange(attribute, prevValue, currentValue, minValue, maxValue);

        if(currentValue <= 0) Die();
        
        SetHealth(currentValue, maxValue);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
