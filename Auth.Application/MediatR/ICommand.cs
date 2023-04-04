using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Responses;

namespace Auth.Application.MediatR
{
    public interface ICommand
    {
        KOActionResult Validate();
    }
}
