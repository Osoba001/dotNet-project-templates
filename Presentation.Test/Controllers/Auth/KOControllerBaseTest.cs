using Auth.Application.Commands;
using Auth.Application.MediatR;
using Auth.Application.Models;
using Auth.Application.QueryAndHandlers;
using FluentAssertions;
using KO.WebAPI.Controllers.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;
using Moq;
using Utilities.Responses;

namespace Presentation.Test.Controllers.Auth
{
    public class KOControllerBaseTest
    {
        private readonly Mock<IMediatKO> _mediatorMock;
        private readonly KOControllerBase _controller;

        public KOControllerBaseTest()
        {
            _mediatorMock = new Mock<IMediatKO>();
            _controller = new KOControllerBase(_mediatorMock.Object);
        }
        //ExecuteAsync
        [Fact]
        public async void ExecuteAsync_ReturnBadRequestAndErroMessage_WhenCommandIsNotValid()
        {
            //Arrange
            var command = new SoftDeleteCommand { Id = Guid.NewGuid() };
            var response = new KOActionResult();
            response.AddError("Invalid command.");
            _mediatorMock
                .Setup(x => x.ExecuteCommandAsync<SoftDeleteCommand, SoftDeleteHandler>(command))
                .ReturnsAsync(response);

            //Act
            var actionResult = await _controller.ExecuteAsync<SoftDeleteCommand, SoftDeleteHandler>(command);

            //Assert
            actionResult.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be(response.ReasonPhrase);
        }

        [Fact]
        public async void ExecuteAsync_ValidCommand_ReturnsOkWithResultData()
        {
            //Arrange
            var command = new SoftDeleteCommand { Id = Guid.NewGuid() };
            var response = new KOActionResult();
            _mediatorMock.Setup(x => x.ExecuteCommandAsync<SoftDeleteCommand, SoftDeleteHandler>(command))
                .ReturnsAsync(response);

            //Act
            var actionResult = await _controller.ExecuteAsync<SoftDeleteCommand, SoftDeleteHandler>(command);

            //Assert
            actionResult.Should().BeOfType<OkObjectResult>();
        }


        //QueryAsync
        [Fact]
        public async void QueryAsync_ReturnBadRequestAndErrorMessage_WhenResponseIsNotSuccessFul()
        {
            //Arrange
            var query=new UserById() { Id = Guid.NewGuid() };
            var response = new KOActionResult();
            response.AddError("Not successfull.");

            _mediatorMock.Setup(x=>x.QueryAsync<UserById,UserByIdQueryHadler>(query))
                .ReturnsAsync(response);

            //Act
           var actionResponse= await _controller.QueryAsync<UserById,UserByIdQueryHadler>(query);

            //Assert
            actionResponse.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be(response.ReasonPhrase);
        }

        [Fact]
        public async void QueryAsync_ReturnOkResultAndTheData_WhenResponseIsSuccessFul()
        {
            //Arrange
            var query = new UserById() { Id = Guid.NewGuid() };
            var response = new KOActionResult();
            response.data=Guid.NewGuid().ToString();

            _mediatorMock.Setup(x => x.QueryAsync<UserById, UserByIdQueryHadler>(query))
                .ReturnsAsync(response);

            //Act
            var actionResponse = await _controller.QueryAsync<UserById, UserByIdQueryHadler>(query);

            //Assert
            actionResponse.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be(response.data);
        }
    }

    
}
