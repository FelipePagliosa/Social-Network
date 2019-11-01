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
    public class AmizadeRepository {

        public static void AddAmizade(int id,Usuario usuario) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {


                var commandText = "INSERT INTO Amizades(IdUsuarioOrigem,IdUsuarioDestino,Aceito) VALUES(@IdUsuarioOrigem,@IdUsuarioDestino,@Aceito)";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                
                selectCommand.Parameters.AddWithValue("@IdUsuarioOrigem",usuario.Id);
                selectCommand.Parameters.AddWithValue("@IdUsuarioDestino", id);
                selectCommand.Parameters.AddWithValue("@Aceito", false);
                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();

                }
                finally {

                    connection.Close();
                }
            }
        }

        public static bool ChecarAmizadeOrigem(Usuario usuario,int id) {
            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "SELECT * FROM Amizades WHERE (IdUsuarioOrigem=@IdUsuarioOrigem AND IdUsuarioDestino=@IdUsuarioDestino) OR (IdUsuarioOrigem=@IdUsuarioDestino AND IdUsuarioDestino=@IdUsuarioOrigem)";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdUsuarioOrigem", usuario.Id);
                selectCommand.Parameters.AddWithValue("@IdUsuarioDestino", id);
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

        public static List<Amizade> ListAllConvitesAmizade(Usuario usuario) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "SELECT * FROM Amizades WHERE IdUsuarioDestino=@IdUsuarioDestino AND Aceito=@Aceito";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdUsuarioDestino", usuario.Id);
                selectCommand.Parameters.AddWithValue("@Aceito", false);
                var listAmizades = new List<Amizade>();
                try {
                    connection.Open();
                    using (var reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (reader.HasRows) {
                            while (reader.Read()) {
                                var amizade = new Amizade();
                                amizade.IdUsuarioOrigem = (int)reader["IdUsuarioOrigem"];
                                amizade.NomeUsuario = UsuariosRepository.SelectUsuario(amizade.IdUsuarioOrigem).NomeUsuario;
                                amizade.Preferencia1 = UsuariosRepository.SelectUsuario(amizade.IdUsuarioOrigem).Preferencia1;
                                amizade.Preferencia2 = UsuariosRepository.SelectUsuario(amizade.IdUsuarioOrigem).Preferencia2;
                                amizade.Preferencia3 = UsuariosRepository.SelectUsuario(amizade.IdUsuarioOrigem).Preferencia3;
                                amizade.IdUsuarioDestino = (int)reader["IdUsuarioDestino"];
                                amizade.Aceito = (bool)reader["Aceito"];
                                listAmizades.Add(amizade);
                            }
                        }
                    }
                }
                finally {

                    connection.Close();
                }
                return listAmizades;
            }
        }

        public static void AceitarConviteAmizade(int id,Usuario usuario) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "UPDATE Amizades SET Aceito=@Aceito WHERE IdUsuarioOrigem=@IdUsuarioOrigem AND IdUsuarioDestino=@IdUsuarioDestino";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@Aceito", true);
                selectCommand.Parameters.AddWithValue("@IdUsuarioDestino", usuario.Id);
                selectCommand.Parameters.AddWithValue("@IdUsuarioOrigem", id);
                try {
                    connection.Open();
                    selectCommand.ExecuteNonQuery();

                }
                finally {

                    connection.Close();
                }
            }
        }

        public static List<Amizade> ListAllAmigos(Usuario usuario) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "SELECT * FROM Amizades WHERE (IdUsuarioDestino=@IdUsuarioDestino OR IdUsuarioOrigem=@IdUsuarioDestino) AND Aceito=@Aceito";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdUsuarioDestino", usuario.Id);
                selectCommand.Parameters.AddWithValue("@Aceito", true);
                var listAmizades = new List<Amizade>();
                try {
                    connection.Open();
                    using (var reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (reader.HasRows) {
                            while (reader.Read()) {
                                var amizade = new Amizade();
                                
                                amizade.IdUsuarioOrigem = (int)reader["IdUsuarioOrigem"];
                                if(amizade.IdUsuarioOrigem != usuario.Id) {
                                    amizade.NomeUsuario = UsuariosRepository.SelectUsuario(amizade.IdUsuarioOrigem).NomeUsuario;
                                    amizade.Preferencia1 = UsuariosRepository.SelectUsuario(amizade.IdUsuarioOrigem).Preferencia1;
                                    amizade.Preferencia2 = UsuariosRepository.SelectUsuario(amizade.IdUsuarioOrigem).Preferencia2;
                                    amizade.Preferencia3 = UsuariosRepository.SelectUsuario(amizade.IdUsuarioOrigem).Preferencia3;
                                    amizade.IdUsuarioDestino = (int)reader["IdUsuarioDestino"];
                                    amizade.Aceito = (bool)reader["Aceito"];
                                    listAmizades.Add(amizade);
                                }
                                else {
                                    amizade.IdUsuarioDestino = (int)reader["IdUsuarioDestino"];
                                    amizade.NomeUsuario = UsuariosRepository.SelectUsuario(amizade.IdUsuarioDestino).NomeUsuario;
                                    amizade.Preferencia1 = UsuariosRepository.SelectUsuario(amizade.IdUsuarioDestino).Preferencia1;
                                    amizade.Preferencia2 = UsuariosRepository.SelectUsuario(amizade.IdUsuarioDestino).Preferencia2;
                                    amizade.Preferencia3 = UsuariosRepository.SelectUsuario(amizade.IdUsuarioDestino).Preferencia3;
                                    amizade.Aceito = (bool)reader["Aceito"];
                                    listAmizades.Add(amizade);
                                }
                            }
                        }
                    }
                }
                finally {

                    connection.Close();
                }
                return listAmizades;
            }
        }

        public static List<Usuario> ListAllAmigoAsUsuarios(Usuario usuario) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "SELECT * FROM Amizades WHERE (IdUsuarioDestino=@IdUsuarioDestino OR IdUsuarioOrigem=@IdUsuarioDestino) AND Aceito=@Aceito";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@IdUsuarioDestino", usuario.Id);
                selectCommand.Parameters.AddWithValue("@Aceito", true);
                var listAmizadesAsUsuarios = new List<Usuario>();
                try {
                    connection.Open();
                    using (var reader = selectCommand.ExecuteReader(CommandBehavior.CloseConnection)) {
                        if (reader.HasRows) {
                            while (reader.Read()) {
                                var amizade = new Amizade();

                                amizade.IdUsuarioOrigem = (int)reader["IdUsuarioOrigem"];
                                if (amizade.IdUsuarioOrigem != usuario.Id) {
                                    listAmizadesAsUsuarios.Add(UsuariosRepository.SelectUsuarioSemFeed(amizade.IdUsuarioOrigem));
                                }
                                else {
                                    amizade.IdUsuarioDestino = (int)reader["IdUsuarioDestino"];
                                    listAmizadesAsUsuarios.Add(UsuariosRepository.SelectUsuarioSemFeed(amizade.IdUsuarioDestino));
                                }
                            }
                        }
                    }
                }
                finally {

                    connection.Close();
                }
                return listAmizadesAsUsuarios;
            }
        }

        public static void ExcluirAmizade(int ido, int idd) {

            var connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\DB.mdf";
            using (var connection = new SqlConnection(connectionString)) {

                var commandText = "DELETE FROM Amizades WHERE ((IdUsuarioOrigem=@IdUsuarioOrigem AND IdUsuarioDestino=@IdUsuarioDestino) OR (IdUsuarioOrigem=@IdUsuarioDestino AND IdUsuarioDestino=@IdUsuarioOrigem)) AND Aceito=@Aceito";
                var selectCommand = new SqlCommand(commandText, connection);// ele é um comando mas ainda não foi executado, tem que ser executado para fazer o comando.
                selectCommand.Parameters.AddWithValue("@Aceito", true);
                selectCommand.Parameters.AddWithValue("@IdUsuarioDestino", idd);
                selectCommand.Parameters.AddWithValue("@IdUsuarioOrigem", ido);
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