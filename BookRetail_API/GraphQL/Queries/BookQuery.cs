using BookRetail_API.Data;
using BookRetail_API.GraphQL.GraphTypes;
using BookRetail_API.Models;
using GraphQL;
using GraphQL.Types;

namespace BookRetail_API.API.GraphQL.Queries;

public class BookQuery : ObjectGraphType {
    private readonly IBookRetailStorage _db;

    public BookQuery(IBookRetailStorage db) {
        this._db = db;

        Field<ListGraphType<BookGraphType>>("Books", "Query to retrieve all Books",
            resolve: GetAllBooks);

        Field<BookGraphType>("Book", "Query to retrieve a specific Book",
            new QueryArguments(MakeNonNullStringArgument("title", "The title of the Book")),
            resolve: GetBook);

        Field<ListGraphType<BookGraphType>>("BooksByGenre", "Query to retrieve all Books matching the specified genre",
            new QueryArguments(MakeNonNullStringArgument("genre", "The name of a genre, eg 'fiction', 'romance'")),
            resolve: GetBookByGenre);
    }

    private QueryArgument MakeNonNullStringArgument(string name, string description) {
        return new QueryArgument<NonNullGraphType<StringGraphType>> {
            Name = name, Description = description
        };
    }

    private IEnumerable<Book> GetAllBooks(IResolveFieldContext<object> context) => _db.ListBooks();

    private Book GetBook(IResolveFieldContext<object> context) {
        var title = context.GetArgument<string>("title");
        return _db.FindBook(title);
    }

    private IEnumerable<Book> GetBookByGenre(IResolveFieldContext<object> context) {
        var genre = context.GetArgument<string>("genre");
        var books = _db.ListBooks().Where(v => v.Genre.Contains(genre, StringComparison.InvariantCultureIgnoreCase));
        return books;
    }
}