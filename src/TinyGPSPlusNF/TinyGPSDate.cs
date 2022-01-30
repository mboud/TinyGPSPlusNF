namespace TinyGPSPlusNF
{
    /// <summary>
    /// Date data.
    /// </summary>
    public class TinyGPSDate : TinyGPSData
    {
        private uint _date;
        private uint _newDate;

        /// <summary>
        /// Raw date in DDMMYY format.
        /// </summary>
        public uint Value
        {
            get
            {
                this._updated = false;
                return this._date;
            }
        }

        /// <summary>
        /// Year (2000+).
        /// </summary>
        public ushort Year
        {
            get
            {
                this._updated = false;
                return (ushort)(2000 + (this._date % 100));
            }
        }

        /// <summary>
        /// Month (1-12).
        /// </summary>
        public byte Month
        {
            get
            {
                this._updated = false;
                return (byte)((this._date / 100) % 100);
            }
        }

        /// <summary>
        /// Day (1-31).
        /// </summary>
        public byte Day
        {
            get
            {
                this._updated = false;
                return (byte)(this._date / 10000);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyGPSDate"/> class.
        /// </summary>
        public TinyGPSDate()
        {
            this._valid = false;
            this._updated = false;
            this._date = 0;
        }

        internal override void OnCommit()
        {
            this._date = this._newDate;
        }

        internal override void Set(string term)
        {
            if (uint.TryParse(term, out uint i))
            {
                this._newDate = i;
                this._valid = true;
            }
            else
            {
                this._valid = false;
            }
        }
    }
}
