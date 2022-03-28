using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Utilities;

namespace Core.Interfaces
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetAllMessages(QueryParameters queryParameters);
        Task<int> GetCountForMessages();
        Task<Message> GetMessageById(int id);
        Task CreateMessage(Message message);
        Task UpdateMessage(Message message);
        Task DeleteMessage(Message message);
    }
}