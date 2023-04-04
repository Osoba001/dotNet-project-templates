using Auth.Application.Commands;
using Auth.Application.MediatR;
using Auth.Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KO.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KOControllerBase : ControllerBase
    {
        private readonly IMediatKO _mediator;

        public KOControllerBase(IMediatKO mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> ExecuteAsync<TCommand,TCommandHandler>(TCommand command) 
            where TCommand : ICommand
            where TCommandHandler : ICommandHandler<TCommand>
        {
            var validate=command.Validate();
            if (!validate.IsSuccess)
                return BadRequest(validate.ReasonPhrase);

            var result = await _mediator.ExecuteCommandAsync<TCommand, TCommandHandler>(command);
            if (!result.IsSuccess)
                return BadRequest(result.ReasonPhrase);
            return Ok(result.data ?? "Success");
        }

        public async Task<IActionResult> ExecuteSignAsync<TCommand, TCommandHandler>(TCommand command)
            where TCommand : ICommand
            where TCommandHandler : ICommandHandler<TCommand>
        {
            var validate = command.Validate();
            if (!validate.IsSuccess)
                return BadRequest(validate.ReasonPhrase);

            var result = await _mediator.ExecuteCommandAsync<TCommand, TCommandHandler>(command);
            if (!result.IsSuccess)
                return BadRequest(result.ReasonPhrase);
            var tokenModel= result.data as TokenModel;
            Response.Cookies.Append("refreshToken", tokenModel.RefreshToken, new CookieOptions { HttpOnly = true });
            return Ok(tokenModel.AccessToken);
        }


        public async Task<IActionResult> RefreshTokenAsync()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken == null)
                return BadRequest("Refresh token has expired or has never be set.");

            var result = await _mediator.ExecuteCommandAsync<RefreshTokenCommand, RefreshTokenHandler>(new RefreshTokenCommand() { RefreshToken=refreshToken});
            if (!result.IsSuccess)
                return BadRequest(result.ReasonPhrase);
            var tokenModel = result.data as TokenModel;
            Response.Cookies.Append("refreshToken", tokenModel.RefreshToken, new CookieOptions { HttpOnly = true });
            return Ok(tokenModel.AccessToken);
        }
        public async Task<IActionResult> QueryAsync<TQuery,TQueryHandler>(TQuery query) 
            where TQuery : IQuery
            where TQueryHandler : IQueryHandler<TQuery>
        {
            return Ok(await _mediator.QueryAsync<TQuery,TQueryHandler>(query));
        }
    }
}
