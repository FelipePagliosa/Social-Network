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
    public class PostsRepository {

        public static Post AddPost(Post post, HttpPostedFileBase pasta) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "INSERT INTO Posts(TextoPub,IdUsuario,ImagemPost,NumCurtidasPost,DataPost,IdPostOrigem) VALUES(@TextoPub,@IdUsuario,@ImagemPost,@NumCurtidasPost,@DataPost,@IdPostOrigem)";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@TextoPub", post.TextoPub);
                selectCommand.Parameters.AddWithValue("@IdUsuario", post.IdUsuario);
                selectCommand.Parameters.AddWithValue("@NumCurtidasPost", 0);
                selectCommand.Parameters.AddWithValue("@IdPostOrigem", DBNull.Value);
                selectCommand.Parameters.AddWithValue("@DataPost", DateTime.Now);
                string imgpath = Path.Combine(HttpContext.Current.Server.MapPath("~/UserImages/"), pasta.FileName);//mandando a pasta para o diretório
                pasta.SaveAs(imgpath);
                selectCommand.Parameters.AddWithValue("@ImagemPost", pasta.FileName);//mandando para o banco, então manda só o file name
                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();

                }
                finally {

                    connection.Close();
                }
                return post;
            }
        }

        public static void CompartilharPost(Post post)
        {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString))
            {

                var commandText = "INSERT INTO Posts(TextoPub,IdUsuario,IdPostOrigem,ImagemPost,NumCurtidasPost,DataPost) VALUES(@TextoPub,@IdUsuario,@IdPostOrigem,@ImagemPost,@NumCurtidasPost,@DataPost)";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@TextoPub", post.TextoPub);
                selectCommand.Parameters.AddWithValue("@IdUsuario", post.IdUsuario);
                selectCommand.Parameters.AddWithValue("@IdPostOrigem", post.IdPostOrigem);
                selectCommand.Parameters.AddWithValue("@NumCurtidasPost", 0);
                selectCommand.Parameters.AddWithValue("@DataPost", DateTime.Now);
                selectCommand.Parameters.AddWithValue("@ImagemPost", post.ImagemPost);//mandando para o banco, então manda só o file name
                try
                {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();

                }
                finally
                {

                    connection.Close();
                }
            }
        }

        public static List<Post> ListAllPostsFromUser(Usuario usuario) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "SELECT * FROM Posts WHERE IdUsuario=@IdUsuario";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdUsuario", usuario.Id);
                var listPosts = new List<Post>();
                try {
                    connection.Open();
                    using (var reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (reader.HasRows) {
                            while (reader.Read()) {
                                var post = new Post();
                                post.IdPost = (int)reader["IdPost"];
                                post.IdUsuario = usuario.Id;
                                var teste = reader["IdPostOrigem"].ToString();
                                if (teste != "")
                                {
                                    post.IdPostOrigem = (int)reader["IdPostOrigem"];
                                }
                                post.TextoPub = reader["TextoPub"].ToString();
                                post.ImagemPost = reader["ImagemPost"].ToString();
                                post.NumCurtidasPost = (int)reader["NumCurtidasPost"];
                                post.DataPost = (DateTime)reader["DataPost"];                              
                                post.Comentario = new List<Comentario>();
                                foreach (var item in ComentariosRepository.ListAllComents()) {
                                    if (item.IdPost == post.IdPost) {
                                        post.Comentario.Add(item);
                                    }
                                }
                                post.NomeUsuario = UsuariosRepository.SelectNomeUsuario(post.IdUsuario);
                                listPosts.Add(post);
                            }
                        }
                    }
                }
                finally {

                    connection.Close();
                }
                return listPosts;
            }
        }

        public static void EditPost(Post post, HttpPostedFileBase pasta) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "UPDATE Posts SET TextoPub=@TextoPub,ImagemPost=@ImagemPost WHERE IdPost=@IdPost";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdPost", post.IdPost);
                selectCommand.Parameters.AddWithValue("@TextoPub", post.TextoPub);
                string imgpath = Path.Combine(HttpContext.Current.Server.MapPath("~/UserImages/"), pasta.FileName);//mandando a pasta para o diretório
                pasta.SaveAs(imgpath);
                selectCommand.Parameters.AddWithValue("@ImagemPost", pasta.FileName);//mandando para o banco, então manda só o file name
                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();

                }
                finally {

                    connection.Close();
                }
            }
        }


        public static void EditPostSemMudarImagem(Post post) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "UPDATE Posts SET TextoPub=@TextoPub WHERE IdPost=@IdPost";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdPost", post.IdPost);
                selectCommand.Parameters.AddWithValue("@TextoPub", post.TextoPub);
                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();

                }
                finally {

                    connection.Close();
                }
            }
        }

        public static void DeletePost(int id) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "DELETE FROM Posts WHERE IdPost=@IdPost";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdPost", id);

                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();
                }
                finally {
                    connection.Close();
                }
            }
        }

        public static Post SelectPost(int id) {
            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "SELECT * FROM Posts WHERE IdPost=@IdPost";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdPost", id);
                var post = new Post();
                try {
                    connection.Open();
                    using (var reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (reader.HasRows) {
                            while (reader.Read()) {
                                post.IdPost = (int)reader["IdPost"];
                                post.IdUsuario = (int)reader["IdUsuario"];
                                var teste = reader["IdPostOrigem"].ToString();
                                if (teste != "")
                                {
                                    post.IdPostOrigem = (int)reader["IdPostOrigem"];
                                }
                                post.TextoPub = reader["TextoPub"].ToString();
                                post.ImagemPost= reader["ImagemPost"].ToString();
                                post.NumCurtidasPost = (int)reader["NumCurtidasPost"];
                                post.DataPost = (DateTime)reader["DataPost"];
                                post.NomeUsuario = UsuariosRepository.SelectUsuario(post.IdUsuario).NomeUsuario;
                                post.Comentario = new List<Comentario>();
                                foreach (var item in ComentariosRepository.ListAllComents())
                                {
                                    if (item.IdPost == post.IdPost)
                                    {
                                        post.Comentario.Add(item);
                                    }
                                }
                                return post;
                            }
                        }
                    }
                }
                finally {

                    connection.Close();
                }
                return post;
            }
        }

        public static void LikePost(Post post) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {
                var kek = post.NumCurtidasPost;

                var commandText = "UPDATE Posts SET NumCurtidasPost=@NumCurtidasPost WHERE IdPost=@IdPost";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                int valor = post.NumCurtidasPost + 1;
                selectCommand.Parameters.AddWithValue("@NumCurtidasPost", valor);
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

        public static void DislikePost(Post post) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {
                var kek = post.NumCurtidasPost;

                var commandText = "UPDATE Posts SET NumCurtidasPost=@NumCurtidasPost WHERE IdPost=@IdPost";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                int valor = post.NumCurtidasPost - 1;
                selectCommand.Parameters.AddWithValue("@NumCurtidasPost", valor);
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

        public static List<Post> ListPostsAmigos(Usuario usuario) {
            List<Usuario> listAmigos = AmizadeRepository.ListAllAmigoAsUsuarios(usuario);
            List<Post> feed = new List<Post>();
            foreach (var user in listAmigos) {
                feed.AddRange(PostsRepository.ListAllPostsFromUser(user));
            }
            return feed;
        }       
    }
}
