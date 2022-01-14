namespace TinyGPSPlusNF
{
    public class TinyGPSInteger : TinyGPSData
    {
        private int _val;
        private int _newVal;

        /// <summary>
        /// Number of satellites in use.
        /// </summary>
        public int Value
        {
            get
            {
                this._updated = false;
                return this._val;
            }
        }

        public TinyGPSInteger()
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
            if (TryParse.Int32(term, out int i))
            {
                this._newVal = i;
                this._isOkToCommit = true;
            }
            else
            {
                this._isOkToCommit = false;
            }
        }
    }
}
