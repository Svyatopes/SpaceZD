using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services
{
    public class PlatformMaintenanceService : IPlatformMaintenanceServices
    {
        private readonly IMapper _mapper;
        private readonly IRepositorySoftDelete<PlatformMaintenance> _platformMaintenanceRepository;

        public PlatformMaintenanceService(IMapper mapper, IRepositorySoftDelete<PlatformMaintenance> platformMaintenanceRepository)
        {
            _mapper = mapper;
            _platformMaintenanceRepository = platformMaintenanceRepository;
        }

        public PlatformMaintenanceModel GetById(int id)
        {
            var platformMaintenance = GetPlatformMaintenanceById(id);
            return _mapper.Map<PlatformMaintenanceModel>(platformMaintenance);
        }

        public List<PlatformMaintenanceModel> GetList(bool allIncluded)
        {
            var platformsMaintenance = _platformMaintenanceRepository.GetList(allIncluded);
            return _mapper.Map<List<PlatformMaintenanceModel>>(platformsMaintenance);
        }

        public int Add(PlatformMaintenanceModel platformMaintenance)
        {
            var platformMaintenanceEntity = _mapper.Map<PlatformMaintenance>(platformMaintenance);
            var id = _platformMaintenanceRepository.Add(platformMaintenanceEntity);
            return id;
        }

        public void Update(int id, PlatformMaintenanceModel platformMaintenance)
        {
            var platformMaintenanceEntity = GetPlatformMaintenanceById(id);
            var newPlatformMaintenanceEntity = _mapper.Map<PlatformMaintenance>(platformMaintenance);
            _platformMaintenanceRepository.Update(platformMaintenanceEntity, newPlatformMaintenanceEntity);
        }

        public void Restore(int id)
        {
            var platformMaintenance = GetPlatformMaintenanceById(id);
            _platformMaintenanceRepository.Update(platformMaintenance, false);
        }

        public void Delete(int id)
        {
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
