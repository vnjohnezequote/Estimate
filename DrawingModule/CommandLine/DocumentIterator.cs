using System;
using System.Collections;

namespace DrawingModule.CommandLine
{
    public class DocumentIterator: IEnumerator
    {
        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public object Current { get; }
    }
}
