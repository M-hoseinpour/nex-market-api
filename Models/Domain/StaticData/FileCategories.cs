using market.Data.Domain;

namespace market.Data.StaticData;

public class FileCategories
{
    public static FileCategory Pictures { get; } =
        new()
        {
            Id = 1,
            Title = "Pictures",
            Description = "All The public pictures, can be hosted in cdn",
            SubDirectory = "pictures"
        };

    public static FileCategory Documents { get; } =
        new()
        {
            Id = 2,
            Title = "Documents",
            Description =
                "All kind of documents, like identity or ownership documents. "
                + "This files are private all the time",
            SubDirectory = "documents"
        };
}