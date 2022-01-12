namespace TinyGPSPlusNF
{
    public class TinyGPSDecimal : TinyGPSData
    {
        private double _val;
        private double _newVal;

        public virtual double Value
        {
            get
            {
                this._updated = false;
                return this._val;
            }
        }

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
                this._isOkToCommit = true;
            }
            else
            {
                this._isOkToCommit = false;
            }
        }
    }
}
