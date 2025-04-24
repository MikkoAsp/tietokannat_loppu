# **Loppuraportti**

## **Reseptisovellus**

- **Project Name**: Reseptisovellus
- **Student Name(s)**: Joona Mäkelä, Mikko Asp ja Onni Valtonen
- **Course/Module**: Databases 2025

---

## **Table of Contents**

1. [Step 1: Database Design](#step-1-database-design)
   1. [Schema Overview](#schema-overview)
   2. [Entities and Relationships](#entities-and-relationships)
   3. [Normalization & Constraints](#normalization--constraints)
   4. [Design Choices & Rationale](#design-choices--rationale)
2. [Step 2: Database Implementation](#step-2-database-implementation)
   1. [Table Creation](#table-creation)
   2. [Data Insertion](#data-insertion)
   3. [Validation & Testing](#validation--testing)
3. [Step 3: .NET Core Console Application Enhancement](#step-3-net-core-console-application-enhancement)
   1. [EF Core Configuration](#ef-core-configuration)
   2. [Implemented Features](#implemented-features)
   3. [Advanced Queries & Methods](#advanced-queries--methods)
4. [Challenges & Lessons Learned](#challenges--lessons-learned)
5. [Conclusion](#conclusion)

---

## **Step 1: Database Design**

### **Schema Overview**

- **High-Level Description**: Aloitimme tehtävän luomalla ER-kaavion, jota käytimme perustana reseptisovelluksen tietokannan luomiselle. ER-kaavion löytää tästä samasta GitHub-repositoriosta. Reseptisovellus on tarkoitettu yksittäisten henkilöiden reseptien tallentamiseen omaan henkilökohtaiseen käyttöön, ei varsinaisesti niiden jakamiseen. Sovellukseen kirjaudutaan omalla tunnuksella, jonka voi luoda käyttöliittymää avattaessa. Reseptit ovat kategorisoitu ruokatyypin sekä ruokalajin perusteella.
  
- **Diagram**: ER-Diagrammi löytyy tästä GitHub-repositoriosta.

### **Entities and Relationships**

- **List of Entities**:
- Recipen primääriavain on RecipeId, Instructionsin on InstructionsId, Recipe_Ingredientsillä on yhdistelmäavain, Ingredientillä IngredientId, Userilla UserId.
- Entiteettejä tietokannassamme ovat User, Recipe, Instructions, Recipe_Ingredients sekä Ingredient. Diet sekä Dish ovat Enumeraattoreita.

- **Relationship Descriptions**:
- Recipe.RecipeId M - M Recipe_Ingredients.RecipeId
- Recipe.RecipeId M - 1 Instructions.RecipeId
- Recipe.InstructionsId M - 1 InstructionsId
- Recipe.UserId M - 1 User.UserId
- Recipe_Ingredients.IngredientId 1 - M Ingredient.IngredientId
- Recipen ja Instructions taulun välillä on kaksi yhteyttä koska halusimme yhdistää instructions taulussa olevat kokkausohjeet yksittäisiin vaiheisiin.

### **Normalization & Constraints**

- **Normalization Level**:
- Meidän normalisaatio on 3NF tasolla, instructions taulussa on sirculaarinen dependenssi recipe taulun kanssa ylläolevasta syystä.
  
- **Constraints**:
- User taulussa oleva UserId toimii Primary Keynä. Halusimme käyttäjän email:in olevan sen yksilöivä tekijä ja että samalla sähköpostilla ei voi tehdä useampaa tiliä, siksi se on UNIQUE. Samassa taulussa username, email sekä password ovat NOT NULL koska niitä vaaditaan kirjautumiseen. Jokaisen käyttäjän nimi generoidaan satunnaisesti algoritmillä olevassa olevasta sanaluettelosta joita yhdistetään satunnaisesti.
- Recipe taulu sisältää primääriavaimen recipe_id joka on myös UNIQUE. Recipetaulussa on myös enumeraattorit Diet sekä Dish joilla yksilöidään reseptejä.
- Recipe_Ingredients taulussa luodaan yhdistelmäavain recipe_id:n ja ingredient_id:n kanssa.
- Ingredient taulu sisältää primääriavaimen ingredient_id jolla yksilöidään eri ainesosia jos niitä halutaan käyttää useammassa reseptissä.
- Instructions taulussa on sirculaarinen yhteys recipe_id:llä jota tarvitsimme sovellusratkaisuun jotta pystyimme yksilöimään reseptien instruction askeleet. Tämä ei välttämättä ollut paras ratkaisu, mutta päädyimme tähän loppujen lopuksi.

### **Design Choices & Rationale**:

- **Reasoning** 
- Ulkoistimme käyttäjän omaksi tauluksi, koska halusimme luoda kirjautumiskäyttöliittymän ja yksilöidä jokaisen reseptin henkilökohtaiseksi. Käyttäjiä voi olla vain yksi sovelluksessa yhdellä laitteella.
- Ingredient taulu luotiin siksi, että pystyimme välttämään duplikaattien luomisen kun useita reseptejä luotiin. Loimme yhdistelmätaulun recipe_ingredients toimimaan recipen ja ingredientin välillä yllämainitusta syystä.
- Instructions taulu on erillinen recipestä koska halusimme eritellä instructions taulussa sisältävät asiat erikseen. Jokaisella reseptillä täytyy olla valmistusvaiheet, siksi käytämme kahta yhdistävää tekijää recipe taulun kanssa.
- Käytimme enumeraattoreita diet ja dish taulujen sijaan koska taulut olivat ennalta valittuja ja taulujen sisältö olisi käynyt pieneksi. Enumeraattoreilla pystyimme pitää "taulujen" koon aina samana.
  
---

## **Step 2: Database Implementation**

### **Table Creation**

- **SQL Scripts Overview**:
- Enumeraattorien käyttö
     - Estää kirjoitusvirheet
     - Kategoriat aina samat
     - Taulut pieniä, ei tarvetta luoda erillisiä tauluja Diet ja Dish Enumeraattoreille
- Users taulu
     - Tallentaa käyttäjätiedot
     - sähköposti on UNIQUE, käyttäjä ei voi luoda kahta tiliä samalla sähköpostilla
     - Yksilöi reseptit
- Recipe
     - Tallentaa reseptien tiedot
     - Ei voi olla NULL
     - Primääriavaimena user_id
- Ingredient
     - Yksittäiset raaka-aineet
     - Ei redundanssia dataa
- Recipe Ingredients
     - Yhdistää reseptit ja ainekset
     - Kardinaliteetti M - M
- Instructions
     - Vaiheittaiset reseptien valmistusohjeet
     - Ohje ei voi olla tyhjä, ja jos resepti poistetaan niin ohjeet poistetaan
- Lisäsimme Alter Table komennon myöhemmin koska halusimme yhdistää ohjeet reseptiin.

### **Data Insertion**

- **Sample Data**:
- Tietokantaan lisätyt syötteet luotiin jotta saisimme kehitettyä sovelluksen. Insert tiedosto löytyy GitHub repositoriosta.
- Sovelluksella pystyy luomaan omia syötteitä.

### **Validation & Testing**

- **Basic Queries**: 
- Select * From recipe;

-	"Macaronibox"	"Meat"	"Main"	"2025-04-21 21:08:42.280538+03"
-	"Gnocchi with burnt butter and walnuts"	"Vegetarian"	"Main"	"2025-04-21 21:08:42.280538+03"
-	"Tarmos Chickpea Curry with Spinach and Rice"	"Vegan"	"Main"	"2025-04-21 21:08:42.280538+03"

- Ylläolevalla kysellä haetaan kaikki reseptit

- SELECT r.recipe_name, i.ingredient_name, ri.quantity, ri.unit_type 
- FROM recipe r
- JOIN recipe_ingredients ri ON r.recipe_id = ri.recipe_id
- JOIN ingredient i ON ri.ingredient_id = i.ingredient_id
- WHERE r.recipe_id = 1;

   - "Macaronibox"	"Ground meat"	400.00	"g"
   - "Macaronibox"	"Macaroni"	5.50	"dl"
   - "Macaronibox"	"Onion"	1.00	"pcs"
   - "Macaronibox"	"Salt"	1.50	"tsp"
   - "Macaronibox"	"Curry"	1.00	"tsp"
   - "Macaronibox"	"White pepper"	0.20	"tsp"
   - "Macaronibox"	"Paprika powder"	1.00	"tsp"
   - "Macaronibox"	"Egg"	3.00	"pcs"
   - "Macaronibox"	"Milk or meat broth"	7.00	"dl"

- Ylläolevalla kyselyllä haetaan kaikki reseptin ainekset

## **Step 3: .NET Core Console Application Enhancement**

### **EF Core Configuration**

- **Connection String**: 
- Käytimme opetustehtävissä tehtyä tapaa scaffoldata tietokanta projektiin
- Visual Studio loi entiteetit automaattisesti, muutimme ainoastaan rivit 145 ja 150 .OnDelete(DeleteBehavior.Cascade):ksi alkuperäisestä automatisoidusta scaffoldauksesta.
- **DbContext**:
- IDE automatisoi kontekstin meidän puolesta.

### **Implemented Features**

- **CRUD Operations**:
- Create
   - Kysytään käyttäjältä tiedot
   - Haetaan käyttäjä sähköpostilla tietokannasta
   - Tiedot tallennetaan tauluun
   - Taulun tiedot lähetetään tietokantaan
- Read
     - Etsitään User_Id:llä kaikki reseptit
     - Etsitään kaikki relevantit tiedot, jotka vastaavat user_id:tä
     - Listataan reseptit käyttäjälle nähtäväksi
- Update
     - Haetaan tietokannasta resepti
     - Kysytään käyttäjältä tarvittavat muutokset reseptiin
     - Päivitetään tietokantaan tarvittavat muutokset reseptille
- Delete
     - Etsitään käyttäjän omista resepteistä reseptiId:llä resepti joka valitaan poistettavaksi
     - Kysytään haluaako käyttäjä poistaa kyseisen reseptin
     - Resepti poistetaan tietokannasta
       
- **Advanced Features**:
- Kategorianmuutos löytyy Task UpdateRecipeInDb (rivi 112 DatabaseManagerista)
- Haku Dietin ja Dishin perusteella on identtinen

   var results = await dbContext.Recipes
     .Include(recipe => recipe.RecipeIngredients)
     .ThenInclude(recipeIngredients => recipeIngredients.Ingredient)
     .Include(recipe => recipe.Instructions)
     .Where(recipe => recipe.UserId == user.Id && recipe.Dish / Diet == dish / diet)
     .ToListAsync();

     return  results;

- Haku Ingredient perusteella on täsmällinen haku

   var results = await dbContext.Recipes
    .Include(recipe => recipe.RecipeIngredients)
    .ThenInclude(ri => ri.Ingredient)
    .Include(recipe => recipe.Instructions)
    .Where(recipe => recipe.UserId == user.Id &&
    recipe.RecipeIngredients.Any(ri => ingredients.Contains(ri.Ingredient.IngredientName)))
    .ToListAsync();

   return results;

---

### **Advanced Queries & Methods**

- **LINQ Queries**: Yllämainittu ingredient haku

   var results = await dbContext.Recipes
            .Include(recipe => recipe.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient)
            .Include(recipe => recipe.Instructions)
            .Where(recipe => recipe.UserId == user.Id &&
            recipe.RecipeIngredients.Any(ri => ingredients.Contains(ri.Ingredient.IngredientName)))
            .ToListAsync();

   return results;

- **Performance Considerations**:
- Performanssi ei ollut meidän prioriteetti, tehtävän tekeminen loppuun oli etusijalla.

---

## **Challenges & Lessons Learned**

- **Obstacles Faced**:
- Tehtävä toimi hyvänä kokonaisuutena databasen yhdistämisessä applikaatioon. Ryhmätyötä tehdessä oppi myös hyödyntämään GitHubia projektin jakamiseen. Haastavaa oli organisoida tehtävä, niin että työnjako olisi tasainen. Tässä ryhmätyössä se ei tapahtunut. Projektimanagerin käyttö olisi voinut olla hyvä ratkaisu tähän ongelmaan.
- Aikaisempiin projekteihin ei tullut käytettyä abstraktiota ollenkaan, mutta tässä tehtävässä sovelluksen tekemisessä harjaantui kyseisen suunnittelumallin käytössä.
- Tietokannan suunnittelussa isona plussana oli Enumeraattoireiden käyttö pienille tauluille mikä teki sovelluksen kehittämisestä huomattavasti helpompaa
- **Key Takeaways**:
- Käytä projektimanageria tiimitöissä
- Käytä enumeraattoreita pienien taulujen sijaan
- Käytä aikaa tietokannan suunnitteluun, koska hyvin suunniteltu tietokanta helpottaa sovelluksen koodausta

---

## **Conclusion**

- **Project Summary**: 
- Projektista tuli kompleksimpi mitä alkujaan oli ajateltu, mutta kaikki tehtävässä vaaditut kriteerit täytettiin
- Tietokannan integrointi projektiin onnistui nopeasti
- Sovellusta testattaessa ohjelmisto ei kaatunut
- **Future Enhancements**: 
- Reseptien jakaminen muille käyttäjille
- Reseptien arvostelu
- Graafisen käyttöliittymän luominen

---

### **Instructions for Use**

1. Lataa projekti GitHubista Visual Studioon
2. Pullaa uusin päivitys
3. Vaihda TietokannatLoppuContext riviltä 61 tietokantasi nimi sekä salasana
4. Luo tietokanta tämän GitHub repositorion kyselyillä (Database CREATE, INSERT kansio)
5. Käynnistä sovellus, voit käyttää DEBUG vaihtoehtoa joka kirjautuu debuggaus käyttäjätilillä sovellukseen
6. Seuraa ohjelman ohjeita
