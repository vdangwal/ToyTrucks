using System;

namespace Ordering.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public NotFoundException(string name, int id)
        {
            Id = id;
            Name = name;
        }
    }
}