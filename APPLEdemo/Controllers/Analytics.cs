
using Newtonsoft.Json;

namespace APPLEdemo.Controllers
{
    public class Analytics
    {
       // Define the path to the JSON data file
        private readonly string jsondatafilepath = Path.GetDirectoryName(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName) + @"\data\Data.json";

        // Define a list of employees to store the deserialized data
        private List<Employee> employees;

        public Analytics()
        {
            // Load the data from the file and deserialize it
            string jsondatafromfile = File.ReadAllText(jsondatafilepath);
            employees = JsonConvert.DeserializeObject<List<Employee>>(jsondatafromfile);

            // Sort the toppings for each employee
            foreach (Employee e in employees)
            {
                Array.Sort(e.Toppings);
            }
        }

        public bool Compute()
        {
            // Get the statistical data desired from the JSON data

            // A. The department that most likes pineapple as a topping on their pizza
            Console.WriteLine("Department that most likes pineapple: {0}", GetMostPopularDepartment("Pineapple"));
            Console.WriteLine("");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("");

            // B. The department that most likes pepperoni and onions as toppings on their pizza
            Console.WriteLine("Department that most likes pepperoni and onions: {0}", GetMostPopularDepartment("Pepperoni", "Onions"));
            Console.WriteLine("");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("");

            // C. The number of employees that like anchovies as a topping on their pizza
            Console.WriteLine("Employees that like anchovies: {0}", GetEmployeeCount("Anchovies").ToString());
            Console.WriteLine("");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("");

            // D. The number of pizzas needed to feed the engineering department regardless of preference

            Console.WriteLine("Pizzas needed to feed the engineering department regardless of preference: {0}", GetPizzaCount("Engineering").ToString());
            Console.WriteLine("");
            Console.WriteLine("-------------------------------------------------");
            Console.WriteLine("");

            // E. The most popular combination of toppings for pizza by department
            GetMostPopularToppings();

            return true;
        }

        // A helper method to get the department that most likes a given set of toppings
        private string GetMostPopularDepartment(params string[] toppings)
        {
            // Filter the employees by the toppings and group them by department
            var resultset = employees.Where(e => toppings.All(t => e.Toppings.Contains(t))).GroupBy(e => e.Department).OrderByDescending(g => g.Count()).FirstOrDefault();

            // Return the department name or an empty string if no match
            return resultset?.Key ?? "";
        }

        // A helper method to get the number of employees that like a given set of toppings
        private int GetEmployeeCount(params string[] toppings)
        {
            // Filter the employees by the toppings and count them
            return employees.Count(e => toppings.All(t => e.Toppings.Contains(t)));
        }

        // A helper method to get the number of pizzas needed to feed a given department
        private int GetPizzaCount(string department)
        {
            // Filter the employees by the department and divide by 4 (assuming one pizza feeds 4 people)
            return employees.Count(e => e.Department.Equals(department)) / 4;
        }

        // A helper method to get the most popular combination of toppings for pizza by department
        private void GetMostPopularToppings()
        {
            // Group the employees by department
            var departments = employees.GroupBy(e => e.Department);

            // For each department, group the employees by their toppings and get the most frequent one
            foreach (var department in departments)
            {
                var mostPopularToppings = department.GroupBy(e => string.Join(", ", e.Toppings)).OrderByDescending(g => g.Count()).FirstOrDefault();
                Console.WriteLine("Most popular toppings for {0}: {1} : liked by {2} employees", department.Key, mostPopularToppings.Key, mostPopularToppings.Count());
            }
        }

        private class Employee
        {
            //  0.  This class represents the internal data structure used to work with the JSON data to be analyzed.
            public required string Name { get; set; }
            public required string Department { get; set; }
            public required string[] Toppings { get; set; }
        }

    }
}
