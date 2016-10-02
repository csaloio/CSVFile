using System.Collections.Generic;
using System.IO;

namespace CSV
{
    /// <summary>
    /// A StreamReader that can read CSV (Comma Separated Values)
    /// </summary>
    public class CSVReader : StreamReader
    {
        public CSVReader(Stream stream) : base(stream)
        {}

        public CSVReader(string path) : base(path)
        {}

        /// <summary>
        /// Read a CSV row from the stream returning the values as an IEnumerable<string>. Returns null if no values found.
        /// </summary>
        public IEnumerable<string> ReadRow()
        {
            int    start;
            string field;
            string line = ReadLine();                               //Read line
            if (string.IsNullOrWhiteSpace(line)) { return null; }   //Return null if line is empty

            var row = new List<string>();
            int pos = 0;

            while(pos < line.Length)
            {
                if (line[pos] == '"') //Parse quoted field
                {
                    pos++;            //Skip starting quote
                    start = pos;
                    while (pos < line.Length)
                    {
                        if(line[pos] == '"')  //Found a quote
                        {
                            int next = pos+1;                   //Index of next char
                            if (next >= line.Length) { break; } //Quote was last char of line
                            if (line[next] != '"')   { break; } //If next char is not an escaped quote we are at the end of the field
                            else                     { pos++; } //If next char would be the escaped quote, skip over it
                        }
                        pos++;
                    }
                    field = line.Substring(start, pos - start);
                    field = field.Replace("\"\"", "\"");         //Replace doubled quotes with a single one 
                }
                else                  //Parse unquoted field
                {
                    start = pos;
                    while (pos < line.Length && line[pos] != ',') { pos++; }
                    field = line.Substring(start, pos - start);
                }
                row.Add(field);

                //Skip forward until we pass a field ending comma or end of line
                while (pos < line.Length && line[pos] != ',') { pos++; }
                pos++; //Pass by that comma
            }
            
            return row;
        }
    }
}