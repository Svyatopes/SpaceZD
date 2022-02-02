using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class TrainRepository
    {

        public List<Train> GetTrains()
        {
            var context = VeryVeryImportantContext.GetInstance();
            var trains = context.Trains.ToList();
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

        public void DeleteTrain(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var train = context.Trains.FirstOrDefault(t => t.Id == id);
            train.IsDeleted = true;
            context.SaveChanges();
        }

        public void EditTrain(Train train)
        {
            var context = VeryVeryImportantContext.GetInstance();

            var trainInDb = GetTrainById(train.Id);

            if (trainInDb == null)
                throw new Exception($"Not found ticket with {train.Id} to edit");

            if (trainInDb.Carriages != null && trainInDb.Carriages != train.Carriages)
                trainInDb.Carriages = train.Carriages;            

            context.SaveChanges();
        }

    }
}





