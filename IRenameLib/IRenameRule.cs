using System;
using System.Collections.Generic;

namespace IRenameLib
{
    public interface IRenameRule
    {
        string Rename(string original);
        virtual string Rename(string original, string oldFileName) { return null; }
    }
}
