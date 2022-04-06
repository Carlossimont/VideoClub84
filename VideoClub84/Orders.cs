using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoClub84
{
    internal class Orders
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["VideoClub84"].ConnectionString;
        static SqlConnection connection = new SqlConnection(connectionString);
        static string cadena;
        static SqlCommand command;
        static SqlDataReader registros;

        //METODO MIS PELICULAS

        public static void MyMovies(Users u1)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("==MIS ALQUILERES==\n_________________");
                connection.Open();

                string query = $"select * from movies inner join orders on Movies.ID = Orders.MoviesID WHERE EmailUser = '{u1.Email}' and ReturnedDate IS NULL";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader user = command.ExecuteReader();
                List<int> idmovies = new List<int>();
                while (user.Read())
                {
                    if ((Convert.ToDateTime(user["DevDate"]) - DateTime.Now).Days < 0)
                    {
                        Console.Write($"\n{user["MoviesID"]} {user["Title"]}");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"{ user["DevDate"]}\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        idmovies.Add(Convert.ToInt32(user["MoviesID"]));
                    }
                    else
                    {
                        Console.WriteLine($"\n{user["MoviesID"]} {user["Title"]} {user["DevDate"]}");
                        idmovies.Add(Convert.ToInt32(user["MoviesID"]));
                    }
                }

                connection.Close();

                connection.Open();
                int id = 0;
                if (idmovies.Count == 0)
                {
                    Console.WriteLine("No tienes películas alquiladas\n");
                    exit = true;
                }
                else
                {
                    try
                    {
                        Console.WriteLine("\n¿Desea devolver alguna película?\nSi la respuesta es SI introduce el nº de la película a devolver.\nSi la respuesta es no pulse 0");
                        id = Int32.Parse(Console.ReadLine());
                        if (id == 0)
                        {
                            exit = true;
                        }
                        else if (idmovies.IndexOf(id) == -1)
                        {
                            Console.WriteLine("ERROR!!! La película introducida no es válida.\n");
                        }
                        else
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\n\tERROR! introduce una opción válida\n");
                    }
                }

                string queryupdate = $"UPDATE Movies set Available = 'Y' where ID = {id}";
                SqlCommand command2 = new SqlCommand(queryupdate, connection);
                command2.ExecuteNonQuery();

                string queryupdate2 = $"Update Orders set ReturnedDate = Getdate() Where MoviesID = {id}";
                SqlCommand command3 = new SqlCommand(queryupdate2, connection);
                command3.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
