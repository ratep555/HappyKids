using System.Threading.Tasks;
using Core.Entities.ClientBaskets;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private readonly IConfiguration _config;
        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger,
            IConfiguration config)
        {
            _logger = logger;
            _paymentService = paymentService;
            _config = config;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<ClientBasket>> CreatingOrUpdatingPaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreatingOrUpdatingPaymentIntent(basketId);

            if (basket == null) 
                return BadRequest(/* new ServerResponse(400, */ "Problem creating or updating basket");

            return basket;
        }
    }
}










