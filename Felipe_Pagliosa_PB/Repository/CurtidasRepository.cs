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
    public class CurtidasRepository {

        public static bool ChecarCurtida(Publicacao post) {
            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "SELECT * FROM Curtidas WHERE Id_Usuario=@Id_Usuario AND Post_Id=@Post_Id ";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@Id_Usuario", post.Id_Usuario);
                selectCommand.Parameters.AddWithValue("@Post_Id", post.Post_Id);
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
                finally {

                    connection.Close();
                }
                return teste;
            }

        }

        public static void AddCurtidaBanco(Publicacao post) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "INSERT INTO Curtidas(Post_Id,Id_Usuario) VALUES(@Post_Id,@Id_Usuario)";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@Post_Id", post.Post_Id);
                selectCommand.Parameters.AddWithValue("@Id_Usuario", post.Id_Usuario);
                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();

                }
                finally {

                    connection.Close();
                }
            }
        }

        public static void DeleteCurtidaBanco(Publicacao post) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "DELETE FROM Curtidas WHERE Post_Id=@Post_Id AND Id_Usuario=@Id_Usuario";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@Post_Id", post.Post_Id);
                selectCommand.Parameters.AddWithValue("@Id_Usuario", post.Id_Usuario);
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