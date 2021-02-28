using CCGFileConverter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text.Json;
using System.Xml;

namespace CCGFileConverterUnitTests
{
    [TestClass]
    public class UnitTestCSVToXML
    {
        [TestMethod]
        public void TestCSVToXMLConversionOccurs()
        {
            Translator unitUnderTest = new CSVToXML();
            string result = unitUnderTest.translateSupportedFile("./test.csv");
            Assert.AreEqual("./test.xml", result);
            if (File.Exists(result))
            {
                File.Delete(result); //clean up after test to not invalidate future tests
            }
        }
        [TestMethod]
        public void testCSVToXMLconversionIsCorrect()
        {
            XmlDocument expectedResult = new XmlDocument();
            expectedResult.Load("./expectedXMLTestResult.xml");
            XmlDocument result = new XmlDocument();

            Translator unitUnderTest = new CSVToXML();
            string fileLocation = unitUnderTest.translateSupportedFile("./test.csv");
            if (File.Exists(fileLocation))
            {
                result.Load(fileLocation);
            }
            Assert.AreEqual(expectedResult.OuterXml.ToString(), result.OuterXml.ToString());

            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation); //clean up after test to not invalidate future tests
            }
        }
        [TestMethod]
        public void testCSVToXMLconversionFailsWhenNotGivenCSV()
        {
            Translator unitUnderTest = new CSVToXML();
            string fileLocation = unitUnderTest.translateSupportedFile("./test.csv");
            string result = unitUnderTest.translateSupportedFile(fileLocation);
            Assert.AreEqual("", result);
            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation); //clean up after test to not invalidate future tests
            }
            if (File.Exists(result))
            {
                File.Delete(result); //clean up after test to not invalidate future tests
            }
        }
    }

    [TestClass]
    public class UnitTestCSVToJson
    {
        [TestMethod]
        public void TestCSVToJsonConversionOccurs()
        {
            Translator unitUnderTest = new CSVToJson();
            string result = unitUnderTest.translateSupportedFile("./test.csv");
            Assert.AreEqual("./test.json", result);
            if (File.Exists(result))
            {
                File.Delete(result); //clean up after test to not invalidate future tests
            }
        }
        [TestMethod]
        public void testCSVToJsonconversionIsCorrect()
        {
            string expectedResult = File.ReadAllText("./expectedJsonTestResult.json");
            string result = "";

            Translator unitUnderTest = new CSVToJson();
            string fileLocation = unitUnderTest.translateSupportedFile("./test.csv");
            if (File.Exists(fileLocation))
            {
                result = File.ReadAllText(fileLocation);
            }
            Assert.AreEqual(expectedResult, result);

            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation); //clean up after test to not invalidate future tests
            }
        }
        [TestMethod]
        public void testCSVToJsonconversionFailsWhenNotGivenCSV()
        {
            Translator unitUnderTest = new CSVToJson();
            string fileLocation = unitUnderTest.translateSupportedFile("./test.csv");
            string result = unitUnderTest.translateSupportedFile(fileLocation);
            Assert.AreEqual("", result);
            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation); //clean up after test to not invalidate future tests
            }
            if (File.Exists(result))
            {
                File.Delete(result); //clean up after test to not invalidate future tests
            }
        }
    }
}
