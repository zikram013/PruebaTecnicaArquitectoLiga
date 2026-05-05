using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using SportsClubPlatform.Domain.Entities;

namespace SportsClubPlatform.UnitTests.Domain
{
    public sealed class PlayerContractTests
    {
        [Fact]
        public void IsValidOn_Should_Return_True_When_Contract_Is_Active_And_Date_Is_In_Range()
        {
            // Arrange
            PlayerContract contract = new(
                playerId: 1,
                clubId: 2,
                startDateUtc: new DateTime(2025, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                endDateUtc: new DateTime(2028, 6, 30, 0, 0, 0, DateTimeKind.Utc),
                annualSalary: 1_000_000m,
                isActive: true);

            // Act
            bool result = contract.IsValidOn(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsValidOn_Should_Return_False_When_Contract_Is_Inactive()
        {
            // Arrange
            PlayerContract contract = new(
                playerId: 1,
                clubId: 2,
                startDateUtc: new DateTime(2025, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                endDateUtc: new DateTime(2028, 6, 30, 0, 0, 0, DateTimeKind.Utc),
                annualSalary: 1_000_000m,
                isActive: true);

            contract.Deactivate();

            // Act
            bool result = contract.IsValidOn(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc));

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Constructor_Should_Throw_When_EndDate_Is_Before_StartDate()
        {
            // Act
            Action act = () => new PlayerContract(
                playerId: 1,
                clubId: 2,
                startDateUtc: new DateTime(2028, 6, 30, 0, 0, 0, DateTimeKind.Utc),
                endDateUtc: new DateTime(2025, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                annualSalary: 1_000_000m,
                isActive: true);

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
