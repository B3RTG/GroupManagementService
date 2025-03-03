using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Domain.Exceptions
{
    public class RegistrationFailedException(IEnumerable<string> errors) : Exception($"Registration failed with following errors: {string.Join(Environment.NewLine, errors)}")
    {
    }
}
