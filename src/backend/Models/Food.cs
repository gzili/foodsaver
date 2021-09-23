﻿namespace backend.Models
{
    public class Food
    {
        public Food(int id, string name, string description, string imagePath, string unit)
        {
            Id = id;
            Name = name;
            Description = description;
            ImagePath = imagePath;
            Unit = unit;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; } 
        public string Unit { get; set; }

        public override string ToString()
        {
            return Id + ", " +  Name + ", " + Description + ", " + ImagePath + ", " + Unit;
        }
    }
}