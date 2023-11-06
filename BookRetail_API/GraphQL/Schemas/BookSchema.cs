using BookRetail_API.API.GraphQL.Queries;
using BookRetail_API.Data;
using BookRetail_API.GraphQL.Mutations;
using GraphQL.Types;

namespace BookRetail_API.GraphQL.Schemas;

public class BookSchema : Schema {
    public BookSchema(IBookRetailStorage db)
    {
         Query = new BookQuery(db);
         Mutation = new BookMutation(db);
    }
}