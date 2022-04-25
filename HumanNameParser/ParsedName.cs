﻿using System;

namespace NovelTheory.HumanNameParser
{
    public class ParsedName
    {
        public string FullName { get; set; }

        public string Title { get; set; }
        public string LeadingInitial { get; set; }
        public string First { get; set; }
        public string Nicknames { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
        public string Suffix { get; set; }

        public ParsedName(string name)
        {
            Title = "";
            First = "";
            Nicknames = "";
            Middle = "";
            Last = "";
            Suffix = "";
            LeadingInitial = "";

            FullName = name;
        }
    }
}
