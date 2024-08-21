using CrashKonijn.Goap.Classes.References;
using CrashKonijn.Goap.Interfaces;

public class AttackData : CommonData
{
    [GetComponent]
    public AbilityManager AbilityManager { get; set; }
}