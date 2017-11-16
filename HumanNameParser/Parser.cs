using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HumanNameParser
{
    public class Parser
    {

        private static readonly string[] _suffixes = { "esq", "esquire", "jr", "sr", "2", "ii", "iii", "iv" , "v", "phd"};
        private static readonly string[] _prefixes = {"bar","ben","bin","da","dal","de la", "de", "del","der","di", "ibn","la","le","san","st","ste","van", "van der", "van den", "vel","von"   };
        private static readonly string[] _title  = {"mr", "master", "mister", "mrs", "miss", "ms", "dr", "prof", "rev", "fr", "judge", "honorable", "hon", "tuan", "sr", "srta", "br", "pr", "mx", "sra"};

        const RegexOptions _options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline;

        private static readonly Regex _nicknamesRegex =	new Regex(@"('|\""|\(\""*'*)\s*(.+?)(\""*'*\)|'|\"")", _options); // names that starts or end w/ an apostrophe break this
        private static readonly Regex _titleRegex;
        private static readonly Regex _suffixRegex;
        private static readonly Regex _lastRegex;
        private static readonly Regex _leadingInitRegex = new Regex(@"^(\w\.*)(?=\s\w{2})",_options); // note the lookahead, which isn't returned or replaced
        private static readonly Regex _firstRegex = new Regex(@"^[^ ]+", _options);


        private string _name;


        static  Parser()
        {
            var suf = @",?\s((" + String.Join(@"\.?)|(", _suffixes) + @"\.?))$";
            _suffixRegex = new Regex(suf, _options);

            // must use "[^ ]" instead of \w because \w doesn't include apostrophes
            var pref = @"(?!^)\b([^ ]+ y |" + String.Join(" |", _prefixes) + @" )*([^ ]+)$";
            _lastRegex = new Regex(pref, _options);

            var titl = @"^\s*((" + String.Join(@"\.?)|(", _title) + @".?))\s+";
            _titleRegex = new Regex(titl, _options);
        }

        public Parser()
        {

        }

        public ParsedName Parse(string name)
        {
            _name = name;
            var pname = new ParsedName(name);

            if (name.IndexOf(' ') == -1)
                // Consider a single name as a last name (because that's the name used to index it)
                pname.Last = name;
            else
            {
                pname.Nicknames = chopWithRegex(_nicknamesRegex, 2);
                pname.Suffix = chopWithRegex(_suffixRegex, 1);
                _name = flip(_name, ',');

                pname.Title = chopWithRegex(_titleRegex, 1);
                pname.LeadingInitial = chopWithRegex(_leadingInitRegex, 1);

                pname.Last = chopWithRegex(_lastRegex, 0);

                pname.First = chopWithRegex(_firstRegex, 0);

                pname.Middle = _name.Trim();
            }

            return pname;
        }

        private string  chopWithRegex(Regex regex, int submatchIndex = 0)
        {
            var match = regex.Match(_name);
            if (!match.Success)
                return String.Empty;

            var text = match.Groups[submatchIndex].Value;
            _name = normalize(_name.Replace(match.Value, ""));

            return text;
        }

        /*
         * Flips the front and back parts of a name with one another.
         * Front and back are determined by a specified character somewhere in the
         * middle of the string.
         *
         * @param	String $flipAroundChar	the character(s) demarcating the two halves you want to flip.
         * @return Bool True on success.
         */
        public string flip(string name, char flipAroundChar)
        {
            var substrings = name.Split(flipAroundChar);
            var newname = name;

            if (substrings.Length == 2)
            {
                 newname = substrings[1] + " " + substrings[0];
                newname = normalize(newname);
            }
            if (substrings.Length > 2)
            {
                throw new ArgumentException("Can't flip around multiple '$flipAroundChar' characters in namestring.");
            }

            return newname;
        }

        /**
* Removes extra whitespace and punctuation from $this->str
* Strips whitespace chars from ends, strips redundant whitespace, converts whitespace chars to " ".
*
* @return Bool True on success
*/
        public  string normalize(string str)
        {
            var sb = new StringBuilder(str.Length);
            var lastSpace = true;

            foreach(char c in str)
            {
                if (Char.IsWhiteSpace(c)) // || c==',')
                {
                    if (!lastSpace)
                        sb.Append(' ');
                    lastSpace = true;
                }
                else
                {
                    lastSpace = false;
                    sb.Append(c);
                }
            }
            if (lastSpace && sb.Length > 0)
                sb.Length--;        // remove trailing space;

            return sb.ToString();
        }

    }
}
