using Auth.Application.Commands;
using Auth.Application.MediatR;
using Auth.Application.QueryAndHandlers;
using FluentAssertions;
using KO.WebAPI.Controllers.Auth;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Utilities.Responses;

namespace WebApiPresentation.Test.Controllers.Auth
{
    public class KOControllerBaseTest
    {
        private readonly Mock<IMediatKO> _mediatorMock;
        private readonly AuthControllerBase _controller;
        private readonly SoftDeleteCommand command = new() { Id = Guid.NewGuid() };
        private readonly UserById query = new() { Id = Guid.NewGuid() };
        public KOControllerBaseTest()
        {
            _mediatorMock = new Mock<IMediatKO>();
            _controller = new AuthControllerBase(_mediatorMock.Object);
        }

        [Fact]
        public async void ExecuteAsync_ValidCommand_ReturnsOkWithResultData()
        {
            //Arrange
            var response = new KOActionResult();
            _mediatorMock.Setup(x => x.ExecuteCommandAsync<SoftDeleteCommand, SoftDeleteHandler>(command))
                .ReturnsAsync(response);

            //Act
            var actionResult = await _controller.ExecuteAsync<SoftDeleteCommand, SoftDeleteHandler>(command);

            //Assert
            actionResult.Should().BeOfType<OkObjectResult>();
            _mediatorMock.Verify(x => x.ExecuteCommandAsync<SoftDeleteCommand, SoftDeleteHandler>(command), Times.Once());
        }


        //QueryAsync
        [Fact]
        public async void QueryAsync_ReturnBadRequestAndErrorMessage_WhenResponseIsNotSuccessFul()
        {
            //Arrange
            var response = new KOActionResult();
            response.AddError("Not successfull.");

            _mediatorMock.Setup(x => x.QueryAsync<UserById, UserByIdQueryHadler>(query))
                .ReturnsAsync(response);

            //Act
            var actionResponse = await _controller.QueryAsync<UserById, UserByIdQueryHadler>(query);

            //Assert
            actionResponse.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be(response.ReasonPhrase);
            _mediatorMock.Verify(x => x.QueryAsync<UserById, UserByIdQueryHadler>(query), Times.Once());
        }

        [Fact]
        public async void QueryAsync_ReturnOkResultAndTheData_WhenResponseIsSuccessFul()
        {
            //Arrange
            var response = new KOActionResult();
            response.Data = Guid.NewGuid().ToString();

            _mediatorMock.Setup(x => x.QueryAsync<UserById, UserByIdQueryHadler>(query))
                .ReturnsAsync(response);

            //Act
            var actionResponse = await _controller.QueryAsync<UserById, UserByIdQueryHadler>(query);

            //Assert
            actionResponse.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be(response.Data);
            _mediatorMock.Verify(x => x.QueryAsync<UserById, UserByIdQueryHadler>(query), Times.Once());
        }
    }


}
