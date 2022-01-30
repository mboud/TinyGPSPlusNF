namespace TinyGPSPlusNF
{
    /// <summary>
    /// Speed data.
    /// </summary>
    public class TinyGPSSpeed : TinyGPSFloat
    {
        /// <summary>
        /// Raw speed in knots.
        /// </summary>
        public override float Value => base.Value;

        /// <summary>
        /// Speed in knots.
        /// </summary>
        public float Knots => Utils.ToFixed(this.Value, 2);

        /// <summary>
        /// Speed in miles per hour.
        /// </summary>
        public float Mph => Utils.ToFixed(TinyGPSPlus._GPS_MPH_PER_KNOT * this.Value, 2);

        /// <summary>
        /// Speed in meters per second.
        /// </summary>
        public float Mps => Utils.ToFixed(TinyGPSPlus._GPS_MPS_PER_KNOT * this.Value, 2);

        /// <summary>
        /// Speed in kilometers per hour.
        /// </summary>
        public float Kmph => Utils.ToFixed(TinyGPSPlus._GPS_KMPH_PER_KNOT * this.Value, 2);
    }
}
