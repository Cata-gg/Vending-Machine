using System;
using System.Collections.Generic;
using System.Text;

namespace iQuest.VendingMachine
{
    internal class CancelException : Exception
    {
        public CancelException()
            : base("Purchase cancelled.")
        {
        }
    }
}
