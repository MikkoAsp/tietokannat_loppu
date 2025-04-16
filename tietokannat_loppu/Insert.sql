-- Inserting user
INSERT INTO users (username, email, password)
VALUES ('Jarkko Jarmonen', 'Jarkko.Jarmonen@gmail.com', 'JarkonKokkaukset27');

-- Inserting instructions (steps)
INSERT INTO instructions (cooking_instructions, step)
VALUES
('If using pre-cooked macaroni, cook the macaroni according to the package instructions. You can also skip pre-cooking for an easier method.', 1),
('Brown the ground meat and chopped onion in a frying pan. Season the cooked ground meat.', 2),
('In a greased oven dish, layer macaroni and ground meat.', 3),
('Mix the eggs and milk (or meat broth) together and pour the mixture evenly into the oven dish.', 4),
('Bake on the lower rack of a 200°C oven for 1 hour.', 5),
('If you did not pre-cook the macaroni, stir the mixture once or twice during the beginning or middle of baking to prevent the macaroni from settling at the bottom.', 6);

-- Inserting ingredients
INSERT INTO ingredient (ingredient_name)
VALUES
('Ground meat'),
('Macaroni'),
('Onion'),
('Salt'),
('Curry'),
('White pepper'),
('Paprika powder'),
('Egg'),
('Milk or meat broth');

-- Inserting recipe (assuming `user_id` = 1 and last `instructions_id` = 1)
INSERT INTO recipe (instructions_id, user_id, recipe_name, diet, dish)
VALUES (1, 1, 'Macaronibox', 'Meat', 'Main');

-- Inserting recipe ingredients
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity, unit_type)
VALUES
(1, 2, 5.5, 'dl'),     -- Macaroni
(1, 1, 400, 'g'),      -- Ground meat
(1, 3, 1, 'pcs'),      -- Onion
(1, 4, 1.5, 'tsp'),    -- Salt
(1, 5, 1, 'tsp'),      -- Curry
(1, 6, 0.2, 'tsp'),    -- White pepper
(1, 7, 1, 'tsp'),      -- Paprika powder
(1, 8, 3, 'pcs'),      -- Egg
(1, 9, 7, 'dl');       -- Milk or meat broth
