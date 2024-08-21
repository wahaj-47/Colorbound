using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AbilitySystem;
using AbilitySystem.Authoring;
using AYellowpaper.SerializedCollections;
using GameplayTag.Authoring;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class LOSSensor : MonoBehaviour
{
    public AttackConfigSO AttackConfig;
    private List<Collider> TargetsInViewRadius = new List<Collider>();
    public List<Collider> TargetsInSight = new List<Collider>();

    [HideInInspector]
    public SphereCollider Collider;

    public delegate void TargetEnterEvent(Transform target);
    public delegate void TargetExitEvent(Transform target);

    public event TargetEnterEvent OnTargetEnter;
    public event TargetExitEvent OnTargetExit;

    private void Awake()
    {
        Collider = GetComponent<SphereCollider>();
    }

    private void FixedUpdate()
    {
        Observe();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((AttackConfig.AttackableLayerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            TargetsInViewRadius.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((AttackConfig.AttackableLayerMask.value & (1 << other.gameObject.layer)) > 0)
        {
            TargetsInViewRadius.Remove(other);
            if (TargetsInViewRadius.Count < 1)
            {
                OnTargetExit?.Invoke(other.transform);
            }
        }
    }

    private void Observe()
    {
        TargetsInSight.Clear();

        foreach (Collider target in TargetsInViewRadius)
        {
            if (target != null && gameObject.CanSee(target.transform, AttackConfig.ViewAngle, AttackConfig.ObstacleLayerMask, out RaycastHit hit))
            {
                TargetsInSight.Add(target);
            }
        }

        TargetsInSight.Sort(
            delegate (Collider a, Collider b)
            {
                return (transform.position - a.transform.position).sqrMagnitude.CompareTo((transform.position - b.transform.position).sqrMagnitude);
            }
        );

        if (TargetsInSight.Count > 0)
        {
            OnTargetEnter?.Invoke(TargetsInSight[0].transform);
        }
    }
}
