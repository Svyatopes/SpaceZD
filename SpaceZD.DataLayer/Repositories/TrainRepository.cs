using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class TrainRepository
    {
        private readonly VeryVeryImportantContext _context;        

        public TrainRepository() => _context = VeryVeryImportantContext.GetInstance();
       
        public List<Train> GetTrains(bool includeAll = false) => _context.Trains.Where(c => !c.IsDeleted || includeAll).ToList();
           
        public Train GetTrainById(int id) => _context.Trains.FirstOrDefault(t => t.Id == id);
            

        public void AddTrain(Train train)
        {
            _context.Trains.Add(train);
            _context.SaveChanges();

        }

        public bool UpdateTrain(Train train)
        {
            var trainInDb = GetTrainById(train.Id);

            if (trainInDb == null)
                return false;

            if (trainInDb.Carriages != null && trainInDb.Carriages != train.Carriages)
                trainInDb.Carriages = train.Carriages;

            _context.SaveChanges();
            return false;
        }

        public bool UpdateTrain(int id, bool isDeleted)
        {
            var train = GetTrainById(id);
            if (train is null)
                return false;

            train.IsDeleted = isDeleted;
            _context.SaveChanges();

            return true;

        }

    }
}





