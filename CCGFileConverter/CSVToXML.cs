using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;

namespace CCGFileConverter
{
    public class CSVToXML: Translator
    {
        public override string supportedFileType { get; } = ".csv";
        public override string resultingFileType { get; } = ".xml";
        private string resultingFileLocation = "";
        private string fileLocation = "";
        private List<List<string>> listOfAtributesAndValues;
        /*
        * returns a string result with the final file's location. 
        * if conversion fails result is ""
        */
        public override string translateSupportedFile(string incomingFileLocation)
        {
            fileLocation = incomingFileLocation;
            if (File.Exists(fileLocation) && fileLocation.Contains(supportedFileType))
            {
                listOfAtributesAndValues = getAttributesAndValues(incomingFileLocation);
                if(createNewFile(listOfAtributesAndValues))
                {
                    return resultingFileLocation;
                }
            }
            return "";
        }

        /*
        * returns a List of String-lists. first list contains atributes, further list contains values
        */
        private List<List<string>> getAttributesAndValues(string incomingFileLocation)
        {
            List<List<string>> result = new List<List<string>>();
            using (StreamReader reader = new StreamReader(incomingFileLocation))//ensure file does not hang around open in mem
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    List<string> temp = new List<string>();
                    string[] lineSegmented = line.Split(",");
                    foreach (string attribute in lineSegmented)
                    {
                        temp.Add(attribute);
                    }
                    result.Add(temp);
                }
            }
            return result;
        }
        private bool createNewFile(List<List<string>> attributesAndValues)
        {
            bool success = false;
            XmlDocument xml = new XmlDocument();
            XmlNode rootNode = xml.CreateElement("Data");
            xml.AppendChild(rootNode);

            for (int i = 1; i < attributesAndValues.Count; i++) //skip first item on the list as this is our atribute names
            {
                XmlNode dataItem = xml.CreateElement("Entry");
                rootNode.AppendChild(dataItem);
                for (int j = 0; j < attributesAndValues[j].Count; j++)
                {
                    XmlAttribute attribute = xml.CreateAttribute(attributesAndValues[0].ElementAt(j));
                    attribute.Value = attributesAndValues[i].ElementAt(j);
                    dataItem.Attributes.Append(attribute);
                }
            }
            resultingFileLocation = fileLocation.Remove(fileLocation.Length - 4);
            resultingFileLocation += resultingFileType;
            try
            {
                xml.Save(resultingFileLocation);
                success = true;
            }
            catch(Exception e)
            {
                Console.WriteLine("failed to save to " + resultingFileLocation + "\n " + e.Message);
            }

            return success;
        }
    }
}
