using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoClub84
{
    internal class Users
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["VideoClub84"].ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString);
        static string cadena;
        static SqlCommand comando;
        static SqlDataReader registros;

        public string NameUser { get; set; }
        public string SurnameUser { get; set; }
        public string BirthDate { get; set; }
        public string Email { get; set; }
        public string _Password { get; set; }
        public int Age { get; set; }

        public Users()
        {

        }


        //METODO LOGIN
        public void Login()
        {
            bool exit = false;//Hay que revisar esto para cuando el user mete un string de letras
            while (!exit)
            {
                try
                {
                    Console.WriteLine("===LOGIN===\n___________\nIntroduce tu correo");
                    string email = Console.ReadLine();
                    string emailLower = email.ToLower();
                    Console.WriteLine("introduce tu contraseña");
                    string password = Console.ReadLine();

                    connection.Open();

                    string query = $"SELECT * FROM Users where Email = '{emailLower}' and _Password = '{password}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader user = command.ExecuteReader();

                    if (user.HasRows)
                    {
                        while (user.Read())
                        {
                            Email = user["Email"].ToString();//codigo Erlantz
                            _Password = user["_Password"].ToString();
                            NameUser = user["NameUser"].ToString();
                            BirthDate = user["BirthDate"].ToString();
                            SurnameUser = user["SurnameUser"].ToString();
                        }
                        exit = true;
                    }
                    else
                    {
                        Console.WriteLine("Error al introducir los datos");
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
        //METODO NUEVO REGISTRO
        public static void NewRegister()
        {

            bool exit = false;
            while (!exit)
            {
                try
                {
                    Console.WriteLine("¿Qué desea hacer?\n\t1.- Crear Usuario.\n\t2.-Login.");
                    int option = Int32.Parse(Console.ReadLine());

                    switch (option)
                    {
                        case 1:

                            Console.WriteLine("Introduce tu nombre");
                            string nameUser = Console.ReadLine();

                            Console.WriteLine("Introduce tu apellido");
                            string surnameUser = Console.ReadLine();

                            Console.WriteLine("Introduce tu fecha de nacimiento AAAA-MM-DD");
                            string birthDate = Console.ReadLine();

                            Console.WriteLine("Introduce tu email");
                            string email = Console.ReadLine();

                            Console.WriteLine("Introduce tu contraseña");
                            string password = Console.ReadLine();


                            if (String.IsNullOrEmpty(nameUser) || String.IsNullOrEmpty(surnameUser) || String.IsNullOrEmpty(birthDate) || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
                            {
                                Console.WriteLine("\tNo se pueden dejar campos vacíos.\n");
                            }
                            else
                            {
                                connection.Open();
                                string queryInsert = $"Insert into Users (NameUser, SurnameUser, BirthDate, Email, _Password) values ('{nameUser}','{surnameUser}','{birthDate}','{email}', '{password}')";
                                comando = new SqlCommand(queryInsert, connection);
                                comando.ExecuteNonQuery();

                                connection.Close();

                                Console.WriteLine("+Usuario registrado satisfactoriamente+");
                                exit = true;

                            }
                            
                            break;

                        case 2:

                            exit = true;

                            break;

                        default:
                            Console.WriteLine("Has introducido una opción errónea.\n");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n\tERROR! introduce una opción válida\n");
                }
            }
        }
        public int CalculateAge()
        {
            DateTime birthDate = Convert.ToDateTime(BirthDate);
            DateTime n = DateTime.Now;
            int age = n.Year - birthDate.Year;
            Age = age;
            if (n.Month < birthDate.Month || (n.Month == birthDate.Month && n.Day < birthDate.Day))
                age--;

            return age;
        }
    }
}