using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace HumanNameParser.Tests
{
    [TestClass]
    public class UnitTest1
    {
	    private readonly Parser _parser;

	    public UnitTest1()
	    {
				_parser = new Parser();
	    }
        [TestMethod]
        public void BasicTest()
        {
            var pName = _parser.Parse("Björn O'Malley");

            Assert.AreEqual("Björn", pName.First);
            Assert.AreEqual("O'Malley", pName.Last);
            Assert.AreEqual("", pName.Middle);
            Assert.AreEqual("", pName.Nicknames);
            Assert.AreEqual("", pName.Suffix);
        }

        [TestMethod]
        public void Nickname()
        {
            var pName = _parser.Parse("James C. ('Jimmy') O'Dell, Jr.");

            Assert.AreEqual("Jimmy", pName.Nicknames);
            Assert.AreEqual("", pName.LeadingInitial);
            Assert.AreEqual("Jr.", pName.Suffix);
            Assert.AreEqual("O'Dell", pName.Last);
            Assert.AreEqual("James", pName.First);
            Assert.AreEqual("C.", pName.Middle);
        }

        [TestMethod]
        public void OneName()
        {
            var pName = _parser.Parse("Cher");

            Assert.AreEqual("", pName.Nicknames);
            Assert.AreEqual("", pName.LeadingInitial);
            Assert.AreEqual("", pName.Suffix);
            Assert.AreEqual("Cher", pName.Last);
            Assert.AreEqual("", pName.First);
            Assert.AreEqual("", pName.Middle);
        }

        [TestMethod]
        public void LeadingInitial()
        {
            var pName = _parser.Parse("J. Walter Weatherman");

            Assert.AreEqual("J.", pName.LeadingInitial);
            Assert.AreEqual("Walter", pName.First);
            Assert.AreEqual("", pName.Middle);
            Assert.AreEqual("Weatherman", pName.Last);
            Assert.AreEqual("", pName.Nicknames);
            Assert.AreEqual("", pName.Suffix);

        }

        [TestMethod]
        public void LeadingInitial_Name_SameLetter()
        {
	        var pName = _parser.Parse("A Anderson");

	        Assert.AreEqual("A", pName.LeadingInitial);
	        Assert.AreEqual("", pName.First);
	        Assert.AreEqual("", pName.Middle);
	        Assert.AreEqual("Anderson", pName.Last);		// was parsing as "nderson" as first name
	        Assert.AreEqual("", pName.Nicknames);
	        Assert.AreEqual("", pName.Suffix);

        }


		[TestMethod]
        public void WackyFormat1()
        {
            var pName = _parser.Parse("de la Cruz, Ana M.");

            Assert.AreEqual("", pName.LeadingInitial);
            Assert.AreEqual("Ana", pName.First);
            Assert.AreEqual("M.", pName.Middle);
            Assert.AreEqual("de la Cruz", pName.Last);
            Assert.AreEqual("", pName.Nicknames);
            Assert.AreEqual("", pName.Suffix);

        }

        [TestMethod]
        public void Title()
        {
            var pName = _parser.Parse("Mr. William R. De La Cruz III");

            Assert.AreEqual("Mr.", pName.Title);
            Assert.AreEqual("", pName.LeadingInitial);
            Assert.AreEqual("William", pName.First);
            Assert.AreEqual("R.", pName.Middle);
            Assert.AreEqual("De La Cruz", pName.Last);
            Assert.AreEqual("", pName.Nicknames);
            Assert.AreEqual("III", pName.Suffix);

        }


        // Test Normalize

        [TestMethod]
        public void NormalizeTests()
        {
            Assert.AreEqual("abc def ghi", _parser.normalize("  abc  def  ghi   "));
          //  Assert.AreEqual("abc def ghi", parser.normalize("  abc,  def,,  ghi ,  "));
        }

        [TestMethod]
        public void FlipTests()
        {
            Assert.AreEqual("John Smith", _parser.flip("John Smith", ','));
            Assert.AreEqual("John Smith", _parser.flip("Smith, John", ','));
            Assert.AreEqual("John Smith", _parser.flip("Smith,John", ','));
            Assert.AreEqual("John Smith", _parser.flip("Smith,    John    ", ','));
        }
     

        [TestMethod]
        public void TestFullList()
        {
            // <nameString>|<firstInitial>|<firstName>|<nicknames>|<middleNames>|<lastNames>|<suffix>
            var names = File.ReadAllLines("testNames.txt");
            foreach(var name  in names)
            {
                if (name[0] == '*')
                    continue;

                var parts = name.Split('|');
                var pName = _parser.Parse(parts[0]);

                Assert.AreEqual(parts[1], pName.Title, name);
                Assert.AreEqual(parts[2], pName.LeadingInitial, name);
                Assert.AreEqual(parts[3], pName.First, name);
                Assert.AreEqual(parts[4], pName.Nicknames, name);
                Assert.AreEqual(parts[5], pName.Middle, name);
                Assert.AreEqual(parts[6], pName.Last, name);
                Assert.AreEqual(parts[7], pName.Suffix, name);
            }
        }
    }
}
