using Auth.Application.Commands;
using Auth.Application.EventData;
using Auth.Application.MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KO.WebAPI.Controllers.Auth
{
    public class KOControllerBase : ControllerBase
    {
        private readonly IMediatKO _mediator;

        public KOControllerBase(IMediatKO mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> ExecuteAsync<TCommand, TCommandHandler>(TCommand command)
            where TCommand : ICommand
            where TCommandHandler : ICommandHandler<TCommand>
        {
            var validate = command.Validate();
            if (!validate.IsSuccess)
                return BadRequest(validate.ReasonPhrase);
            var result = await _mediator.ExecuteCommandAsync<TCommand, TCommandHandler>(command);
            if (!result.IsSuccess)
                return BadRequest(result.ReasonPhrase);
            return Ok(result.data ?? "Success");
        }
        public async Task<IActionResult> QueryAsync<TQuery, TQueryHandler>(TQuery query)
            where TQuery : IQuery
            where TQueryHandler : IQueryHandler<TQuery>
        {
            var res = await _mediator.QueryAsync<TQuery, TQueryHandler>(query);
            if (res.IsSuccess)
                return Ok(res.data);
            else
                return BadRequest(res.ReasonPhrase);
        }
    }
}
