using Xunit;
using CodeChallenge.Cli;

namespace CodeChallenge.Cli.Tests
{
    public class InputCollectorTests
	{
		[Theory]
		[InlineData(5)]
		[InlineData(100)]
		[InlineData(0)]
		public void CanValidatePositiveInteger(string value)
		{
			InputCollector target = new InputCollector("default");
			Assert.True(target.ValidateInteger(value, 0, 100));
		}

		[Theory]
		[InlineData("five")]
		[InlineData(-10)]
		[InlineData(2.5)]
		[InlineData(100000)]
		public void CanRejectNonPositiveInteger(string value)
		{
			InputCollector target = new InputCollector("default");
			Assert.False(target.ValidateInteger(value, 0, 100));
		}

		[Theory]
		[InlineData("Y")]
		[InlineData("y")]
		[InlineData("N")]
		[InlineData("n")]
		public void CanValidateYesNo(string value)
		{
			InputCollector target = new InputCollector("default");
			Assert.True(target.ValidateYesNo(value));
		}

		[Theory]
		[InlineData("asdsad")]
		[InlineData("")]
		[InlineData(123)]
		[InlineData(@"/r/n")]
		public void CanRejectInvalidYesNo(string value)
		{
			InputCollector target = new InputCollector("default");
			Assert.False(target.ValidateYesNo(value));
		}
	}
}
