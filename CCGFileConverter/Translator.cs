using System;
using System.Collections.Generic;
using System.Text;

namespace CCGFileConverter
{
    public abstract class Translator
    {
        public abstract string supportedFileType { get; }
        public abstract string resultingFileType { get; }
        /*returns a result with the final resultant file's location. 
         * if conversion fails result is ""*/
        public abstract string translateSupportedFile(string incomingFileLocation);
    }
}
