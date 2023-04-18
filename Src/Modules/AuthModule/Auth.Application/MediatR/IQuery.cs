using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.MediatR
{
    /// <summary>
    /// A contranct that every Query must implentent before it can be called by QueryAsync method in the custome mediator (MediatKO)
    /// </summary>
    public interface IQuery
    {
    }
}
