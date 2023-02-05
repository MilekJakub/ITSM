﻿#pragma warning disable CS8618

namespace ITSM.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
