﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Exceptions
{
    public class TooManyException : Exception
    {
        public TooManyException(string message)
            : base(message)
        {
        }
    }
}
