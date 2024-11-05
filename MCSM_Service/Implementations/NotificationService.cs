using AutoMapper;
using AutoMapper.QueryableExtensions;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MCSM_Data;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Service.Interfaces;
using MCSM_Utility.Exceptions;
using Microsoft.EntityFrameworkCore;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using Google.Api.Gax;

namespace MCSM_Service.Implementations
{
    public class NotificationService : BaseService, INotificationService
    {
        private readonly IDeviceTokenRepository _deviceTokenRepository;
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _deviceTokenRepository = unitOfWork.DeviceToken;
            _notificationRepository = unitOfWork.Notification;
        }

        public async Task<NotificationViewModel> GetNotification(Guid id)
        {
            return await _notificationRepository.GetMany(notification => notification.Id.Equals(id))
                .ProjectTo<NotificationViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Notification not found");
        }

        public async Task<ListViewModel<NotificationViewModel>> GetNotifications(Guid accountId, PaginationRequestModel pagination)
        {
            var query = _notificationRepository.GetMany(noti => noti.AccountId.Equals(accountId));
            var notifications = await query
                .OrderByDescending(noti => noti.CreateAt)
                .ProjectTo<NotificationViewModel>(_mapper.ConfigurationProvider)
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize)
                .AsNoTracking()
                .ToListAsync();
            var totalRow = await query.AsNoTracking().CountAsync();
            return notifications != null ? new ListViewModel<NotificationViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow
                },
                Data = notifications
            } : null!;
        }

        

        public async Task<bool> SendNotification(ICollection<Guid> accountIds, CreateNotificationModel model)
        {
            var deviceTokens = await _deviceTokenRepository.GetMany(token => accountIds.Contains(token.AccountId))
                .Select(token => token.Token)
                .ToListAsync();
            var now = DateTime.UtcNow.AddHours(7);
            foreach (var accountId in accountIds)
            {
                var notification = new MCSM_Data.Entities.Notification
                {
                    Id = Guid.NewGuid(),
                    AccountId = accountId,
                    CreateAt = now,
                    Body = model.Body,
                    Type = model.Data.Type,
                    Link = model.Data.Link,
                    Title = model.Title
                };
                _notificationRepository.Add(notification);
            }

            var result = await _unitOfWork.SaveChanges();
            if (result > 0)
            {
                if (deviceTokens.Any())
                {
                    var messageData = new Dictionary<string, string>
                    {
                        { "type", model.Data.Type ?? "" },
                        { "link", model.Data.Link ?? "" },
                        { "createAt", now.ToString() }
                    };
                    var message = new MulticastMessage()
                    {
                        Notification = new Notification()
                        {
                            Title = model.Title,
                            Body = model.Body
                        },
                        Data = messageData,
                        Tokens = deviceTokens
                    };
                    var app = FirebaseApp.DefaultInstance;
                    if (FirebaseApp.DefaultInstance == null)
                    {
                        GoogleCredential credential;
                        var credentialJson = Environment.GetEnvironmentVariable("GoogleCloudCredential");
                        if (string.IsNullOrWhiteSpace(credentialJson))
                        {
                            var basePath = AppDomain.CurrentDomain.BaseDirectory;
                            var projectRoot = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", ".."));
                            string credentialPath = Path.Combine(projectRoot, "MCSM_Utility", "Helpers", "CloudStorage", "mcsm-fa24se115-firebase-adminsdk-9hr0w-9c32ab4d6f.json");
                            credential = GoogleCredential.FromFile(credentialPath);
                        }
                        else
                        {
                            credential = GoogleCredential.FromJson(credentialJson);
                        }

                        app = FirebaseApp.Create(new AppOptions()
                        {
                            Credential = credential
                        });
                    }
                    FirebaseMessaging messaging = FirebaseMessaging.GetMessaging(app);
                    await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
                }
            }
            return true;
        }

        public async Task<NotificationViewModel> UpdateNotification(Guid id, UpdateNotificationModel model)
        {
            var notification = await _notificationRepository.GetMany(notification => notification.Id.Equals(id)).FirstOrDefaultAsync();
            if (notification == null)
            {
                return null!;
            }
            notification.IsRead = true;
            _notificationRepository.Update(notification);
            var result = await _unitOfWork.SaveChanges();
            return result > 0 ? await GetNotification(id) : null!;
        }

        public async Task<bool> MakeAsRead(Guid accountId)
        {
            var notifications = await _notificationRepository.GetMany(notification => notification.AccountId.Equals(accountId)).ToListAsync();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
            _notificationRepository.UpdateRange(notifications);
            var result = await _unitOfWork.SaveChanges();
            return result > 0;
        }

        public async Task<bool> TestSendNotification(Guid accountId)
        {
            var deviceTokens = await GetDeviceToken(accountId);
            if(deviceTokens == null || deviceTokens.Count == 0)
            {
                throw new BadRequestException("Tài khoản này chưa đăng ký mã thông báo thiết bị với hệ thống");
            }

            if (deviceTokens.Any())
            {
                var messageData = new Dictionary<string, string>
                    {
                        { "type", "type_ne" },
                        { "link", "link_ne" },
                        { "createAt", DateTime.Now.ToString() }
                    };
                var message = new MulticastMessage()
                {
                    Notification = new FirebaseAdmin.Messaging.Notification()
                    {
                        Title = "Test thông báo",
                        Body = "Test thông báo."
                    },
                    Data = messageData,
                    Tokens = deviceTokens
                };
                var app = FirebaseApp.DefaultInstance;
                if (FirebaseApp.DefaultInstance == null)
                {
                    GoogleCredential credential;
                    var credentialJson = Environment.GetEnvironmentVariable("GoogleCloudCredential");
                    if (string.IsNullOrWhiteSpace(credentialJson))
                    {
                        var basePath = AppDomain.CurrentDomain.BaseDirectory;
                        var projectRoot = Path.GetFullPath(Path.Combine(basePath, "..", "..", "..", ".."));
                        string credentialPath = Path.Combine(projectRoot, "MCSM_Utility", "Helpers", "CloudStorage", "mcsm-fa24se115-firebase-adminsdk-9hr0w-9c32ab4d6f.json");
                        credential = GoogleCredential.FromFile(credentialPath);
                    }
                    else
                    {
                        credential = GoogleCredential.FromJson(credentialJson);
                    }

                    app = FirebaseApp.Create(new AppOptions()
                    {
                        Credential = credential
                    });
                }
                FirebaseMessaging messaging = FirebaseMessaging.GetMessaging(app);
                var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
                return true;
            }

            return false;
        }

        public async Task<List<string>> GetDeviceToken(Guid accountId)
        {
            return await _deviceTokenRepository.GetMany(token => token.AccountId.Equals(accountId))
                .Select(token => token.Token)
                .ToListAsync();
        }
    }
}
