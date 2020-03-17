using Models;
using System;
using System.Collections.Generic;

namespace Main
{
    class Program
    {
        static void Main()
        {

            // Make blockchain
            var bc = new Blockchain();
           
            Console.WriteLine("=========================");
            Console.WriteLine("1. Get Genesis Block");
            Console.WriteLine("2. Get Last Block");
            Console.WriteLine("3. Add a transaction");
            Console.WriteLine("4. Display Blockchain");
            Console.WriteLine("5. Exit");
            Console.WriteLine("=========================");

            int selection = 0;
            while (selection != 5)
            {
                switch (selection)
                {
                    case 1:
                        Console.WriteLine("Geting Genesis Block");                       
                        break;
                    case 2:
                        Console.WriteLine("Geting Last Block");
                        break;    

                    case 3:
                        Console.WriteLine("Please enter the receiver name");             
                        break;

                    case 4:
                        Console.WriteLine("Print Blockchain");
                        break;
                }

                Console.WriteLine("Please select an action");
                string action = Console.ReadLine();
                selection = int.Parse(action);
            }


            // Console.ReadKey();
        }



    }



}