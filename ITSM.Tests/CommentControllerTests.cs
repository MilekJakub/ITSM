using ITSM.Controllers;
using ITSM.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Security.Claims;
using Xunit;

namespace ITSM.Tests
{
    public class CommentControllerTests
    {
        [Fact]
        public void AddComment_ReturnsRedirectToActionResult_WithSuccessMessage()
        {
            // Arrange
            var commentServiceMock = new Mock<ICommenctService>();
            commentServiceMock.Setup(x => x.AddComment(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>()))
                .Returns(true);

            var controller = new CommentController(commentServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("name", "testuser") }, "mock"))
                }
            };
            controller.TempData = new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            // Act
            var result = controller.AddComment(1, "test message");

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirectToActionResult.ActionName);
            Assert.Equal("Project", redirectToActionResult.ControllerName);
            Assert.Equal(1, redirectToActionResult?.RouteValues?["id"]);
            Assert.Equal("Comment has been added.", controller.TempData["success"]);
        }

        [Fact]
        public void AddComment_ReturnsRedirectToActionResult_WithErrorMessage()
        {
            // Arrange
            var commentServiceMock = new Mock<ICommenctService>();
            commentServiceMock.Setup(x => x.AddComment(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>()))
                .Returns(false);

            var controller = new CommentController(commentServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("name", "testuser") }, "mock"))
                }
            };
            controller.TempData = new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            // Act
            var result = controller.AddComment(1, "test message");

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirectToActionResult.ActionName);
            Assert.Equal("Project", redirectToActionResult.ControllerName);
            Assert.Equal(1, redirectToActionResult?.RouteValues?["id"]);
            Assert.Equal("Something went wrong while creating your comment.", controller.TempData["error"]);
        }

        [Fact]
        public void DeleteComment_ReturnsRedirectToActionResult_WithSuccessMessage()
        {
            // Arrange
            var commentServiceMock = new Mock<ICommenctService>();
            commentServiceMock.Setup(x => x.DeleteComment(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>())).Returns(true);
            var controller = new CommentController(commentServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("name", "testuser") }, "mock"))
                }
            };
            controller.TempData = new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            // Act
            var result = controller.DeleteComment(1, 1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirectToActionResult.ActionName);
            Assert.Equal("Project", redirectToActionResult.ControllerName);
            Assert.Equal(1, redirectToActionResult?.RouteValues?["id"]);
            Assert.Equal("Comment has been removed.", controller.TempData["success"]);
        }

        [Fact]
        public void DeleteComment_ReturnsRedirectToActionResult_WithErrorMessage()
        {
            // Arrange
            var commentServiceMock = new Mock<ICommenctService>();
            commentServiceMock.Setup(x => x.DeleteComment(It.IsAny<int>(), It.IsAny<ClaimsPrincipal>()))
                .Returns(false);

            var controller = new CommentController(commentServiceMock.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("name", "testuser") }, "mock"))
                }
            };
            controller.TempData = new TempDataDictionary(controller.ControllerContext.HttpContext, Mock.Of<ITempDataProvider>());

            // Act
            var result = controller.DeleteComment(1, 1);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirectToActionResult.ActionName);
            Assert.Equal("Project", redirectToActionResult.ControllerName);
            Assert.Equal(1, redirectToActionResult?.RouteValues?["id"]);
            Assert.Equal("Something went wrong while creating your comment.", controller.TempData["error"]);
        }
    }
}