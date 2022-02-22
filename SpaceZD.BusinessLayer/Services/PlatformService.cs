using AutoMapper;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services
{
    public class PlatformService : BaseService, IPlatformService
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IStationRepository _stationRepository;

        public PlatformService(IMapper mapper, IPlatformRepository platformRepository,
            IUserRepository userRepository,
            IStationRepository stationRepository) : base(mapper, userRepository)
        {
            _platformRepository = platformRepository;
            _stationRepository = stationRepository;
        }

        public List<PlatformModel> GetListByStationId(int userId, int stationId)
        {
            var user = CheckUserRole(userId, Role.Admin, Role.StationManager);

            List<Platform> platforms = new List<Platform>();

            if (user!.Role == Role.StationManager)
                platforms = _platformRepository.GetList(stationId);
            if (user.Role == Role.Admin)
                platforms = _platformRepository.GetList(stationId, true);

            var platformsList = _mapper.Map<List<PlatformModel>>(platforms);
            return platformsList;
        }

        public PlatformModel GetById(int userId, int platformId)
        {
            CheckUserRole(userId, Role.Admin, Role.StationManager);

            var platform = _platformRepository.GetById(platformId);
            ThrowIfEntityNotFound(platform, platformId);

            var platformModel = _mapper.Map<PlatformModel>(platform);
            return platformModel;
        }

        public int Add(int userId, PlatformModel platformModel)
        {
            CheckUserRole(userId, Role.Admin, Role.StationManager);

            var station = _stationRepository.GetById(platformModel.Station.Id);
            ThrowIfEntityNotFound(station, platformModel.Station.Id);

            var platform = _mapper.Map<Platform>(platformModel);
            platform.Station = station!;

            var platformId = _platformRepository.Add(platform);
            return platformId;
        }

        public void Edit(int userId, PlatformModel platformModel)
        {
            CheckUserRole(userId, Role.Admin, Role.StationManager);

            var platform = _platformRepository.GetById(platformModel.Id);
            ThrowIfEntityNotFound(platform, platformModel.Id);

            var newPlatform = _mapper.Map<Platform>(platformModel);

            _platformRepository.Update(platform!, newPlatform);
        }

        public void Delete(int userId, int platformId)
        {
            CheckUserRole(userId, Role.Admin, Role.StationManager);

            var platform = _platformRepository.GetById(platformId);
            ThrowIfEntityNotFound(platform, platformId);

            _platformRepository.Update(platform!, true);
        }

        public void Restore(int userId, int platformId)
        {
            CheckUserRole(userId, Role.Admin);

            var platform = _platformRepository.GetById(platformId);
            ThrowIfEntityNotFound(platform, platformId);

            _platformRepository.Update(platform!, false);
        }
    }
}
