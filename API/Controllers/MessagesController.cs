using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Handles messaging system, visitors can send messages to our employees
    /// </summary>
    public class MessagesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessagesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<MessageDto>>> GetAllMessages([FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.MessageRepository.GetCountForMessages();
            
            var list = await _unitOfWork.MessageRepository.GetAllMessages(queryParameters);

            var data = _mapper.Map<IEnumerable<MessageDto>>(list);

            return Ok(new Pagination<MessageDto>(queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MessageDto>> GetMessageById(int id)
        {
            var message = await _unitOfWork.MessageRepository.GetMessageById(id);

            if (message == null) return NotFound();

            return _mapper.Map<MessageDto>(message);
        }

        [HttpPost]
        public async Task<ActionResult> CreateMessage([FromBody] MessageCreateEditDto messageDto)
        {
            var message = _mapper.Map<Message>(messageDto);
           
            await _unitOfWork.MessageRepository.CreateMessage(message);

            return Ok();
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMessage(int id, [FromBody] MessageCreateEditDto messageDto)
        {
            var message = _mapper.Map<Message>(messageDto);

            if (id != message.Id) return BadRequest("Bad request!");

            await _unitOfWork.MessageRepository.UpdateMessage(message);
                        
            return NoContent();
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var message = await _unitOfWork.MessageRepository.GetMessageById(id);

            if (message == null) return NotFound();

            await _unitOfWork.MessageRepository.DeleteMessage(message);

            return NoContent();
        }          
    }
}



