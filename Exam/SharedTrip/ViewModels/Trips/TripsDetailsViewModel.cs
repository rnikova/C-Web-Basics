﻿using System;

namespace SharedTrip.ViewModels.Trips
{
    public class TripsDetailsViewModel
    {
        public string Id { get; set; }

        public string StartPoint { get; set; }

        public string EndPoint { get; set; }

        public DateTime DepartureTime { get; set; }

        public int Seats { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }
    }
}
