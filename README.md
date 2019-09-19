# Extended Dapper (BETA)

Extends Dapper functionality with a repository (CRUD & LINQ), `OneToMany` and `ManyToOne` mappings.

**This is still a WIP and not ready for production use!** Though it has been been tested in various (non-critical) production environments.

## Known issues
- ~OneToMany mapping doesn't work correctly when the includes aren't in the correct order~ (FIXED)

## Changelog
[View here](CHANGELOG.md)

## TODO

- ~Make sure all queries generate properly (`SELECT`, `UPDATE`, `DELETE`, `INSERT`)~
- ~Mapping to & from POCOs~ 
- ~Make sure `OneToMany` and `ManyToOne` mappings properly apply to `INSERT`s, `UPDATE`s, `DELETE`s & `SELECT`s~
- ~Repositories~
- ~Setup local DB for unittests~
- Write unittests (working on it)
- Setup CI/CD for automated deployment to NuGet
- ~Transaction support~
- Setup documentation
- (Proper) support for more than 1 primary key
- Optimize reflection calls
- Optimize relation mapping (should be able to better implement it with Dapper)
- Implement more `SqlProviders` and refactor them
- Lazy loading of `OneToMany` and `ManyToOne`
- Implement `ManyToMany` attribute
- Better error-handling implementation
- Do benchmarks