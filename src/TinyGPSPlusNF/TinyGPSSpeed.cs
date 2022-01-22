namespace TinyGPSPlusNF
{
    /// <summary>
    /// Speed data.
    /// </summary>
    public class TinyGPSSpeed : TinyGPSDecimal
    {
        /// <summary>
        /// Raw speed in knots.
        /// </summary>
        public override double Value => base.Value;

        /// <summary>
        /// Speed in knots.
        /// </summary>
        public double Knots => Utils.ToFixed(this.Value, 2);

        /// <summary>
        /// Speed in miles per hour.
        /// </summary>
        public double Mph => Utils.ToFixed(TinyGPSPlus._GPS_MPH_PER_KNOT * this.Value, 2);

        /// <summary>
        /// Speed in meters per second.
        /// </summary>
        public double Mps => Utils.ToFixed(TinyGPSPlus._GPS_MPS_PER_KNOT * this.Value, 2);

        /// <summary>
        /// Speed in kilometers per hour.
        /// </summary>
        public double Kmph => Utils.ToFixed(TinyGPSPlus._GPS_KMPH_PER_KNOT * this.Value, 2);
    }
}
