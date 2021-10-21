using System;

namespace ConsoleUI
{ 
       class Program
        {
            static void Main(string[] args)
            {
            //IDAL.DO.BaseStation baseStasion = new IDAL.DO.BaseStation();
            int userA;
           // BusList BL = new BusList();
            Console.WriteLine("Menue: ");
            Console.WriteLine("press 1 to add a drone");
            Console.WriteLine("press 2 to update");
            Console.WriteLine("press 3 to display");
            Console.WriteLine("press 4 to see lists ");
            Console.WriteLine("press 5 to exit");
            IDAL.DO.MainChoice choice;
            bool b;
            string s;
            //do//have to finish this do while  went straight to the enums
            //{
            //    Console.WriteLine("enter a number between 1-5");

            //}
            switch (choice)
            {
                case.MainChoice.add:
                    {
                        Console.WriteLine("enter a station id");
                        userA=int.Parse(Console.ReadLine());
                        //add some sort of array that contains all the satations
                        // we are thinking of changeing to lists and not array

                    }
                    case.MainChoices.update:
                    {
                        
                    }

            }




        }
        }
}
