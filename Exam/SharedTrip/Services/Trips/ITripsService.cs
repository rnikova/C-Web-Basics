using SharedTrip.Models;
using SharedTrip.ViewModels.Trips;
using System.Collections.Generic;

namespace SharedTrip.Services.Trips
{
    public interface ITripsService
    {
        void Add(TripAddInputModel inputModel, string user);

        void AddUserToTrip(string tripId, string userId);

        IEnumerable<Trip> GetAll();

        Trip GetTripById(string id);
    }
}
