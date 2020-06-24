namespace DrawingModule.CommandLine
{
    public struct TextSelectionRange
    {
        // Token: 0x06000597 RID: 1431 RVA: 0x00016D4D File Offset: 0x00015D4D
        public TextSelectionRange(int start, int end)
        {
            this.mStart = start;
            this.mEnd = end;
        }

        // Token: 0x1700014C RID: 332
        // (get) Token: 0x06000598 RID: 1432 RVA: 0x00016D5D File Offset: 0x00015D5D
        public int Start
        {
            get
            {
                return this.mStart;
            }
        }

        // Token: 0x1700014D RID: 333
        // (get) Token: 0x06000599 RID: 1433 RVA: 0x00016D65 File Offset: 0x00015D65
        public int End
        {
            get
            {
                return this.mEnd;
            }
        }

        // Token: 0x0600059A RID: 1434 RVA: 0x00016D6D File Offset: 0x00015D6D
        public override string ToString()
        {
            return this.Start + ", " + this.End;
        }

        // Token: 0x040001F4 RID: 500
        private int mStart;

        // Token: 0x040001F5 RID: 501
        private int mEnd;
    }
}