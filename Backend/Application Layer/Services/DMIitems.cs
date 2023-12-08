namespace WebApplication1.Services
{
    public class Geometry
    {
        public List<double> coordinates { get; set; }
        public string type { get; set; }
    }

    public class Properties
    {
        public string country { get; set; }
        public string name { get; set; }
        public string owner { get; set; }
        public List<string> parameterId { get; set; }
        public string stationId { get; set; }
        public string status { get; set; }
        public DateTime timeCreated { get; set; }
        public DateTime timeOperationFrom { get; set; }
        public string timeOperationTo { get; set; }
        public string timeUpdated { get; set; }
        public DateTime timeValidFrom { get; set; }
        public string timeValidTo { get; set; }
        public string type { get; set; }
    }

    public class Feature
    {
        public Geometry geometry { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public Properties properties { get; set; }
    }

    public class Links
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string type { get; set; }
        public string title { get; set; }
    }

    public class Root
    {
        public string type { get; set; }
        public List<Feature> features { get; set; }
        public DateTime timeStamp { get; set; }
        public int numberReturned { get; set; }
        public List<Links> links { get; set; }
    }
}
