using System;

namespace ExtLib
{
    [Flags]
    public enum Commands : byte
    {
        GetInfoBin = 0x0a,
        GetInfoJSON = 0x0b,
        GetInfoXML = 0x0c,
        GetScreen = 0x14,
        GetUpdate = 0x15,
        GetTest = 0xff
    }
}
