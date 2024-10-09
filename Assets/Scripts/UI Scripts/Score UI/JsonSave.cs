using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Script
{
    public class JsonSave
    {
        private static int MaxPlays = 10;

        public static void SaveGame(float score, TimeSpan finishTime)
        {
            // Load the existing list from JSON, or create a new list if null
            List<StatSave> resultList = LoadListFromJson() ?? new List<StatSave>();

            // Create a new instance of StatSave and add it to the list
            StatSave newPlay = new StatSave
            {
                Score = score,
                Time = finishTime.ToString("mm':'ss'.'ff")
            };

            resultList.Add(newPlay);  // Add the new play to the list

            // Ensure we only keep the latest 10 plays
            if (resultList.Count > MaxPlays)
            {
                resultList.RemoveAt(0);  // Remove the oldest play if the list exceeds MaxPlays
            }

            // Save the updated list back to JSON
            SaveListToJson(resultList);
        }

        public static void SaveListToJson(List<StatSave> resultList)
        {
            // Create a wrapper around the list to serialize it
            ResultWrapper wrapper = new ResultWrapper(resultList);

            // Convert the wrapper to a JSON string
            string jsonString = JsonUtility.ToJson(wrapper, true);

            // Define the file path and write the JSON string to the file
            string filePath = Path.Combine(Application.persistentDataPath, "SaveFile.json");
            File.WriteAllText(filePath, jsonString);
        }

        public static List<StatSave> LoadListFromJson()
        {
            // Define the file path
            string filePath = Path.Combine(Application.persistentDataPath, "SaveFile.json");

            // If the file doesn't exist, return null
            if (!File.Exists(filePath))
            {
                Debug.LogWarning("Save file not found!");
                return null;
            }

            // Read the JSON string from the file
            string jsonString = File.ReadAllText(filePath);

            // Deserialize the JSON string into a ResultWrapper object
            ResultWrapper wrapper = JsonUtility.FromJson<ResultWrapper>(jsonString);

            // Return the list of results from the wrapper
            return wrapper?.results;
        }

        [System.Serializable]
        public class ResultWrapper
        {
            public List<StatSave> results;

            public ResultWrapper(List<StatSave> result)
            {
                results = result;
            }
        }
    }    
}
