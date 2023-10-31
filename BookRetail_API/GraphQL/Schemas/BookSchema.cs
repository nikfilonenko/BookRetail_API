using BookRetail_API.API.GraphQL.Mutations;
using BookRetail_API.API.GraphQL.Queries;
using BookRetail_API.Data;
using GraphQL.Types;

namespace BookRetail_API.GraphQL.Schemas;

public class BookSchema : Schema {
    public BookSchema(IBookRetailStorage db)
    {
         Query = new BookQuery(db);
         Mutation = new BookMutation(db);
    }
}