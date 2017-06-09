using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace FooBot.Controllers
{
        class BDController
        {
            SqlConnectionStringBuilder builder;
            public void initBD()
            {
                try
                {
                    if (builder == null)
                    {
                        builder = new SqlConnectionStringBuilder();
                        builder.DataSource = ConfigurationManager.AppSettings["bdDataSource"];
                        builder.UserID = ConfigurationManager.AppSettings["bdUserID"];
                        builder.Password = ConfigurationManager.AppSettings["bdPassword"];
                        builder.InitialCatalog = ConfigurationManager.AppSettings["bdInitialCatalog"];
                    }

                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ToString());
                }
            }


            //Con esta función obtenemos las frases de la BD que pueden ser de diferentes tipos
            //tipo 1 - Saludo
            //Revisar Wiki para ver todos los tipos https://github.com/Javierif/FooBot/wiki
            public string getFrases(int tipo)
            {
                string frase = string.Empty;
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();

                    sb.Append("SELECT frase FROM Frases Where tipo='" + tipo + "'");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //el while no es necesario realmente, pero lo dejo por ahora para que tengais un ejemplo de obtener varios elementos.
                            while (reader.Read())
                            {
                                frase = reader.GetString(0);
                            }
                        }
                    }
                }
                return frase;
            }
        }
    }
