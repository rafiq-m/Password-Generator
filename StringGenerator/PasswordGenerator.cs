using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringGenerator
{
    class PasswordGenerator
    {
        static List<int> indexes;
        static Random rand = new Random();
        static Random random = new Random();
        public static string passwordGenerator(int no_of_repeated_characters, int length, List<string> dataset)
        {
            if(no_of_repeated_characters > 3)
            {
                no_of_repeated_characters = 3;
            }
            //List<List<int>> list = new List<List<int>>();
            string check_string = GenerateRandomStringFromDataSet(length, dataset);
            //converted string to char array
            char[] check = check_string.ToCharArray();
            //applied locked parallel to char array so that correct character may not get swapped
            bool[] locked = new bool[check.Length];    

            //initialized locked with false
            for (int i = 0; i < locked.Length; i++)     
                locked[i] = false;

            //loop it upto our char array length which is out input_string or checking string
            for (int i = 0; i < check.Length; i++)      
            {
                //made list of any character indices which is repeating in our database,
                //it will include indices of a database character which is presesnt in our 
                //input string character so that we can generate string according to the third condition
                indexes = new List<int>();          

                if (!locked[i])
                {
                    //checks last 12 entries of database
                    int arr_count = 12;

                    //checks last 12 entries of databse
                    for (int j = dataset.Count - 1; j > 0 && arr_count >= 0; j--, arr_count--)      
                    {
                        for (int k = 0; k < dataset[j].Length; k++)
                        {
                            if (check[i] == dataset[j][k])
                                indexes.Add(k);
                        }
                    }

                    indexes.Sort();
                    //only contains distinct and sorted indices of database in which our 
                    //input string character is repeating
                    indexes = indexes.Distinct().ToList();     

                    for (int lm = 0; lm < indexes.Count; lm++)
                    {
                        if (indexes.Contains(i))
                        {
                            bool inside_check = false;
                            //removing the character by swapping so our 3rd condition gets satisfied
                            for (int lm_inner = 0; lm_inner < indexes[indexes.Count - 1]; lm_inner++)   
                            {
                                if (i != lm_inner)
                                {
                                    char temp = check[i];
                                    check[i] = check[lm_inner];
                                    check[lm_inner] = temp;
                                    locked[lm_inner] = true;
                                    inside_check = true;
                                }
                            }
                            if (!inside_check)
                            {
                                char temp = check[i];
                                check[i] = check[indexes[indexes.Count - 1] + 1];
                                check[indexes[indexes.Count - 1] + 1] = temp;
                                locked[indexes[indexes.Count - 1] + 1] = true;
                            }
                        }
                    }
                    if (indexes.Count == 0)
                    {
                        locked[i] = true;
                    }
                }
            }

            bool flag = false;

            //provided relaxation feature so characters don't get exhausted
            int condition_relaxation = no_of_repeated_characters;

            //checking if first letter is not a number or special character
            if (!Char.IsLetter(check[0]))
            {
                char character;
                bool checkLetter = true;
                do
                {
                    int indexOfDataset = random.Next(0, dataset.Count);
                    string letter = dataset[indexOfDataset];

                    int indexOfLetter = random.Next(0, letter.Length);
                    character = letter[indexOfLetter];

                    if (Char.IsLetter(character))
                    {
                        checkLetter = false;
                    }
                } while (checkLetter);
                check[0] = character;
            }

            //Checking if our current string satisfies our 3rd condition
            for (int i = 0; i < check.Length; i++)      
            {
                if (i < dataset.Count && i < dataset[i].Length)
                {
                    int database_Count = 12;
                    for (int j = dataset.Count - 1; j > 0 && database_Count > 0; j--)
                    {
                        if (dataset[j][i] == check[i] && condition_relaxation <= 1)
                        {
                            flag = true;
                        }
                        else if (dataset[j][i] == check[i] && condition_relaxation > 1)
                        {
                            condition_relaxation--;
                        }
                        database_Count--;
                    }
                }
            }
            if (flag)
            {
                return passwordGenerator(no_of_repeated_characters, length, dataset);
            }

            return new string(check);
        }

        public static string GenerateRandomString(int length = 8, StringOptions opts = null)
        {
            if (opts == null) opts = new StringOptions()
            {
                RequiredLength = length,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "!@#$%^&*_-?"                   // non-alphanumeric
        };

            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }
            return new string(chars.ToArray());
        }

        private static string GenerateRandomStringFromDataSet(int length, List<string> dataset)
        {
            string randomString = "";
            //got random number index
            int firstNFromIndex = random.Next(0, dataset.Count);
            string firstHalf;
            if (length / 2 > dataset[firstNFromIndex].Length)
            {
                //  take initial N characters from the above defined strings
                firstHalf = dataset[firstNFromIndex].Substring(0, dataset[firstNFromIndex].Length);
            }
            else
            {
                //  take initial N characters from the above defined strings
                firstHalf = dataset[firstNFromIndex].Substring(0, length / 2);
            }

            //  now delete the first N characters from that string
            //  so that they cannot be reused
            string temp = dataset[firstNFromIndex];
            dataset[firstNFromIndex] = dataset[firstNFromIndex].Replace(firstHalf, "");

            randomString = firstHalf;

            bool flag = true;
            bool upperChar = false, lowerChar = false, letterDigit = false, digit = false;

            dataset[firstNFromIndex] = temp;
            //loop it until we get the desired string
            while (flag && randomString.Length < length)
            {
                int NAndOnwardFromIndex = random.Next(0, dataset.Count);
                string remainingHalf = dataset[NAndOnwardFromIndex];

                int remainingCharactrFrom = random.Next(0, remainingHalf.Length);
                char character = remainingHalf[remainingCharactrFrom];

                if (Char.IsUpper(character) && !upperChar)
                {
                    randomString += character;
                    upperChar = true;
                }

                if (Char.IsLower(character) && !lowerChar)
                {
                    randomString += character;
                    lowerChar = true;
                }

                if (!char.IsLetterOrDigit(character) && !letterDigit)
                {
                    randomString += character;
                    letterDigit = true;
                }

                if (Char.IsDigit(character) && !digit)
                {
                    randomString += character;
                    digit = true;
                }

                if (upperChar && lowerChar && letterDigit && digit && !dataset.Contains(randomString))
                {

                    if (randomString.Length < length)
                    {
                        upperChar = false;
                        lowerChar = false;
                        letterDigit = false;
                        digit = false;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                if (randomString.Length >= length)
                {
                    flag = false;
                }
            }
            return randomString;
        }
    }
}
