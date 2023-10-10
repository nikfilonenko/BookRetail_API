using BookRetail_API.GraphQL.GraphTypes;
using BookRetail_API.Models;
using GraphQL.Types;

namespace BookRetail_API.GraphQL.GraphTypes;

public sealed class BookGraphType : ObjectGraphType<Book> {
    public BookGraphType() {
        Name = "book";
        Field(c => c.BookModel, nullable: false, type: typeof(ModelGraphType))
            .Description("The model of this particular vehicle");
        Field(c => c.Title);
        Field(c => c.Genre);
        Field(c => c.PublicationYear);
    }
}