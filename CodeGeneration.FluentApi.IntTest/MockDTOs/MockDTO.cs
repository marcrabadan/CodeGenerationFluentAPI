using System;

namespace CodeGeneration.FluentApi.IntTest.MockDTOs
{
    public class MockDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birth { get; set; }
        public CountryDTO Country { get; set; }
        public string NIF { get; set; }
    }
}
