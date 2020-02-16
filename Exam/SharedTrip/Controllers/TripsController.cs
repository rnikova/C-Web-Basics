using SharedTrip.Services.Trips;
using SharedTrip.Services.Users;
using SharedTrip.ViewModels.Trips;
using SIS.HTTP;
using SIS.MvcFramework;

namespace SharedTrip.Controllers
{
    public class TripsController : Controller
    {
        private readonly ITripsService tripsService;

        public TripsController(ITripsService tripsService  )
        {
            this.tripsService = tripsService;
        }

        public HttpResponse Add()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Register");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(TripAddInputModel inputModel)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Register");
            }

            if (inputModel.Seats < 2 || inputModel.Seats > 6)
            {
                return this.Redirect("/Trips/Add");
            }

            if (string.IsNullOrEmpty(inputModel.StartPoint) 
                || string.IsNullOrEmpty(inputModel.EndPoint)
                || string.IsNullOrEmpty(inputModel.Description)
                || inputModel.Description.Length > 80)
            {
                return this.Redirect("/Trips/Add");
            }

            var userId = this.User;
            this.tripsService.Add(inputModel, userId);

            return this.Redirect("/Trips/All");
        }


        public HttpResponse Details(TripsDetailsViewModel model)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Register");
            }

            var trip = this.tripsService.GetTripById(model.Id);

            return this.View(trip);
        }

        [HttpPost]
        public HttpResponse AddUserToTrip(string tripId)
        {
            var userId = this.User;

            this.tripsService.AddUserToTrip(tripId, userId);

            return this.Redirect("/Trips/All");
        }

        public HttpResponse All()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Register");
            }

            var allTrips = this.tripsService.GetAll();

            return this.View(allTrips, "All");
        }
    }
}
