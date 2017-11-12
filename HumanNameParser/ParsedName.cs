using System;

namespace HumanNameParser
{
    public class ParsedName
    {
        public string FullName { get;  set; }

        public string LeadingInitial { get; set; }

        public string First { get;  set; }
        public string Nicknames { get;  set; }
        public string Middle { get;  set; }
        public string Last { get;  set; }
        public string Suffix { get;  set; }

        public ParsedName(string name)
        {
            First = "";
            Nicknames = "";
            Middle = "";
            Last = "";
            Suffix = "";

            FullName = name;
        }
}
}
