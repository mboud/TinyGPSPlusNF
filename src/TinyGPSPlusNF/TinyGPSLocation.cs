namespace TinyGPSPlusNF
{
    using System;

    /// <summary>
    /// Pair of latitude and longitude coordinates.
    /// </summary>
    public class TinyGPSLocation : TinyGPSData
    {
        /// <summary>
        /// The latitude.
        /// </summary>
        public readonly TinyGPSDegrees Latitude;

        /// <summary>
        /// The longitude.
        /// </summary>
        public readonly TinyGPSDegrees Longitude;

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyGPSLocation"/> class.
        /// </summary>
        public TinyGPSLocation()
        {
            this._valid = false;
            this._updated = false;
            this._forceCommit = true;

            this.Latitude = new();
            this.Longitude = new();
        }

        internal override void OnCommit()
        {
            this.Latitude.Commit();
            this.Longitude.Commit();

            this._valid = this.Latitude.IsValid && this.Longitude.IsValid;
        }

        internal override void Set(string term)
        {
            throw new InvalidOperationException($"Please set location values using members {nameof(this.Latitude)} and {nameof(this.Longitude)}");
        }
    }
}
