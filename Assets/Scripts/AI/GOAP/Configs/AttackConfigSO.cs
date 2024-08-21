using UnityEngine;

[CreateAssetMenu(menuName = "AI/Attack Config", fileName = "Attack Config", order = 1)]
public class AttackConfigSO : ScriptableObject
{
    public float SensorRadius = 10;
    public float ViewAngle = 180;
    public float MeleeAttackRadius = 1f;
    public float MeleeAttackDelay = 0.8f;
    public int MeleeAttackCost = 1;
    public LayerMask AttackableLayerMask;
    public LayerMask ObstacleLayerMask;
}
