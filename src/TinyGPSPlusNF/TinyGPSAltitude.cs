namespace TinyGPSPlusNF
{
    /// <summary>
    /// Represents altitude info in meters, kilometers, miles or feet.
    /// </summary>
    public class TinyGPSAltitude : TinyGPSFloat
    {
        /// <summary>
        /// Raw altitude in meters.
        /// </summary>
        public override float Value => base.Value;

        /// <summary>
        /// Altitude in meters.
        /// </summary>
        public float Meters => Utils.ToFixed(this.Value, 2);

        /// <summary>
        /// Altitude in miles.
        /// </summary>
        public float Miles => Utils.ToFixed(TinyGPSPlus._GPS_MILES_PER_METER * this.Value, 2);

        /// <summary>
        /// Altitude in kilometers.
        /// </summary>
        public float Kilometers => Utils.ToFixed(TinyGPSPlus._GPS_KM_PER_METER * this.Value, 2);

        /// <summary>
        /// Altitude in feet.
        /// </summary>
        public float Feet => Utils.ToFixed(TinyGPSPlus._GPS_FEET_PER_METER * this.Value, 2);
    }
}
