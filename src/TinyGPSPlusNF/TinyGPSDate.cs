namespace TinyGPSPlusNF
{
    /// <summary>
    /// Date data.
    /// </summary>
    public class TinyGPSDate : TinyGPSData
    {
        private int _date;
        private int _newDate;

        /// <summary>
        /// Raw date in DDMMYY format.
        /// </summary>
        public int Value
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
        public int Year
        {
            get
            {
                this._updated = false;
                return 2000 + (this._date % 100);
            }
        }

        /// <summary>
        /// Month (1-12).
        /// </summary>
        public int Month
        {
            get
            {
                this._updated = false;
                return (this._date / 100) % 100;
            }
        }

        /// <summary>
        /// Day (1-31).
        /// </summary>
        public int Day
        {
            get
            {
                this._updated = false;
                return this._date / 10000;
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
            if (int.TryParse(term, out int i))
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
