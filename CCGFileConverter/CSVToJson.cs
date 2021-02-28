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

        /*
        * where this project to continue, and we were to implement other filetypes to json. this function would be brought out into it's own seperate class, 
        * interacted with here through an interface. this would mean that for other filetypes to json, all we would need to do would be to program the 
        * data retrieval from the origin filetype.
        */
        private bool createNewFile(List<List<string>> attributesAndValues)
        {
            bool success = false;
            bool groupedValues = false;
            /*
            * below we will take the csv attributes and values and create a json-string
            * i have CHOSEN to do it manually instead of using JsonSerializer library, as to my research
            * the serialiser library requires objects to work on.
            * doing it in this manual method would allow us to be able to parse any .CSV that follows the dictated format
            * into a json file. not just customer information.
            */
            string jsonString = "{";
            for (int i = 1; i < attributesAndValues.Count; i++) //skip first item on the list as this is our atribute names
            {
                jsonString += "\n \"Entry" + i + "\": \n  { ";
                for (int j = 0; j < attributesAndValues[j].Count; j++)
                {
                    if (!attributesAndValues[0].ElementAt(j).Contains('_')) //no grouped values, carry on as normal
                    {
                        jsonString += "\n   \"" + attributesAndValues[0].ElementAt(j) + "\":\"" + attributesAndValues[i].ElementAt(j) + "\",";
                    }
                    else if (!groupedValues) // grouped values and first entry in group
                    {
                        jsonString += "\n   \"" + attributesAndValues[0].ElementAt(j).Split('_')[0] + "\": {";
                        jsonString += "\n     \"" + attributesAndValues[0].ElementAt(j).Split('_')[1] + "\":\"" + attributesAndValues[i].ElementAt(j) + "\",";
                        groupedValues = true;
                    }
                    else //further grouped value
                    {
                        jsonString += "\n     \"" + attributesAndValues[0].ElementAt(j).Split('_')[1] + "\":\"" + attributesAndValues[i].ElementAt(j) + "\",";
                    }
                    if(groupedValues) //check that the group continues, if we are in a group, if not close group
                    {
                        if (j + 1 == attributesAndValues[0].Count || !attributesAndValues[0].ElementAt(j + 1).Contains('_'))
                        {
                            jsonString = jsonString.Remove(jsonString.Length - 1);
                            jsonString += "\n    },";
                            groupedValues = false;
                        }
                    }
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

