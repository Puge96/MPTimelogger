using Application.Shared;
using System.Reflection;
using System.Runtime.Loader;
using Xunit;

namespace API.Test
{
	public class APITests
	{
		[Fact]
		public void ModelResults_ShouldInheritBaseJsonResult()
		{
			// Load application assembly
			var applicationAssembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("Application"));

			// Get all classes ending with "ModelResult"
			var modelResultClasses = applicationAssembly.GetTypes()
				.Where(type => type.IsClass && type.Name.EndsWith("ModelResult"));

			foreach (var modelResultClass in modelResultClasses)
			{
				// Assert that all classes inherits BaseJsonResult class
				Assert.True(typeof(BaseJsonResult).IsAssignableFrom(modelResultClass), $"{modelResultClass.Name} must inherit from {nameof(BaseJsonResult)}.");
			}
		}

		[Fact]
		public void Controllers_ShouldReturnModelResult()
		{
			// Load API assembly
			var applicationAssembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName("API"));

			// Get all classes ending with "Controller" - Could be improved by finding classes that inherits Controller & ControllerBase
			var controllers = applicationAssembly.GetTypes()
				.Where(Type => Type.IsClass && Type.Name.EndsWith("Controller"));

			foreach (var controller in controllers)
			{
				// Get methods that are declared directly in the controller class
				var methods = controller.GetMethods().Where(x => x.DeclaringType == controller);
				foreach (var method in methods)
				{
					var returnType = method.ReturnType.ToString();
					// Assert that all methods returns a class that contains ModelResult or BaseJsonResult
					Assert.True(returnType.Contains("ModelResult") || returnType.Contains("BaseJsonResult"), $"{method.Name} must return a class ending with ModelResult or return a BaseJsonResult");

				}
			}
		}
	}
}