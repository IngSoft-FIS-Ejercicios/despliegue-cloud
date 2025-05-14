using Microsoft.EntityFrameworkCore;
using Repository;

namespace RepositoryTests.Context;

public static class SqlContextFactory
{
    public static SqlContext CreateMemoryContext()
    {
        var optionBuilder = new DbContextOptionsBuilder<SqlContext>();
        optionBuilder.UseInMemoryDatabase("TestDB");
        return new SqlContext(optionBuilder.Options);
    }
}