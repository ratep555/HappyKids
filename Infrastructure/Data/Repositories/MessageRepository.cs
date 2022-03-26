using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly HappyKidsContext _context;
        public MessageRepository(HappyKidsContext context)
        {
            _context = context;
        }

        public async Task<List<Message>> GetAllMessages(QueryParameters queryParameters)
        {
            IQueryable<Message> messages = _context.Messages.AsQueryable().OrderBy(x => x.SendingDate);

            if (queryParameters.HasQuery())
            {
                messages = messages.Where(t => t.FirstLastName.Contains(queryParameters.Query));
            }

            messages = messages.Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);

            if (!string.IsNullOrEmpty(queryParameters.Sort))
            {
                switch (queryParameters.Sort)
                {
                    case "all":
                        messages = messages.OrderByDescending(p => p.SendingDate);
                        break;
                    case "answered":
                        messages = messages.Where(p => p.IsReplied == true);
                        break;
                    case "unanswered":
                        messages = messages.Where(p => p.IsReplied == false);
                        break;
                    default:
                        messages = messages.OrderByDescending(n => n.SendingDate);
                        break;
                }
            }   

            return await messages.ToListAsync();
        }

        public async Task<int> GetCountForMessages()
        {
            return await _context.Messages.CountAsync();
        }

        public async Task<Message> GetMessageById(int id)
        {
            return await _context.Messages.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateMessage(Message message)
        {
            message.SendingDate = DateTime.Now.ToLocalTime();

            _context.Messages.Add(message);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateMessage(Message message)
        {
            _context.Entry(message).State = EntityState.Modified;

             await _context.SaveChangesAsync();
        }
    }
}













