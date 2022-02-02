using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class TrainRepository
    {

        public List<Train> GetTrains()
        {
            var includeAll = false;
            var context = VeryVeryImportantContext.GetInstance();
            var trains = context.Trains.Where(c => !c.IsDeleted || includeAll).ToList();
            return trains;           
           
        }

        public Train GetTrainById(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var train = context.Trains.FirstOrDefault(t => t.Id == id);
            return train;
        }

        public void AddTrain(Train train)
        {
            var context = VeryVeryImportantContext.GetInstance();
            context.Trains.Add(train);
            context.SaveChanges();

        }

        public bool UpdateTrain(Train train)
        {
            var context = VeryVeryImportantContext.GetInstance();

            var trainInDb = GetTrainById(train.Id);

            if (trainInDb == null)
                return false;

            if (trainInDb.Carriages != null && trainInDb.Carriages != train.Carriages)
                trainInDb.Carriages = train.Carriages;

            context.SaveChanges();
            return false;
        }

        public bool UpdateTrain(int id, bool isDeleted)
        {
            var context = VeryVeryImportantContext.GetInstance();

            var train = GetTrainById(id);
            if (train is null)
                return false;

            train.IsDeleted = isDeleted;

            context.SaveChanges();

            return true;

        }

    }
}





