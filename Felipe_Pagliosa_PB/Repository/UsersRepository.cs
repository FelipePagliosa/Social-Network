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
    public class UsersRepository {

        public static void AddUser(Usuario user,HttpPostedFileBase pasta) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "INSERT INTO Users(Nome,Sobrenome,DataDeNascimento,Sexo,Email,Senha,NomeUsuario,Foto) VALUES(@Nome,@Sobrenome,@DataDeNascimento,@Sexo,@Email,@Senha,@NomeUsuario,@Foto)";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@Nome", user.Nome);
                selectCommand.Parameters.AddWithValue("@Sobrenome", user.Sobrenome);
                selectCommand.Parameters.AddWithValue("@DataDeNascimento", user.DataDeNascimento);
                selectCommand.Parameters.AddWithValue("@Sexo", user.Sexo);
                selectCommand.Parameters.AddWithValue("@Email", user.Email);
                selectCommand.Parameters.AddWithValue("@Senha", user.Senha);
                selectCommand.Parameters.AddWithValue("@NomeUsuario", user.NomeUsuario);
                string imgpath = Path.Combine(HttpContext.Current.Server.MapPath("~/UserImages/"), pasta.FileName);//mandando a pasta para o diretório
                pasta.SaveAs(imgpath);
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
                                user.Foto = reader["Foto"].ToString();
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

    } 
}