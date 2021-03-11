using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringGenerator
{
    class Program
    {
        static void Main(string[] args)
        {   

            List<string> dataset = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                dataset.Add(PasswordGenerator.GenerateRandomString(15));
            }
            int arr_count = 12;
            for (int i = dataset.Count-1; i >0 && arr_count >0; i--)
            {
                Console.WriteLine(dataset[i]);
                arr_count--;
            }
            Console.WriteLine("_______________________");
            //Generating Passwords
            Console.WriteLine(PasswordGenerator.passwordGenerator(3, 15, dataset));
            Console.ReadKey();
        }
       

    }
}
