using SharedTrip.Models;
using SharedTrip.Services.Users;
using SharedTrip.ViewModels.Trips;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedTrip.Services.Trips
{
    public class TripsService : ITripsService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IUsersService usersService;

        public TripsService(ApplicationDbContext dbContext, IUsersService usersService)
        {
            this.dbContext = dbContext;
            this.usersService = usersService;
        }

        public void Add(TripAddInputModel inputModel, string user)
        {
            var trip = new Trip
            {
                Id = Guid.NewGuid().ToString(),
                StartPoint = inputModel.StartPoint,
                EndPoint = inputModel.EndPoint,
                DepartureTime = inputModel.DepartureTime,
                ImagePath = inputModel.ImagePath,
                Seats = inputModel.Seats,
                Description = inputModel.Description,
            };

            trip.UserTrips.Add(new UserTrip
            {
                UserId = user
            });
            this.dbContext.Trips.Add(trip);
            this.dbContext.SaveChanges();
        }

        public void AddUserToTrip(string tripId, string userId)
        {
            var currentTrip = this.dbContext.Trips.Where(x => x.Id == tripId).FirstOrDefault();

            currentTrip.UserTrips.Add(new UserTrip { UserId = userId });

            this.dbContext.SaveChanges();

        }

        public IEnumerable<Trip> GetAll()
        {
            return this.dbContext.Trips.Select(x => new Trip
            {
                Id = x.Id,
                StartPoint = x.StartPoint,
                EndPoint = x.EndPoint,
                DepartureTime = x.DepartureTime,
                Seats = x.Seats
            }).ToArray();
        }

        public Trip GetTripById(string id)
        {
            return this.dbContext.Trips.FirstOrDefault(x => x.Id == id);
        }


    }
}
