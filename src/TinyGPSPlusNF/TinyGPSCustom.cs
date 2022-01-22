using System;

namespace TinyGPSPlusNF
{
    /// <summary>
    /// Represents an object allowing custom NMEA sentence extraction of a value.
    /// </summary>
    public class TinyGPSCustom : TinyGPSData
    {
        private string _newVal;
        private string _val;

        private bool _isNumeric;
        private TinyGPSDecimal _decimal;

        /// <summary>
        /// Sentence identifier.
        /// </summary>
        public string SentenceName { get; private set; }

        /// <summary>
        /// Index of the term in the sentence.
        /// </summary>
        public int TermNumber { get; private set; }

        /// <summary>
        /// Extracted value as <see cref="string"/>.
        /// </summary>
        public string Value
        {
            get
            {
                this._updated = false;
                return this._val;
            }
        }

        /// <summary>
        /// Extracted value as <see cref="TinyGPSDecimal"/>.
        /// </summary>
        public TinyGPSDecimal NumericValue
        {
            get
            {
                if (!this._isNumeric)
                {
                    throw new InvalidOperationException();
                }

                return this._decimal;
            }
        }

        internal TinyGPSCustom Next { get; set; }

        private TinyGPSCustom()
        {
            this._lastCommitTime = 0;
            this._updated = false;
            this._valid = false;

            this._newVal = null;
            this._val = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyGPSCustom"/> class.
        /// </summary>
        /// <param name="gps"><see cref="TinyGPSPlus"/> instance.</param>
        /// <param name="sentenceName">Sentence identifier.</param>
        /// <param name="termNumber">Index of the term in the sentence.</param>
        /// <param name="isNumeric">Value will be available as a <see cref="TinyGPSDecimal"/> if <c>true</c>.</param>
        public TinyGPSCustom(TinyGPSPlus gps, string sentenceName, int termNumber, bool isNumeric = false) : this()
        {
            this.Begin(gps, sentenceName, termNumber, isNumeric);
        }

        private void Begin(TinyGPSPlus gps, string sentenceName, int termNumber, bool isNumeric)
        {
            this.SentenceName = sentenceName;
            this.TermNumber = termNumber;

            this._isNumeric = isNumeric;

            if (this._isNumeric)
            {
                this._decimal = new TinyGPSDecimal();
            }

            // Insert this item into the GPS tree
            gps.InsertCustom(this);
        }

        internal override void OnCommit()
        {
            this._val = this._newVal;

            if (this._isNumeric)
            {
                this._decimal.Commit();
            }
        }

        internal override void Set(string term)
        {
            this._newVal = term;
            this._valid = term != null;

            if (this._isNumeric)
            {
                this._decimal.Set(term);
            }
        }
    }
}
