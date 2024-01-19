using Xunit;

namespace Application.Test
{
	[CollectionDefinition("YourCollection")]
	public class StartupFixture : ICollectionFixture<Startup>
	{
		// This class doesn't need any code because its purpose is to run the startup logic
	}
}