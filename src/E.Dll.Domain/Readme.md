# Domain Layer  
  
What goes in Core ?

- A model of the problem space composed of Entities, Interfaces (IRepository, IUnitOfWork, IKafKa, etc. ), Services, Value Objects, Aggregates, Domain Services, Exceptions, DTO´s
- optional: Domain Events, Event Handlers, Specifications

## Typical Folders in order of dependencies

### Clean Architecture

- FrontEnd
- Application Interface (API)
- Service Interfaces (Application)
- Repository Interfaces (Data)
- Domain Core
