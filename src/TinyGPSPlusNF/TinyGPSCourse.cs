namespace TinyGPSPlusNF
{
    /// <summary>
    /// Cardinal direction data.
    /// </summary>
    public class TinyGPSCourse : TinyGPSFloat
    {
        /// <summary>
        /// Raw course in degrees.
        /// </summary>
        public override float Value => base.Value;

        /// <summary>
        /// Course in degrees.
        /// </summary>
        public float Degrees => Utils.ToFixed(this.Value, 2);
    }
}
