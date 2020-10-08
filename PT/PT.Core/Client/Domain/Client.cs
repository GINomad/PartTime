using PT.BuildingBlocks.Abstractions;
using System;

namespace PT.Core.Client.Domain
{
    public class Client: IIdentifiable<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
