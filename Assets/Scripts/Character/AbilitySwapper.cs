using AYellowpaper.SerializedCollections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AbilitySwapper : MonoBehaviour
{
    [SerializeField] private ColorboundCharacter Character;
    [SerializeField] private ETypes ActiveType;
    [SerializeField] private AbilityManager AbilityManager;
    [SerializeField] private SerializedDictionary<ETypes, AbilitySetScriptableObject> AbilitySets;

    private void Start()
    {
        ActiveType |= Character.TypeTag.Type;
        SwapAbilitySet();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ColorboundCharacter>(out var character))
        {
            ActiveType |= character.TypeTag.Type;
            SwapAbilitySet();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ColorboundCharacter>(out var character))
        {
            ActiveType &= ~character.TypeTag.Type;
            SwapAbilitySet();
        }
    }

    private void SwapAbilitySet()
    {
        if (AbilitySets.TryGetValue(ActiveType, out AbilitySetScriptableObject abilitySet))
        {
            AbilityManager.AssignAbilitySet(abilitySet);
        }
    }
}
