using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practic.MVVM.Model
{
    public class DataBase
    {
        private readonly string _connectionString = "database.db";
        private readonly string _users = "Users";
        private readonly string _tickets = "Tickets";
        public bool CreateDBUsers()
        {
            using (var connection = new SQLiteConnection($"Data Source={_connectionString}"))
            {
                connection.Open();
                SQLiteCommand command = new();
                command.Connection = connection;
                command.CommandText = $"CREATE TABLE IF NOT EXISTS {_users}(Id INTEGER NOT NULL PRIMARY KEY, firstName TEXT, " +
                                      "secondName TEXT, login TEXT, password TEXT, isAdmin INTEGER)";
                try
                {
                    int _ = command.ExecuteNonQuery();
                    return true;
                }
                catch (SQLiteException e)
                {
                    return false;
                }
            }
        }

        public List<User> GetDBUsers()
        {
            List<User> result = new();

            using (var connection = new SQLiteConnection($"Data Source={_connectionString}"))
            {
                connection.Open();

                SQLiteCommand command = new($"SELECT * FROM {_users};", connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user = new();

                        user.id = Convert.ToInt32(reader["Id"]);
                        user.firstName = reader["firstName"].ToString();
                        user.secondName = reader["secondName"].ToString();
                        user.login = reader["login"].ToString();
                        user.password = reader["password"].ToString();
                        user.isAdmin = Convert.ToBoolean(reader["isAdmin"]);

                        result.Add(user);
                    }
                }
            }

            return result;
        }
        public async Task<bool> AddDBUsersAsync(User user)
        {
            using (var connection = new SQLiteConnection($"Data Source={_connectionString}"))
            {
                await connection.OpenAsync();

                SQLiteCommand command = new();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO {_users} (Id, firstName, secondName, login, password, isAdmin) " +
                                      $"VALUES (@Id, @firstName, @secondName, @login, @password, @isAdmin)";
                command.Parameters.AddWithValue("@Id", GetLastNewId(_users));
                command.Parameters.AddWithValue("@firstName", user.firstName);
                command.Parameters.AddWithValue("@secondName", user.secondName);
                command.Parameters.AddWithValue("@login", user.login);
                command.Parameters.AddWithValue("@password", user.password);
                command.Parameters.AddWithValue("@isAdmin", user.isAdmin ? 1 : 0);
                try
                {
                    await command.ExecuteNonQueryAsync();
                    return true;
                }
                catch (SQLiteException e)
                {
                    return false;
                }
            }
        }
        public async Task<bool> DeleteAllRowsDb(string _dbname)
        {
            using (var connection = new SQLiteConnection($"Data Source={_connectionString}"))
            {
                await connection.OpenAsync();

                SQLiteCommand command = new();
                command.Connection = connection;
                command.CommandText = $"DELETE FROM {_dbname};";
                try
                {
                    await command.ExecuteNonQueryAsync();
                    return true;
                }
                catch (SQLiteException e)
                {
                    return false;
                }
            }
        }

        public bool CreateDBTickets()
        {
            using (var connection = new SQLiteConnection($"Data Source={_connectionString}"))
            {
                connection.Open();
                SQLiteCommand command = new();
                command.Connection = connection;
                command.CommandText = $"CREATE TABLE IF NOT EXISTS {_tickets}(Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, dateOfCreation TEXT, " +
                                      "CauseBy TEXT, TypeOfCause TEXT, Status TEXT, Client_Id INTEGER, FOREIGN KEY (Client_Id) REFERENCES Users(Id))";
                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SQLiteException e)
                {
                    return false;
                }
            }
        }
        public List<Ticket> GetDBTickets()
        {
            List<Ticket> result = new();

            using (var connection = new SQLiteConnection($"Data Source={_connectionString}"))
            {
                connection.Open();

                SQLiteCommand command = new SQLiteCommand($"SELECT * FROM {_tickets};", connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Ticket answer = new();
                        answer.id = Convert.ToInt32(reader["Id"]);
                        answer.client_id = Convert.ToInt32(reader["Client_Id"]);
                        answer.date = reader["dateOfCreation"].ToString();
                        answer.causeby = reader["CauseBy"].ToString();
                        answer.typeofcause = reader["TypeOfCause"].ToString();
                        answer.status = reader["Status"].ToString();

                        result.Add(answer);
                    }
                }
            }

            return result;
        }
        public async Task<bool> AddDBTicketsAsync(Ticket answer)
        {
            using (var connection = new SQLiteConnection($"Data Source={_connectionString}"))
            {
                await connection.OpenAsync();
                SQLiteCommand command = new();
                command.Connection = connection;
                command.CommandText = $"INSERT INTO Answers (dateOfCreation, CauseBy, TypeOfCause, Status, Client_Id) " +
                                      $"VALUES (@dateOfCreation, @CauseBy, @TypeOfCause, @Status, @Client_Id";
                command.Parameters.AddWithValue("@dateOfCreation", answer.date);
                command.Parameters.AddWithValue("@CauseBy", answer.causeby);
                command.Parameters.AddWithValue("@TypeOfCause", answer.typeofcause);
                command.Parameters.AddWithValue("@Status", answer.status);
                command.Parameters.AddWithValue("@Client_Id", answer.client_id);
                try
                {
                    await command.ExecuteNonQueryAsync();
                    return true;
                }
                catch (SQLiteException e)
                {
                    return false;
                }
            }
        }

        public bool UpdateDBTickets(Ticket answer)
        {
            using (var connection = new SQLiteConnection($"Data Source={_connectionString}"))
            {
                connection.Open();

                SQLiteCommand command = new(connection);
                command.CommandText = $"UPDATE {_tickets} SET dateOfCreation = @dateOfCreation, CauseBy = @CauseBy, TypeOfCause = @TypeOfCause, Status = @Status, Client_Id = @Client_Id WHERE Id = @Id";

                command.Parameters.AddWithValue("@Id", answer.id);
                command.Parameters.AddWithValue("@dateOfCreation", answer.date);
                command.Parameters.AddWithValue("@CauseBy", answer.causeby);
                command.Parameters.AddWithValue("@TypeOfCause", answer.typeofcause);
                command.Parameters.AddWithValue("@Status", answer.status);
                command.Parameters.AddWithValue("@Client_Id", answer.client_id);

                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SQLiteException e)
                {
                    return false;
                }
            }
        }

        public bool DeleteRowDB(string _dbName, int id)
        {
            using (var connection = new SQLiteConnection($"Data Source={_connectionString}"))
            {
                connection.Open();

                SQLiteCommand command = new(connection);
                command.CommandText = $"DELETE FROM {_dbName} WHERE Id = @Id";

                command.Parameters.AddWithValue("@Id", id);

                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (SQLiteException e)
                {
                    return false;
                }
            }
        }
        public bool IsLoginUnique(string login)
        {
            string connectionString = $"Data Source={_connectionString}";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT COUNT(*) FROM Users WHERE login = @Login";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Login", login);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count == 0;
                }
            }
        }
        public int GetLastNewId(string _dbName)
        {
            using (var connection = new SQLiteConnection($"Data Source={_connectionString}"))
            {
                connection.Open();

                SQLiteCommand command = new();
                command.Connection = connection;
                command.CommandText = $"SELECT MAX(Id) FROM {_dbName};";
                try
                {
                    object _type = command.ExecuteScalar();
                    int _lastId = int.TryParse(command.ExecuteScalar().ToString(), out _) ? Convert.ToInt32(_type) : 0;
                    _lastId += 1;
                    return _lastId;
                }
                catch (SQLiteException e)
                {
                    return -1;
                }
            }

        }
    }
}
