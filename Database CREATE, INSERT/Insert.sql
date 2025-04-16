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

-- 1. Insert user
INSERT INTO users (username, email, password)
VALUES ('Markko Tirppula', 'Markko.Tirppula@gmail.com', 'RuokaMias69');

-- 2. Insert recipe (user_id = 2)
-- Instructions will be linked after
INSERT INTO recipe (user_id, recipe_name, diet, dish)
VALUES (2, 'Gnocchi with burnt butter and walnuts', 'Vegetarian', 'Main');

-- 3. Insert ingredients
INSERT INTO ingredient (ingredient_name)
VALUES
('Basic Potato Gnocchi'),     -- id = 10
('Butter'),                   -- id = 11
('Sage leaves'),              -- id = 12
('Garlic cloves'),            -- id = 13
('Coles Australian Baby Spinach'), -- id = 14
('Walnuts'),                  -- id = 15
('Grated parmesan');          -- id = 16



-- 4. Insert instructions (after recipe exists)
-- assuming recipe_id = 2
INSERT INTO instructions (cooking_instructions, step, recipe_id)
VALUES
('Line a large baking tray with baking paper. Bring a large saucepan of water to the boil. Add one-quarter of gnocchi. Cook for 2-3 mins or until the gnocchi rise to surface of the pan. Use a slotted spoon to transfer to lined tray. Repeat, in 3 batches, with remaining gnocchi.', 7, 2),
('Meanwhile, melt the butter in a large frying pan over medium heat. Cook for 2-3 mins or until the butter melts and begins to foam. Add the sage and cook for 1-2 mins or until crisp. Use a slotted spoon to transfer the sage to a plate.', 8, 2),
('Add the garlic, gnocchi and spinach to the pan. Season. Cook, tossing, until the gnocchi is coated in butter mixture and the spinach wilts.', 9, 2),
('Divide the gnocchi mixture among serving bowls. Sprinkle with fried sage and walnuts. Sprinkle with parmesan.', 10, 2);


-- 5. Insert recipe ingredients (recipe_id = 2)
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity, unit_type)
VALUES
(2, 10, 500, 'g'),       -- Basic Potato Gnocchi (estimated typical amount)
(2, 11, 50, 'g'),        -- Butter
(2, 12, 12, 'leaves'),   -- Sage leaves
(2, 13, 2, 'cloves'),    -- Garlic cloves
(2, 14, 60, 'g'),        -- Coles Australian Baby Spinach
(2, 15, 40, 'g'),        -- Walnuts
(2, 16, 30, 'g');        -- Grated parmesan


-- 1. Insert user
INSERT INTO users (username, email, password)
VALUES ('Tarmo Kovapoika', 'Kovapoika@email.com', 'Asdfasdfasdf');

-- 2. Insert recipe (user_id = 2)
-- Instructions will be linked after
INSERT INTO recipe (user_id, recipe_name, diet, dish)
VALUES (3, 'Tarmos Chickpea Curry with Spinach and Rice', 'Vegan', 'Main');

-- 3. Insert ingredients
INSERT INTO ingredient (ingredient_name)
VALUES
('Avocado oil'),     -- id = 17
('Garlic cloves'),   -- id = 18
('Brown sugar'),     -- id = 19
('Red curry paste'), -- id = 20
('Coconut milk'),    -- id = 21
('Soy sauce'),       -- id = 22
('Chickpeas'),       -- id = 23
('Fresh spinach'),   -- id = 24
('Cilantro'),        -- id = 25
('Jasmine rice');    -- id = 26


-- 4. Insert instructions (after recipe exists)
-- assuming recipe_id = 2
INSERT INTO instructions (cooking_instructions, step, recipe_id)
VALUES
('Cook rice according to package directions.', 11, 3),
('Heat the avocado oil over medium heat. Add the garlic and curry paste; sauté until softened and fragrant, about 1-2 minutes.', 12, 3),
('Add the brown sugar, coconut milk, and soy sauce. Bring to a low simmer until thickened slightly.', 13, 3),
('Add chickpeas, spinach, and cilantro; cook until chickpeas are heated through and spinach is wilted. Mash the chickpeas ever so slightly with the back of a wooden spoon if you want to change up the texture and make it more creamy.', 14, 3),
('Taste and adjust to your liking – optional: add lime, ginger, or lemongrass. Serve over rice with chili crisp and a side of pickled cucumber salad. Top with extra cilantro if desired.', 15, 3);


-- 5. Insert recipe ingredients (recipe_id = 3)
INSERT INTO recipe_ingredients (recipe_id, ingredient_id, quantity, unit_type)
VALUES
(3, 17, 2, 'tbsp'),     -- Avocado oil
(3, 18, 2, 'cloves'),   -- Garlic cloves
(3, 19, 1, 'tbsp'),     -- Brown sugar
(3, 20, 2, 'tbsp'),     -- Red curry paste
(3, 21, 400, 'ml'),     -- Coconut milk
(3, 22, 1.5, 'tbsp'),   -- Soy sauce
(3, 23, 400, 'g'),      -- Chickpeas (1 can)
(3, 24, 100, 'g'),      -- Fresh spinach
(3, 25, 0.25, 'cup'),   -- Cilantro (chopped)
(3, 26, 2, 'cups');     -- Jasmine rice (uncooked)
