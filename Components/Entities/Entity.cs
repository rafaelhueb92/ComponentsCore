using System;

namespace Components.Entities
{

    public class Entity
    {

        public int Id { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public DateTime DataAlteracao { get; set; }

    }

}
