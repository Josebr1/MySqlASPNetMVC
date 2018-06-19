using MySqlASPNetMVC.Models;
using MySqlASPNetMVC.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace MySqlASPNetMVC.Application
{
    public class PersonApplication
    {
        private readonly Contexto contexto;

        public PersonApplication()
        {
            contexto = new Contexto();
        }

        public List<Person> ListAll()
        {
            var persons = new List<Person>();
            const string strQuery = "SELECT Id, FROM Person";

            var rows = contexto.ExecuteCommandSQL(strQuery);
            foreach (var row in rows)
            {
                var tempPerson = new Person
                {
                    Id = int.Parse(!string.IsNullOrEmpty(row["Id"]) ? row["Id"] : "0"),
                    Name = row["Name"]
                };
                persons.Add(tempPerson);
            }
            return persons;
        }

        public int Insert(Person person)
        {
            const string commandSQL = "INSERT INTO Person (Name) VALUES (@Name)";
            var parameters = new Dictionary<string, object>
            {
                {"Name", person.Name}
            };
            return contexto.ExecuteCommand(commandSQL, parameters);
        }

        private int update(Person person)
        {
            var commandSQL = "UPDATE Person SET Nome=@Name, WHERE Id=@Id";
            var parameters = new Dictionary<string, object>
            {
                {"Id", person.Id},
                {"Name", person.Name}
            };
            return contexto.ExecuteCommand(commandSQL, parameters);
        }

        public void Save(Person person) 
        {
            if (person.Id > 0)
                update(person);
            else
                Insert(person);
        }

        public int Delete(int Id)
        {
            const string commandSQL = "DELETE FROM Person WHERE Id=@Id";
            var parameters = new Dictionary<string, object>
            {
                {"Id", Id}
            };
            return contexto.ExecuteCommand(commandSQL, parameters);
        }

        public Person ListPersonForId(int Id)
        {
            var persons = new List<Person>();
            const string commandSQL = "SELECT Id, Name FROM Person WHERE Id=@Id";
            var parameters = new Dictionary<string, object>
            {
                {"Id", Id}
            };

            var rows = contexto.ExecuteCommandSQL(commandSQL, parameters);

            foreach (var row in rows)
            {
                var tempPerson = new Person()
                {
                    Id = int.Parse(!string.IsNullOrEmpty(row["Id"]) ? row["Id"] : "0"),
                    Name = row["Name"]
                };
                persons.Add(tempPerson);
            }
            return persons.FirstOrDefault();
        }
    }
}
