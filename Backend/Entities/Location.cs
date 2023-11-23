﻿namespace WebApplication1.Models
{
    public class Location : EntityBase
    {
        public Coordinates Coordinates { get; set; }
    }

    public class Coordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}