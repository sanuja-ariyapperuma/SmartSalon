using Microsoft.AspNetCore.Mvc;
using Moq;
using SalonCS.Controllers;
using SalonCS.DTO;
using SalonCS.IServices;
using SalonCS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SalonCS.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly AuthController _sut;

        private readonly Mock<IAuthenticationService> _authservice = new Mock<IAuthenticationService> ();
        private readonly Mock<IPasswordService> _passwordService = new Mock<IPasswordService>();
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();
        private readonly Mock<IAsymmetricEncryption> _asymmetricEncryption = new Mock<IAsymmetricEncryption>();

        public AuthControllerTests()
        {
            _sut = new AuthController(_authservice.Object, _passwordService.Object, _userService.Object, _asymmetricEncryption.Object);
        }

        [Fact]
        public void When_Login_Valid_User_Response_Success_Success()
        {
            //Arrange
            var usercred = new UserCredentials();
            var jwt = "jwt token goes here";

            //Act
            _authservice.Setup(x => x.Authenticate(usercred)).ReturnsAsync(jwt);
            var result = _sut.Login(usercred);

            //Assert
            var convertedObjectResult = (OkObjectResult)result.Result.Result;
            var convertedServiceResponse = (ServiceResponse<string>)convertedObjectResult.Value;
            
            Assert.True(convertedServiceResponse.Success);
            Assert.Equal(jwt, convertedServiceResponse.Data);

        }
        [Fact]
        public void When_Login_Invalid_User_Response_Success_False()
        {
            //Arrange
            var usercred = new UserCredentials();
            var jwt = "";

            //Act
            _authservice.Setup(x => x.Authenticate(usercred)).ReturnsAsync(jwt);
            var result = _sut.Login(usercred);

            //Assert
            var convertedObjectResult = (OkObjectResult)result.Result.Result;
            var convertedServiceResponse = (ServiceResponse<string>)convertedObjectResult.Value;

            Assert.False(convertedServiceResponse.Success);
            Assert.Null(convertedServiceResponse.Data);

        }
    }
}
