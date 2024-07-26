using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

namespace functionsBasics
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }

        [Function("WelcomeName")]
        public async Task <IActionResult> Welcome([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string name = null;
            string surname = null;
            string age = null;
            name = req.Query["name"];
            surname = req.Query["surname"];
            age = req.Query["Age"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConverter, DeserializeObject(requestBody);
            //name = name ?? data?.Name;
            //surname = surname ?? data?.Surname;

            string responseMessage;
            if((name == null) || (surname == null) || (age == null))
            {
                responseMessage = "Please enter in the name and surname";
            }
            
            else
            {
                responseMessage = $"Hello,{name} {surname} and your age {age} This trigger executed successfully";
            }

            return new OkObjectResult(responseMessage);
        }


        [Function("GetPersonInfo")]
        public IActionResult GetPersonInfo([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var people = new List<Person>
    {
        new Person { ID = 1, Name = "Bob", Surname = "Smith", Age = 50, BankBalance = 2.5, FamilyLeft = "90" },
        new Person { ID = 2, Name = "John", Surname = "Smith", Age = 12, BankBalance = 1000000, FamilyLeft = "90" },
        new Person { ID = 3, Name = "Sara", Surname = "Smith", Age = 2, BankBalance = 8555, FamilyLeft = "90" },
        new Person { ID = 4, Name = "Pop", Surname = "Luke", Age = 75, BankBalance = 900000, FamilyLeft = "2" },
        new Person { ID = 5, Name = "James", Surname = "Luke", Age = 80, BankBalance = 8, FamilyLeft = "2" },
        new Person { ID = 6, Name = "Ock", Surname = "No idea", Age = 99, BankBalance = 99000000, FamilyLeft = "1 dog" }
    };

            string name = req.Query["name"];
            string surname = req.Query["surname"];
            int age;
            int.TryParse(req.Query["age"], out age);

            var person = people.FirstOrDefault(p => p.Name == name && p.Surname == surname && p.Age == age);

            string responseMessage;
            if (person == null)
            {
                responseMessage = "Person not found. Please provide valid details.";
            }
            else
            {
                responseMessage = $"Person found: {person.Name} {person.Surname}, Age: {person.Age}, Bank Balance: {person.BankBalance}, Family Left: {person.FamilyLeft}.";
            }

            return new OkObjectResult(responseMessage);
        }
    }
}
