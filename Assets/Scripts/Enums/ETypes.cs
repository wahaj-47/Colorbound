using System;


[Flags]
public enum ETypes
{
    None = 0,
    Red = 1 << 1,
    Blue = 1 << 2,
    Green = 1 << 3,
    Magenta = Red | Blue,
    Yellow = Red | Green,
    Cyan = Blue | Green,
};
