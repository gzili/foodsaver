using System.Collections.Generic;
using backend.Models;

namespace backend.Services
{
    public static class DataLoadingService
    {
        private const string offersPath = "OFFERS PATH HERE";
        private const string usersPath = "USERS PATH HERE";
        private const string foodsPath = "FOODS PATH HERE";

        public static List<Offer> LoadOffers()
        {
            return FileService<List<Offer>>.ReadJson(offersPath);
        }

        public static List<User> LoadUsers()
        {
            return FileService<List<User>>.ReadJson(usersPath);
        }

        public static List<Food> LoadFoods()
        {
            return FileService<List<Food>>.ReadJson(foodsPath);
        }


        public static void SaveOffers(List<Offer> offers)
        {
            FileService<List<Offer>>.WriteJson(offersPath, offers);
        }

        public static void SaveUsers(List<User> users)
        {
            FileService<List<User>>.WriteJson(usersPath, users);
        }

        public static void SaveFoods(List<Food> foods)
        {
            FileService<List<Food>>.WriteJson(foodsPath, foods);
        }
    }
}