using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services
{
    public class PlatformMaintenanceService : BaseService, IPlatformMaintenanceService
    {
        private readonly IRepositorySoftDelete<PlatformMaintenance> _platformMaintenanceRepository;
        private readonly IPlatformRepository _platformRepository;
        private readonly Role[] _allowedRoles = { Role.Admin, Role.StationManager };

        public PlatformMaintenanceService(IMapper mapper, IUserRepository userRepository,
            IRepositorySoftDelete<PlatformMaintenance> platformMaintenanceRepository, IPlatformRepository platformRepository)
            : base(mapper, userRepository)
        {
            _platformMaintenanceRepository = platformMaintenanceRepository;
            _platformRepository = platformRepository;
        }


        public PlatformMaintenanceModel GetById(int id)
        {
            var platformMaintenance = GetPlatformMaintenanceById(id);
            return _mapper.Map<PlatformMaintenanceModel>(platformMaintenance);
        }

        public List<PlatformMaintenanceModel> GetList(int userId)
        {
            CheckUserRole(userId, _allowedRoles);

            var platformsMaintenance = _platformMaintenanceRepository.GetList();
            var x = _mapper.Map<List<PlatformMaintenanceModel>>(platformsMaintenance);
            return x;
        }

        public List<PlatformMaintenanceModel> GetListDeleted(int userId)
        {
            CheckUserRole(userId, Role.Admin);

            var platformsMaintenance = _platformMaintenanceRepository.GetList(true).Where(t => t.IsDeleted);
            return _mapper.Map<List<PlatformMaintenanceModel>>(platformsMaintenance);
        }

        public int Add(int userId, PlatformMaintenanceModel platformMaintenance)
        {
            CheckUserRole(userId, _allowedRoles);

            var platformMaintenanceEntity = _mapper.Map<PlatformMaintenance>(platformMaintenance);
            var platform = _platformRepository.GetById(platformMaintenance.Platform.Id);
            platformMaintenanceEntity.Platform = platform;
            return _platformMaintenanceRepository.Add(platformMaintenanceEntity);

        }

        public void Update(int userId, int id, PlatformMaintenanceModel platformMaintenance)
        {
            CheckUserRole(userId, _allowedRoles);

            var platformMaintenanceEntity = GetPlatformMaintenanceById(id);
            var newPlatformMaintenanceEntity = _mapper.Map<PlatformMaintenance>(platformMaintenance);
            var platform = _platformRepository.GetById(newPlatformMaintenanceEntity.Platform.Id);
            newPlatformMaintenanceEntity.Platform = platform;
            _platformMaintenanceRepository.Update(platformMaintenanceEntity, newPlatformMaintenanceEntity);
        }

        public void Restore(int userId, int id)
        {
            CheckUserRole(userId, Role.Admin);

            var platformMaintenance = GetPlatformMaintenanceById(id);
            _platformMaintenanceRepository.Update(platformMaintenance, false);
        }

        public void Delete(int userId, int id)
        {
            CheckUserRole(userId, _allowedRoles);

            var platformMaintenance = GetPlatformMaintenanceById(id);
            _platformMaintenanceRepository.Update(platformMaintenance, true);
        }

        private PlatformMaintenance GetPlatformMaintenanceById(int id)
        {
            var platformMaintenance = _platformMaintenanceRepository.GetById(id);
            if (platformMaintenance == null)
                throw new NotFoundException(nameof(PlatformMaintenance), id);
            return platformMaintenance;
        }
    }
}
