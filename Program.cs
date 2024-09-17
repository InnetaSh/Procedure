
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Xml.Linq;

namespace ConsoleApp54
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            //string CommandText = "CREATE DATABASE Users";
            //await ExecuteCommandBase(CommandText);



            //string sqlQuery = "CREATE TABLE Users (UserId  INT PRIMARY KEY IDENTITY, UserName  NVARCHAR(40) NOT NULL,Email NVARCHAR(100) CHECK (Email != '' AND Email LIKE '%@%.%') UNIQUE NOT NULL)";
            //await ExecuteCommand(sqlQuery);


            string creatProc = @"
                 CREATE PROCEDURE AddUserWithOutput
                 @UserName NVARCHAR(50),
                 @Email NVARCHAR(100),
                 @NewUserId INT OUTPUT
                 AS 
                 BEGIN 
                 INSERT INTO Users(UserName, Email)
                 VALUES (@UserName, @Email)
                 SET @NewUserId = SCOPE_IDENTITY()
                 END
                    ";


            string name = "Sam";
            string email = "sam1@gmail.com";
            //await ExecuteCommandCreateProc(creatProc);
            //await ExecuteCommand_ExecutionPROC_AddUserWithOutput(name, email);
            name = "Bob";
            email = "bob1@gmail.com";
            //await ExecuteCommand_ExecutionPROC_AddUserWithOutput(name, email);

            name = "Tom";
            email = "tom1@gmail.com";
            //await ExecuteCommand_ExecutionPROC_AddUserWithOutput(name, email);

            name = "Ann";
            email = "ann1@gmail.com";
            //await ExecuteCommand_ExecutionPROC_AddUserWithOutput(name, email);

            //------------------------------------------------------------------------------------
            //string deleteProc = "DROP PROCEDURE AddUserWithOutput";
            //await ExecuteCommand(deleteProc);
            //------------------------------------------------------------------------------------


            //Создайте хранимую процедуру AddUserWithOutput, которая добавляет нового пользователя в таблицу Users. 
            //Процедура должна принимать параметры @Name и @Email, а также иметь выходной параметр @NewUserId, в который будет возвращаться Id нового пользователя.


            static async Task ExecuteCommand_ExecutionPROC_AddUserWithOutput(string name, string email)
            {
                string connectionString = @"Data Source = DESKTOP-ITRLGSN; Initial Catalog = Users; Trusted_Connection=True; TrustServerCertificate = True";
                string executeProc = "AddUserWithOutput";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(executeProc, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                       
                        command.Parameters.AddWithValue("@UserName", name);
                        command.Parameters.AddWithValue("@Email", email);

                        
                        SqlParameter outputParam = new SqlParameter
                        {
                            ParameterName = "@NewUserId",
                            SqlDbType = SqlDbType.Int, 
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);


                        await command.ExecuteNonQueryAsync();

                        int newUserId = (int)outputParam.Value;
                        Console.WriteLine($"Новый пользователь добавлен с Id: {newUserId}");
                    }
                }
            }



            //-----------------2-------------------------------------
            creatProc = @"
                 CREATE PROCEDURE UpdateUserEmail 
                     @UserId INT,
                     @NewEmail NVARCHAR(100)
                 AS 
                 BEGIN 
                     UPDATE Users
                     SET Email = @NewEmail
                     WHERE UserId = @UserId
                 END
                    ";

            int UserId = 1;
            email = "SAMUEL11@gmail.com";
            //await ExecuteCommandCreateProc(creatProc);
            //await ExecuteCommand_ExecutionPROC_UpdateUserEmail(UserId, email);

            //Создайте хранимую процедуру UpdateUserEmail, которая обновляет адрес электронной почты пользователя по его Id. 
            //Процедура должна принимать параметры @UserId и @NewEmail.

            static async Task ExecuteCommand_ExecutionPROC_UpdateUserEmail(int UserId, string email)
            {
                string connectionString = @"Data Source = DESKTOP-ITRLGSN; Initial Catalog = Users; Trusted_Connection=True; TrustServerCertificate = True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("UpdateUserEmail", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;


                        command.Parameters.AddWithValue("@UserId", UserId);
                        command.Parameters.AddWithValue("@NewEmail", email);


                        await command.ExecuteScalarAsync();
                        Console.WriteLine("Email пользователя обновлен");
                    }
                }
            }


            //-----------------3-------------------------------------
            creatProc = @"
                 CREATE PROCEDURE DeleteUser 
                     @UserId INT
                 AS 
                 BEGIN 
                     DELETE Users
                     WHERE UserId = @UserId
                 END
                    ";

            UserId = 1;
            //await ExecuteCommandCreateProc(creatProc);
            //await ExecuteCommand_ExecutionPROC_DeleteUser(UserId);

            //Создайте хранимую процедуру DeleteUser, которая удаляет пользователя из таблицы по его Id.Процедура должна принимать параметр @UserId.
            static async Task ExecuteCommand_ExecutionPROC_DeleteUser(int UserId)
            {
                string connectionString = @"Data Source = DESKTOP-ITRLGSN; Initial Catalog = Users; Trusted_Connection=True; TrustServerCertificate = True";
                string executeProc = "DeleteUser";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(executeProc, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;


                        command.Parameters.AddWithValue("@UserId", UserId);


                        await command.ExecuteScalarAsync();
                        Console.WriteLine($"Пользователь с Id {UserId} удален");
                    }
                }
            }



            //-----------------4-------------------------------------


            await Menu();

            static async Task Menu()
            {

                Console.WriteLine("Выберите:");
                int action;


                Console.WriteLine("1 - Добавление нового пользователя.");
                Console.WriteLine("2 - Обновление адреса электронной почты существующего пользователя.");
                Console.WriteLine("3 - Удаление пользователя");
                Console.WriteLine("4 - Просмотр списка всех пользователей");

                Console.WriteLine("5 - выход");


                Console.Write("действие - ");
                while (!Int32.TryParse(Console.ReadLine(), out action) || action < 1 || action > 5)
                {
                    Console.WriteLine("Не верный ввод.Введите число:");
                    Console.Write("действие - ");
                }

                try
                {
                    switch (action)
                    {
                        case 1:
                            Console.Write("Введите имя пользователя: ");
                            string name = Console.ReadLine();

                            while (string.IsNullOrWhiteSpace(name))
                            {
                                Console.WriteLine("Имя пользователя не может быть пустым.");
                                Console.Write("Введите имя пользователя: ");
                                name = Console.ReadLine();
                               
                            }


                            Console.Write("Введите адреса электронной почты пользователя: ");
                            string email = Console.ReadLine();


                            while (string.IsNullOrWhiteSpace(email))
                            {
                                Console.WriteLine("Адрес электронной почты не может быть пустым.");
                                Console.Write("Введите адреса электронной почты пользователя: ");
                                email = Console.ReadLine();
                            }

                            await ExecuteCommand_ExecutionPROC_AddUserWithOutput(name, email);


                            Thread.Sleep(3000);
                            Console.Clear();
                            await Menu();
                            break;
                        case 2:
                            Console.Write("Введите Id пользователя: ");
                            int UserId;
                            while (!Int32.TryParse(Console.ReadLine(), out UserId))
                            {
                                Console.WriteLine("Не верный ввод.Введите число:");
                                Console.Write("Id - ");
                            }

                            Console.Write("Введите адреса электронной почты пользователя: ");
                            email = Console.ReadLine();

                            while (string.IsNullOrWhiteSpace(email))
                            {
                                Console.WriteLine("Адрес электронной почты не может быть пустым.");
                                Console.Write("Введите адреса электронной почты пользователя: ");
                                email = Console.ReadLine();
                            }

                            await ExecuteCommand_ExecutionPROC_UpdateUserEmail(UserId, email);


                            Thread.Sleep(5000);
                            Console.Clear();
                            await Menu();
                            break;


                        case 3:
                            Console.Write("Введите Id пользователя: ");
                            int Id;
                            while (!Int32.TryParse(Console.ReadLine(), out Id))
                            {
                                Console.WriteLine("Не верный ввод.Введите число:");
                                Console.Write("Id - ");
                            }

                            await ExecuteCommand_ExecutionPROC_DeleteUser(Id);


                            Thread.Sleep(5000);
                            Console.Clear();
                            await Menu();
                            break;

                        case 4:
                            await ExecuteCommand_ExecutionPROC_GetAllUsers();

                            Console.WriteLine("Нажмите любую клавишу для выхода в меню...");
                            Console.ReadKey();
                            Console.Clear();
                            await Menu();
                            break;

                        case 5:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
                finally
                {
                    Console.Clear();
                    await Menu();
                }

            }








                // Просмотр списка всех пользователей.
                //-Создайте и вызовите хранимую процедуру `GetAllUsers`, которая возвращает все записи из таблицы `Users`.
                //-Отобразите список пользователей в консоли.

            string createProc = @"
                CREATE PROCEDURE GetAllUsers 
                AS 
                BEGIN 
                    SELECT * FROM Users
                END
                ";

            //await ExecuteCommandCreateProc(createProc);
            //await ExecuteCommand_ExecutionPROC_GetAllUsers();


            static async Task ExecuteCommand_ExecutionPROC_GetAllUsers()
            {
                string connectionString = @"Data Source = DESKTOP-ITRLGSN; Initial Catalog = Users; Trusted_Connection=True; TrustServerCertificate = True";
                string executeProc = "GetAllUsers";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(executeProc, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlDataReader reader = await command.ExecuteReaderAsync();


                    if (reader.HasRows)
                    {
                        string columnName1 = reader.GetName(0);
                        string columnName2 = reader.GetName(1);
                        string columnName3 = reader.GetName(2);



                        Console.WriteLine($"{columnName1,-5}\t{columnName2,-20}\t{columnName3,-40}");


                        while (await reader.ReadAsync())
                        {

                            int UserId = reader.GetInt32(0);
                            string UserName = reader.GetString(1);
                            string email = reader.GetString(2);



                            Console.WriteLine($"{UserId,-5} \t{UserName,-20} \t{email,-40}");
                        }
                    }
                    }
                }
            }




            static async Task ExecuteCommandCreateProc(string creatProc)
            {
                string connectionString = @"Data Source = DESKTOP-ITRLGSN; Initial Catalog = Users; Trusted_Connection=True; TrustServerCertificate = True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(creatProc, connection))
                    {
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("Процедура создана");
                    }
                }
            }
         
            
            static async Task ExecuteCommandBase(string sqlQuery)
            {
                string connectionString = @"Data Source = DESKTOP-ITRLGSN; Initial Catalog = master; Trusted_Connection=True; TrustServerCertificate = True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    await command.ExecuteNonQueryAsync();
                    Console.WriteLine("DB created");


                }

            }

            static async Task ExecuteCommand(string sqlQuery)
            {
                string connectionString = @"Data Source = DESKTOP-ITRLGSN; Initial Catalog = Users; Trusted_Connection=True; TrustServerCertificate = True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand(sqlQuery, connection);

                    await command.ExecuteNonQueryAsync();
                    Console.WriteLine("Table created");


                }

            }
        }
    }
}