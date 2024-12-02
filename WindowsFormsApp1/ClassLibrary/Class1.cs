using System;
using System.Collections.Generic;
using Npgsql;

namespace ClassLibrary
{
    public class DatabaseService
    {
        private readonly string _connectionString =
            "Host=povt-cluster.tstu.tver.ru;" +
            "Port=5432;" +
            "Database=autostoredb;" +
            "Username=mpi;" +
            "Password=135a1;";

        public string ConnectionString => _connectionString;
        // Добавление автомобиля
        public void AddCar(Car car)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("INSERT INTO carsA (brand, engine_capacity, release_date) VALUES (@brand, @engine_capacity, @release_date)", connection))
                {
                    command.Parameters.AddWithValue("brand", car.Brand);
                    command.Parameters.AddWithValue("engine_capacity", car.EngineCapacity);
                    command.Parameters.AddWithValue("release_date", car.ReleaseDate);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Добавление клиента
        public void AddClient(Client client)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("INSERT INTO clientss (name, contact_info) VALUES (@name, @contact_info)", connection))
                {
                    command.Parameters.AddWithValue("name", client.Name);
                    command.Parameters.AddWithValue("contact_info", client.ContactInfo);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Получение ID клиента
        public int GetClientIdByName(string name)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT clientid FROM clientss WHERE name = @name", connection))
                {
                    command.Parameters.AddWithValue("name", name);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return Convert.ToInt32(reader["clientid"]);
                        }
                    }
                }
            }
            throw new Exception("Клиент не найден.");
        }

        // Добавление заявки клиента
        public void AddClientRequest(int clientId, string desiredBrand, double desiredEngineCapacity)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    "INSERT INTO client_requestss (clientid, desired_brand, desired_engine_capacity) VALUES (@clientid, @desired_brand, @desired_engine_capacity)",
                    connection))
                {
                    command.Parameters.AddWithValue("clientid", clientId);
                    command.Parameters.AddWithValue("desired_brand", desiredBrand);
                    command.Parameters.AddWithValue("desired_engine_capacity", desiredEngineCapacity);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Поиск автомобилей
        public List<Car> SearchCars(string brand, double engineCapacity)
        {
            var cars = new List<Car>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT * FROM carsA WHERE brand = @brand AND engine_capacity = @engine_capacity", connection))
                {
                    command.Parameters.AddWithValue("brand", brand);
                    command.Parameters.AddWithValue("engine_capacity", engineCapacity);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cars.Add(new Car(
                                reader["brand"].ToString(),
                                Convert.ToDouble(reader["engine_capacity"]),
                                Convert.ToDateTime(reader["release_date"])
                            ));
                        }
                    }
                }
            }
            return cars;
        }
    }

    public class Car
    {
        public string Brand { get; private set; }
        public double EngineCapacity { get; private set; }
        public DateTime ReleaseDate { get; private set; }

        public Car(string brand, double engineCapacity, DateTime releaseDate)
        {
            Brand = brand;
            EngineCapacity = engineCapacity;
            ReleaseDate = releaseDate;
        }
    }

    public class Client
    {
        public string Name { get; private set; }
        public string ContactInfo { get; private set; }

        public Client(string name, string contactInfo)
        {
            Name = name;
            ContactInfo = contactInfo;
        }
    }
}
