The example system with six use cases::

- Order VM by user
- Viewing the list of VMs by the user
- Viewing VM by user
- Viewing the list of VMs ordered by the user
- Viewing the VM ordered by the user
- Administrator deposits funds into the user's account

Architectural decisions:

- According to S, we divide the components by actor into the change axes: administrators and users.
- According to O, we divide the code by the speed of change into layers: core, use cases, virtual infrastructure, webapi, data.
- According to I we do not create universal contracts.
- According to D, we will wrap the dependencies to higher-level policies.
- Let's divide the system into two contexts: finance and virtual infrastructure.
- There will be a strict boundary between contexts.
- There will be a soft boundary between the context components.
- We will introduce a dependency on EF into the core to generate converters. This will be a backdoor. We will not use it for anything else.
- We will introduce a dependency on CSharpFunctionalExtensions to the core for error transmission.
- We will introduce a dependency on Newtonsoft.Json to the core for working with json
- We will introduce a dependency on Mediatr into the core to implement the CQRS pattern
- We will introduce a dependency on NodaTime into the core to work with dates