using FluentAssertions;
using RestaurantSystem.Services;

namespace RestaurantSystem.Tests
{
    public class PaymentServiceTests
    {
        [Fact]
        public void PaymentService_AddRandomMinutes_ShouldAddRandomMinutes()
        {
            // Arrange
            PaymentService paymentService = new PaymentService();
            var dateTime = new DateTime(2022, 1, 1, 12, 0, 0);

            // Act
            var result = paymentService.AddRandomMinutes(dateTime);

            // Assert
            result.Should().BeAfter(dateTime);
        }

        [Fact]
        public void PaymentService_GetPayment_ReturnTrue()
        {
            // Arrange
            PaymentService paymentService = new PaymentService();

            // Act
            var result = paymentService.GetPayment();

            // Assert
            result.Should().BeTrue();
        }

    }
}
