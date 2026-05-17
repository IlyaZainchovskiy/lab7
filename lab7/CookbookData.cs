using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab7
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<Recipe> Recipes { get; set; } = new List<Recipe>();
    }

    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string Instructions { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    }

    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }

    public class CookbookDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=cookbook.db");
        }
    }
}
