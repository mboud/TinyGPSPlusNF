namespace TinyGPSPlusNF
{
    using System;

    public abstract class TinyGPSData
    {
        protected bool _isOkToCommit;
        protected bool _valid;
        protected bool _updated;
        protected long _lastCommitTime;

        /// <summary>
        /// Indicates whether the object contains any valid data and is safe to query.
        /// </summary>
        public bool IsValid => this._valid;

        /// <summary>
        /// Indicates whether the object’s value has been updated (not necessarily changed) since the last time it's been queried.
        /// </summary>
        public bool IsUpdated => this._updated;

        /// <summary>
        /// Gets the number of ticks since the object's last update. A big value may be a sign of a problem like a lost fix.
        /// </summary>
        public long Age => this._valid ? DateTime.UtcNow.Ticks - this._lastCommitTime : long.MaxValue;

        internal void Commit()
        {
            this._valid = this._isOkToCommit;
            this._updated = this._isOkToCommit;

            if (this._isOkToCommit)
            {
                this.OnCommit();
            }

            this._lastCommitTime = DateTime.UtcNow.Ticks;
        }

        internal abstract void OnCommit();

        internal abstract void Set(string term);
    }
}
