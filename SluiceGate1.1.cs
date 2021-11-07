using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SluiceGate1._1
{
    class Program
    {
        static void Main()
        {


            bool userContinue = true;
            Console.Title = "Sluice Gate";

            double p = 1000;                            //density of water
            double g = 9.81;                            //accel due to gravity
            double W, R, theta, H, BearingLimit;        //doubles used for calculations
            bool successH, successW, successBL;         //correct format input bools
            double TH, TV, THsqrd, TVsqrd, Ttotal;      //doubles used for thrust calculations
            double FOS;                                 //factor of safety
            
            do                      //do loop to loop whole input, calculation and table readout while userContinue = true
            {
                
                //input filtering for H to be double and >0
                do
                {
                    Console.Write("Input a value for the max height of the water, H: ");
                    successH = double.TryParse(Console.ReadLine(), out H) && H > 0;
                    if (!successH)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("* * * Please input a valid value for H! * * *");
                        successH = false;
                        Console.ResetColor();
                    }

                } while (!successH);

                //input filtering for W to be double and >0
                do
                {
                    Console.Write("Input a value for the width of the sluice gate, W: ");
                    successW = double.TryParse(Console.ReadLine(), out W) && W > 0;
                    if (!successW)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("* * * Please input a valid value for W! * * *");
                        successW = false;
                        Console.ResetColor();
                    }

                } while (!successW);

                //input filtering for BearingLimit to be double and >0
                do
                {
                    Console.Write("Input a value for the bearing limit in kN: ");
                    successBL = double.TryParse(Console.ReadLine(), out BearingLimit) && BearingLimit > 0;
                    if (!successBL)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("* * * Please input a valid value for the bearing limit! * * *");
                        successBL = false;
                        Console.ResetColor();
                    }

                } while (!successBL);

                //Read out all currently stored values to the user
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Density of Water:                {0}kg/m^3", p);
                Console.WriteLine("Acceleration due to gravity:     {0}m/s^2", g);
                Console.WriteLine("Maximum height:                  {0}m", H);
                Console.WriteLine("Width of sluice:                 {0}m", W);
                Console.WriteLine("Bearing Limit                    {0}kN", BearingLimit);

                //Headings of table; write before loop
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Angle(deg)   Radius(m)   Thrust(kN)  F.O.S");
                Console.ForegroundColor = ConsoleColor.Gray;

                //maths calculations for each data value
                for (theta = 7.5; theta <= 90; theta += 7.5)
                {
                    //convert theta to rads for calculation
                    double thetaRads = theta * ((Math.PI) / 180);

                    //Calculation of radius
                    R = (H) / (2 * Math.Sin(thetaRads));

                    //Calculations for TH and TV to get Ttotal
                    TH = (p * g * Math.Pow(H, 2) * W) / 2;
                    TV = (p * g * Math.Pow(R, 2) * W) * (thetaRads - (0.5 * Math.Sin(2 * thetaRads)));
                    THsqrd = TH * TH;
                    TVsqrd = TV * TV;

                    Ttotal = (Math.Sqrt(THsqrd + TVsqrd)) / 1000;       //Ttotal converted into kN to match input in kN

                    FOS = BearingLimit / Ttotal;                        //if FOS < 1 then thrust > bearing rating ie bearing will break

                    //Bearing too small break condition
                    if (FOS < 1.00)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("* * * Thrust force exceeds bearing limit! * * *");
                        Console.ResetColor();
                        break;
                    }

                    //2nd attempt at table formatting
                    Console.WriteLine("{0,8:f1} {1,10:f2} {2,12:f1} {3,8:f2}", theta, R, Ttotal, FOS);

                

                    //old formatting for table that isnt efficient but spent too long on it to delete
                    //  Console.WriteLine("{0}           {1}       {2}      {3}", theta.ToString("00.0"), R.ToString("00.00"), Ttotal.ToString("F1"), FOS.ToString("F2"));
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Would you like to input new data values? (Y/N): ");
                Console.ResetColor();
                if (Console.ReadKey().Key == ConsoleKey.Y)
                {
                    userContinue = true;
                    Console.WriteLine();
                    Console.WriteLine();
                }
                else
                {
                    userContinue = false;
                }

            } while (userContinue);

            Console.WriteLine();
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();

        }
    }
}
