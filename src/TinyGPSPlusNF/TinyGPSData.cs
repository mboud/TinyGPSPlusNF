namespace TinyGPSPlusNF
{
    using System;

    /// <summary>
    /// Common definition for GPS data extracted from NMEA sentences.
    /// </summary>
    public abstract class TinyGPSData
    {
        private static long MillisSinceSystemStart => Environment.TickCount64 / 10000;

        /// <summary>
        /// Indicates whether the current data's value is considered valid or not.
        /// </summary>
        protected bool _valid;

        /// <summary>
        /// Indicates whether the object's value has been updated since the last time it's been queried.
        /// </summary>
        protected bool _updated;

        /// <summary>
        /// Gets the number of milliseconds since the object's last update.
        /// </summary>
        protected long _lastCommitTime;

        /// <summary>
        /// Indicates whether the current data's value should be committed regardless of its validity.
        /// </summary>
        protected bool _forceCommit;

        /// <summary>
        /// Indicates whether the object contains any valid data and is safe to query.
        /// </summary>
        public bool IsValid => this._valid;

        /// <summary>
        /// Indicates whether the object's value has been updated (not necessarily changed) since the last time it's been queried.
        /// </summary>
        public bool IsUpdated => this._updated;

        /// <summary>
        /// Gets the number of milliseconds since the object's last update. A big value may be a sign of a problem like a lost fix.
        /// </summary>
        public long Age => this._valid ? MillisSinceSystemStart - this._lastCommitTime : long.MaxValue;

        internal void Commit()
        {
            this._updated = this._valid;

            if (this._valid || this._forceCommit)
            {
                this.OnCommit();
            }

            this._lastCommitTime = MillisSinceSystemStart;
        }

        internal abstract void OnCommit();

        internal abstract void Set(string term);
    }
}
