-- 1. Insert user
INSERT INTO users (username, email, password)
VALUES ('Jarkko Jarmonen', 'Jarkko.Jarmonen@gmail.com', 'JarkonKokkaukset27');

-- 2. Insert recipe (user_id = 1)
-- Instructions will be linked after
INSERT INTO recipe (user_id, recipe_name, diet, dish)
VALUES (1, 'Macaronibox', 'Meat', 'Main');

-- 3. Insert ingredients
INSERT INTO ingredient (ingredient_name)
VALUES
('Ground meat'),         -- id = 1
('Macaroni'),            -- id = 2
('Onion'),               -- id = 3
('Salt'),                -- id = 4
('Curry'),               -- id = 5
('White pepper'),        -- id = 6
('Paprika powder'),      -- id = 7
('Egg'),                 -- id = 8
('Milk or meat broth');  -- id = 9

-- 4. Insert instructions (after recipe exists)
-- assuming recipe_id = 1
INSERT INTO instructions (cooking_instructions, step, recipe_id)
VALUES
('If using pre-cooked macaroni, cook the macaroni according to the package instructions. You can also skip pre-cooking for an easier method.', 1, 1),
('Brown the ground meat and chopped onion in a frying pan. Season the cooked ground meat.', 2, 1),
('In a greased oven dish, layer macaroni and ground meat.', 3, 1),
('Mix the eggs and milk (or meat broth) together and pour the mixture evenly into the oven dish.', 4, 1),
('Bake on the lower rack of a 200°C oven for 1 hour.', 5, 1),
('If you did not pre-cook the macaroni, stir the mixture once or twice during the beginning or middle of baking to prevent the macaroni from settling at the bottom.', 6, 1);

-- 5. Insert recipe ingredients (recipe_id = 1)
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
