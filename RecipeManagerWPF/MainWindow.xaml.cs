using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RecipeManagerWPF;

namespace RecipeManagerWPF
{
    public partial class MainWindow : Window
    {
        private RecipeManager recipeManager;

        public MainWindow()
        {
            InitializeComponent();
            this.FontFamily = new FontFamily("Segoe UI");
            this.Background = Brushes.LightGray;
            recipeManager = new RecipeManager();
            recipeManager.RecipeListUpdated += RecipeManager_RecipeListUpdated;
        }

        private void BtnEnterRecipe_Click(object sender, RoutedEventArgs e)
        {
            recipeManager.EnterRecipe();
            lstRecipes.Items.Clear(); 
        }

        private void BtnDisplayRecipes_Click(object sender, RoutedEventArgs e)
        {
            recipeManager.DisplayRecipes();
        }

        private void BtnScaleRecipe_Click(object sender, RoutedEventArgs e)
        {
            recipeManager.ScaleRecipe();
        }

        private void RecipeManager_RecipeListUpdated(object sender, List<Recipe> recipes)
        {
            lstRecipes.Items.Clear();
            foreach (Recipe recipe in recipes)
            {
                lstRecipes.Items.Add(recipe.Name);
            }
        }

        private void BtnFilterByIngredient_Click(object sender, RoutedEventArgs e)
        {
            string ingredientName = Microsoft.VisualBasic.Interaction.InputBox("Enter an ingredient name:", "Filter Recipes");
            if (!string.IsNullOrEmpty(ingredientName))
            {
                recipeManager.FilterByIngredient(ingredientName);
            }
        }

        private void BtnFilterByFoodGroup_Click(object sender, RoutedEventArgs e)
        {
            string foodGroup = Microsoft.VisualBasic.Interaction.InputBox("Enter a food group:", "Filter Recipes");
            if (!string.IsNullOrEmpty(foodGroup))
            {
                recipeManager.FilterByFoodGroup(foodGroup);
            }
        }

        private void BtnFilterByMaxCalories_Click(object sender, RoutedEventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter the maximum number of calories:", "Filter Recipes");
            if (int.TryParse(input, out int maxCalories))
            {
                recipeManager.FilterByMaxCalories(maxCalories);
            }
        }
    }
}