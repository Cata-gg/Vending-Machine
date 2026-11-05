using System;
using System.Collections.Generic;
using System.Text;

namespace iQuest.VendingMachine
{
    internal class InvalidColumnException : Exception
    {
        public InvalidColumnException()
            : base("Invalid product column.")
        {
        }
    }
}