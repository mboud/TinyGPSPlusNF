namespace TinyGPSPlusNF
{
    using System;

    public class TinyGPSLocation : TinyGPSData
    {
        public readonly TinyGPSDegrees Latitude;
        public readonly TinyGPSDegrees Longitude;

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
