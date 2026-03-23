
using Application.DTOs.User;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Domain.Enums;
using Domain.Entities.UsersEnities;

namespace Application.Services
{
    public class BrokerService : IBrokerService
    {
        private readonly IBrokerRepo _brokerRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public BrokerService(IBrokerRepo brokerRepo, IMapper mapper, UserManager<User> userManager)
        {
            _brokerRepo = brokerRepo;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<List<BrokerItemList>> GetAllBrokers(CancellationToken ct)
        {
            var brokers = await _userManager.GetUsersInRoleAsync(UserRoles.Broker.ToString().ToUpper());
            return _mapper.Map<List<BrokerItemList>>(brokers);
        }

        public async Task<Broker> GetBrokerBySlug(string slug, CancellationToken ct)
        {
            var broker = await _brokerRepo.GetBrokerBySlug(slug, ct);
            if (broker is null) throw ApiException.NotFound("هذا مستخدم غير موجود");
            return _mapper.Map<Broker>(broker);
        }
    }
}
