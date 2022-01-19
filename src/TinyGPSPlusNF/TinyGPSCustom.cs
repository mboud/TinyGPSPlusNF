using System;

namespace TinyGPSPlusNF
{
    /// <summary>
    /// Represents an object allowing custom NMEA sentence extraction.
    /// </summary>
    public class TinyGPSCustom : TinyGPSData
    {
        private string _newVal;
        private string _val;

        private bool _isNumeric;
        private TinyGPSDecimal _decimal;

        public string SentenceName { get; private set; }

        public int TermNumber { get; private set; }

        public TinyGPSCustom Next { get; set; }

        public string Value
        {
            get
            {
                this._updated = false;
                return this._val;
            }
        }

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

        private TinyGPSCustom()
        {
            this._lastCommitTime = 0;
            this._updated = false;
            this._valid = false;

            this._newVal = null;
            this._val = null;
        }

        public TinyGPSCustom(TinyGPSPlus gps, string sentenceName, int termNumber, bool isNumeric = false) : this()
        {
            this.Begin(gps, sentenceName, termNumber, isNumeric);
        }

        public void Begin(TinyGPSPlus gps, string sentenceName, int termNumber, bool isNumeric)
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
