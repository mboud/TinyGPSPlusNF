namespace TinyGPSPlusNF
{
    /// <summary>
    /// Integer data.
    /// </summary>
    public class TinyGPSInteger : TinyGPSData
    {
        private int _val;
        private int _newVal;

        /// <summary>
        /// Integer value.
        /// </summary>
        public int Value
        {
            get
            {
                this._updated = false;
                return this._val;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyGPSInteger"/> class.
        /// </summary>
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
            if (int.TryParse(term, out int i))
            {
                this._newVal = i;
                this._valid = true;
            }
            else
            {
                this._valid = false;
            }
        }
    }
}
