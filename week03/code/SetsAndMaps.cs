using System.Text.Json;

public static class SetsAndMaps
{
    /// <summary>
    /// The words parameter contains a list of two character 
    /// words (lower case, no duplicates). Using sets, find an O(n) 
    /// solution for returning all symmetric pairs of words.  
    ///
    /// For example, if words was: [am, at, ma, if, fi], we would return :
    ///
    /// ["am & ma", "if & fi"]
    ///
    /// The order of the array does not matter, nor does the order of the specific words in each string in the array.
    /// at would not be returned because ta is not in the list of words.
    ///
    /// As a special case, if the letters are the same (example: 'aa') then
    /// it would not match anything else (remember the assumption above
    /// that there were no duplicates) and therefore should not be returned.
    /// </summary>
    /// <param name="words">An array of 2-character words (lowercase, no duplicates)</param>
    public static string[] FindPairs(string[] words)
    {
        HashSet<string> wordsSet = new HashSet<string>(words); // passing to a set so it eliminates duplicates
        HashSet<string> hasPair = new HashSet<string>(); // the set where it will be store 

        foreach (var word in wordsSet) // checking through the cleaned set
        {
            // reversing the word 
            char[] chars = word.ToCharArray();
            Array.Reverse(chars);

            string reversed = new string(chars);

            // adding to hasPair set, avoiding "aa" examples and duplication as "ma & am" "am & ma".
            if (word != reversed && !hasPair.Contains($"{reversed} & {word}"))
            {
                if (wordsSet.Contains(reversed))
                {
                    hasPair.Add($"{word} & {reversed}");
                }
            }

        }

        return hasPair.ToArray(); // changed to array so it meets the output of the fucntion
    }

    /// <summary>
    /// Read a census file and summarize the degrees (education)
    /// earned by those contained in the file.  The summary
    /// should be stored in a dictionary where the key is the
    /// degree earned and the value is the number of people that 
    /// have earned that degree.  The degree information is in
    /// the 4th column of the file.  There is no header row in the
    /// file.
    /// </summary>
    /// <param name="filename">The name of the file to read</param>
    /// <returns>fixed array of divisors</returns>
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();
        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(",");

            if (degrees.ContainsKey(fields[3])) // checks if key is already created
            {
                degrees[fields[3]] += 1; // if so adds one more person
            }
            else
            {
                degrees[fields[3]] = 1; // if not created the key and adds as value a person
            }

        }

        return degrees;
    }

    /// <summary>
    /// Determine if 'word1' and 'word2' are anagrams.  An anagram
    /// is when the same letters in a word are re-organized into a 
    /// new word.  A dictionary is used to solve the problem.
    /// 
    /// Examples:
    /// is_anagram("CAT","ACT") would return true
    /// is_anagram("DOG","GOOD") would return false because GOOD has 2 O's
    /// 
    /// Important Note: When determining if two words are anagrams, you
    /// should ignore any spaces.  You should also ignore cases.  For 
    /// example, 'Ab' and 'Ba' should be considered anagrams
    /// 
    /// Reminder: You can access a letter by index in a string by 
    /// using the [] notation.
    /// </summary>
    public static bool IsAnagram(string word1, string word2)
    {
        // words are no longer case sensitive or space sensitive.
        string firstWord = word1.ToLower().Replace(" ", "");
        string secondWord = word2.ToLower().Replace(" ", "");

        // first test if one on longer there is no way they are anagrams
        if (firstWord.Length != secondWord.Length)
        {
            return false;
        }

        //dictionary for each word as counter of how many times a character is been used
        var dictionary1 = new Dictionary<char, int>();
        var dictionary2 = new Dictionary<char, int>();



        foreach (var character in firstWord) // word 1 checking key in order to add it to dictionary.
        {
            if (dictionary1.ContainsKey(character))
            {
                dictionary1[character] += 1;
            }
            else
            {
                dictionary1[character] = 1;
            }
        }

        foreach (var character in secondWord) // word 2 : checking key in order to add it to dictionary.
        {

            if (dictionary2.ContainsKey(character))
            {
                dictionary2[character] += 1;
            }
            else
            {
                dictionary2[character] = 1;
            }
        }

        bool isAnagram = false;

        //checking if dic 1 is equal to dic 2 in content
        foreach (var letter in firstWord)
        {
            if (dictionary1.ContainsKey(letter) && dictionary2.ContainsKey(letter))
            {
                if (dictionary1[letter] == dictionary2[letter])
                {
                    isAnagram = true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return isAnagram;
    }

    /// <summary>
    /// This function will read JSON (Javascript Object Notation) data from the 
    /// United States Geological Service (USGS) consisting of earthquake data.
    /// The data will include all earthquakes in the current day.
    /// 
    /// JSON data is organized into a dictionary. After reading the data using
    /// the built-in HTTP client library, this function will return a list of all
    /// earthquake locations ('place' attribute) and magnitudes ('mag' attribute).
    /// Additional information about the format of the JSON data can be found 
    /// at this website:  
    /// 
    /// https://earthquake.usgs.gov/earthquakes/feed/v1.0/geojson.php
    /// 
    /// </summary>
    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
        using var client = new HttpClient();
        using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using var jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
        using var reader = new StreamReader(jsonStream);
        var json = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);

        List<string> summaries = new List<string>();


        foreach (var feature in featureCollection.Features)
        {
            string place = feature.Properties.Place;
            double mag = feature.Properties.Mag;

            summaries.Add($"{place} - Mag {mag}");
        }


        return summaries.ToArray();
    }
}
