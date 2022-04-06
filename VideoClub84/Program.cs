using System;
using System.Configuration;
using System.Data.SqlClient;

namespace VideoClub84
{
    internal class Program
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["VideoClub84"].ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString);
        static string cadena;
        static SqlCommand comando;
        static SqlDataReader registros;


        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Bienvenido al Video Club 84\n");
                Users.NewRegister();

                Users u1 = new Users();
                u1.Login();
                Menu(u1);
            }
        }


        //METODO MENU
        public static void Menu(Users u1)
        {
            bool exit = false;
            while (!exit)
            {
                try
                {
                    int option = 0;
                    Console.WriteLine();
                    Console.WriteLine("Introduce el número de la operación que desees.\n\n\t1.- Mostrar Películas.\n\t2.- Alquilar Películas.\n\t3.- Mis Alquileres.\n\t4.-Logout");
                    option = Int32.Parse(Console.ReadLine());
                    switch (option)
                    {
                        case 1:
                            Movies.ListMovies(u1);
                            break;

                        case 2:
                            Movies.RentMovie(u1);
                            break;

                        case 3:
                            Orders.MyMovies(u1);
                            break;

                        case 4:
                            exit = true;
                            break;

                        default:

                            break;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n\tERROR! El formato de fecha es incorrecto\n");
                }
            }



        }
    }
}
