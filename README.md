# Extended Dapper (BETA)

[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Extended.Dapper)](https://www.nuget.org/packages/Extended.Dapper)

Extends Dapper with a repository (CRUD & LINQ) and native `OneToMany` & `ManyToOne` mappings.

**This is still a WIP and not ready for production use!** Though it has been been tested in various (non-critical) production environments.

## Getting started

[View here](docs/getting-started.md)

## Features

- Implements repositories with `CRUD` actions such as `Insert`, `Update`, `Get`, `GetById`, `GetAll` & `Delete` on top of Dapper.
- LINQ queries/searches executed as native SQL queries (no client-side filtering).
- Support for `OneToMany` and `ManyToOne` attributes on entity properties.
    - Choose per `Get(All)` which children you want to include
    - Lazy loading with dedicated methods
- Support for SQLite, MSSQL and MySQL/MariaDB. More planned.

## Changelog
[View here](CHANGELOG.md)

## Known issues
- None; if you find one; please create an issue :)

## TODO

- Write unittests (working on it, most of the EntityRepository is covered)
- ~Setup CI/CD for automated deployment to NuGet~
- Setup documentation
- (Proper) support for more than 1 primary key
- Support for autovalues other than guid/uuid
- Optimize reflection calls
- Optimize relation mapping (should be able to better implement it with Dapper)
- Implement more `SqlProviders`
- Lazy loading of `OneToMany` and `ManyToOne` (semi-implemented; loading must be done manually)
- Implement `ManyToMany` attribute
- Do benchmarks