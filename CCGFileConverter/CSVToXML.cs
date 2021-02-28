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

        /*
        * where this project to continue, and we were to implement other filetypes to XML. this function would be brought out into it's own seperate class, 
        * interacted with here through an interface. this would mean that for other filetypes to XML, all we would need to do would be to program the 
        * data retrieval from the origin filetype.
        */
        private bool createNewFile(List<List<string>> attributesAndValues)
        {
            bool success = false;
            XmlDocument xml = new XmlDocument();
            XmlNode rootNode = xml.CreateElement("Data");
            xml.AppendChild(rootNode);
            bool groupedValues = false;
            for (int i = 1; i < attributesAndValues.Count; i++) //skip first item on the list as this is our atribute names
            {
                XmlNode dataItem = xml.CreateElement("Entry");
                rootNode.AppendChild(dataItem);
                XmlNode item = xml.CreateElement("tmp");
                for (int j = 0; j < attributesAndValues[j].Count; j++)
                {
                    if (!attributesAndValues[0].ElementAt(j).Contains('_')) //no grouped values, carry on as normal
                    {
                        item = xml.CreateElement(attributesAndValues[0].ElementAt(j));
                        item.InnerText = attributesAndValues[i].ElementAt(j);
                        dataItem.AppendChild(item);
                    }
                    else if (!groupedValues)
                    {
                        item = xml.CreateElement(attributesAndValues[0].ElementAt(j).Split('_')[0]);
                        dataItem.AppendChild(item);
                        XmlNode subItem = xml.CreateElement(attributesAndValues[0].ElementAt(j).Split('_')[1]);
                        subItem.InnerText = attributesAndValues[i].ElementAt(j);
                        item.AppendChild(subItem);
                        groupedValues = true;
                    }
                    else
                    {
                        XmlNode subItem = xml.CreateElement(attributesAndValues[0].ElementAt(j).Split('_')[1]);
                        subItem.InnerText = attributesAndValues[i].ElementAt(j);
                        item.AppendChild(subItem);
                        groupedValues = true;
                    }
                    if (groupedValues) //check that the group continues, if we are in a group, if not close group
                    {
                        if (j + 1 == attributesAndValues[0].Count || !attributesAndValues[0].ElementAt(j + 1).Contains('_'))
                        {
                            groupedValues = false;
                        }
                    }
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
