using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Felipe_Pagliosa_PB.Models {

    public class Usuario {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Entre com o nome !")]
        [Display(Name ="Nome :")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Entre com o sobrenome !")]
        [Display(Name = "Sobrenome :")]
        public string Sobrenome { get; set; }

        [Required]
        [Display(Name = "Data de nascimento :")]
        [DataType(DataType.Date)]
        public DateTime DataDeNascimento { get; set; }

        [Required(ErrorMessage = "Selecione o gênero !")]
        [Display(Name = "Gênero :")]
        public string Sexo { get; set; }

        [Required(ErrorMessage = "Entre com o e-mail !")]
        [Display(Name = "E-mail :")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Entre com a senha !")]
        [Display(Name = "Senha :")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Required(ErrorMessage = "As senhas não são iguais !")]
        [Display(Name = "Confirme a senha: ")]
        [DataType(DataType.Password)]
        [Compare("Senha")]
        public string ConfirmSenha { get; set; }

        [Required(ErrorMessage = "Entre com o nome de usuário (esse nome é o que irá aparecer para os outros usuários) !")]
        [Display(Name = "Nome de usuário :")]
        public string NomeUsuario { get; set; }

        [Required(ErrorMessage = "Faça o upload da sua foto de perfil")]
        [Display(Name = "Foto de perfil :")]
        public string Foto { get; set; }

        //[Required(ErrorMessage = "Escreva uma preferência de conteúdo")]
        [Display(Name = "Preferência de conteúdo 1: :")]
        public string Preferencia1 { get; set; }

        //[Required(ErrorMessage = "Escreva uma preferência de conteúdo")]
        [Display(Name = "Preferência de conteúdo 2: :")]
        public string Preferencia2 { get; set; }

        //[Required(ErrorMessage = "Escreva uma preferência de conteúdo")]
        [Display(Name = "Preferência de conteúdo 3: :")]
        public string Preferencia3 { get; set; }

        public List<Post> postsAmigos { get; set; }
    }
}