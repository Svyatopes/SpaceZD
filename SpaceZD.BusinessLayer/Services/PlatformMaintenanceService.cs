using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;
using SpaceZD.DataLayer.Repositories;

namespace SpaceZD.BusinessLayer.Services
{
    public class PlatformMaintenanceService : BaseService, IPlatformMaintenanceService
    {
        private readonly IPlatformMaintenanceRepository _platformMaintenanceRepository;
        private readonly IPlatformRepository _platformRepository;
        private readonly Role[] _allowedRoles = { Role.Admin, Role.StationManager };

        public PlatformMaintenanceService(IMapper mapper, IUserRepository userRepository,
            IPlatformMaintenanceRepository platformMaintenanceRepository, IPlatformRepository platformRepository)
            : base(mapper, userRepository)
        {
            _platformMaintenanceRepository = platformMaintenanceRepository;
            _platformRepository = platformRepository;
        }


        public PlatformMaintenanceModel GetById(int id, int userId)
        {
            CheckUserRole(userId, _allowedRoles);

            var platformMaintenance = GetPlatformMaintenanceById(id);
            var platformMaintenanceModel = _mapper.Map<PlatformMaintenanceModel>(platformMaintenance);
            return platformMaintenanceModel;
        }

        public List<PlatformMaintenanceModel> GetListByStationId(int stationId, int userId)
        {
            CheckUserRole(userId, _allowedRoles);

            var platformsMaintenance = _platformMaintenanceRepository.GetListByStationId(stationId);
            var platformMaintenanceModels = _mapper.Map<List<PlatformMaintenanceModel>>(platformsMaintenance);
            return platformMaintenanceModels;
        }

        public List<PlatformMaintenanceModel> GetListDeletedByStationId(int stationId, int userId)
        {
            CheckUserRole(userId, Role.Admin);

            var platformsMaintenance = _platformMaintenanceRepository.GetListByStationId(stationId, true).Where(t => t.IsDeleted);
            var platformMaintenanceModels = _mapper.Map<List<PlatformMaintenanceModel>>(platformsMaintenance);
            return platformMaintenanceModels;
        }

        public int Add(int userId, PlatformMaintenanceModel platformMaintenanceModel)
        {
            CheckUserRole(userId, _allowedRoles);

            var platformMaintenance = _mapper.Map<PlatformMaintenance>(platformMaintenanceModel);
            var platform = _platformRepository.GetById(platformMaintenanceModel.Platform.Id);
            platformMaintenance.Platform = platform;
            ThrowIfEntityNotFound(platform, platformMaintenance.Platform.Id);

            return _platformMaintenanceRepository.Add(platformMaintenance);
        }

        public void Update(int userId, int id, PlatformMaintenanceModel platformMaintenanceModel)
        {
            CheckUserRole(userId, _allowedRoles);

            var platformMaintenance = GetPlatformMaintenanceById(id);
            var newPlatformMaintenance = _mapper.Map<PlatformMaintenance>(platformMaintenanceModel);
            var platform = _platformRepository.GetById(platformMaintenanceModel.Platform.Id);
            ThrowIfEntityNotFound(platform, platformMaintenanceModel.Platform.Id);
            newPlatformMaintenance.Platform = platform;
            _platformMaintenanceRepository.Update(platformMaintenance, newPlatformMaintenance);
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
