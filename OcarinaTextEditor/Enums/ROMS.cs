using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcarinaTextEditor.Enums
{
    public enum ROMVer
    {
        Debug = 0,
        N1_0 = 1,
        Unknown = 20
    }

    public enum EditMode
    {
        ROM = 0,
        ZZRT = 1,
        ZZRPL = 2,
        Z64ROM = 3,
    }
}
