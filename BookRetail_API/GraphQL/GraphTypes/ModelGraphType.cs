using BookRetail_API.Models;
using GraphQL.Types;

namespace BookRetail_API.GraphQL.GraphTypes;

public sealed class ModelGraphType : ObjectGraphType<ProductModel> {
    public ModelGraphType() {
        Name = "model";
        Field(m => m.Code).Description("The code of this model, e.g. Golf, Beetle, 5 Series, Model X");
        Field(m => m.Publisher, type: typeof(PublisherGraphType)).Description("The make of this model of book");
    }
}