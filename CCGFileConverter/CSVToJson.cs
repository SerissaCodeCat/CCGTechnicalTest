using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CCGFileConverter
{
    public class CSVToJson: Translator
    {
        public override string supportedFileType { get; } = ".csv";
        public override string resultingFileType { get; } = ".json";
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
                if (createNewFile(listOfAtributesAndValues))
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

            string jsonString = "{";
            for (int i = 1; i < attributesAndValues.Count; i++) //skip first item on the list as this is our atribute names
            {
                jsonString += "\n \"Entry" + i + "\": \n  { ";
                for (int j = 0; j < attributesAndValues[j].Count; j++)
                {
                    jsonString += "\n   \"" + attributesAndValues[0].ElementAt(j) + "\":\"" + attributesAndValues[i].ElementAt(j) + "\",";
                }
                jsonString = jsonString.Remove(jsonString.Length - 1);
                jsonString += "\n  },";
            }
            jsonString = jsonString.Remove(jsonString.Length - 1);
            jsonString += "\n}";
            resultingFileLocation = fileLocation.Remove(fileLocation.Length - 4);
            resultingFileLocation += resultingFileType;
            try
            {
                File.WriteAllText(resultingFileLocation, jsonString);
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("failed to save to " + resultingFileLocation + "\n " + e.Message);
            }

            return success;
        }
    }
}

