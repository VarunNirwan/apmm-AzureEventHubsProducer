using AzureEventHubs.Model;
using AzureEventHubs.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AzureEventHubs.UnitTests
{
    [TestFixture]
    public class EventHubsRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task SendAsync()
        {
            //Assign
            var emp = new Employees.Employee() //Assigning properties to Employee
            {
                Address = "Bhubaneswar",
                Age = 2
            };
            var names = new Employees.Names() //Assigning properties to Names
            {
                FirstName = "Kumar Aditya",
                LastName = "Barik"
            };
            var employee = new Employees() //Assembling Employee + Names and Assigning properties to Employees
            {
                EmpDetails = emp,
                EmpName = names
            };
            var mock = new Mock<ILogger<EventHubsRepository>>(); //Mocking ILogger
            var producer = new EventHubsRepository(mock.Object); //Injecting mocked ILogger object into the constructor of EventHubsRepository.

            try
            {
                //Act
                await producer.SendAsync(employee);

                //Assert
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [Test]
        public void HelpText()
        {
            //Assign
            var mock = new Mock<ILogger<EventHubsRepository>>();
            var producer = new EventHubsRepository(mock.Object);
            string helpXML = @"Inorder to POST your data to the API, use below format of XML in Request Body.
                                
                                 <?xml version=""1.0"" encoding=""UTF-8""?>
                                 <Employees>
                                    <EmpDetails>
	                                    <Address>Bhubaneswar</Address>
	                                    <Age>32</Age>
                                    </EmpDetails>
                                    <EmpName>
	                                    <FirstName>Kumar Debasis</FirstName>
	                                    <LastName>Barik</LastName>
                                    </EmpName>
                                 </Employees>";

            //Act
            var result = producer.HelpText();

            //Assert
            Assert.AreEqual(helpXML, result);
        }
    }
}