using System;
using CodeChallenge.Domain.Model.Parcel;
using CodeChallenge.Domain.Exceptions.Parcel;
using CodeChallenge.Domain;

namespace CodeChallenge.Cli
{
    /* Specifications call for 4 integers as an input, which has been validated for as requested. 
     * However in my opinion an integer does not allow for enough precision against the weight so 
     * internally the domain model could use a decimal. It also seems inefficient to use integers 
     * to store the physical dimensions of the package when an unsigned short will still allow for 
     * a package 65m^3 in size. */
    public class Program
    {
        public static void Main(string[] args)
        {
            BeginCycle();
        }

        /// <summary>
        /// Main loop for the console application. The user will continue to be allowed to calculate new
        /// parcel costs until they key in N or n to exit the application. 
        /// </summary>
        public static void BeginCycle() 
        {
            bool cycle = true;
            while (cycle == true)
            {
                RenderWelcomePage();

                #region - Get all the parcel metrics from the user -
                InputCollector weightCollector = new InputCollector("weight (kg)");
                int weight = weightCollector.ReadInteger(BaseParcel.MinimumWeight, BaseParcel.MaximumWeight);

                InputCollector heightCollector = new InputCollector("height (cm)");
                int height = heightCollector.ReadInteger(BaseParcel.MinimumHeight, BaseParcel.MaximumHeight);

                InputCollector widthCollector = new InputCollector("width (cm)");
                int width = widthCollector.ReadInteger(BaseParcel.MinimumWidth, BaseParcel.MaximumWidth);

                InputCollector depthCollector = new InputCollector("depth (cm)");
                int depth = depthCollector.ReadInteger(BaseParcel.MinimumDepth, BaseParcel.MaximumDepth);

                InputCollector depthCollector2 = new InputCollector("depth (cm)");
                int depth2 = depthCollector.ReadInteger(BaseParcel.MinimumDepth, BaseParcel.MaximumDepth);

                InputCollector depthCollector3 = new InputCollector("depth (cm)");
                int depth3 = depthCollector.ReadInteger(BaseParcel.MinimumDepth, BaseParcel.MaximumDepth);

                InputCollector depthCollector4 = new InputCollector("depth (cm)");
                int depth4 = depthCollector.ReadInteger(BaseParcel.MinimumDepth, BaseParcel.MaximumDepth);

                InputCollector depthCollector5 = new InputCollector("depth (cm)");
                int depth5 = depthCollector.ReadInteger(BaseParcel.MinimumDepth, BaseParcel.MaximumDepth);

                InputCollector depthCollector6 = new InputCollector("depth (cm)");
                int depth6 = depthCollector.ReadInteger(BaseParcel.MinimumDepth, BaseParcel.MaximumDepth);

                InputCollector depthCollector7 = new InputCollector("depth (cm)");
                int depth7 = depthCollector.ReadInteger(BaseParcel.MinimumDepth, BaseParcel.MaximumDepth);

                InputCollector depthCollector8 = new InputCollector("depth (cm)");
                int depth8 = depthCollector.ReadInteger(BaseParcel.MinimumDepth, BaseParcel.MaximumDepth);

                InputCollector depthCollector9 = new InputCollector("depth (cm)");
                int depth9 = depthCollector.ReadInteger(BaseParcel.MinimumDepth, BaseParcel.MaximumDepth);
                #endregion

                var x = 7;

                /* Attempt to build a parcel from the provided metrics, an oversized parcel
                 * will raise an exception and the user notified with an error. */
                try
                {
                    BaseParcel parcel = ParcelFactory.CreateParcel(weight, height, width, depth);
                    Console.WriteLine();
                    RenderCost(parcel.CalculateCost());
                }
                catch (RejectedParcelException ex)
                {
                    RenderRejectedParcel((Exception)ex);
                }

                cycle = RenderContinue();
            }
        }

        #region - Methods for rendering blocks of text on the screen - 
        public static void RenderRejectedParcel(Exception ex)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The parcel is oversized and has been rejected.");
            Console.ResetColor();

        }

        public static bool RenderContinue()
        {
            Console.WriteLine();

            InputCollector yesNo = new InputCollector("Would you like to calculate the cost for another parcel?");

            return yesNo.ReadYesNo();
        }

        public static void RenderCategory(string category)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Parcel category: {0}", category);
            Console.ResetColor();
        }

        public static void RenderCost(decimal cost)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Price for postage is: ${0}", cost);
            Console.ResetColor();
        }

        public static void RenderWelcomePage()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the CodeChallenge");
            Console.WriteLine(@"    ____             __                      ______      __           __      __            ");
            Console.WriteLine(@"   / __ \____  _____/ /_____ _____ ____     / ____/___ _/ /______  __/ /___ _/ /_____  _____");
            Console.WriteLine(@"  / /_/ / __ \/ ___/ __/ __ `/ __ `/ _ \   / /   / __ `/ / ___/ / / / / __ `/ __/ __ \/ ___/");
            Console.WriteLine(@" / ____/ /_/ (__  ) /_/ /_/ / /_/ /  __/  / /___/ /_/ / / /__/ /_/ / / /_/ / /_/ /_/ / /    ");
            Console.WriteLine(@"/_/    \____/____/\__/\__,_/\__, /\___/   \____/\__,_/_/\___/\__,_/_/\__,_/\__/\____/_/     ");
            Console.WriteLine(@"                           /____/                                                           ");
            Console.WriteLine("");
            Console.WriteLine("");
        }
        #endregion
    }
}
