namespace TinyGPSPlusNF
{
    public class TinyGPSCourse : TinyGPSDecimal
    {
        /// <summary>
        /// Raw course in degrees.
        /// </summary>
        public override double Value => base.Value;

        /// <summary>
        /// Course in degrees.
        /// </summary>
        public double Degrees => Utils.ToFixed(this.Value, 2);
    }
}
