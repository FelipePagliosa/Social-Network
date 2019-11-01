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
    public class CurtidasComentariosRepository {

        public static bool ChecarCurtidaComentario(int idc,int idu) {
            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "SELECT * FROM CurtidasComentarios WHERE IdComentario=@IdComentario AND IdUsuario=@IdUsuario";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdComentario", idc);
                selectCommand.Parameters.AddWithValue("@IdUsuario", idu);         
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

        public static void AddCurtidaComentarioBanco(int idc,int idu) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "INSERT INTO CurtidasComentarios(IdComentario,IdUsuario) VALUES(@IdComentario,@IdUsuario)";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdComentario", idc);
                selectCommand.Parameters.AddWithValue("@IdUsuario", idu);
                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();

                }
                finally {

                    connection.Close();
                }
            }
        }

        public static void DeleteCurtidaComentarioBanco(int idc,int idu) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "DELETE FROM CurtidasComentarios WHERE IdComentario=@IdComentario AND IdUsuario=@IdUsuario";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdComentario", idc);
                selectCommand.Parameters.AddWithValue("@IdUsuario", idu);
                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();
                }
                finally {
                    connection.Close();
                }
            }
        }

        public static void DeleteAllCurtidasComentario(Comentario comentario) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "DELETE FROM CurtidasComentarios WHERE IdComentario=@IdComentario";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdComentario", comentario.IdComentario);
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