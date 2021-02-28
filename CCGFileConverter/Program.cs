using System;
using System.Collections.Generic;
using System.IO;

namespace CCGFileConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Translator> translators = new List<Translator>();
            translators.Add(new CSVToXML());
            int userChoice = 0; //options do not go down to 0. 
            int numberOfOptions = 0;
            string userInput = "";

            do
            {
                string introText = "Welcome to the basic file converter. this program currently may complete the following conversions: \n";
                for (int i = 1; i <= translators.Count; i++)
                {
                    introText += "[" + i + "] " + translators[i - 1].supportedFileType + " -> " + translators[i - 1].resultingFileType + "\n";
                }
                numberOfOptions = translators.Count;
                introText += "Which of these file types would you like to convert?";
                Console.WriteLine(introText);

                userInput = Console.ReadLine();
            } 
            while (!int.TryParse(userInput, out userChoice) || userChoice == 0 || userChoice > numberOfOptions);

            do
            {
                Console.WriteLine("please enter the location of the file that you wish to convert");
                userInput = Console.ReadLine();
            }
            while (!File.Exists(userInput) && !userInput.Contains(translators[userChoice - 1].supportedFileType));

            Console.WriteLine("your converted file is located at " + translators[userChoice - 1].translateSupportedFile(userInput));

        }

    }
}
