using ITSM.Controllers;
using ITSM.Models;
using ITSM.Services;
using ITSM.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ITSM.Tests
{
    public class JobPositionControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult_WithPositions()
        {
            // Arrange
            var jobPositions = new List<JobPosition>
            {
                new JobPosition { Id = 1, Position = "Manager" },
                new JobPosition { Id = 2, Position = "Developer" }
            };

            var jobPositionServiceMock = new Mock<IJobPositionService>();
            jobPositionServiceMock.Setup(x => x.GetAll()).Returns(jobPositions);

            var controller = new JobPositionController(jobPositionServiceMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PositionViewModel>(viewResult.ViewData.Model);
            Assert.Equal(jobPositions, model.JobPositions);
            Assert.IsType<JobPosition>(model.Position);
        }

        [Fact]
        public void Index_Post_RedirectsToIndex_WithSuccessMessage()
        {
            // Arrange
            var jobPositionVM = new PositionViewModel
            {
                Position = new JobPosition { Id = 1, Position = "Manager" }
            };

            var jobPositionServiceMock = new Mock<IJobPositionService>();
            jobPositionServiceMock.Setup(x => x.AddJobPosition(It.IsAny<JobPosition>())).Returns(true);

            var controller = new JobPositionController(jobPositionServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("name", "testuser") }, "mock"))
                }
            };
            controller.TempData = new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());
            // Act
            var result = controller.Index(jobPositionVM);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(Index), redirectToActionResult.ActionName);
            Assert.Equal("Manager has been added!", controller.TempData["success"]);
        }

        [Fact]
        public void Index_Post_RedirectsToIndex_WithErrorMessage()
        {
            // Arrange
            var jobPositionVM = new PositionViewModel
            {
                Position = new JobPosition { Id = 1, Position = "Manager" }
            };

            var jobPositionServiceMock = new Mock<IJobPositionService>();
            jobPositionServiceMock.Setup(x => x.AddJobPosition(It.IsAny<JobPosition>())).Returns(false);

            var controller = new JobPositionController(jobPositionServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("name", "testuser") }, "mock"))
                }
            };
            controller.TempData = new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            // Act
            var result = controller.Index(jobPositionVM);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(Index), redirectToActionResult.ActionName);
            Assert.Equal("Something went wrong, please try again.", controller.TempData["error"]);
        }
    }
}
