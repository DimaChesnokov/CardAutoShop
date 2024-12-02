using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLibrary;
using System.Reflection;

namespace TestProject
{


    [TestClass]
    public class DatabaseServiceTests
    {
        private readonly DatabaseService _databaseService;

        public DatabaseServiceTests()
        {
            _databaseService = new DatabaseService();
        }


        //ѕровер€ет, что метод AddCar корректно добавл€ет автомобиль в базу данных.
        [TestMethod]
        public void Test_AddCar_ShouldInsertCarIntoDatabase()
        {
            // Arrange
            var car = new Car("TestBrand", 1.6, DateTime.Now);

            // Act
            _databaseService.AddCar(car);

            // Assert
            var cars = _databaseService.SearchCars("TestBrand", 1.6);
            Assert.IsTrue(cars.Count > 0, "Car was not added to the database.");
        }


        //ѕровер€ет, что метод AddClient добавл€ет клиента в базу данных.
        [TestMethod]
        public void Test_AddClient_ShouldInsertClientIntoDatabase()
        {
            // Arrange
            var client = new Client("TestClient", "test@example.com");

            // Act
            _databaseService.AddClient(client);

            // Assert
            int clientId = _databaseService.GetClientIdByName("TestClient");
            Assert.IsTrue(clientId > 0, "Client was not added to the database.");
        }

        //ѕровер€ет, что метод AddClientRequest добавл€ет за€вку клиента в таблицу client_requestss.
        [TestMethod]
        public void Test_AddClientRequest_ShouldInsertRequestIntoDatabase()
        {
            // Arrange
            var client = new Client("RequestClient", "request@example.com");
            _databaseService.AddClient(client);
            int clientId = _databaseService.GetClientIdByName("RequestClient");

            // Act
            _databaseService.AddClientRequest(clientId, "TestBrand", 2.0);

            // Assert
            using (var connection = new Npgsql.NpgsqlConnection(_databaseService.ConnectionString))
            {
                connection.Open();
                using (var command = new Npgsql.NpgsqlCommand("SELECT COUNT(*) FROM client_requestss WHERE clientid = @clientid", connection))
                {
                    command.Parameters.AddWithValue("clientid", clientId);  
                    var result = Convert.ToInt32(command.ExecuteScalar());
                    Assert.IsTrue(result > 0, "Client request was not added to the database.");
                }
            }
        }

        //ѕровер€ет, что метод SearchCars возвращает правильные данные.
        [TestMethod]
        public void Test_SearchCars_ShouldReturnCorrectCars()
        {
            // Arrange
            var car = new Car("SearchBrand", 2.5, DateTime.Now);
            _databaseService.AddCar(car);

            // Act
            var cars = _databaseService.SearchCars("SearchBrand", 2.5);

            // Assert
            Assert.IsTrue(cars.Count > 0, "SearchCars did not return any results.");
            Assert.AreEqual("SearchBrand", cars[0].Brand, "Returned car brand does not match.");
        }





        [TestMethod]
        public void Test_AddCar_ShouldInsertCarIntoDatabaseTDD()
        {
            // Arrange
            var car = new Car("Toyota", 2.5, new DateTime(2020, 12, 15));

            // Act
            _databaseService.AddCar(car);

            // Assert
            var cars = _databaseService.SearchCars("Toyota", 2.5);
            Assert.IsTrue(cars.Count > 0, "Car was not added to the database.");
        }

        [TestMethod]
        public void Test_AddClient_ShouldInsertClientIntoDatabaseTDD()
        {
            // Arrange
            var client = new Client("John Doe", "john.doe@example.com");

            // Act
            _databaseService.AddClient(client);

            // Assert
            int clientId = _databaseService.GetClientIdByName("John Doe");
            Assert.IsTrue(clientId > 0, "Client was not added to the database.");
        }


        [TestMethod]
        public void Test_AddClientRequest_ShouldInsertRequestIntoDatabaseTDD()
        {
            // Arrange
            var client = new Client("Jane Doe", "jane.doe@example.com");
            _databaseService.AddClient(client);
            int clientId = _databaseService.GetClientIdByName("Jane Doe");

            // Act
            _databaseService.AddClientRequest(clientId, "Toyota", 2.5);

            // Assert
            using (var connection = new Npgsql.NpgsqlConnection(_databaseService.ConnectionString))
            {
                connection.Open();
                using (var command = new Npgsql.NpgsqlCommand("SELECT COUNT(*) FROM client_requestss WHERE clientid = @clientid", connection))
                {
                    command.Parameters.AddWithValue("clientid", clientId);
                    var result = Convert.ToInt32(command.ExecuteScalar());
                    Assert.IsTrue(result > 0, "Client request was not added to the database.");
                }
            }
        }


        [TestMethod]
        public void Test_SearchCars_ShouldReturnCorrectCarsTDD()
        {
            // Arrange
            var car = new Car("Honda", 1.8, new DateTime(2018, 6, 20));
            _databaseService.AddCar(car);

            // Act
            var cars = _databaseService.SearchCars("Honda", 1.8);

            // Assert
            Assert.IsTrue(cars.Count > 0, "SearchCars did not return any results.");
            Assert.AreEqual("Honda", cars[0].Brand, "Returned car brand does not match.");
        }




    }





}