using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using SportsClubPlatform.Domain.Entities;

namespace SportsClubPlatform.UnitTests.Domain
{
    public sealed class BudgetTests
    {
        [Fact]
        public void Reserve_Should_Decrease_AvailableAmount_When_Budget_Is_Enough()
        {
            // Arrange
            Budget budget = new(
                clubId: 1,
                availableAmount: 10_000_000m,
                currency: "EUR");

            // Act
            budget.Reserve(2_500_000m);

            // Assert
            budget.AvailableAmount.Should().Be(7_500_000m);
        }

        [Fact]
        public void Reserve_Should_Throw_When_Budget_Is_Not_Enough()
        {
            // Arrange
            Budget budget = new(
                clubId: 1,
                availableAmount: 500_000m,
                currency: "EUR");

            // Act
            Action act = () => budget.Reserve(5_000_000m);

            // Assert
            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("The club does not have enough available budget.");
        }

        [Fact]
        public void Release_Should_Increase_AvailableAmount()
        {
            // Arrange
            Budget budget = new(
                clubId: 1,
                availableAmount: 1_000_000m,
                currency: "EUR");

            // Act
            budget.Release(250_000m);

            // Assert
            budget.AvailableAmount.Should().Be(1_250_000m);
        }

        [Fact]
        public void Reserve_Should_Throw_When_Amount_Is_Zero()
        {
            // Arrange
            Budget budget = new(
                clubId: 1,
                availableAmount: 1_000_000m,
                currency: "EUR");

            // Act
            Action act = () => budget.Reserve(0);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
