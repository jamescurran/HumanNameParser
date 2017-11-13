using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace HumanNameParser.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void BasicTest()
        {
            var parser = new Parser();
            var pname = parser.Parse("Björn O'Malley");

            Assert.AreEqual("Björn", pname.First);
            Assert.AreEqual("O'Malley", pname.Last);
            Assert.AreEqual("", pname.Middle);
            Assert.AreEqual("", pname.Nicknames);
            Assert.AreEqual("", pname.Suffix);
        }

        [TestMethod]
        public void Nickname()
        {
            var parser = new Parser();
            var pname = parser.Parse("James C. ('Jimmy') O'Dell, Jr.");

            Assert.AreEqual("Jimmy", pname.Nicknames);
            Assert.AreEqual("", pname.LeadingInitial);
            Assert.AreEqual("Jr.", pname.Suffix);
            Assert.AreEqual("O'Dell", pname.Last);
            Assert.AreEqual("James", pname.First);
            Assert.AreEqual("C.", pname.Middle);
        }

        [TestMethod]
        public void OneName()
        {
            var parser = new Parser();
            var pname = parser.Parse("Cher");

            Assert.AreEqual("", pname.Nicknames);
            Assert.AreEqual("", pname.LeadingInitial);
            Assert.AreEqual("", pname.Suffix);
            Assert.AreEqual("Cher", pname.Last);
            Assert.AreEqual("", pname.First);
            Assert.AreEqual("", pname.Middle);
        }



        [TestMethod]
        public void LeadingInitial()
        {
            var parser = new Parser();
            var pname = parser.Parse("J. Walter Weatherman");

            Assert.AreEqual("J.", pname.LeadingInitial);
            Assert.AreEqual("Walter", pname.First);
            Assert.AreEqual("", pname.Middle);
            Assert.AreEqual("Weatherman", pname.Last);
            Assert.AreEqual("", pname.Nicknames);
            Assert.AreEqual("", pname.Suffix);

        }

        [TestMethod]
        public void WackyFormat1()
        {
            var parser = new Parser();
            var pname = parser.Parse("de la Cruz, Ana M.");

            Assert.AreEqual("", pname.LeadingInitial);
            Assert.AreEqual("Ana", pname.First);
            Assert.AreEqual("M.", pname.Middle);
            Assert.AreEqual("de la Cruz", pname.Last);
            Assert.AreEqual("", pname.Nicknames);
            Assert.AreEqual("", pname.Suffix);

        }

        // Test Normalize

        [TestMethod]
        public void NormalizeTests()
        {
            var parser = new Parser();
            Assert.AreEqual("abc def ghi", parser.normalize("  abc  def  ghi   "));
          //  Assert.AreEqual("abc def ghi", parser.normalize("  abc,  def,,  ghi ,  "));
        }

        [TestMethod]
        public void FlipTests()
        {
            var parser = new Parser();
            Assert.AreEqual("John Smith", parser.flip("John Smith", ','));
            Assert.AreEqual("John Smith", parser.flip("Smith, John", ','));
            Assert.AreEqual("John Smith", parser.flip("Smith,John", ','));
            Assert.AreEqual("John Smith", parser.flip("Smith,    John    ", ','));
        }

        [TestMethod]
        public void TestFullList()
        {
            // <nameString>|<firstInitial>|<firstName>|<nicknames>|<middleNames>|<lastNames>|<suffix>
            var names = File.ReadAllLines("testNames.txt");
            var parser = new Parser();
            foreach(var name  in names)
            {
                var parts = name.Split('|');
                var pname = parser.Parse(parts[0]);

                Assert.AreEqual(parts[1] ?? "", pname.LeadingInitial, name);
                Assert.AreEqual(parts[2] ?? "", pname.First, name);
                Assert.AreEqual(parts[3] ?? "", pname.Nicknames, name);
                Assert.AreEqual(parts[4] ?? "", pname.Middle, name);
                Assert.AreEqual(parts[5] ?? "", pname.Last, name);
                Assert.AreEqual(parts[6] ?? "", pname.Suffix, name);


            }


        }
    }
}
