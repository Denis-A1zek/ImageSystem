using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSystem.Core;

public class AccessException : Exception
{
    public AccessException(string? message) : base(message)
    {
    }
}
