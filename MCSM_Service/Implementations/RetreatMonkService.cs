using AutoMapper;
using MCSM_Data.Entities;
using MCSM_Data.Models.Requests.Filters;
using MCSM_Data.Models.Requests.Get;
using MCSM_Data.Models.Requests.Post;
using MCSM_Data.Models.Requests.Put;
using MCSM_Data.Models.Views;
using MCSM_Data.Repositories.Interfaces;
using MCSM_Data;
using MCSM_Utility.Exceptions;
using MCSM_Service.Interfaces;
using MCSM_Data.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using MCSM_Utility.Constants;

namespace MCSM_Service.Implementations
{
    public class RetreatMonkService : BaseService, IRetreatMonkService
    {
        private readonly IRetreatMonkRepository _retreatMonkRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRetreatRepository _retreatRepository;
        public RetreatMonkService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _retreatMonkRepository = unitOfWork.RetreatMonk;
            _accountRepository = unitOfWork.Account;
            _retreatRepository = unitOfWork.Retreat;
        }

        public async Task<ListViewModel<RetreatMonkViewModel>> GetRetreatMonksOfARetreat(Guid retreatId, PaginationRequestModel pagination)
        {
            var query = _retreatMonkRepository.GetMany(rm => rm.RetreatId == retreatId).Include(rm => rm.Retreat)
                                                                                       .Include(rm => rm.Monk)
                                                                                       .Include(rm => rm.Monk.Profile);
                                                       //.Where(rm => rm.Monk.Status.Equals()); 
                                                       // ### pending account's status as a constant ###

            var totalRow = await query.AsNoTracking().CountAsync();
            var paginatedQuery = query
                .Skip(pagination.PageNumber * pagination.PageSize)
                .Take(pagination.PageSize);

            var retreatMonks = await paginatedQuery
                .OrderBy(rm => rm.Monk.Profile!.FirstName).ThenBy(rm => rm.Monk.Profile!.LastName)
                .ProjectTo<RetreatMonkViewModel>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();

            return new ListViewModel<RetreatMonkViewModel>
            {
                Pagination = new PaginationViewModel
                {
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize,
                    TotalRow = totalRow,
                },
                Data = retreatMonks
            };
        }

        public async Task<RetreatMonkViewModel> GetRetreatMonk(Guid id)
        {
            return await _retreatMonkRepository.GetMany(rm => rm.Id == id)
                .ProjectTo<RetreatMonkViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy retreat monk");
        }

        public async Task<RetreatMonkViewModel> CreateRetreatMonk(CreateRetreatMonkModel model)
        {
            var existRetreat = await _retreatRepository.GetMany(r => r.Id == model.RetreatId)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy retreat");

            var existMonk = await _accountRepository.GetMany(a => a.Id == model.MonkId).Include(a => a.Role)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy monk");

            if (existMonk.Status.Equals("Inactive")) throw new NotFoundException("Tài khoản monk này đã bị khóa!");

            if (!existMonk.Role.Name.Equals(AccountRole.Monk)) throw new NotFoundException("Người được thêm vào retreat không phải là monk!");

            // ### pending account's status as a constant ###
            // if (existMonk.Status.Equals()) throw new Exception
            // ### NEEDS A CHECK FOR DUPLICATED MONK IN RETREAT

            var retreatMonkId = Guid.NewGuid();
            
            var retreatMonk = new RetreatMonk
            {
                Id = retreatMonkId,
                MonkId = model.MonkId,
                RetreatId = model.RetreatId
            };
            _retreatMonkRepository.Add(retreatMonk);

            var result = await _unitOfWork.SaveChanges();

            return result > 0 ? await GetRetreatMonk(retreatMonkId) : null!;
        }

        public async Task DeleteRetreatMonk(Guid id)
        {
            var existRetreatMonk = await _retreatMonkRepository.GetMany(rm => rm.Id == id)
                .FirstOrDefaultAsync() ?? throw new NotFoundException("Không tìm thấy retreat monk");

            _retreatMonkRepository.Remove(existRetreatMonk);

            await _unitOfWork.SaveChanges();
        }
    }
}
