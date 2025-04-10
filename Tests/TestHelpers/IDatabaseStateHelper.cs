using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.TestHelpers
{
    interface IDatabaseStateHelper
    {
        public abstract void StoreState();
        public abstract void RestoreState();
    }
}
