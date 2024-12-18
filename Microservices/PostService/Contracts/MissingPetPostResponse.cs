﻿namespace PostService.Contracts
{
    public class MissingPetPostResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime MissingDate { get; set; }
        public int Gender { get; set; }
        public Guid BreadId { get; set; }
    }
}
