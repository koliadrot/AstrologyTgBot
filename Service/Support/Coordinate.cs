namespace Service.Support
{
    /// <summary>
    /// Координаты
    /// </summary>
    public class Coordinate
    {
        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude { get; private set; }

        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude { get; private set; }

        public Coordinate(string latitude, string longitude)
        {
            Latitude = double.TryParse(latitude, out double parsedLatitude) ? parsedLatitude : 0;
            Longitude = double.TryParse(longitude, out double parsedLongitude) ? parsedLongitude : 0;
        }
    }
}
