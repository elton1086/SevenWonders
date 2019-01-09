using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.BaseEntities
{
    [Flags]
    public enum PlayerDirection
    {
        None = 0,
        Myself = 1,
        ToTheLeft = 2,
        ToTheRight = 4,
    }
}
