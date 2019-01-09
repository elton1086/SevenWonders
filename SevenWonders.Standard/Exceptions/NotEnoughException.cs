using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Exceptions
{
    public class NotEnoughException : Exception
    {
        //
        // Summary:
        //     Initializes a new instance of the SevenWonder.Exceptions.NotEnoughException class with a specified
        //     error message.
        //
        // Parameters:
        //   message:
        //     The message that describes the error.
        public NotEnoughException(string message)
            : base(message)
        {
        }
    }
}
