using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace RecipeManagerApp
{
    class RecipeManager
    {
        private List<Recipe> recipes = new List<Recipe>();
        private MainWindow mainWindow;

        public RecipeManager()
        {
            mainWindow = Application.Current.MainWindow as MainWindow;
        }

        public void EnterRecipe()
        {
            // Code to enter a new recipe
            string recipeName = GetUserInput("Please enter the name of the recipe");

            List<Ingredient> ingredients = GetIngredients();

            List<string> steps = GetSteps();

            Recipe recipe = new Recipe
            {
                Name = recipeName,
                Ingredients = ingredients.ToArray(),
                Steps = steps.ToArray()
            };

            recipes.Add(recipe);
            UpdateRecipeList();
        }

        private List<Ingredient> GetIngredients()
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            int numIngredients = int.Parse(GetUserInput("Enter the number of ingredients: "));

            for (int i = 0; i < numIngredients; i++)
            {
                string name = GetUserInput($"Enter name of Ingredient {i + 1}:");
                double quantity = double.Parse(GetUserInput($"Enter quantity for {name}:"));
                string unit = GetUserInput($"Enter unit of measurement for {name}:");
                int calories = int.Parse(GetUserInput($"Enter number of calories for {name}:"));
                string foodGroup = GetUserInput($"Enter food group for {name}:");

                ingredients.Add(new Ingredient
                {
                    Name = name,
                    OriginalQuantity = quantity,
                    Quantity = quantity,
                    Unit = unit,
                    Calories = calories,
                    FoodGroup = foodGroup
                });
            }

            return ingredients;
        }

        private List<string> GetSteps()
        {
            List<string> steps = new List<string>();
            int numSteps = int.Parse(GetUserInput("Enter the number of steps: "));

            for (int i = 0; i < numSteps; i++)
            {
                string step = GetUserInput($"Enter description for step {i + 1}:");
                steps.Add(step);
            }

            return steps;
        }

        private string GetUserInput(string message)
        {
            string input = string.Empty;
            Application.Current.Dispatcher.Invoke(() =>
            {
                input = Microsoft.VisualBasic.Interaction.InputBox(message, "Recipe Manager");
            });
            return input;
        }

        public void DisplayRecipes()
        {
            if (recipes.Count == 0)
            {
                ShowMessage("No recipes have been entered yet.");
                return;
            }

            string recipeList = "Recipes:\n";
            for (int i = 0; i < recipes.Count; i++)
            {
                recipeList += $"{i + 1}. {recipes[i].Name}\n";
            }

            int recipeIndex = int.Parse(GetUserInput(recipeList + "\nEnter the recipe number to display:")) - 1;

            if (recipeIndex < 0 || recipeIndex >= recipes.Count)
            {
                ShowMessage("Invalid recipe number.");
                return;
            }

            DisplayRecipe(recipes[recipeIndex]);
        }

        public void DisplayRecipe(Recipe recipe)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{recipe.Name}");

            sb.AppendLine("\nIngredients:");
            int totalCalories = 0;
            for (int i = 0; i < recipe.Ingredients.Length; i++)
            {
                int caloriesForIngredient = (int)(recipe.Ingredients[i].Calories * recipe.Ingredients[i].Quantity);
                sb.AppendLine($"{i + 1}. {recipe.Ingredients[i].Quantity} {recipe.Ingredients[i].Unit} of {recipe.Ingredients[i].Name} ({caloriesForIngredient} calories, {recipe.Ingredients[i].FoodGroup})");
                totalCalories += caloriesForIngredient;
            }

            sb.AppendLine($"\nTotal Calories: {totalCalories}");

            if (totalCalories > 300)
            {
                sb.AppendLine("Warning: This recipe exceeds 300 calories.");
            }

            sb.AppendLine("\nSteps:");
            for (int i = 0; i < recipe.Steps.Length; i++)
            {
                sb.AppendLine($"{i + 1}. {recipe.Steps[i]}");
            }

            ShowMessage(sb.ToString());
        }

        public void ScaleRecipe()
        {
            if (recipes.Count == 0)
            {
                ShowMessage("No recipes have been entered yet.");
                return;
            }

            string recipeList = "Recipes:\n";
            for (int i = 0; i < recipes.Count; i++)
            {
                recipeList += $"{i + 1}. {recipes[i].Name}\n";
            }

            int recipeIndex = int.Parse(GetUserInput(recipeList + "\nEnter the recipe number to scale:")) - 1;

            if (recipeIndex < 0 || recipeIndex >= recipes.Count)
            {
                ShowMessage("Invalid recipe number.");
                return;
            }

            string scalingOptions = "Choose scaling factor:\n1. 0.5 (Half)\n2. 2 (Double)\n3. 3 (Triple)";
            int scalingChoice = int.Parse(GetUserInput(scalingOptions + "\nEnter your choice:"));

            double scaleFactor = GetScaleFactor(scalingChoice);

            Recipe recipe = recipes[recipeIndex];
            Ingredient[] scaledIngredients = ScaleIngredients(recipe.Ingredients, scaleFactor);

            Recipe scaledRecipe = new Recipe
            {
                Name = recipe.Name,
                Ingredients = scaledIngredients,
                Steps = recipe.Steps
            };

            DisplayRecipe(scaledRecipe);
        }

        private Ingredient[] ScaleIngredients(Ingredient[] ingredients, double scaleFactor)
        {
            Ingredient[] scaledIngredients = new Ingredient[ingredients.Length];

            for (int i = 0; i < ingredients.Length; i++)
            {
                scaledIngredients[i] = new Ingredient
                {
                    Name = ingredients[i].Name,
                    OriginalQuantity = ingredients[i].OriginalQuantity,
                    Quantity = ingredients[i].Quantity * scaleFactor,
                    Unit = ingredients[i].Unit,
                    Calories = ingredients[i].Calories,
                    FoodGroup = ingredients[i].FoodGroup
                };
            }

            return scaledIngredients;
        }

        private double GetScaleFactor(int choice)
        {
            switch (choice)
            {
                case 1: return 0.5;
                case 2: return 2.0;
                case 3: return 3.0;
                default: return 1.0;
            }
        }

        private void ShowMessage(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(message, "Recipe Manager");
            });
        }

        private void UpdateRecipeList()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                mainWindow.lstRecipes.Items.Clear();
                foreach (Recipe recipe in recipes)
                {
                    mainWindow.lstRecipes.Items.Add(recipe.Name);
                }
            });
        }
    }

    class Recipe
    {
        public string Name { get; set; }
        public Ingredient[] Ingredients { get; set; }
        public string[] Steps { get; set; }
    }

    struct Ingredient
    {
        public string Name;
        public double OriginalQuantity;
        public double Quantity;
        public string Unit;
        public int Calories;
        public string FoodGroup;
    }