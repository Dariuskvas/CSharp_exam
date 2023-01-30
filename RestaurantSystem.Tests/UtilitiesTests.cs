global using Xunit;
using FluentAssertions;

namespace RestaurantSystem.Tests;

public class UtilitiesTests
{
    [Fact]
    public void Utilities_CreateCustomerGroupsSize_ReturnIntArray()
    {
        //Arrange
        Utilities utilities = new Utilities();

        //Act
        var result = utilities.CreateCustomerGroupsSize();

        //Assert
        result.Should().NotBeNull();
        result.Length.Should().Be(8);
    }

    [Fact]
    public void Utilities_CreateTimeSegments_ReturnIntIntFromInterval()
    {
        //Arrange
        Utilities utilities = new Utilities();

        //Act
        var result = utilities.CreateTimeSegments();

        //Assert
        result.Should().BeInRange(40, 80);
    }
}