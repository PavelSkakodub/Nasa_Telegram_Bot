using Telegram.Bot.Types.ReplyMarkups;


namespace NASA_Bot
{
    public class Controller
    {
        //меню
        public static InlineKeyboardMarkup MenuBoard { get; set; }
        public static InlineKeyboardMarkup EarthBoard { get; set; }
        public static InlineKeyboardMarkup SpaceBoard { get;set; }
        public static ReplyKeyboardMarkup TechnologiesKeyboard { get; set; }

        //статический конструктор
        static Controller()
        {
            InitializeMenuBoard();
            InitializeEarthBoard();
            InitializeSpaceBoard();
            InitializeTechnologiesKeyBoard();
        } 

        //инициализация меню
        private static void InitializeMenuBoard()
        {
            MenuBoard = new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Космическое пространство"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Проекты NASA"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("События"),
                        },
                        new[]
                        {
                            InlineKeyboardButton.WithUrl("NASA English","https://www.nasa.gov/"),
                            InlineKeyboardButton.WithUrl("NASA Русский","https://rusnasa.ru/")
                        }
                    });
        } //главное меню
        private static void InitializeEarthBoard()
        {
            EarthBoard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Полихроматическое изображение"),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Информация о спутниках")

                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Фото местности"),
                }
            });
        } //меню Земля
        private static void InitializeSpaceBoard()
        {
            SpaceBoard = new InlineKeyboardMarkup(new[]
            {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Астрономическая картина дня"),
                },
                new[]
                {
                    InlineKeyboardButton.WithUrl("Поход по солнечной системе","https://trek.nasa.gov/")
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("Земля"),
                    InlineKeyboardButton.WithCallbackData("Марс")
                }
            });
        } //меню Космос
        private static void InitializeTechnologiesKeyBoard()
        {
            TechnologiesKeyboard = new ReplyKeyboardMarkup
            {
                Keyboard = new[] 
                {
                    new[]
                    {
                        new KeyboardButton("Номера проектов"),
                        new KeyboardButton("Данные проекта")
                    },
                },
                ResizeKeyboard = true
            };
        } //клавиатура выбора технологий
    }
}
