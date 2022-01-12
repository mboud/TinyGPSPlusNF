namespace TinyGPSPlusNF
{
    using System;

    /// <summary>
    /// Represents an object allowing custom NMEA sentence extraction.
    /// </summary>
    public class TinyGPSCustom : TinyGPSData
    {
        private readonly char[] _stagingBuffer;
        private readonly char[] _buffer;

        public string SentenceName { get; private set; }

        public int TermNumber { get; private set; }

        public TinyGPSCustom Next { get; set; }

        public char[] Value
        {
            get
            {
                this._updated = false;
                return this._buffer;
            }
        }

        public TinyGPSCustom()
        {
            this._stagingBuffer = new char[TinyGPSPlus._GPS_MAX_FIELD_SIZE + 1];
            this._buffer = new char[TinyGPSPlus._GPS_MAX_FIELD_SIZE + 1];
        }

        public TinyGPSCustom(TinyGPSPlus gps, string sentenceName, int termNumber) : this()
        {
            this.Begin(gps, sentenceName, termNumber);
        }

        public void Begin(TinyGPSPlus gps, string sentenceName, int termNumber)
        {
            this._lastCommitTime = 0;
            this._updated = false;
            this._valid = false;
            this.SentenceName = sentenceName;
            this.TermNumber = termNumber;

            for (int i = 0; i < TinyGPSPlus._GPS_MAX_FIELD_SIZE + 1; i++)
            {
                this._stagingBuffer[i] = '\0';
                this._buffer[i] = '\0';
            }

            // Insert this item into the GPS tree
            gps.InsertCustom(this);
        }

        internal override void OnCommit()
        {
            for (int i = 0; i < TinyGPSPlus._GPS_MAX_FIELD_SIZE + 1; i++)
            {
                this._buffer[i] = this._stagingBuffer[i];
            }
        }

        internal override void Set(string term)
        {
            int length = Math.Min(this._stagingBuffer.Length, term.Length);

            for (int i = 0; i < length; i++)
            {
                this._stagingBuffer[i] = term[i];
            }

            this._isOkToCommit = true;
        }
    }
}
