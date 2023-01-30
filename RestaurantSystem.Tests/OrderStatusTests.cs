using FluentAssertions;
using RestaurantSystem.Services;

namespace RestaurantSystem.Tests
{
    public class OrderStatusTests
    {
        [Fact]
        public void OrderService_CountAllDifferentChoices_ShouldReturnCorrectCounts()
        {
            //Arrange
            OrderService orderService = new OrderService();
            string[] list = new string[] { "A1", "B1", "A1", "B2", "C1" };
            Dictionary<string, int> expected = new Dictionary<string, int>()
            {
                { "A1", 2 },
                { "B1", 1 },
                { "B2", 1 },
                { "C1", 1 }
            };

            //Act
            var result = orderService.CountAllDifferentChoices(list);

            //Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
