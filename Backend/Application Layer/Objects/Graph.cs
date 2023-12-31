﻿namespace WebApplication1.Application_Layer.Objects
{
public class TimeAndValue<Type>
    {
        public DateTime dateTime { get; set; }
        public Type value { get; set; }
    }

    public class CoordinatesAndValue<Type>
    {
        public Coordinates coordinates { get; set; }
        public Type Value { get; set; }

        public class Coordinates
        {
            public double X { get; set; }
            public double Y { get; set; }
        }
    }
}
