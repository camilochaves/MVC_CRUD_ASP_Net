using System;
using System.Collections.Generic;

namespace Domain.Common
{

    public interface IHasDomainEvents
    {
        public List<DomainEvent> DomainEvents { get; set; }        
    }

    public abstract class DomainEvent
    {
        public DateTimeOffset DateOccured { get; protected set; } = DateTime.UtcNow;
    }
    
  

}