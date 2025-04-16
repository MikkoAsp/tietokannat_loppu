-- Insert User
INSERT INTO users (username, email, password)
VALUES (username: "user_name", email: "user_email", password: "user_password");

-- Insert Recipe (Linking user_id to the specific user)
INSERT INTO recipe (user_id, recipe_name, diet, dish)
VALUES (user_id: "user_id_value", recipe_name: "recipe_name", diet: "diet_type", dish: "dish_type");

-- Insert Ingredients (No need to specify recipe here, just ingredients)
INSERT INTO ingredient (ingredient_name)
VALUES (ingredient_name: "ingredient_name");

-- Insert Instructions (Linking to the recipe by recipe_id)
INSERT INTO instructions (cooking_instructions, step, recipe_id)
VALUES (cooking_instructions: "instruction_text", step: "step_number", recipe_id: "recipe_id_value");

-- Insert Recipe Ingredients (Linking both recipe_id and ingredient_id)
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity, unit_type)
VALUES (recipe_id: "recipe_id_value", ingredient_id: "ingredient_id_value", quantity: "ingredient_quantity", unit_type: "ingredient_unit");
