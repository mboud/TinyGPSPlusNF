namespace TinyGPSPlusNF
{
    /// <summary>
    /// Decimal degrees notation for latitude and longitude.
    /// </summary>
    public class TinyGPSDegrees : TinyGPSData
    {
        private ushort _newHoleDegrees;
        private uint _newBillionth;
        private double _newDegrees;
        private bool _newNegative;

        private bool _negative;

        /// <summary>
        /// Degrees hole part (absolute value).
        /// </summary>
        public ushort HoleDegrees { get; private set; }

        /// <summary>
        /// Degrees fractional part.
        /// </summary>
        public uint Billionths { get; private set; }

        /// <summary>
        /// Degrees value.
        /// </summary>
        public double Degrees { get; private set; }

        /// <summary>
        /// Indicates wheter the <c>Degrees</c> value is negative or not.
        /// </summary>
        public bool Negative
        {
            get
            {
                return this._negative;
            }

            private set
            {
                this._negative = value;

                if ((this._negative && this.Degrees > 0) ||
                    (!this._negative && this.Degrees < 0))
                {
                    this.Degrees *= -1;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyGPSDegrees"/> class.
        /// </summary>
        public TinyGPSDegrees()
        {
            this._valid = false;
            this._updated = false;
        }

        internal override void OnCommit()
        {
            this.HoleDegrees = this._newHoleDegrees;
            this.Billionths = this._newBillionth;
            this.Degrees = this._newDegrees;
            this.Negative = this._newNegative;
        }

        internal override void Set(string term)
        {
            string[] nmeaParts = term.Split('.');

            if (int.TryParse(nmeaParts[0], out int leftOfDecimal))
            {
                this._valid = true;
            }
            else
            {
                this._valid = false;
                return;
            }

            var minutes = leftOfDecimal % 100;
            uint multiplier = 10000000;
            var tenMillionthsOfMinutes = minutes * multiplier;

            this._newHoleDegrees = (ushort)(leftOfDecimal / 100);

            for (int i = 0; i < nmeaParts[1].Length; i++)
            {
                if (Utils.IsDigit(nmeaParts[1][i]))
                {
                    multiplier /= 10;
                    tenMillionthsOfMinutes += (nmeaParts[1][i] - '0') * multiplier;
                }
            }

            this._newBillionth = (uint)((5 * tenMillionthsOfMinutes + 1) / 3);
            this._newDegrees = this._newHoleDegrees + Utils.ToFixed(this._newBillionth / 1000000000.0, 6);
            this._newNegative = false;
        }

        internal void SetSign(bool negative)
        {
            this._newNegative = negative;
        }
    }
}
