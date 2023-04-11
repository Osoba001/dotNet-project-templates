using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Responses;

namespace Auth.Application.MediatR
{
    /// <summary>
    /// A contranct that every command must implentent before it can be called by ExecutCommandAsync of the custome mediator (MediatKO)
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// The command parameters are validate here. 
        /// If all the parameters are valid, then can the command execute by the handler.
        /// </summary>
        /// <returns>KOActonResult</returns>
        KOActionResult Validate();
    }
}
