namespace S4N.SuCorrientazoADomicilio.Dto
{
    /// <summary>
    /// Data transfer object for coordinates
    /// </summary>
    public class CoordinatesDto
    {
        /// <summary>
        /// The x coordinate value
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// The y coordinate vale
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// The cardinal direction
        /// </summary>
        public Orientation CardinalDirection { get; set; }
    }
}
