namespace TinyGPSPlusNF
{
    /// <summary>
    /// Decimal data.
    /// </summary>
    public class TinyGPSDecimal : TinyGPSData
    {
        private double _val;
        private double _newVal;

        /// <summary>
        /// Decimal value as <see cref="double"/>.
        /// </summary>
        public virtual double Value
        {
            get
            {
                this._updated = false;
                return this._val;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyGPSDecimal"/> class.
        /// </summary>
        public TinyGPSDecimal()
        {
            this._valid = false;
            this._updated = false;
            this._val = 0;
        }

        internal override void OnCommit()
        {
            this._val = this._newVal;
        }

        internal override void Set(string term)
        {
            if (TryParse.Double(term, out double d))
            {
                this._newVal = d;
                this._valid = true;
            }
            else
            {
                this._valid = false;
            }
        }
    }
}
