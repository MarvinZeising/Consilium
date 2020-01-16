﻿using System;

namespace Entities.DataTransferObjects
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime LastUpdatedTime { get; set; }
        public ProjectDto Project { get; set; }
    }
}
