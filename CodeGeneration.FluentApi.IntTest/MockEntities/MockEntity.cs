using System;

namespace CodeGeneration.FluentApi.IntTest.MockEntities
{
    public class MockEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birth { get; set; }
        public CountryEntity Country { get; set; }
        public string NIF { get; set; }
        public string PhoneNumber { get; set; }

        public string AimsName { get; set; }
    }
}
