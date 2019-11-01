using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using Felipe_Pagliosa_PB.Models;
using System.Configuration;
using System.IO;

namespace Felipe_Pagliosa_PB.Repository {
    public class CurtidasPostsRepository {

        public static bool ChecarCurtidaPost(int idp,int idu) {
            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "SELECT * FROM CurtidasPosts WHERE IdUsuario=@IdUsuario AND IdPost=@IdPost ";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdPost", idp);
                selectCommand.Parameters.AddWithValue("@IdUsuario", idu);

                var user = new Usuario();
                bool teste = false;
                try {
                    connection.Open();
                    using (var reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (reader.HasRows) {
                            teste = true;
                            return teste;                          
                        }
                    }
                }
                catch (Exception ex)
                {
                   
                }
                finally {

                    connection.Close();
                }
                return teste;
            }

        }

        public static void AddCurtidaPostBanco(int idp, int idu) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "INSERT INTO CurtidasPosts(IdPost,IdUsuario) VALUES(@IdPost,@IdUsuario)";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdPost", idp);
                selectCommand.Parameters.AddWithValue("@IdUsuario", idu);
                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();

                }
                catch (Exception ex)
                {

                }
                finally {

                    connection.Close();
                }
            }
        }

        public static void DeleteCurtidaPostBanco(int idp, int idu) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "DELETE FROM CurtidasPosts WHERE IdPost=@IdPost AND IdUsuario=@IdUsuario";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdPost", idp);
                selectCommand.Parameters.AddWithValue("@IdUsuario", idu);
                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                finally {
                    connection.Close();
                }
            }
        }

        public static void DeleteAllCurtidasPost(Post post) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "DELETE FROM CurtidasPosts WHERE IdPost=@IdPost";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdPost", post.IdPost);
                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();
                }
                finally {
                    connection.Close();
                }
            }
        }
    }
}