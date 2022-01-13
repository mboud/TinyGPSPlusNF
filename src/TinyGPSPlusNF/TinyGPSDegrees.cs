namespace TinyGPSPlusNF
{
    public class TinyGPSDegrees : TinyGPSData
    {
        private int _newDeg;
        private long _newBillionth;
        private double _newDegrees;
        private bool _newNegative;

        private bool _negative;

        /// <summary>
        /// Degrees hole part (absolute value).
        /// </summary>
        public int Deg { get; private set; }

        /// <summary>
        /// Degrees fractional part.
        /// </summary>
        public long Billionths { get; private set; }

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

        public TinyGPSDegrees()
        {
            this._valid = false;
            this._updated = false;
        }

        internal override void OnCommit()
        {
            this.Deg = this._newDeg;
            this.Billionths = this._newBillionth;
            this.Degrees = this._newDegrees;
            this.Negative = this._newNegative;
        }

        internal override void Set(string term)
        {
            string[] nmeaParts = term.Split('.');

            if (TryParse.Int32(nmeaParts[0], out int leftOfDecimal))
            {
                this._isOkToCommit = true;
            }
            else
            {
                this._isOkToCommit = false;
                return;
            }

            var minutes = leftOfDecimal % 100;
            uint multiplier = 10000000;
            var tenMillionthsOfMinutes = minutes * multiplier;

            this._newDeg = leftOfDecimal / 100;

            for (int i = 0; i < nmeaParts[1].Length; i++)
            {
                if (Utils.IsDigit(nmeaParts[1][i]))
                {
                    multiplier /= 10;
                    tenMillionthsOfMinutes += (nmeaParts[1][i] - '0') * multiplier;
                }
            }

            this._newBillionth = (5 * tenMillionthsOfMinutes + 1) / 3;
            this._newDegrees = this._newDeg + Utils.ToFixed(this._newBillionth / 1000000000.0, 6);
            this._newNegative = false;
        }

        internal void SetSign(bool negative)
        {
            this._newNegative = negative;
        }
    }
}
