using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClassLibrary;
using Npgsql;



namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
       // private AutoStore autoStore = new AutoStore();
        private readonly DatabaseService _databaseService = new DatabaseService();

        private readonly string _connectionString =
           "Host=povt-cluster.tstu.tver.ru;" +
           "Port=5432;" +
           "Database=autostoredb;" +
           "Username=mpi;" +
           "Password=135a1;";
        public Form1()
        {
            InitializeComponent();
        }




        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void txtBrand_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtEngineCapacity_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtReleaseDate_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            try
            {
                string brand = txtBrand.Text;
                if (string.IsNullOrWhiteSpace(brand) || string.IsNullOrWhiteSpace(txtEngineCapacity.Text) || string.IsNullOrWhiteSpace(txtReleaseDate.Text))
                {
                    MessageBox.Show("Заполните все поля перед добавлением автомобиля.");
                    return;
                }

                double engineCapacity = double.Parse(txtEngineCapacity.Text);
                DateTime releaseDate = DateTime.Parse(txtReleaseDate.Text);

                var car = new Car(brand, engineCapacity, releaseDate);
                _databaseService.AddCar(car);

                MessageBox.Show("Автомобиль успешно добавлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void btnAddClient_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtClientName.Text;
                string contactInfo = txtContactInfo.Text;

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(contactInfo))
                {
                    MessageBox.Show("Заполните все поля перед добавлением клиента.");
                    return;
                }

                var client = new Client(name, contactInfo);
                _databaseService.AddClient(client);

                MessageBox.Show("Клиент успешно добавлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void btnAddClientRequest_Click(object sender, EventArgs e)
        {
            try
            {
                string clientName = txtClientName.Text;
                string desiredBrand = txtDesiredBrand.Text;

                if (string.IsNullOrWhiteSpace(clientName) || string.IsNullOrWhiteSpace(desiredBrand) ||
                    !double.TryParse(txtDesiredEngineCapacity.Text, out double desiredEngineCapacity))
                {
                    MessageBox.Show("Заполните все поля корректно.");
                    return;
                }

                int clientId = _databaseService.GetClientIdByName(clientName);
                _databaseService.AddClientRequest(clientId, desiredBrand, desiredEngineCapacity);

                MessageBox.Show("Заявка успешно добавлена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string brand = txtSearchBrand.Text;
                double engineCapacity = double.Parse(txtSearchEngineCapacity.Text);

                var cars = _databaseService.SearchCars(brand, engineCapacity);

                lstResults.Items.Clear();
                foreach (var car in cars)
                {
                    lstResults.Items.Add($"{car.Brand}, {car.EngineCapacity}L, {car.ReleaseDate.ToShortDateString()}");
                }

                if (cars.Count == 0)
                {
                    MessageBox.Show("Подходящих автомобилей не найдено.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }




        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string clientName = txtClientName.Text;
                string desiredBrand = txtDesiredBrand.Text;

                if (string.IsNullOrWhiteSpace(clientName) || string.IsNullOrWhiteSpace(desiredBrand) ||
                    !double.TryParse(txtDesiredEngineCapacity.Text, out double desiredEngineCapacity))
                {
                    MessageBox.Show("Заполните все поля корректно.");
                    return;
                }

                int clientId = _databaseService.GetClientIdByName(clientName);
                _databaseService.AddClientRequest(clientId, desiredBrand, desiredEngineCapacity);

                MessageBox.Show("Заявка успешно добавлена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }




        private void LoadDataIntoGrid(string query)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = new NpgsqlCommand(query, connection))
                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView1.DataSource = table;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM clientss";
            LoadDataIntoGrid(query);
        }

        private void машиныToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM carsa";
            LoadDataIntoGrid(query);
        }

        private void заявкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM client_requestss";
            LoadDataIntoGrid(query);
        }
    }
}
