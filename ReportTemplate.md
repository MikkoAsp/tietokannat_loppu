# **Project Report Template**

## **Title Page**

- **Project Name**: Recipe database project
- **Student Name(s)**:
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

- **High-Level Description**: Give a concise explanation of your schema’s purpose (e.g., to store recipes, ingredients, categories, etc.).
- **Diagram**: Insert or reference your Entity-Relationship Diagram (ERD) here. You can include it in this section.

### **Entities and Relationships**

- **List of Entities**: Describe each table (e.g., `Recipe`, `Ingredient`), including their main attributes.
- **Relationship Descriptions**: Explain the relationships (one-to-many, many-to-many, etc.) and how they are represented (junction tables, foreign keys, etc.).

### **Normalization & Constraints**

- **Normalization Level**: State the level of normalization (e.g., 3NF) you aimed for and why.
- **Constraints**: Discuss your use of primary keys, foreign keys, `NOT NULL`, `UNIQUE`, etc.

### **Design Choices & Rationale**

- **Reasoning**: Justify **why** you structured the schema the way you did. For instance, “We used a junction table for Recipe-Ingredient because it’s a many-to-many relationship.”
- **Alternatives Considered**: Note any alternative designs you evaluated and why you chose not to implement them.

---

## **Step 2: Database Implementation**

### **Table Creation**

- **SQL Scripts Overview**: Provide or reference your `CREATE TABLE` statements.
- **Explanation of Key Fields**: For each table, briefly explain the most important columns and their data types.
- **Constraints**: Show how you implemented the constraints (e.g., `PRIMARY KEY`, `FOREIGN KEY`, etc.) in SQL.

### **Data Insertion**

- **Sample Data**: Summarize the sample data you inserted. For example, 5 ingredients, 3 recipes, multiple categories, etc.
- **INSERT Statements**: Provide or reference your data insertion scripts (`INSERT INTO ...`).

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
