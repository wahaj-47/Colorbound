using CrashKonijn.Goap.Interfaces;

public class CommonData : IActionData
{
    public ITarget Target { get; set; }
    public float Timer {get; set;}
}