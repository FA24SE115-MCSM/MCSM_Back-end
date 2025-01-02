using AutoMapper;
using AutoMapper.QueryableExtensions;
using MCSM_Data;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
using MCSM_Utility.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace MCSM_Service.Implementations
{
    public class ChatService : BaseService, IChatService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly IConversationParticipantRepository _conversationParticipantRepository;
        private readonly IMessageRepository _messageRepository;

        public ChatService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _accountRepository = unitOfWork.Account;
            _conversationRepository = unitOfWork.Conversation;
            _conversationParticipantRepository = unitOfWork.ConversationParticipant;
            _messageRepository = unitOfWork.Message;
        }

        public async Task<List<AccountViewModel>> GetAccounts()
        {
            return await _accountRepository.GetAll()
                .ProjectTo<AccountViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task IsAccountOnline(Guid accountId, bool isOnline)
        {
            var user = await _accountRepository.GetMany(acc => acc.Id == accountId)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Account not found");
            user.IsOnline = isOnline;
            _accountRepository.Update(user);
            var result = await _unitOfWork.SaveChanges();
        }


        public async Task<ConversationViewModel> GetConversation(Guid conversationId)
        {
            var conversation = await _conversationRepository.GetMany(c => c.Id == conversationId)
                .ProjectTo<ConversationViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Conversation not found");

            conversation.Messages = conversation.Messages
                .OrderBy(m => m.SendAt)
                .ToList();

            return conversation;
        }


        public async Task<ConversationViewModel> GetConversation(Guid senderId, Guid receiverId)
        {
            var conversation = await _conversationRepository.GetMany(conversation => conversation.ConversationParticipants
                                                                                        .Any(participant => participant.AccountId == senderId)
                                                                                     && conversation.ConversationParticipants
                                                                                        .Any(participant => participant.AccountId == receiverId)).FirstOrDefaultAsync();

            if (conversation == null)
            {
                var newConversationId = Guid.NewGuid();

                var newConversation = new Conversation
                {
                    Id = newConversationId,
                };
                _conversationRepository.Add(newConversation);

                var newParticipant = new ConversationParticipant
                {
                    ConversationId = newConversationId,
                    AccountId = senderId,
                };
                _conversationParticipantRepository.Add(newParticipant);

                var newParticipant2 = new ConversationParticipant
                {
                    ConversationId = newConversationId,
                    AccountId = receiverId,
                };
                _conversationParticipantRepository.Add(newParticipant2);

                await _unitOfWork.SaveChanges();
                conversation = newConversation;
            }

            var messageToUpdate = await _messageRepository.GetMany(m => m.ConversationId == conversation.Id &&
                                                                    m.SenderId == receiverId &&
                                                                    m.IsRead == false).ToListAsync();
            if (messageToUpdate.Any())
            {
                foreach (var message in messageToUpdate)
                {
                    message.IsRead = true;
                }

                _messageRepository.UpdateRange(messageToUpdate);
                await _unitOfWork.SaveChanges();
            }

            return await GetConversation(conversation.Id);
        }


        public async Task CreateMessage(CreateMessageModel model)
        {
            var message = new Message
            {
                Id = Guid.NewGuid(),
                ConversationId = model.ConversationId,
                SenderId = model.SenderId,
                Content = model.Content,
                IsRead = model.IsRead,
            };
            _messageRepository.Add(message);
            await _unitOfWork.SaveChanges();

            await Task.CompletedTask;
        }

        public async Task DeleteMessage(Guid messageId, Guid userId)
        {
            var message = await _messageRepository.GetMany(m => m.Id == messageId).FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy");
            if (message.SenderId == userId)
            {
                _messageRepository.Remove(message);
                await _unitOfWork.SaveChanges();
            }

            await Task.CompletedTask;
        }

        public async Task<List<Guid>> GetUnReadMessage(Guid accountId)
        {
            var unReadSenderIds = await _messageRepository.GetMany(m => !m.IsRead && m.SenderId != accountId && m.Conversation.ConversationParticipants.Any(cp => cp.AccountId == accountId))
                .Select(m => m.SenderId)
                .Distinct()
                .ToListAsync();
            return unReadSenderIds;
        }
    }
}
