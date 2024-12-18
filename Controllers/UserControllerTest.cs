using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Controllers;
using MyMvcApp.Models;
using Xunit;

namespace MyMvcApp.Tests.Controllers
{
    public class UserControllerTest
    {
        private UserController _controller;
        private List<User> _userList;

        public UserControllerTest()
        {
            _userList = new List<User>
            {
                new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
                new User { Id = 2, Name = "Jane Doe", Email = "jane@example.com" }
            };
            UserController.userlist = _userList;
            _controller = new UserController();
        }

        [Fact]
        public void Index_ReturnsViewResult_WithListOfUsers()
        {
            var result = _controller.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<User>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void Details_ReturnsViewResult_WithUser()
        {
            var result = _controller.Details(1);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<User>(viewResult.ViewData.Model);
            Assert.Equal("John Doe", model.Name);
        }

        [Fact]
        public void Details_ReturnsNotFound_WhenUserNotFound()
        {
            var result = _controller.Details(3);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_Get_ReturnsViewResult()
        {
            var result = _controller.Create();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            var newUser = new User { Id = 3, Name = "Sam Smith", Email = "sam@example.com" };
            var result = _controller.Create(newUser);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(3, UserController.userlist.Count);
        }

        [Fact]
        public void Create_Post_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var newUser = new User { Id = 3, Email = "sam@example.com" };
            var result = _controller.Create(newUser);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Edit_Get_ReturnsViewResult_WithUser()
        {
            var result = _controller.Edit(1);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<User>(viewResult.ViewData.Model);
            Assert.Equal("John Doe", model.Name);
        }

        [Fact]
        public void Edit_Get_ReturnsNotFound_WhenUserNotFound()
        {
            var result = _controller.Edit(3);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            var updatedUser = new User { Id = 1, Name = "John Smith", Email = "johnsmith@example.com" };
            var result = _controller.Edit(1, updatedUser);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("John Smith", UserController.userlist[0].Name);
        }

        [Fact]
        public void Edit_Post_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var updatedUser = new User { Id = 1, Email = "johnsmith@example.com" };
            var result = _controller.Edit(1, updatedUser);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Delete_Get_ReturnsViewResult_WithUser()
        {
            var result = _controller.Delete(1);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<User>(viewResult.ViewData.Model);
            Assert.Equal("John Doe", model.Name);
        }

        [Fact]
        public void Delete_Get_ReturnsNotFound_WhenUserNotFound()
        {
            var result = _controller.Delete(3);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_Post_ReturnsRedirectToActionResult()
        {
            var result = _controller.Delete(1, null);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(1, UserController.userlist.Count);
        }
    }
}