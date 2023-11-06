using BookRetail_API.Data;
using BookRetail_API.GraphQL.GraphTypes;
using BookRetail_API.Models;
using GraphQL;
using GraphQL.Types;

namespace BookRetail_API.GraphQL.Mutations;

public class BookMutation: ObjectGraphType
{
    private readonly IBookRetailStorage _db;

    public BookMutation(IBookRetailStorage db)
    {
        this._db = db;
        
        Field<BookGraphType>(
            "updateBook",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "title"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "author"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "price"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "genre"},
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "publicationYear"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "productCode"}
            ),
            resolve: context =>
            {
                var title = context.GetArgument<string>("title");
                var author = context.GetArgument<string>("author");
                var price = context.GetArgument<string>("price");
                var genre = context.GetArgument<string>("genre");
                var year = context.GetArgument<int>("publicationYear");
                var modelCode = context.GetArgument<string>("productCode");

                var bookModel = _db.FindModel(modelCode);
                var existingBook = _db.FindBook(title);

                if (existingBook == null)
                {
                    throw new ExecutionError("Книга с указанным Title не найдена.");
                }
                
                existingBook.Title = title;
                existingBook.Genre = genre;
                existingBook.Author = author;
                existingBook.Price = price;
                existingBook.PublicationYear = year;
                existingBook.BookModel = bookModel;
                existingBook.ProductCode = bookModel.Code;

                _db.UpdateBook(existingBook);

                return existingBook;
            }
        );
        
        Field<BookGraphType>(
            "createBook",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "title"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "author"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "price"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "genre"},
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "publicationYear"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "productCode"}
            ),
            resolve: context =>
            {
                var title = context.GetArgument<string>("title");
                var author = context.GetArgument<string>("author");
                var price = context.GetArgument<string>("price");
                var genre = context.GetArgument<string>("genre");
                var year = context.GetArgument<int>("publicationYear");
                var modelCode = context.GetArgument<string>("productCode");

                var bookModel = db.FindModel(modelCode);
                
                if (bookModel == null)
                {
                    throw new ExecutionError("Книга с указанным productCode не найдена.");
                }
                
                var book = new Book
                {
                    Title = title,
                    Genre = genre,
                    Author = author,
                    Price = price,
                    PublicationYear = year,
                    BookModel = bookModel,
                    ProductCode = bookModel.Code
                };
                _db.CreateBook(book);
                return book;
            }
        );
    }
}