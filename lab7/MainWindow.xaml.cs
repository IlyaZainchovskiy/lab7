using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace lab7
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            InitializeDatabase();
            LoadData();
        }

        private void InitializeDatabase()
        {
            using (var db = new CookbookDbContext())
            {
                db.Database.EnsureCreated();

                if (!db.Categories.Any())
                {
                    db.Categories.Add(new Category { Name = "Основні страви" });
                    db.Categories.Add(new Category { Name = "Десерти" });
                    db.SaveChanges();
                }
            }
        }

        private void LoadData()
        {
            using (var db = new CookbookDbContext())
            {
                var recipes = db.Recipes.Include(r => r.Category).ToList();
                RecipesListView.ItemsSource = recipes;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RecipeNameTextBox.Text)) return;

            using (var db = new CookbookDbContext())
            {
                var firstCategory = db.Categories.FirstOrDefault();

                var newRecipe = new Recipe
                {
                    Title = RecipeNameTextBox.Text,
                    Instructions = InstructionsTextBox.Text,
                    CategoryId = firstCategory?.CategoryId ?? 1
                };

                db.Recipes.Add(newRecipe);
                db.SaveChanges();
            }

            LoadData();
            ClearFields();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecipesListView.SelectedItem is Recipe selectedRecipe)
            {
                using (var db = new CookbookDbContext())
                {
                    var recipeToUpdate = db.Recipes.Find(selectedRecipe.RecipeId);
                    if (recipeToUpdate != null)
                    {
                        recipeToUpdate.Title = RecipeNameTextBox.Text;
                        recipeToUpdate.Instructions = InstructionsTextBox.Text;
                        db.SaveChanges();
                    }
                }
                LoadData();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecipesListView.SelectedItem is Recipe selectedRecipe)
            {
                using (var db = new CookbookDbContext())
                {
                    var recipeToDelete = db.Recipes.Find(selectedRecipe.RecipeId);
                    if (recipeToDelete != null)
                    {
                        db.Recipes.Remove(recipeToDelete);
                        db.SaveChanges();
                    }
                }
                LoadData();
                ClearFields();
            }
        }

        private void RecipesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecipesListView.SelectedItem is Recipe selectedRecipe)
            {
                RecipeNameTextBox.Text = selectedRecipe.Title;
                InstructionsTextBox.Text = selectedRecipe.Instructions;
            }
        }

        private void ClearFields()
        {
            RecipeNameTextBox.Text = string.Empty;
            InstructionsTextBox.Text = string.Empty;
            RecipesListView.SelectedItem = null;
        }
    }
}