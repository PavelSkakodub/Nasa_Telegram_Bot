using System;
using Telegram.Bot;
using System.IO;
using Telegram.Bot.Types.InputFiles;
using System.Collections.Generic;
using Telegram.Bot.Types;
using System.Text.RegularExpressions;

namespace NASA_Bot
{
    class NasaBot
    {
        private readonly string token = "5221000376:AAHliROhNHNxDSa4e1F6G0zv2qTisQasQCM"; //токен бота
        private static TelegramBotClient client; //клиент бота

        //условия ввода от пользователя 
        private static bool IsNumberProject;
        private static bool IsAPOD;
        private static bool IsLocallity;
        private static bool IsSatellite;
        private static bool IsProject;
        private static bool IsMars;
        private static bool IsEvent;
        private static bool IsPolychromatic;

        public NasaBot()
        {
            Initialize();
        } //конструктор бота

        private void Initialize()
        {
            client = new TelegramBotClient(token); //инициализация бота

            //выводим информацию о боте
            var me = client.GetMeAsync().Result;
            Console.WriteLine($"Bot_ID: {me.Id} | Bot_Name: {me.FirstName}");

            //делаем соединеие и добавляем обработчик
            client.StartReceiving();
            client.OnMessage += Client_OnMessage;
            client.OnCallbackQuery += Client_OnCallbackQuery;
            Console.ReadLine();
            client.StopReceiving();
        } //инициализация бота
        private void DefaultInput()
        {
            IsNumberProject = IsAPOD = IsLocallity = IsSatellite = IsProject = IsMars = IsEvent = IsPolychromatic = false;
        } //убираем ввод от юзера при переходе между меню

        //обработчик кнопок
        private async void Client_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            DefaultInput(); //убираем ввод от юзера при клике на новые кнопки

            switch (e.CallbackQuery.Data)
            {
                #region Меню Космическое пространство
                case "Космическое пространство":
                    await client.SendPhotoAsync(e.CallbackQuery.From.Id, "https://i.yapx.ru/QLJoi.jpg", replyMarkup: Controller.SpaceBoard);
                    break;

                case "Астрономическая картина дня":
                    await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "Введите дату для получения снимка. Формат даты: гг-мм-дд. \nНапример: 2022-02-02.");
                    IsAPOD = true; //возможность ввода данных от пользователя       
                    break;

                #region Меню кнопок Земля
                case "Земля":
                    await client.SendPhotoAsync(e.CallbackQuery.From.Id, "https://kosmosgid.ru/wp-content/uploads/2019/10/Planeta-Zemlya-1.jpg",replyMarkup:Controller.EarthBoard);
                    break;

                case "Фото местности":
                    await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "Чтобы получить фото введите через двоеточие Дату (гг-мм-дд), Широту и Долготу (с точкой). \nПример: 2021-02-02:55.4507:37.3656");
                    IsLocallity = true;
                    break;

                case "Информация о спутниках":
                    int count = NasaInfo.GetSatellitesCount();
                    await client.SendTextMessageAsync(e.CallbackQuery.From.Id, $"Всего спутников на орбите {count}.\nДанные выводятся в соотношении 20:1, поэтому введите число от 0 до {count/20}.");
                    IsSatellite = true;
                    break;

                case "Полихроматическое изображение":
                    await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "Для получения снимков введите дату в формате гг-мм-дд");
                    IsPolychromatic = true;
                    break;
                #endregion

                case "Марс":
                    await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "Для получения снимка с Марса введите дату в формате гг-мм-дд. \nНапример: 2015-6-3.");
                    IsMars = true;
                    break;

                #endregion

                case "Проекты NASA":
                    await client.SendPhotoAsync(e.CallbackQuery.From.Id, "https://image.freepik.com/free-photo/digital-technology-network-data-and-communication-concept-abstract-background-earth-element-furnished-by-nasa_24070-628.jpg", 
                        $"Здесь вы можете посмотреть существующие проекты NASA, в том числе завершённые. \n\nДля этого кликните в меню 'Номера проектов' и укажите сколько номеров вам нужно. Проекты отсортированы по дате обновления.\n\nПосле этого кликните 'Данные проекта' и введите выбранный номер.", replyMarkup: Controller.TechnologiesKeyboard);       
                    break;

                case "События":
                    await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "Для получения информации о событиях введите дату в формате гг-мм-дд. \nЕсли в этот день не было событий вы получите ближайшее к этому дню.");
                    IsEvent = true;
                    break;
            }
        }

        //обработчик введённых сообщений
        private static async void Client_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message; //получем сообщение от юзера

            if (message.Text != null) //если сообщение не пустое
            {
                #region Обработка ввода от юзера
                if (IsNumberProject)
                {
                    try
                    {
                        //получаем номера проектов
                        RootProjectCount projectCount = NasaInfo.GetCountProject();
                        //выбор кол-ва номеров
                        NasaInfo.GetFileProjectCount(projectCount, Convert.ToInt32(message.Text));
                        //конвертируем в онлайн-документ
                        using (FileStream stream = System.IO.File.OpenRead(@"C:\Users\win10\source\repos\NASA_Bot\ProjectsID.txt"))
                        {
                            InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, "Номера проектов.txt");
                            await client.SendDocumentAsync(message.Chat.Id, inputOnlineFile);
                        }
                        IsNumberProject = false;
                        return; //делаем возврат чтобы не шло выполнение switch и пр.
                    }
                    catch
                    {
                        await client.SendTextMessageAsync(message.Chat.Id, "Неверно введены данные! Введите целое число в указанном диапазоне.");
                        return;
                    }
                } //обработка ввода для номеров проектов
                if(IsProject)
                {
                    try
                    {
                        //получаем данные проекта
                        NasaInfo.GetTechnologies(Convert.ToInt32(message.Text));
                        //конвертируем в онлайн - документ
                        using (FileStream stream = System.IO.File.OpenRead(@"C:\Users\win10\source\repos\NASA_Bot\ProjectInfo.txt"))
                        {
                            InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, $"NASA проект #{message.Text}.txt");
                            await client.SendDocumentAsync(message.Chat.Id, inputOnlineFile);
                        }
                        IsProject = false;
                        return;
                    }
                    catch
                    {
                        await client.SendTextMessageAsync(message.Chat.Id, "Проект не найден! Проверьте правильность номера и повторите.");
                        return;
                    }
                } //обработка ввода для данных проекта
                if(IsAPOD)
                {
                    try
                    {
                        APOD apod = NasaInfo.GetAPOD(message.Text);
                        //выводим пост с инфой
                        await client.SendPhotoAsync(message.Chat.Id, apod.url,$"\t{apod.Title}\n\n{apod.Explanation}");
                        return; //делаем возврат чтобы не было след.проверок
                    }
                    catch
                    {
                        await client.SendTextMessageAsync(message.Chat.Id, "Для такой даты нет картины! Проверьте правильность ввода или измените дату.");
                        return; //делаем возврат чтобы не было след.проверок
                    }
                } //обработка ввода для картины дня
                if(IsLocallity)
                {
                    try
                    {
                        string[] info = message.Text.Split(':'); //разбиваем сообщение на подстроки
                        Earth earth = NasaInfo.GetEarth(info[0], info[1], info[2]);
                        await client.SendPhotoAsync(message.Chat.Id, photo: earth.url, $"Ближайшая дата снимка {earth.Date}");
                        return; //делаем возврат чтобы не было след.проверок
                    }
                    catch
                    {
                        await client.SendTextMessageAsync(message.Chat.Id, "Снимка не существует! Измените дату (например 2021-02-02) или координаты.");
                        return; //делаем возврат чтобы не было след.проверок
                    }
                } //обработка ввода для фото местности
                if(IsSatellite)
                {
                    try
                    {
                        NasaInfo.GetSatellites(Convert.ToInt32(message.Text));
                        //конвертируем в онлайн-документ
                        using (FileStream stream = System.IO.File.OpenRead(@"C:\Users\win10\source\repos\NASA_Bot\Satellites.txt"))
                        {
                            InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, "Спутники.txt");
                            await client.SendDocumentAsync(message.From.Id, inputOnlineFile);
                        }
                        return;
                    }
                    catch
                    {
                        await client.SendTextMessageAsync(message.Chat.Id, "Неверно введены данные! Укажите целое число в указанном диапазоне.");
                        return;
                    }
                } //обработка ввода для инфы по спутникам
                if(IsMars)
                {
                    try
                    {
                        //получаем информацию
                        Mars mars = NasaInfo.GetMarsPhoto(message.Text);
                        //формируем текст поста
                        string result =
                           $"Камера: {mars.photos[0].camera.id} {mars.photos[0].camera.full_name}\nSOL: {mars.photos[0].sol}\nДата снимка: {mars.photos[0].earth_date}" +
                           $"\nМанифест: {mars.photos[0].rover.id} {mars.photos[0].rover.name}\nДата посадки на Марс: {mars.photos[0].rover.landing_date}\nДата запуска марсохода с Земли: {mars.photos[0].rover.launch_date}\nСтатус: {mars.photos[0].rover.status}";
                        //создаём группу фото и добавляем в неё снимки с объекта mars
                        List<InputMediaPhoto> mediaPhotos = new List<InputMediaPhoto>();
                        foreach(var s in mars.photos)
                        {
                            mediaPhotos.Add(new InputMediaPhoto(new InputMedia(s.img_src)));
                        }
                        //отправляем группу фото и текст с инфой
                        await client.SendMediaGroupAsync(message.Chat.Id, mediaPhotos);
                        await client.SendTextMessageAsync(message.Chat.Id, result);
                        return;
                    }
                    catch
                    {
                        await client.SendTextMessageAsync(message.Chat.Id, "Снимка не существует! Проверьте правильность ввода или измените дату.");
                        return;
                    }
                } //обработка ввода для инфы с Марса
                if(IsEvent)
                {
                    try
                    {
                        //получаем событие по вводу даты
                        EventInfo eventInfo = NasaInfo.GetEventInfo(message.Text);
                        //шаблон для определения ссылки в строке
                        Regex regex = new Regex(@"(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?");
                        //получаем коллекцию ссылок на файлы события
                        MatchCollection matches = regex.Matches(eventInfo.messageBody);

                        await client.SendTextMessageAsync(message.Chat.Id, $"<b>Id события:</b> {eventInfo.messageID} \n<b>Тип события:</b> {eventInfo.messageType} \n<b>Время события:</b> {eventInfo.messageIssueTime} \n<b>Посмотреть событие:</b> {eventInfo.messageURL}",Telegram.Bot.Types.Enums.ParseMode.Html);
                        await client.SendTextMessageAsync(message.Chat.Id, eventInfo.messageBody);

                        //вывод существующих файлов события
                        if (matches.Count != 0) 
                        {
                            for (int i = 0; i < matches.Count; i++)
                            {
                                try
                                {
                                    await client.SendDocumentAsync(message.Chat.Id, new InputOnlineFile(matches[i].Value));
                                }
                                catch { continue; }
                            }
                        }
                        return;
                    }
                    catch
                    {
                        await client.SendTextMessageAsync(message.Chat.Id, "Событий нету! Проверьте правильность ввода или измените дату.");
                        return;
                    }
                } //обработка ввода для инфы события
                if(IsPolychromatic)
                {
                    try
                    {
                        //получаем список снимков
                        List<string> images = NasaInfo.GetPolychromaticImage(message.Text);
                        //добавляем снимки в группу фото
                        List<InputMediaPhoto> mediaPhotos = new List<InputMediaPhoto>();
                        //добавляем только 10 фото (больше нельзя)
                        for (int i = 0; i < 10; i++)  
                        {
                            mediaPhotos.Add(new InputMediaPhoto(new InputMedia(images[i])));
                        }                      
                        await client.SendMediaGroupAsync(message.Chat.Id, mediaPhotos);
                        //конвертируем в онлайн-документ
                        using (FileStream stream = System.IO.File.OpenRead(@"C:\Users\win10\source\repos\NASA_Bot\PolychromaticImage.txt"))
                        {
                            InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, $"Снимки-{message.Text}.txt");
                            await client.SendDocumentAsync(message.From.Id, inputOnlineFile);
                        }
                        return;
                    }
                    catch
                    {
                        await client.SendTextMessageAsync(message.Chat.Id, "Снимка не существует! Проверьте правильность ввода или измените дату.");
                        return;
                    }
                } //обработка ввода для поли-снимков
                #endregion

                switch (message.Text)
                {
                    #region Обработка сообщений меню
                    case "/start":
                        string text = 
@"Список доступных команд:
/start - Запуск бота
/menu - Командное меню
/info - Инструкции для команд
/about - Информация о боте";
                        await client.SendTextMessageAsync(message.Chat.Id, text);
                        break;

                    case "/menu":
                        await client.SendPhotoAsync(message.Chat.Id, "https://www.zarubejom.ru/wp-content/uploads/2019/11/nasa.jpg", replyMarkup: Controller.MenuBoard);
                        break;
                    #endregion

                    #region Обработка клавиатуры
                    case "Номера проектов":
                        RootProjectCount projectCount = NasaInfo.GetCountProject(); //получение номеров
                        await client.SendTextMessageAsync(message.Chat.Id, $"Введите сколько номеров выдать (от 0 до {projectCount.totalCount}):");
                        IsNumberProject = true; //возможность обработки ввода номера
                        break;
                    case "Данные проекта":
                        await client.SendTextMessageAsync(message.Chat.Id, "Введите номер проекта. Чтобы посмотреть все существующие проекты кликните меню 'Номера проектов'.");
                        IsProject = true;
                        break;
                    #endregion

                    default:
                        await client.SendTextMessageAsync(message.Chat.Id, "Команда введена неверно!");
                        break;
                }
            }
        }
    }
}
