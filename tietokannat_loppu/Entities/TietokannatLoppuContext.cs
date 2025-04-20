using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace tietokannat_loppu.Entities;
public enum Dish
{
    [PgName("None")]
    None = 1,
    [PgName("Main")]
    Main = 2,
    [PgName("Side")]
    Side = 3,
    [PgName("Dessert")]
    Dessert = 4,
    [PgName("Drink")]
    Drink = 5

}
public enum Diet
{
    [PgName("None")]
    None = 1,
    [PgName("Meat")]
    Meat = 2,
    [PgName("Keto")]
    Keto = 3,
    [PgName("Vegetarian")]
    Vegetarian = 4,
    [PgName("Vegan")]
    Vegan = 5,
    [PgName("LactoseFree")]
    LactoseFree = 6,
    [PgName("GlutenFree")]
    GlutenFree = 7
}
public partial class TietokannatLoppuContext : DbContext
{
    public TietokannatLoppuContext()
    {
    }

    public TietokannatLoppuContext(DbContextOptions<TietokannatLoppuContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<Instruction> Instructions { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Tietokannat_loppu;Username=postgres;Password=admin", o => o.MapEnum<Dish>("dish").MapEnum<Diet>("diet"));


    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<Diet>();
        modelBuilder.HasPostgresEnum<Dish>();

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.IngredientId).HasName("ingredient_pkey");

            entity.ToTable("ingredient");

            entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");
            entity.Property(e => e.IngredientName)
                .HasMaxLength(255)
                .HasColumnName("ingredient_name");
        });

        modelBuilder.Entity<Instruction>(entity =>
        {
            entity.HasKey(e => e.InstructionsId).HasName("instructions_pkey");

            entity.ToTable("instructions");

            entity.Property(e => e.InstructionsId).HasColumnName("instructions_id");
            entity.Property(e => e.CookingInstructions).HasColumnName("cooking_instructions");
            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.Step).HasColumnName("step");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Instructions)
                .HasForeignKey(d => d.RecipeId)
                .HasConstraintName("instructions_recipe_id_fkey");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.Property(e => e.Diet).HasColumnName("diet");
            entity.Property(e => e.Dish).HasColumnName("dish");

            entity.HasKey(e => e.RecipeId).HasName("recipe_pkey");

            entity.ToTable("recipe");

            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.InstructionsId).HasColumnName("instructions_id");
            entity.Property(e => e.RecipeName)
                .HasMaxLength(55)
                .HasColumnName("recipe_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.InstructionsNavigation).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.InstructionsId)
                .HasConstraintName("recipe_instructions_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("recipe_user_id_fkey");
        });

        modelBuilder.Entity<RecipeIngredient>(entity =>
        {
            entity.HasKey(e => new { e.RecipeId, e.IngredientId }).HasName("recipe_ingredients_pkey");

            entity.ToTable("recipe_ingredients");

            entity.Property(e => e.RecipeId).HasColumnName("recipe_id");
            entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(10, 2)
                .HasColumnName("quantity");
            entity.Property(e => e.UnitType)
                .HasMaxLength(255)
                .HasColumnName("unit_type");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("recipe_ingredients_ingredient_id_fkey");

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeIngredients)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("recipe_ingredients_recipe_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(55)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
