using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using SportsClubPlatform.Domain.Entities;
using SportsClubPlatform.Domain.Enums;

namespace SportsClubPlatform.UnitTests.Domain
{
    public sealed class TransferTests
    {
        [Fact]
        public void Create_Should_Create_Transfer_With_OfferSubmitted_Status()
        {
            // Act
            Transfer transfer = Transfer.Create(
                playerId: 1,
                sourceClubId: 2,
                destinationClubId: 1,
                offerAmount: 5_000_000m,
                salaryProposed: 1_200_000m);

            // Assert
            transfer.PlayerId.Should().Be(1);
            transfer.SourceClubId.Should().Be(2);
            transfer.DestinationClubId.Should().Be(1);
            transfer.OfferAmount.Should().Be(5_000_000m);
            transfer.SalaryProposed.Should().Be(1_200_000m);
            transfer.Status.Should().Be(TransferStatus.OfferSubmitted);
        }

        [Fact]
        public void Create_Should_Throw_When_Source_And_Destination_Are_The_Same()
        {
            // Act
            Action act = () => Transfer.Create(
                playerId: 1,
                sourceClubId: 1,
                destinationClubId: 1,
                offerAmount: 5_000_000m,
                salaryProposed: 1_200_000m);

            // Assert
            act.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("Source and destination clubs must be different.");
        }

        [Fact]
        public void MarkCompleted_Should_Change_Status_To_Completed()
        {
            // Arrange
            Transfer transfer = Transfer.Create(
                playerId: 1,
                sourceClubId: 2,
                destinationClubId: 1,
                offerAmount: 5_000_000m,
                salaryProposed: 1_200_000m);

            // Act
            transfer.MarkCompleted();

            // Assert
            transfer.Status.Should().Be(TransferStatus.Completed);
        }

        [Fact]
        public void MarkAsFailed_Should_Set_Status_And_ErrorMessage()
        {
            // Arrange
            Transfer transfer = Transfer.Create(
                playerId: 1,
                sourceClubId: 2,
                destinationClubId: 1,
                offerAmount: 5_000_000m,
                salaryProposed: 1_200_000m);

            // Act
            transfer.MarkAsFailed("Payment failed.");

            // Assert
            transfer.Status.Should().Be(TransferStatus.Failed);
            transfer.ErrorMessage.Should().Be("Payment failed.");
        }

        [Fact]
        public void MarkAsCompensated_Should_Change_Status_To_Compensated()
        {
            // Arrange
            Transfer transfer = Transfer.Create(
                playerId: 1,
                sourceClubId: 2,
                destinationClubId: 1,
                offerAmount: 5_000_000m,
                salaryProposed: 1_200_000m);

            // Act
            transfer.MarkAsCompensated();

            // Assert
            transfer.Status.Should().Be(TransferStatus.Compensated);
        }
    }
}
