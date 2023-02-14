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
    public class ProjectControllerTests
    {
        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfTruncatedProjects()
        {
            // Arrange
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.GetAll()).Returns(GetTestProjects());

            var controller = new ProjectController(mockProjectService.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Project>>(viewResult.Model);
            Assert.Equal(GetTestProjects().Count, model.Count());
        }

        private List<Project> GetTestProjects()
        {
            return new List<Project>
            {
                new Project { Id = 1, Name = "Project 1" },
                new Project { Id = 2, Name = "Project 2" },
                new Project { Id = 3, Name = "Project 3" },
            };
        }

        [Fact]
        public void Create_Successful()
        {
            // Arrange
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(p => p.Create(It.IsAny<ProjectDetailsViewModel>()))
                .Returns(true);

            var controller = new ProjectController(mockProjectService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("name", "testuser") }, "mock"))
                }
            };
            controller.TempData = new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            // Act
            var result = controller.Create(new ProjectDetailsViewModel());

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Project has been created successfully.", controller.TempData["success"]);
        }

        [Fact]
        public void Create_ModelStateInvalid()
        {
            // Arrange
            var mockProjectService = new Mock<IProjectService>();

            var controller = new ProjectController(mockProjectService.Object);
            controller.ModelState.AddModelError("Error", "Error occurred");

            // Act
            var result = controller.Create(new ProjectDetailsViewModel());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Details", viewResult.ViewName);
            Assert.IsType<ProjectDetailsViewModel>(viewResult.Model);
        }

        [Fact]
        public void Create_Error()
        {
            // Arrange
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(p => p.Create(It.IsAny<ProjectDetailsViewModel>()))
                .Returns(false);

            var controller = new ProjectController(mockProjectService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("name", "testuser") }, "mock"))
                }
            };
            controller.TempData = new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            // Act
            var result = controller.Create(new ProjectDetailsViewModel());

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Equal("Something went wrong while creating a project.", controller.TempData["error"]);
        }

        [Fact]
        public void GetProjectDetails_ReturnsProjectDetails()
        {
            // Arrange
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.GetProjectDetailsVM(It.IsAny<int>()))
                .Returns(new ProjectDetailsViewModel());
            var controller = new ProjectController(mockProjectService.Object);

            // Act
            var result = controller.Details(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = result.Model as ProjectDetailsViewModel;
        }

        [Fact]
        public void GetProjectDetails_ReturnsRedirectToIndex_WhenProjectDetailsVMIsNull()
        {
            // Arrange
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(service => service.GetProjectDetailsVM(It.IsAny<int>()))
                .Returns((ProjectDetailsViewModel)null!);
            var controller = new ProjectController(mockProjectService.Object);

            // Act
            var result = controller.Details(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void Update_WhenModelStateIsInvalid_ReturnsViewResult()
        {
            // Arrange
            var mockProjectService = new Mock<IProjectService>();
            var controller = new ProjectController(mockProjectService.Object);
            controller.ModelState.AddModelError("error", "error");

            var projectDetailsVM = new ProjectDetailsViewModel
            {
                Project = new Project { Id = 1 }
            };

            // Act
            var result = controller.Update(projectDetailsVM, 1);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void Update_WhenUpdateIsSuccessful_RedirectsToDetailsAction()
        {
            // Arrange
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(x => x.Update(It.IsAny<ProjectDetailsViewModel>()))
                .Returns(true);

            var controller = new ProjectController(mockProjectService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("name", "testuser") }, "mock"))
                }
            };
            controller.TempData = new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            var projectDetailsVM = new ProjectDetailsViewModel
            {
                Project = new Project { Id = 1 }
            };

            // Act
            var result = controller.Update(projectDetailsVM, 1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirectToActionResult.ActionName);
            Assert.Equal(projectDetailsVM.Project.Id, redirectToActionResult.RouteValues["id"]);
            Assert.Equal("Project has been updated succesfully.", controller.TempData["success"]);
        }

        [Fact]
        public void Update_WhenUpdateIsUnsuccessful_RedirectsToDetailsAction()
        {
            // Arrange
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService.Setup(x => x.Update(It.IsAny<ProjectDetailsViewModel>()))
                .Returns(false);

            var controller = new ProjectController(mockProjectService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("name", "testuser") }, "mock"))
                }
            };
            controller.TempData = new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            var projectDetailsVM = new ProjectDetailsViewModel
            {
                Project = new Project { Id = 1 }
            };

            // Act
            var result = controller.Update(projectDetailsVM, 1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirectToActionResult.ActionName);
            Assert.Equal(projectDetailsVM.Project.Id, redirectToActionResult.RouteValues["id"]);
            Assert.Equal("Something went wrong while updating the project.", controller.TempData["error"]);
        }
    }
}
