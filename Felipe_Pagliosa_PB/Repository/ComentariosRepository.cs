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
    public class ComentariosRepository {
        //usar um método para pegar tudo na tabela comentarios e retornar uma lista com tudo<<<<<<

        public static void AddComentario(Comentario comentario) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "INSERT INTO Comentarios(IdPost,IdUsuario,TextoComent,NumCurtidasComent,DataComentario) VALUES(@IdPost,@IdUsuario,@TextoComent,@NumCurtidasComent,@DataComentario)";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdPost", comentario.IdPost);
                selectCommand.Parameters.AddWithValue("@IdUsuario", comentario.IdUsuario);
                selectCommand.Parameters.AddWithValue("@TextoComent", comentario.TextoComent);
                selectCommand.Parameters.AddWithValue("@NumCurtidasComent", 0);
                selectCommand.Parameters.AddWithValue("@DataComentario", DateTime.Now);


                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();

                }
                finally {

                    connection.Close();
                }
            }
        }

        public static List<Comentario> ListAllComents() {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "SELECT * FROM Comentarios";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                var listComents = new List<Comentario>();
                try {
                    connection.Open();
                    using (var reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (reader.HasRows) {
                            while (reader.Read()) {
                                var comentario = new Comentario();
                                comentario.IdComentario = (int)reader["IdComentario"];
                                comentario.IdUsuario = (int)reader["IdUsuario"];
                                comentario.IdPost = (int)reader["IdPost"];                              
                                comentario.TextoComent = reader["TextoComent"].ToString();
                                comentario.NumCurtidasComent = (int)reader["NumCurtidasComent"];
                                comentario.NomeUsuario = UsuariosRepository.SelectNomeUsuario(comentario.IdUsuario); 
                                comentario.DataComentario = (DateTime)reader["DataComentario"];
                                listComents.Add(comentario);
                            }
                        }
                    }
                }
                finally {

                    connection.Close();
                }
                return listComents;
            }
        }

        public static Comentario SelectComent(int id) {
            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "SELECT * FROM Comentarios WHERE IdComentario=@IdComentario";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdComentario", id);
                var comentario = new Comentario();
                try {
                    connection.Open();
                    using (var reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (reader.HasRows) {
                            while (reader.Read()) {
                                comentario.IdComentario = (int)reader["IdComentario"];
                                comentario.IdPost = (int)reader["IdPost"];
                                comentario.IdUsuario = (int)reader["IdUsuario"];
                                comentario.TextoComent = reader["TextoComent"].ToString();
                                comentario.NumCurtidasComent = (int)reader["NumCurtidasComent"];
                                comentario.DataComentario = (DateTime)reader["DataComentario"];
                                comentario.NomeUsuario = UsuariosRepository.SelectNomeUsuario(comentario.IdUsuario);
                                return comentario;
                            }
                        }
                    }
                }
                finally {

                    connection.Close();
                }
                return comentario;
            }
        }

        public static void EditComentario(Comentario comentario) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "UPDATE Comentarios SET TextoComent=@TextoComent WHERE IdComentario=@IdComentario";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdComentario",comentario.IdComentario);
                selectCommand.Parameters.AddWithValue("@TextoComent", comentario.TextoComent);              
                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();

                }
                finally {

                    connection.Close();
                }
            }
        }

        public static void DeleteComentario(int id) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "DELETE FROM Comentarios WHERE IdComentario=@IdComentario";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdComentario", id);

                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();
                }
                finally {
                    connection.Close();
                }
            }
        }

        public static void DeleteAllComentariosDoPost(Post post) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "DELETE FROM Comentarios WHERE IdPost=@IdPost";
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

        public static void LikeComentario(Comentario comentario) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {
                var kek = comentario.NumCurtidasComent;

                var commandText = "UPDATE Comentarios SET NumCurtidasComent=@NumCurtidasComent WHERE IdComentario=@IdComentario";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                int valor = comentario.NumCurtidasComent + 1;
                selectCommand.Parameters.AddWithValue("@NumCurtidasComent", valor);
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

        public static void DislikeComentario(Comentario comentario) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {
                var kek = comentario.NumCurtidasComent;

                var commandText = "UPDATE Comentarios SET NumCurtidasComent=@NumCurtidasComent WHERE IdComentario=@IdComentario";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                int valor = comentario.NumCurtidasComent - 1;
                selectCommand.Parameters.AddWithValue("@NumCurtidasComent", valor);
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