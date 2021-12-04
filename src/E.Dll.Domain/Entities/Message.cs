using System;

namespace Domain.Entities
{
    public class Message<T> 
    {
        public DateTime Time { get; set; } = DateTime.Now;
        public T? Data {get; set;}
    }
}