using FluentAssertions;
using RestaurantSystem.Services;

namespace RestaurantSystem.Tests
{
    public class TableReservationServiceTests
    {
        [Fact]
        public void TableReservationService_ChekWaitingPeople_CustGroupsArrayIsEmpty()
        {
            //Arrange
            var tableReservationService = new TableReservationService();
            tableReservationService.custGroupsArray = new int[1];

            // Act
            var result = tableReservationService.ChekWaitingPeople();

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 2, 3 })]
        [InlineData(new int[] { 4, 5, 6, 7 }, new int[] { 5, 6, 7 })]
        public void TableReservationService_DeleteFirstFromArray_ShouldRemoveFirstElement(int[] inputArray, int[] expectedArray)
        {
            //Arrange
            var tableReservationService = new TableReservationService();
            tableReservationService.custGroupsArray = inputArray;
            // Act
            tableReservationService.DeleteFirstFromArray(inputArray);

            // Assert
            tableReservationService.custGroupsArray.Should().Equal(expectedArray);
        }




    }
}