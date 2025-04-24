# **Loppuraporttie**

## **Title Page**

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
   4. [Design Choices & Rationale](#design-choices--rationale-2)
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

- **Normalization Level**: State the level of normalization (e.g., 3NF) you aimed for and why.
- Meidän normalisaatio on 3NF tasolla, instructions taulussa on circulaarinen dependenssi recipe taulun kanssa ylläolevasta syystä.
- **Constraints**:
- User taulussa oleva UserId toimii Primary Keynä. Halusimme käyttäjän email:in olevan sen yksilöivä tekijä ja että samalla sähköpostilla ei voi tehdä useampaa tiliä, siksi se on UNIQUE. Samassa taulussa username, email sekä password ovat NOT NULL koska niitä vaaditaan kirjautumiseen. Jokaisen käyttäjän nimi generoidaan satunnaisesti algoritmillä olevassa olevasta sanaluettelosta joita yhdistetään satunnaisesti.
- Recipe taulu sisältää primääriavaimen recipe_id joka on myös UNIQUE. Recipetaulussa on myös enumeraattorit Diet sekä Dish joilla yksilöidään reseptejä.
- Recipe_Ingredients taulussa luodaan yhdistelmäavain recipe_id:n ja ingredient_id:n kanssa.
- Ingredient taulu sisältää primääriavaimen ingredient_id jolla yksilöidään eri ainesosia jos niitä halutaan käyttää useammassa reseptissä.
- Instructions taulussa on circulaarinen yhteys recipe_id:llä jota tarvitsimme sovellusratkaisuun jotta pystyimme yksilöimään reseptien instruction askeleet. Tämä ei välttämättä ollut paras ratkaisu, mutta päädyimme tähän loppujen lopuksi.

### **Design Choices & Rationale**

- **Reasoning**: 
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

### **Data Insertion** // Tästä jatkuu seuraavaksi!

- **Sample Data**: Summarize the sample data you inserted. For example, 5 ingredients, 3 recipes, multiple categories, etc.
- Tietokantaan lisätyt syötteet luotiin jotta saisimme kehitettyä sovelluksen. Insert tiedosto löytyy GitHub repositoriosta.
- Sovelluksella pystyy luomaan omia syötteitä.

### **Validation & Testing**

- **Basic Queries**: Document a few test queries you ran using `psql` or another tool (e.g. `pgAdmin`) to confirm your data was inserted correctly.
- **Results**: Summarize the outcome (e.g., “Query shows 3 recipes in the `Recipe` table. Each has multiple entries in `RecipeIngredient`. No foreign key violations.”)

---

## **Step 3: .NET Core Console Application Enhancement**

### **EF Core Configuration**

- **Connection String**: Describe where and how you manage the database connection string (e.g., `appsettings.json`, environment variables).
- **DbContext**: Summarize your e.g. `RecipeDbContext` setup, how you map entities.

### **Implemented Features**

- **CRUD Operations**: Explain your approach for Create, Read, Update, and Delete methods (e.g., adding new recipes, listing all recipes).
- **Advanced Features**: List any advanced features such as searching by multiple ingredients or retrieving recipes by category.

### **Advanced Queries & Methods**

- **LINQ Queries**: Show or summarize the LINQ queries used for more complex requirements (e.g., “Fetch recipes containing all specified ingredients”).
- **Performance Considerations**: If relevant, mention any indexes or optimizations you added.

---

## **Challenges & Lessons Learned**

- **Obstacles Faced**: Mention any difficulties you encountered (e.g., configuring EF Core, handling many-to-many relationships).
- **Key Takeaways**: Summarize the primary lessons you learned about database design, SQL, and .NET Core development.

---

## **Conclusion**

- **Project Summary**: Recap the final state of the project, including major accomplishments (e.g., fully functioning console app integrated with your Postgres database).
- **Future Enhancements**: Suggest any next steps or improvements you would make if you had more time (e.g., add user authentication, implement rating systems, or expand the domain).

---

### **Instructions for Use**

1. **Fill Out Each Section**: Provide clear, concise, and **original** explanations.
2. **Include Screenshots or Snippets**: If it helps clarify a point (e.g., menu output from the console app, partial code snippets).
3. **Maintain Professional Formatting**: Use consistent headers, bullet points, and references.
