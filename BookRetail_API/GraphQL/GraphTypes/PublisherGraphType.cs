using BookRetail_API.Models;
using GraphQL.Types;

namespace BookRetail_API.GraphQL.GraphTypes;

public sealed class PublisherGraphType : ObjectGraphType<Publisher> {
    public PublisherGraphType() {
        Name = "publisher";
        Field(c => c.Name).Description("The name of the publisher, e.g. Tesla, Volkswagen, Ford");
    }
}