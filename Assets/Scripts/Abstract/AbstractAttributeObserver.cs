using AttributeSystem.Authoring;
using UnityEngine;

public abstract class AbstractAttributeObserver : MonoBehaviour
{
    public virtual void HandleChange(AttributeScriptableObject attribute, float prevValue, float currentValue, float maxValue, float minValue){}
}