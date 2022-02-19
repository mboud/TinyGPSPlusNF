namespace TinyGPSPlusNF
{
    /// <summary>
    /// Float data.
    /// </summary>
    public class TinyGPSFloat : TinyGPSData
    {
        private float _val;
        private float _newVal;

        /// <summary>
        /// Float value.
        /// </summary>
        public virtual float Value
        {
            get
            {
                this._updated = false;
                return this._val;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyGPSFloat"/> class.
        /// </summary>
        public TinyGPSFloat()
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
            if (float.TryParse(term, out float f))
            {
                this._newVal = f;
                this._valid = true;
            }
            else
            {
                this._newVal = default;
                this._valid = false;
            }
        }
    }
}
