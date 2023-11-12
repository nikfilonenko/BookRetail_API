using BookRetail_API.GraphQL.GraphTypes;
using BookRetail_API.Models;
using GraphQL.Types;

namespace BookRetail_API.GraphQL.GraphTypes;

public sealed class BookGraphType : ObjectGraphType<Book> {
    public BookGraphType() {
        Name = "book";
        Field(c => c.BookModel, nullable: false, type: typeof(ModelGraphType))
            .Description("The model of this particular book");
        Field(c => c.Title);
        Field(c => c.Author);
        Field(c => c.ProductCode);
        Field(c => c.Genre);
        Field(c => c.PublicationYear);
    }
}