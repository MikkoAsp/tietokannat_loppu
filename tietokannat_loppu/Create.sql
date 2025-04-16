CREATE TYPE Diet AS ENUM ('None', 'Meat', 'Keto', 'Vegetarian', 'Vegan', 'LactoseFree', 'GlutenFree');
CREATE TYPE Dish AS ENUM ('None', 'Main', 'Side', 'Dessert', 'Drink');

CREATE TABLE users (
    user_id SERIAL PRIMARY KEY,
    username VARCHAR(55) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE instructions (
    instructions_id SERIAL PRIMARY KEY,
    cooking_instructions TEXT NOT NULL,
    step INTEGER NOT NULL
);

CREATE TABLE recipe (
    recipe_id SERIAL PRIMARY KEY,
    instructions_id INTEGER REFERENCES instructions(instructions_id),
    user_id INTEGER REFERENCES users(user_id),
    recipe_name VARCHAR(55) NOT NULL,
    diet Diet,
    dish Dish,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE ingredient (
    ingredient_id SERIAL PRIMARY KEY,
    ingredient_name VARCHAR(255) NOT NULL,
	recipe_id INTEGER REFERENCES recipe(recipe_id)
);

CREATE TABLE recipe_ingredients (
    recipe_id INTEGER REFERENCES recipe(recipe_id),
    ingredient_id INTEGER REFERENCES ingredient(ingredient_id),
    quantity NUMERIC(10, 2) NOT NULL CHECK (quantity > 0),
    unit_type VARCHAR(255),
    PRIMARY KEY (recipe_id, ingredient_id)
);
