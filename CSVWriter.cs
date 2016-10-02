using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSV
{
    /// <summary>
    /// A StreamWriter that can write an IEnumerable<String> encoded as CSV (Comma Separated Values)
    /// </summary>
    public class CSVWriter : StreamWriter
    {
        public CSVWriter(Stream stream) : base(stream)
        {}

        public CSVWriter(string path) : base(path)
        {}

        public CSVWriter(string path, bool append) : base(path, append)
        {}


        /// <summary>
        /// Writes a row of values to the stream
        /// </summary>
        public void WriteRow(IEnumerable<string> fields)
        {
            var  sb         = new StringBuilder();
            bool firstField = true;

            foreach (var field in fields)
            {
                if (!firstField) { sb.Append(','); } //Add comma separator if this isn't the first field in the row

                //If the field contains a quote, comma, or new line...(Old MAC software uses \r)
                if (field.IndexOfAny(new char[] { '"', ',', '\r', '\n' }) != -1)
                {
                    sb.AppendFormat("\"{0}\"", field.Replace("\"", "\"\"")); //Wrap in quotes and double up any quotes in the field value
                }
                else
                {
                    sb.Append(field);          //Otherwise just write the field as is
                }
                firstField = false;
            }
            WriteLine(sb.ToString());
        }
    }
}