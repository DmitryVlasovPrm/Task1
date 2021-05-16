using WebApi;
using WebApi.Controllers;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace UnitTests
{
	public class UsersController_Tests
	{
		private const string connString = "Server=(localdb)\\MSSQLLocalDB;Database=Server;Trusted_Connection=True;";

		[Fact]
		public async void GetNonExistentUser()
		{
			// Arrange
			var dbContext = new ServerContext(connString);
			var controller = new UsersController(dbContext);
			// Act
			var result = await controller.Get(9999);
			// Assert
			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async void GetExistentUser()
		{
			// Arrange
			var dbContext = new ServerContext(connString);
			var controller = new UsersController(dbContext);
			// Act
			var result = await controller.Get(2006);
			// Assert
			Assert.IsType<ObjectResult>(result.Result);
		}
	}
}
