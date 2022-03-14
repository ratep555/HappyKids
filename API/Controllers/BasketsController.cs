using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos.BasketsDtos;
using Core.Entities.ClientBaskets;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketsController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketsController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ClientBasket>> GetClientBasket(string id)
        {
            var basket = await _basketRepository.GetClientBasket(id);

            return Ok(basket ?? new ClientBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<ClientBasket>> EditClientBasket(ClientBasketDto basketDto)
        {
            var basket = _mapper.Map<ClientBasket>(basketDto);

            var editedBasket = await _basketRepository.EditClientBasket(basket);

            return Ok(editedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteClientBasket(id);
        }
    }
}