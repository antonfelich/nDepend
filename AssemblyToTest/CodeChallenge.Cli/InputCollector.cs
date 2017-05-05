using System;
using System.Text.RegularExpressions;

namespace CodeChallenge.Cli
{
    public class InputCollector
    {
		private readonly string _message;

		public InputCollector(string message)
		{
			_message = message;
		}

		/// <summary>
		/// Prompts the user with _message then asks them to key in either Y or N (case insensitive)
		/// </summary>
		public bool ReadYesNo()
	    {
		    string input = string.Empty;

		    bool valid = false;
		    while (!valid)
		    {
			    Console.ForegroundColor = ConsoleColor.Yellow;
			    Console.Write("{0} Y/N [Y]", _message);
			    input = Console.ReadLine();

				/* Default the user input to Y if nothing is keyed in */
				if (input == string.Empty) input = "Y";

				valid = ValidateYesNo(input);

			    Console.ResetColor();
		    }

		    return input.ToUpper() == "Y" ? true : false;
	    }


		/// <summary>
		/// Ensures the user input contains either y, n, Y or N
		/// <param name="input">Console input from user</param>
		/// </summary>
		public bool ValidateYesNo(string input)
	    {
			/* Check the input string for only Y y N n responses */
		    if (Regex.IsMatch(input, "^(?i:[YN]{1})"))
		    {
			    return true;
		    }
		    else
		    {
				RenderCollectionError("You did not make a valid selection. Please enter either Y or N.");
			    return false;
		    }
	    }

		/// <summary>
		/// Prompts the user based on _message and expects them to key in an integer based on the range provided.
		/// </summary>
		/// <param name="min">Minimum allowable value for input</param>
		/// <param name="max">Maximum allowable value for input</param>
		public int ReadInteger(int min, int max)
		{
			string input = string.Empty;

			/* Keep prompting the user until we get a valid input that matches the defined ranges*/
			// TODO: Allow for CTRL-X to bail out of the process so they don't get stuck in a loop
			bool valid = false;
			while (!valid)
			{
				Console.Write("Please enter the {0} from {1} to {2}: ", _message, min, max);
				input = Console.ReadLine();
				valid = ValidateInteger(input, min, max);
			}

			return int.Parse(input);
		}

		/// <summary>
		/// Validates that input is an integer and that it fits between the ranges provided
		/// </summary>
		/// <param name="input">Console input from user</param>
		/// <param name="min">Minimum allowable value for input</param>
		/// <param name="max">Maximum allowable value for input</param>
		public bool ValidateInteger(string input, int min, int max)
		{
			int value;
			
			/* If its not even an integer then auto-fail */
			if (int.TryParse(input, out value) == false)
			{
				RenderCollectionError("The value entered was not a number.");
				return false;
			}

			/* Make sure it fits within the required ranges */
			if (value < min || value > max)
			{
				RenderCollectionError(string.Format("The number entered must be between {0} and {1}.", min, max));
				return false;
			}

			/* All good */
			return true;
		}

		/// <summary>
		/// Displays an error message to the user in red text
		/// </summary>
		public void RenderCollectionError(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.ResetColor();
		}
    }
}
