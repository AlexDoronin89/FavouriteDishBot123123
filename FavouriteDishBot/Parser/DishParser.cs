using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using FavouriteDishBot.Db.Models;

namespace FavouriteDishBot.Parser
{
    internal class DishParser
    {
        private const string MainUrl = "https://eda.ru/recepty?page=";

        private HtmlWeb htmlWeb;

        public DishParser()
        {
            htmlWeb = new HtmlWeb();
        }
        public Dish GetRandomDish()
        {
            List<Dish> dishes = GetRandomDishes();

            if (dishes.Count == 0)
            {
                throw new Exception("Ошибка в методе GetRandomDish при получении блюд при использовании метода GetRandomDishes");
            }

            Random random = new Random();
            int randomIndex = random.Next(0, dishes.Count);

            return dishes[randomIndex];
        }

        public List<Dish> GetRandomDishes()
        {
            List<Dish> dishes = new List<Dish>();

            List<string> links = GetAllRecipeLinks();

            Parallel.ForEach(links, link =>
            {
                Dish dish = ParseDishFromUrl(link);
                if (dish != null)
                {
                    lock (dishes)
                    {
                        dishes.Add(dish);
                    }
                }
            });

            return dishes;
        }

        private List<string> GetAllRecipeLinks()
        {
            List<string> links = new List<string>();
            Random random = new Random();

            int minPageNumber = 1;
            int maxPageNumber = 714;

            string url = MainUrl + random.Next(minPageNumber, maxPageNumber);
            HtmlDocument document = htmlWeb.Load(url);
            HtmlNodeCollection linkNodes = document.DocumentNode.SelectNodes("//a[@class='emotion-18hxz5k']");

            foreach (HtmlNode node in linkNodes)
            {
                string relativeLink = node.GetAttributeValue("href", "");
                if (!string.IsNullOrEmpty(relativeLink))
                {
                    string absoluteLink = "https://eda.ru" + relativeLink;

                    links.Add(absoluteLink);
                }
            }

            return links;
        }

        private Dish ParseDishFromUrl(string url)
        {
            Dish dish = new Dish();

            try
            {
                HtmlDocument document = htmlWeb.Load(url);

                HtmlNode titleNode = document.DocumentNode.SelectSingleNode("//h1[@class='emotion-gl52ge']");
                if (titleNode != null)
                {
                    dish.Title = titleNode.InnerText.Replace("<.*?>",String.Empty).Trim();
                }
                else { throw new Exception("Ошибка в получении названия" + "" + url); }

                HtmlNodeCollection ingredientTypeNode = document.DocumentNode.SelectNodes("//span[@class='emotion-mdupit']");
                HtmlNodeCollection ingredientCountNode = document.DocumentNode.SelectNodes("//span[@class='emotion-bsdd3p']");
                if (ingredientTypeNode != null && ingredientCountNode != null)
                {
                    for (int i = 0; i < ingredientTypeNode.Count; i++)
                    {
                        HtmlNode ingredientType = ingredientTypeNode[i];
                        HtmlNode ingredientCount = ingredientCountNode[i];

                        dish.Ingredients += $"{ingredientType.InnerText} {ingredientCount.InnerText} \n";
                    }
                }
                else { throw new Exception("Ошибка в получении ингредиентов" + "" + url); }


                HtmlNodeCollection recipeNode = document.DocumentNode.SelectNodes("//span[@class='emotion-wdt5in']");
                if (recipeNode != null)
                {
                    for (int i = 0; i < recipeNode.Count; i++)
                    {
                        HtmlNode recipe = recipeNode[i];

                        dish.Recipe += $"{i + 1}. {recipe.InnerText.Trim()} \n";
                    }
                }
                else { throw new Exception("Ошибка в получении рецепта" + "" + url); }

            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при парсинге блюда по ссылке {url}: {ex.Message}");
            }

            return dish;
        }
    }
}
