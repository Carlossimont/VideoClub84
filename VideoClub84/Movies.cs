using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoClub84
{
    internal static class Movies
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["VideoClub84"].ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString);
        static string cadena;
        static SqlCommand comando;
        static SqlDataReader registros;

        public static string Title { get; set; }

        public static int RecommendedAge { get; set; }

        public static string Available { get; set; }

        //METODO LISTADO DE PELICULAS
        public static void ListMovies(Users u1)
        {
            Console.WriteLine("\n==LISTA DE PELICULAS DISPONIBLES==\n______________________________");
            connection.Open();

            string query = $"select * from Movies where RecommendedAge < {u1.CalculateAge()}";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader user = command.ExecuteReader();
            while (user.Read())
            {
                string patata;
                Title = user["Title"].ToString();
                patata = user["RecommendedAge"].ToString();
                RecommendedAge = Int32.Parse(patata);
                Available = user["Available"].ToString();

                Console.WriteLine($"\t {user["ID"]} {user["Title"]}");
            }
            connection.Close();
            bool exit = false;
            while (!exit)
            {
                try
                {
                    Console.WriteLine("\nIntroduce el nº de la película para ver los detalles o 0 para ir al menu anterior.");
                    int option = Int32.Parse(Console.ReadLine());

                    if (option == 0)
                    {
                        exit = true;
                    }
                    else
                    {
                        connection.Open();

                        SqlCommand comand = new SqlCommand($"SELECT * FROM Movies where ID = {option}", connection);
                        SqlDataReader movie = comand.ExecuteReader();
                        while (movie.Read())
                        {
                            Console.Write($"\n\t______________________________\n\n\tTítulo: {movie["Title"]}\n\n\tResumen: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write($"{ movie["Synopsis"]}");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"\n\n\tDisponible para alquilar (Y/N):  {movie["Available"]}\n\t______________________________");
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n\tERROR! introduce una opción válida\n");
                }
            }
        }

        //METODO DE ALQUILER DE PELICULAS
        public static void RentMovie(Users u1)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("== LISTA DE PELÍCULAS DISPONIBLES PARA ALQUILAR ==\n__________________________________________________");
                connection.Open();

                string query = $"select * from Movies where Available = 'Y' and RecommendedAge < {u1.CalculateAge()}";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader user = command.ExecuteReader();
                List<int> idmovies = new List<int>();
                while (user.Read())
                {
                    Console.WriteLine($"\t{user["ID"]} {user["Title"]}");

                    idmovies.Add(Convert.ToInt32(user["ID"]));
                }
                connection.Close();
                try
                {
                    Console.WriteLine("\nIntroduce el ID de la película que quieras alquilar o Pulsa 0 para volver al menu anterior");
                    int id = Int32.Parse(Console.ReadLine());

                    if (id == 0)
                    {
                        exit = true;
                    }
                    else if (idmovies.IndexOf(id) == -1)
                    {
                        Console.WriteLine("ERROR!!! La película introducida no es válida.\n");
                    }
                    else if (id < 0)
                    {
                        Console.WriteLine("ERROR!!! No se pueden introducir valores negativos");
                    }
                    else
                    {
                        connection.Open();

                        string queryupdate = $"UPDATE Movies set Available = 'N' where ID = {id}";
                        SqlCommand command2 = new SqlCommand(queryupdate, connection);
                        command2.ExecuteNonQuery();

                        string queryupdate2 = $"INSERT INTO Orders(MoviesID, EMailUser, RentDate, DevDate) VALUES('{id}', '{u1.Email}', GetDate(), Dateadd(day, 3, Getdate()))";
                        SqlCommand command3 = new SqlCommand(queryupdate2, connection);
                        command3.ExecuteNonQuery();

                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n\tERROR! introduce una opción válida\n");
                }
            }
        }
    }
}
