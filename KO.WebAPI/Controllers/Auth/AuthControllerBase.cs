using Auth.Application.Commands;
using Auth.Application.EventData;
using Auth.Application.MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KO.WebAPI.Controllers.Auth
{
    /// <summary>
    /// Represents a base controller for executing commands and queries using MediatR.
    /// </summary>
    public class AuthControllerBase : ControllerBase
    {
        private readonly IMediatKO _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthControllerBase"/> class with the specified MediatR mediator.
        /// </summary>
        /// <param name="mediator">The MediatR mediator to use.</param>
        public AuthControllerBase(IMediatKO mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Executes the specified command using MediatR.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command to execute.</typeparam>
        /// <typeparam name="TCommandHandler">The type of the command handler to use.</typeparam>
        /// <param name="command">The command to execute.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the command execution.</returns>
        /// <remarks>
        /// This method validates the command before executing it by calling its Validate() method. If the validation fails, it returns a BadRequest response.
        /// If the command execution fails, it returns a BadRequest response with the reason phrase. If the execution succeeds, it returns an Ok response with the result data, or "Success" if the data is null.
        /// </remarks>
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

        /// <summary>
        /// Executes the specified tokencommand using MediatR.
        /// The manage refresh token subscriber manage the refresh token.
        /// </summary>
        /// <typeparam name="TCommand">The type of the token command to execute.</typeparam>
        /// <typeparam name="TCommandHandler">The type of the command handler to use.</typeparam>
        /// <param name="command">The token command to execute.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the command execution.</returns>
        /// <remarks>
        /// This method is similar to <see cref="ExecuteAsync{TCommand, TCommandHandler}(TCommand)"/>, but it also subscribes to the GeneratedRefreshToken event of the command and calls the ManageRefreshToken method when the event is raised.
        /// </remarks>
        public async Task<IActionResult> ExecuteTokenAsync<TCommand, TCommandHandler>(TCommand command)
            where TCommand : ITokenCommand
            where TCommandHandler : ICommandHandler<TCommand>
        {
            var validate = command.Validate();
            if (!validate.IsSuccess)
                return BadRequest(validate.ReasonPhrase);
            command.GeneratedRefreshToken+= ManageRefreshToken;
            var result = await _mediator.ExecuteCommandAsync<TCommand, TCommandHandler>(command);
            if (!result.IsSuccess)
                return BadRequest(result.ReasonPhrase);
            return Ok(result.data ?? "Success");
        }


        private void ManageRefreshToken(object? sender, string e)
        {
            Response.Cookies.Append("refreshToken", e, new CookieOptions { HttpOnly = true });
        }

        /// <summary>
        /// Executes the specified query using MediatR.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query to execute.</typeparam>
        /// <typeparam name="TQueryHandler">The type of the query handler to use.</typeparam>
        /// <param name="query">The query to execute.</param>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the query execution.</returns>
        /// <remarks>
        /// This method validates the query before executing it by calling its Validate() method. If the validation fails, it returns a BadRequest response.
        /// If the query execution fails, it returns a BadRequest response with the reason phrase. If the execution succeeds, it returns an Ok response with the result data.
        /// </remarks>
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
