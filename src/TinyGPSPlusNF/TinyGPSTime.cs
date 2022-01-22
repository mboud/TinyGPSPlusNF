namespace TinyGPSPlusNF
{
    /// <summary>
    /// Time data.
    /// </summary>
    public class TinyGPSTime : TinyGPSData
    {
        private uint _time;
        private uint _newTime;

        /// <summary>
        /// Raw time in HHMMSSCC format.
        /// </summary>
        public uint Value
        {
            get
            {
                this._updated = false;
                return this._time;
            }
        }

        /// <summary>
        /// Hour (0-23).
        /// </summary>
        public byte Hour
        {
            get
            {
                this._updated = false;
                return (byte)(this._time / 1000000);
            }
        }

        /// <summary>
        /// Minute (0-59).
        /// </summary>
        public byte Minute
        {
            get
            {
                this._updated = false;
                return (byte)((this._time / 10000) % 100);
            }
        }

        /// <summary>
        /// Second (0-59).
        /// </summary>
        public byte Second
        {
            get
            {
                this._updated = false;
                return (byte)((this._time / 100) % 100);
            }
        }

        /// <summary>
        /// 100ths of a second (0-99).
        /// </summary>
        public byte Centisecond
        {
            get
            {
                this._updated = false;
                return (byte)(this._time % 100);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyGPSTime"/> class.
        /// </summary>
        public TinyGPSTime()
        {
            this._valid = false;
            this._updated = false;
            this._time = 0;
        }

        internal override void OnCommit()
        {
            this._time = this._newTime;
        }

        internal override void Set(string term)
        {
            if (double.TryParse(term, out double d))
            {
                this._newTime = (uint)(100 * d);
                this._valid = true;
            }
            else
            {
                this._valid = false;
            }
        }
    }
}
