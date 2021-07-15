using AzureEventHubs.Model;
using AzureEventHubs.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AzureEventHubs.UnitTests
{
    [TestFixture]
    public class UserRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task SendAsync()
        {
            //Assign
            var user = new User() //Assigning properties to User
            {
                id=100,
                name="Debasis",
                pwd="pass!*%#@word"
            };
            var mock = new Mock<ILogger<UserRepository>>(); //Mocking ILogger
            var producer = new UserRepository(mock.Object); //Injecting mocked ILogger object into the constructor of UserRepository.

            try
            {
                //Act
                await producer.SendAsync(user);

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