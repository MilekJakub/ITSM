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
    public class WorkItemControllerTests
    {

        private readonly WorkItemController _controller;
        private readonly Mock<IWorkItemService> _mockWorkItemService;

        public WorkItemControllerTests()
        {
            _mockWorkItemService = new Mock<IWorkItemService>();
            _controller = new WorkItemController(_mockWorkItemService.Object);
        }

        [Fact]
        public void Index_ShouldReturnCorrectViewModel()
        {
            // Arrange
            var workItems = new List<WorkItem>
            {
                new Issue { Id = 1, Title = "Work Item 1" },
                new Issue { Id = 2, Title = "Work Item 2" },
                new Issue { Id = 3, Title = "Work Item 3" }
            };

            var workItemServiceMock = new Mock<IWorkItemService>();
            workItemServiceMock.Setup(x => x.GetAll()).Returns(workItems);

            var controller = new WorkItemController(workItemServiceMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<WorkItem>>(viewResult.Model);
            Assert.Equal(workItems.Count(), model.Count());
        }

        [Fact]
        public void Details_ReturnsViewResult_WhenWorkItemVMIsNotNull()
        {
            // Arrange
            var workItemService = new Mock<IWorkItemService>();
            var workItemVM = new WorkItemDetailsViewModel();
            workItemService.Setup(x => x.GetViewModel(It.IsAny<int>(), It.IsAny<string>())).Returns(workItemVM);
            var controller = new WorkItemController(workItemService.Object);

            // Act
            var result = controller.Details(1, "discriminator");

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(workItemVM, (result as ViewResult)!.Model);
        }

        [Fact]
        public void Details_ReturnsRedirectToActionResult_WhenWorkItemVMIsNull()
        {
            // Arrange
            var workItemService = new Mock<IWorkItemService>();
            workItemService.Setup(x => x.GetViewModel(It.IsAny<int>(), It.IsAny<string>())).Returns((WorkItemDetailsViewModel)null!);
            var controller = new WorkItemController(workItemService.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("name", "testuser") }, "mock"))
                }
            };
            controller.TempData = new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            // Act
            var result = controller.Details(1, "discriminator");

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.Equal(nameof(Index), redirectResult?.ActionName);
            Assert.True(controller.TempData.ContainsKey("error"));
            Assert.Equal("Bad request.", controller.TempData["error"]);
        }

        [Fact]
        public void Create_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange
            var workItemServiceMock = new Mock<IWorkItemService>();
            var controller = new WorkItemController(workItemServiceMock.Object);
            controller.ModelState.AddModelError("error", "error");

            var workItemVM = new WorkItemDetailsViewModel();

            // Act
            var result = controller.Create(workItemVM);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Details", viewResult.ViewName);
            Assert.Same(workItemVM, viewResult.Model);
        }

        [Fact]
        public void Create_SetsSuccessTempData_WhenWorkItemIsCreatedSuccessfully()
        {
            // Arrange
            var workItemServiceMock = new Mock<IWorkItemService>();
            workItemServiceMock
                .Setup(x => x.Create(It.IsAny<WorkItemDetailsViewModel>()))
                .Returns(true);
            var controller = new WorkItemController(workItemServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("name", "testuser") }, "mock"))
                }
            };
            controller.TempData = new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            var workItemVM = new WorkItemDetailsViewModel();

            // Act
            var result = controller.Create(workItemVM);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(Index), redirectToActionResult.ActionName);
            Assert.True(controller.TempData.ContainsKey("success"));
            Assert.Equal("Task has been created successfully.", controller.TempData["success"]);
        }

        [Fact]
        public void Create_SetsErrorTempData_WhenWorkItemCreationFails()
        {
            // Arrange
            var workItemServiceMock = new Mock<IWorkItemService>();
            workItemServiceMock
                .Setup(x => x.Create(It.IsAny<WorkItemDetailsViewModel>()))
                .Returns(false);
            var controller = new WorkItemController(workItemServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("name", "testuser") }, "mock"))
                }
            };
            controller.TempData = new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            var workItemVM = new WorkItemDetailsViewModel();

            // Act
            var result = controller.Create(workItemVM);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(Index), redirectToActionResult.ActionName);
            Assert.True(controller.TempData.ContainsKey("error"));
            Assert.Equal("Something went wrong while creating your task.", controller.TempData["error"]);
        }

        [Fact]
        public void Update_ValidWorkItemDetailsViewModel_ReturnsRedirectResult()
        {
            // Arrange
            var workItemService = new Mock<IWorkItemService>();
            var controller = new WorkItemController(workItemService.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("name", "testuser") }, "mock"))
                }
            };
            controller.TempData = new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            var workItemVM = new WorkItemDetailsViewModel
            {
                // set properties of the view model
            };

            workItemService.Setup(x => x.Update(workItemVM)).Returns(true);

            // Act
            var result = controller.Update(workItemVM);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
            Assert.True(controller.TempData.ContainsKey("success"));
            Assert.Equal("Task has been updated successfully.", controller.TempData["success"]);
        }

        [Fact]
        public void Update_InvalidWorkItemDetailsViewModel_ReturnsViewResult()
        {
            // Arrange
            var workItemService = new Mock<IWorkItemService>();
            var controller = new WorkItemController(workItemService.Object);

            var workItemVM = new WorkItemDetailsViewModel
            {
                // set properties of the view model in a way that ModelState is not valid
            };

            controller.ModelState.AddModelError("", "Some error");

            // Act
            var result = controller.Update(workItemVM);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(workItemVM, (result as ViewResult).Model);
        }

        [Fact]
        public void Update_WorkItemServiceReturnsFalse_ReturnsRedirectResult()
        {
            // Arrange
            var workItemService = new Mock<IWorkItemService>();
            var controller = new WorkItemController(workItemService.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("name", "testuser") }, "mock"))
                }
            };
            controller.TempData = new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            var workItemVM = new WorkItemDetailsViewModel
            {
                // set properties of the view model
            };

            workItemService.Setup(x => x.Update(workItemVM)).Returns(false);

            // Act
            var result = controller.Update(workItemVM);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
            Assert.True(controller.TempData.ContainsKey("error"));
            Assert.Equal("Something went wrong while updating your task.", controller.TempData["error"]);
        }
    }
}
