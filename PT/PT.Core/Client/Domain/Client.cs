using PT.BuildingBlocks.Abstractions;
using System;

namespace PT.Core.Client.Domain
{
    public class Client: IIdentifiable<int>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
    }
}
