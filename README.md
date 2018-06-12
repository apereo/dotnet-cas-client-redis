# Apereo .NET CAS Client Redis backed Proxy/Service Ticket Managers #

This project is an add-on to the Apereo .NET CAS Client that implements the proxy and service ticket managers backed by a Redis data store.

By storing your proxy and service tickets in a centralized data store your applications running in a distributed, clustered or load balanced environment will all have access to the same proxy and service ticket data.  This is not possible to achieve with the default in-memory proxy and service ticket managers that ships with the Apereo .NET CAS Client.
