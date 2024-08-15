using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AttributeSystem.Authoring;
using AttributeSystem.Components;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Gameplay Ability System/Attribute Event Handler/Broadcast Attribute")]
public class BroadcastAtrributeEventHandler : AbstractAttributeEventHandler
{
    [SerializeField]
    private AttributeScriptableObject PrimaryAttribute;
    [SerializeField]
    private AttributeScriptableObject MinAttribute;
    [SerializeField]
    private AttributeScriptableObject MaxAttribute;
    
    [SerializeField]
    [MonoScript(typeof(AbstractAttributeObserver))]
    private string ObserverComponent;

    public override void PreAttributeChange(AttributeSystemComponent attributeSystem, List<AttributeValue> prevAttributeValues, ref List<AttributeValue> currentAttributeValues)
    {
        var attributeCacheDict = attributeSystem.mAttributeIndexCache;
        if (attributeCacheDict.TryGetValue(PrimaryAttribute, out var primaryAttributeIndex))
        {
            var prevValue = prevAttributeValues[primaryAttributeIndex].CurrentValue;
            var currentValue = currentAttributeValues[primaryAttributeIndex].CurrentValue;
            
            var minValue = 0f;
            if(MinAttribute != null && attributeCacheDict.TryGetValue(MinAttribute, out var minAttributeIndex))
            {
                minValue = currentAttributeValues[minAttributeIndex].CurrentValue;
            }

            var maxValue = 100f;
            if(MaxAttribute != null && attributeCacheDict.TryGetValue(MaxAttribute, out var maxAttributeIndex))
            {
                maxValue = currentAttributeValues[maxAttributeIndex].CurrentValue;
            }

            if (prevValue != currentValue)
            {
                if(attributeSystem.TryGetComponent(Type.GetType(ObserverComponent), out var attributeObserver))
                {
                    (attributeObserver as AbstractAttributeObserver).HandleChange(PrimaryAttribute, prevValue, currentValue, minValue, maxValue);
                }
            }
        }
    }
}
