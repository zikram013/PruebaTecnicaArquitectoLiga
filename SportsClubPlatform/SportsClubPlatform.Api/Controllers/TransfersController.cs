using Microsoft.AspNetCore.Mvc;
using SportsClubPlatform.Application.Abstractions;
using SportsClubPlatform.Contracts.Transfers;

namespace SportsClubPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class TransfersController : ControllerBase
    {
        private readonly ITransferApplicationService _transferApplicationService;

        public TransfersController(ITransferApplicationService transferApplicationService)
        {
            _transferApplicationService = transferApplicationService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(TransferResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> SubmitTransferOffer(
            [FromBody] SubmitTransferOfferRequest request,
            CancellationToken cancellationToken)
        {
            TransferResponse response = await _transferApplicationService
                .SubmitTransferOfferAsync(request, cancellationToken);

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(TransferResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken cancellationToken)
        {
            TransferResponse? response = await _transferApplicationService
                .GetByIdAsync(id, cancellationToken);

            if (response is null)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
