namespace realworldapp.Handlers
{
    interface IQueryPagination
    {
        int Offset { get; set; } 
        int Limit { get; set; }
    }
}
