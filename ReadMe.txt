Resource Managers
A RM is a system, product, or any component that manages data participating in transactions.
A RM either commits or rolls back changes
Examples include, SQL Server and MSMQ
Durable: resilient to system failures (machine restarts), SQL Server and MSMQ are durable types
Volatile: non-resilient to system failures, custom in-memory RMs such as Microsoft VRM

ACID
Atomicity: transaction succeeds or fails as a unit, i.e. all operations either commit or rollback together
Consistency:  remains in a consistent state after transaction completion, i.e. in databases, this means preserving referential integrity (primary and foreign keys etc)
Isolation: concurrent transactions have the same effect as if being run serially, i.e. in databases, this is the "Serialization Isolation Level", protect against dirty reads, phantom data, locking esstentially
Durability: a transaction result can sustain system failures, i.e. a transaction result is never lost once committed

Transaction Types
Long Running: long time to complete, sometimes referred to business transaction, waiting time for actions and/or messages, explicit commit & rollback (i.e. Compensation)
Atomic (Note: I did not say ACID!): take little time to finish, implicit commit & rollback, must satisfy all ACID attributes

Transaction Protocols
Lightweight: single application in an AppDomain, single durable RM, multiple volatile RMs, no cross AppDomain calls (no client - service calls)
OleTx: allows cross AppDomain, allows cross machine boundary calls, multiple durable RMs, Windows Remote Procedure Calls (RPC) calls only, no cross platform communication, usually not allowed through firewalls
WSAT: WS-Atomic protocol, one of the WS-* standards, same as OleTx plus interoperable communication

Transaction Managers
Lightweight Transaction Manager (LTM): .NET Framework 2.0, uses the Lightweight Protocol
Kernel Resource Manager (KRM): uses the Lightweight Protocol, ability to call on the transactional file system (TXF) and transactional registry (TXR) on windows
Distributed Transaction Manager (DTC): managing transactions across process and machine boundaries, uses either the OleTx or WSAT protocols