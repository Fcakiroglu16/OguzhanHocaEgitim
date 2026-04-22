using System;
using System.Collections.Generic;
using System.Text;

namespace Domains.Exceptions
{
    public class BusinessException(string message) : Exception(message);
}
