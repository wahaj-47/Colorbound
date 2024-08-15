using System.Collections;
using System.Collections.Generic;
using AttributeSystem.Authoring;
using AttributeSystem.Components;
using UnityEngine;

[CreateAssetMenu(menuName = "Gameplay Ability System/Attribute Event Handler/Clamp Attribute")]
public class ClampAttributeEventHandler : AbstractAttributeEventHandler
{
    [SerializeField]
    private AttributeScriptableObject PrimaryAttribute;
    [SerializeField]
    private AttributeScriptableObject MinAttribute;
    [SerializeField]
    private AttributeScriptableObject MaxAttribute;
    public override void PreAttributeChange(AttributeSystemComponent attributeSystem, List<AttributeValue> prevAttributeValues, ref List<AttributeValue> currentAttributeValues)
    {
        var attributeCacheDict = attributeSystem.mAttributeIndexCache;
        ClampAttribute(PrimaryAttribute, MinAttribute, MaxAttribute, currentAttributeValues, attributeCacheDict);
    }

    private void ClampAttribute(AttributeScriptableObject Attribute, AttributeScriptableObject MinAttribute, AttributeScriptableObject MaxAttribute, List<AttributeValue> attributeValues, Dictionary<AttributeScriptableObject, int> attributeCacheDict)
    {
        if (attributeCacheDict.TryGetValue(Attribute, out var primaryAttributeIndex)
            && attributeCacheDict.TryGetValue(MinAttribute, out var minAttributeIndex)
            && attributeCacheDict.TryGetValue(MaxAttribute, out var maxAttributeIndex))
        {
            var primaryAttribute = attributeValues[primaryAttributeIndex];
            var minAttribute = attributeValues[minAttributeIndex];
            var maxAttribute = attributeValues[maxAttributeIndex];

            // Clamp current and base values
            if (primaryAttribute.CurrentValue < minAttribute.CurrentValue) primaryAttribute.CurrentValue = minAttribute.CurrentValue;
            if (primaryAttribute.CurrentValue > maxAttribute.CurrentValue) primaryAttribute.CurrentValue = maxAttribute.CurrentValue;
            if (primaryAttribute.BaseValue < minAttribute.BaseValue) primaryAttribute.BaseValue = minAttribute.BaseValue;
            if (primaryAttribute.BaseValue > maxAttribute.BaseValue) primaryAttribute.BaseValue = maxAttribute.BaseValue;

            attributeValues[primaryAttributeIndex] = primaryAttribute;
        }
    }
}
