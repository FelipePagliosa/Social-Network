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
    public class UsuariosRepository {

        public static void AddUser(Usuario user,HttpPostedFileBase pasta) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "INSERT INTO Users(Nome,Sobrenome,DataDeNascimento,Sexo,Email,Senha,NomeUsuario,Preferencia1,Preferencia2,Preferencia3,Foto) VALUES(@Nome,@Sobrenome,@DataDeNascimento,@Sexo,@Email,@Senha,@NomeUsuario,@Preferencia1,@Preferencia2,@Preferencia3,@Foto)";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@Nome", user.Nome);
                selectCommand.Parameters.AddWithValue("@Sobrenome", user.Sobrenome);
                selectCommand.Parameters.AddWithValue("@DataDeNascimento", user.DataDeNascimento);
                selectCommand.Parameters.AddWithValue("@Sexo", user.Sexo);
                selectCommand.Parameters.AddWithValue("@Email", user.Email);
                selectCommand.Parameters.AddWithValue("@Senha", user.Senha);
                selectCommand.Parameters.AddWithValue("@NomeUsuario", user.NomeUsuario);
                if (user.Preferencia1 != null) {
                    selectCommand.Parameters.AddWithValue("@Preferencia1", user.Preferencia1);
                }
                else {
                    selectCommand.Parameters.AddWithValue("@Preferencia1", DBNull.Value); /*NÃO ACEITA NÃO SEI PQ*/
                }

                if (user.Preferencia2 != null) {
                    selectCommand.Parameters.AddWithValue("@Preferencia2", user.Preferencia2);
                }
                else {
                    selectCommand.Parameters.AddWithValue("@Preferencia2", DBNull.Value);
                }

                if (user.Preferencia3 != null) {
                    selectCommand.Parameters.AddWithValue("@Preferencia3", user.Preferencia3);
                }
                else {
                    selectCommand.Parameters.AddWithValue("@Preferencia3", DBNull.Value);
                }
                string imgpath = Path.Combine(HttpContext.Current.Server.MapPath("~/UserImages/"), pasta.FileName);//mandando a pasta para o diretório
                pasta.SaveAs(imgpath);
                //File.Delete(imgpath);
                selectCommand.Parameters.AddWithValue("@Foto", pasta.FileName);
                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();

                }
                finally {

                    connection.Close();
                }
            }
        }

        public static bool Checar(Usuario usuario) {
            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "SELECT * FROM Users WHERE Email=@Email";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@Email", usuario.Email);
                var user = new Usuario();
                bool teste = false;
                try {
                    connection.Open();
                    using (var reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (reader.HasRows) {
                            teste = true;
                            while (reader.Read()) {

                                user.Email = reader["Email"].ToString();
                                return teste;
                            }
                        }
                    }
                }
                finally {

                    connection.Close();
                }
                return teste;
            }

        }

        public static Usuario Login (Usuario usuario) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "SELECT * FROM Users WHERE Email=@Email AND Senha=@Senha";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@Email", usuario.Email);
                selectCommand.Parameters.AddWithValue("@Senha", usuario.Senha);
                var user = new Usuario();
                //bool teste = false;
                try {
                    connection.Open();
                    using (var reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (reader.HasRows) {
                            //teste = true;
                            while (reader.Read()) {
                                user.Id = (int)reader["Id"];
                                user.Nome = reader["Nome"].ToString();
                                user.Sobrenome = reader["Sobrenome"].ToString();
                                user.DataDeNascimento = (DateTime)reader["DataDeNascimento"];
                                user.Sexo = reader["Sexo"].ToString();
                                user.Email = reader["Email"].ToString();
                                user.Senha = reader["Senha"].ToString();
                                user.NomeUsuario = reader["NomeUsuario"].ToString();
                                user.Preferencia1 = reader["Preferencia1"].ToString();
                                user.Preferencia2 = reader["Preferencia2"].ToString();
                                user.Preferencia3 = reader["Preferencia3"].ToString();
                                user.Foto = reader["Foto"].ToString();
                                user.postsAmigos = PostsRepository.ListPostsAmigos(user);
                            }
                        }
                    }
                }
                finally {

                    connection.Close();
                }
                return user;
            }
        }

        public static List<Usuario> ListAllUsers(Usuario usuario,string pesquisa) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "SELECT * FROM Users WHERE (Id<>@Id) AND (NomeUsuario LIKE '%" + pesquisa + "%' OR Preferencia1 LIKE '%" + pesquisa + "%' OR Preferencia2 LIKE '%" + pesquisa + "%' OR Preferencia3 LIKE '%" + pesquisa + "%')";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.     
                selectCommand.Parameters.AddWithValue("@Id", usuario.Id);
                //selectCommand.Parameters.AddWithValue("@Pesquisa", pesquisa);
                var usuarios = new List<Usuario>();
                try {
                    connection.Open();
                    using (var reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection)) {
                        while (reader.Read()) {
                            var user = new Usuario();
                            user.Id = (int)reader["Id"];
                            user.Nome = reader["Nome"].ToString();
                            user.Sobrenome = reader["Sobrenome"].ToString();
                            user.DataDeNascimento = (DateTime)reader["DataDeNascimento"];
                            user.Sexo = reader["Sexo"].ToString();
                            user.Email = reader["Email"].ToString();
                            user.Senha = reader["Senha"].ToString();
                            user.NomeUsuario = reader["NomeUsuario"].ToString();
                            user.Preferencia1 = reader["Preferencia1"].ToString();
                            user.Preferencia2 = reader["Preferencia2"].ToString();
                            user.Preferencia3 = reader["Preferencia3"].ToString();
                            user.Foto = reader["Foto"].ToString();
                            if (AmizadeRepository.ChecarAmizadeOrigem(usuario, user.Id) == false) {
                                usuarios.Add(user);
                            }
                        }                       
                    }
                }
                finally {

                    connection.Close();
                }
                return usuarios;
            }
        }

        public static void EditUsuarioSemMudarFoto(Usuario usuario) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "UPDATE Users SET Nome=@Nome,Sobrenome=@Sobrenome,NomeUsuario=@NomeUsuario,Email=@Email,Senha=@Senha,Preferencia1=@Preferencia1,Preferencia2=@Preferencia2,Preferencia3=@Preferencia3 WHERE Id=@Id";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@Id", usuario.Id);
                selectCommand.Parameters.AddWithValue("@Nome", usuario.Nome);
                selectCommand.Parameters.AddWithValue("@Sobrenome", usuario.Sobrenome);
                selectCommand.Parameters.AddWithValue("@Email", usuario.Email);
                selectCommand.Parameters.AddWithValue("@Senha", usuario.Senha);
                selectCommand.Parameters.AddWithValue("@NomeUsuario", usuario.NomeUsuario);
                if (usuario.Preferencia1 != null) {
                    selectCommand.Parameters.AddWithValue("@Preferencia1", usuario.Preferencia1);
                }
                else {
                    selectCommand.Parameters.AddWithValue("@Preferencia1", DBNull.Value); /*NÃO ACEITA NÃO SEI PQ*/
                }

                if (usuario.Preferencia2 != null) {
                    selectCommand.Parameters.AddWithValue("@Preferencia2", usuario.Preferencia2);
                }
                else {
                    selectCommand.Parameters.AddWithValue("@Preferencia2", DBNull.Value);
                }

                if (usuario.Preferencia3 != null) {
                    selectCommand.Parameters.AddWithValue("@Preferencia3", usuario.Preferencia3);
                }
                else {
                    selectCommand.Parameters.AddWithValue("@Preferencia3", DBNull.Value);
                }
                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();

                }
                finally {

                    connection.Close();
                }
            }
        }

        public static void EditUsuario(Usuario usuario, HttpPostedFileBase pasta) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "UPDATE Users SET Nome=@Nome,Sobrenome=@Sobrenome,NomeUsuario=@NomeUsuario,Email=@Email,Senha=@Senha,Foto=@Foto,Preferencia1=@Preferencia1,Preferencia2=@Preferencia2,Preferencia3=@Preferencia3 WHERE Id=@Id";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@Id", usuario.Id);
                selectCommand.Parameters.AddWithValue("@Nome", usuario.Nome);
                selectCommand.Parameters.AddWithValue("@Sobrenome", usuario.Sobrenome);
                selectCommand.Parameters.AddWithValue("@Email", usuario.Email);
                selectCommand.Parameters.AddWithValue("@Senha", usuario.Senha);
                selectCommand.Parameters.AddWithValue("@NomeUsuario", usuario.NomeUsuario);
                string imgpath = Path.Combine(HttpContext.Current.Server.MapPath("~/UserImages/"), pasta.FileName);
                pasta.SaveAs(imgpath);
                selectCommand.Parameters.AddWithValue("@Foto", pasta.FileName);
                if (usuario.Preferencia1 != null) {
                    selectCommand.Parameters.AddWithValue("@Preferencia1", usuario.Preferencia1);
                }
                else {
                    selectCommand.Parameters.AddWithValue("@Preferencia1", DBNull.Value); 
                }

                if (usuario.Preferencia2 != null) {
                    selectCommand.Parameters.AddWithValue("@Preferencia2", usuario.Preferencia2);
                }
                else {
                    selectCommand.Parameters.AddWithValue("@Preferencia2", DBNull.Value);
                }

                if (usuario.Preferencia3 != null) {
                    selectCommand.Parameters.AddWithValue("@Preferencia3", usuario.Preferencia3);
                }
                else {
                    selectCommand.Parameters.AddWithValue("@Preferencia3", DBNull.Value);
                }
                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();

                }
                finally {

                    connection.Close();
                }
            }
        }

        public static Usuario SelectUsuario(int id) {
            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "SELECT * FROM Users WHERE Id=@Id";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@Id", id);
                var usuario = new Usuario();
                try {
                    connection.Open();
                    using (var reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (reader.HasRows) {
                            while (reader.Read()) {
                                usuario.Id = (int)reader["Id"];
                                usuario.Nome = reader["Nome"].ToString();
                                usuario.Sobrenome = reader["Sobrenome"].ToString();
                                usuario.DataDeNascimento = (DateTime)reader["DataDeNascimento"];
                                usuario.Sexo = reader["Sexo"].ToString();
                                usuario.Email = reader["Email"].ToString();
                                usuario.Senha = reader["Senha"].ToString();
                                usuario.NomeUsuario = reader["NomeUsuario"].ToString();
                                usuario.Preferencia1 = reader["Preferencia1"].ToString();
                                usuario.Preferencia2 = reader["Preferencia2"].ToString();
                                usuario.Preferencia3 = reader["Preferencia3"].ToString();
                                usuario.Foto = reader["Foto"].ToString();
                                usuario.postsAmigos = PostsRepository.ListPostsAmigos(usuario);
                                return usuario;
                            }
                        }
                    }
                }
                finally {

                    connection.Close();
                }
                return usuario;
            }
        }

        public static Usuario SelectUsuarioSemFeed(int id)
        {
            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString))
            {

                var commandText = "SELECT * FROM Users WHERE Id=@Id";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@Id", id);
                var usuario = new Usuario();
                try
                {
                    connection.Open();
                    using (var reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                usuario.Id = (int)reader["Id"];
                                usuario.Nome = reader["Nome"].ToString();
                                usuario.Sobrenome = reader["Sobrenome"].ToString();
                                usuario.DataDeNascimento = (DateTime)reader["DataDeNascimento"];
                                usuario.Sexo = reader["Sexo"].ToString();
                                usuario.Email = reader["Email"].ToString();
                                usuario.Senha = reader["Senha"].ToString();
                                usuario.NomeUsuario = reader["NomeUsuario"].ToString();
                                usuario.Preferencia1 = reader["Preferencia1"].ToString();
                                usuario.Preferencia2 = reader["Preferencia2"].ToString();
                                usuario.Preferencia3 = reader["Preferencia3"].ToString();
                                usuario.Foto = reader["Foto"].ToString();
                                return usuario;
                            }
                        }
                    }
                }
                finally
                {

                    connection.Close();
                }
                return usuario;
            }
        }

        public static string SelectNomeUsuario(int id)
        {
            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString))
            {

                var commandText = "SELECT NomeUsuario FROM Users WHERE Id=@Id";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@Id", id);
                var usuario = new Usuario();
                try
                {
                    connection.Open();
                    using (var reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                usuario.NomeUsuario = reader["NomeUsuario"].ToString();

                                return usuario.NomeUsuario;
                            }
                        }
                    }
                }
                finally
                {

                    connection.Close();
                }
                return usuario.NomeUsuario; 
            }
        }

        public static Usuario DetalhesAmigo(int ido, int idd, int idSessao)
        {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString))
            {
                string commandText = "";

                if (ido == idSessao)
                {
                    commandText = "SELECT * FROM Users WHERE Id=@IdUsuarioDestino";
                }
                if (idd == idSessao)
                {
                    commandText = "SELECT * FROM Users WHERE Id=@IdUsuarioOrigem";
                }
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdUsuarioDestino", idd);
                selectCommand.Parameters.AddWithValue("@IdUsuarioOrigem", ido);
                var usuario = new Usuario();
                try
                {
                    connection.Open();
                    using (var reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                usuario.Nome = reader["Nome"].ToString();
                                usuario.Sobrenome = reader["Sobrenome"].ToString();
                                usuario.DataDeNascimento = (DateTime)reader["DataDeNascimento"];
                                usuario.Sexo = reader["Sexo"].ToString();
                                usuario.NomeUsuario = reader["NomeUsuario"].ToString();
                                usuario.Preferencia1 = reader["Preferencia1"].ToString();
                                usuario.Preferencia2 = reader["Preferencia2"].ToString();
                                usuario.Preferencia3 = reader["Preferencia3"].ToString();
                                usuario.Foto = reader["Foto"].ToString();
                                return usuario;
                            }
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
                return usuario;
            }
        }
    }
}