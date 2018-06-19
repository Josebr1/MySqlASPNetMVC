using System;
using System.ComponentModel.DataAnnotations;

namespace MySqlASPNetMVC.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome da Pessoa é obrigatório")]
        public string Name { get; set; }
    }
}
