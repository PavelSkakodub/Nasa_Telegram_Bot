using System;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace NASA_Bot
{
    class NasaInfo
    {
        //API ключ
        private static readonly string key = "riXHM7NaEWlwHaVIsl19JvkQHhuq147sQ6q9KnrH";

        //запрос на фото дня по дате
        public static APOD GetAPOD(string date)
        {
            //запрос по ключу и ссылке
            string url = "https://api.nasa.gov/planetary/apod?date=" + date + "&api_key=" + key;
            //выполнение запроса и десериализация json строки в объект
            APOD apodJson = JsonConvert.DeserializeObject<APOD>(GetResponse(url));
            //возврат десериализованного объекта
            return apodJson;
        }

        //фото земли по координатам
        public static Earth GetEarth(string date, string latitude, string longitude)
        {
            //dim 0.025 to 1
            //запрос по ключу и ссылке
            string url = "https://api.nasa.gov/planetary/earth/assets?lon=" + longitude + "&lat=" + latitude + "&date=" + date + "&dim=0.1&api_key=" + key;
            //выполнение запроса и десериализация json строки в объект
            Earth earthJson = JsonConvert.DeserializeObject<Earth>(GetResponse(url));
            //возврат десериализованного объекта
            return earthJson;
        }

        //получение фото из марса
        public static Mars GetMarsPhoto(string date)
        {
            //запрос по ключу и ссылке
            string url = "https://api.nasa.gov/mars-photos/api/v1/rovers/curiosity/photos?earth_date=" + date + "&api_key=" + key;
            //выполнение запроса и десериализация json строки в объект
            Mars marsJson = JsonConvert.DeserializeObject<Mars>(GetResponse(url));
            //возврат десериализованного объекта
            return marsJson;
        } 

        //получаем кол-во спутников
        public static int GetSatellitesCount()
        {
            //запрос по ссылке и номеру страницы
            string url = "https://tle.ivanstanojevic.me/api/tle/?page=1";
            //выполнение запроса и десериализация json строки в объект
            Satellite satellites = JsonConvert.DeserializeObject<Satellite>(GetResponse(url));
            //возвращаем кол-во спутников
            return satellites.totalItems;
        }

        //получение файла с данными по спутникам на орбите Земли
        public static void GetSatellites(int count)
        {
            int first = 1; //нач.страница

            //массив страниц с спутниками(20 шт. в одном)
            Satellite[] satellites = new Satellite[count/20]; 

            for (int i = 0; i < satellites.Length; i++)
            {
                //запрос по ссылке и номеру страницы
                string url = "https://tle.ivanstanojevic.me/api/tle/?page=" + first;
                //выполнение запроса и десериализация json строки в объект
                satellites[i] = JsonConvert.DeserializeObject<Satellite>(GetResponse(url));
                //переход на след.страницу
                first++;
            }

            //запись данных о спутниках в файл
            using (StreamWriter sw = new StreamWriter(@"C:\Users\win10\source\repos\NASA_Bot\Satellites.txt", false, Encoding.Default))
            {        
                for (int i = 0; i < count; i++) 
                {
                    for (int j = 0; j < 20; j++)
                    {
                        //вывод основной информации
                        sw.WriteLine("Id спутника: " + satellites[i].member[j].satelliteId);
                        sw.WriteLine("Тип спутника: "+satellites[i].member[j].Type);
                        sw.WriteLine("Имя спутника: " + satellites[i].member[j].name);
                        sw.WriteLine("Дата последнего обновления: " + satellites[i].member[j].date);
                        //вывод дополнительной информации

                        //разбиваем строку на подстроки через пробелы и записываем с второй подстроки
                        string[] info1 = satellites[i].member[j].line1.Split(new char[] { ' ' }, 
                            StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();

                        sw.WriteLine($"Номер спутника: {info1[0][0..4]}");
                        sw.WriteLine($"Классификация: {info1[0][5]}");
                        sw.WriteLine($"Год запуска: {info1[1][0..2]}, номер: {info1[1][2..4]}, стартовая часть: {info1[1][5..6]}  ");
                        sw.WriteLine($"Эпоха: {info1[2]}");
                        sw.WriteLine($"1-ая производная ср.движения от времени: 0{info1[3]}");
                        sw.WriteLine($"2-ая производная ср.движения от времени: {info1[4]}");
                        sw.WriteLine($"BSTAR: {info1[5]}");
                        sw.WriteLine($"Тип эфемирид: {info1[6]}");
                        sw.WriteLine($"Номер элемента: {info1[7][0..3]}, контрольная сумма: {info1[7][3..]}");

                        //разбиваем строку на подстроки через пробелы и записываем с третьей подстроки 
                        string[] info2 = satellites[i].member[j].line2.Split(new char[] { ' ' },
                            StringSplitOptions.RemoveEmptyEntries).Skip(2).ToArray();

                        sw.WriteLine($"Наклон орбиты: {info2[0]}");
                        sw.WriteLine($"Прямое восхождение восходящего узла: {info2[1]}");
                        sw.WriteLine($"Экцентриситет: 0.{info2[2]}");
                        sw.WriteLine($"Аргумент Перигея: {info2[3]}");
                        sw.WriteLine($"Средняя аномалия: {info2[4]}");
                        sw.WriteLine($"Среднее движение: {info2[5]}");

                        //отступ для след.элемента
                        sw.WriteLine("\n");
                    }
                }
            }
        }

        //получение структуры (десериал-ой) проектов
        public static RootProjectCount GetCountProject()
        {
            //запрос по ссылке и номеру страницы
            string url = "https://api.nasa.gov/techport/api/projects?api_key=" + key;
            //выполнение запроса и десериализация json строки в объект
            RootProjectCount projectCount = JsonConvert.DeserializeObject<RootProjectCount>(GetResponse(url));
            //возвращаем структуру
            return projectCount;
        }

        //заполнение файла номеров проектов
        public static void GetFileProjectCount(RootProjectCount projectCount,int count)
        {
            //запись в файл айди и дату проектов
            using (StreamWriter sw = new StreamWriter(@"C:\Users\win10\source\repos\NASA_Bot\ProjectsID.txt", false, Encoding.Default))
            {
                for (int i = 0; i < count; i++)
                {
                    sw.WriteLine("Номер проекта: " + projectCount.projects[i].projectId);
                    sw.WriteLine("Последнее обновление: " + projectCount.projects[i].lastUpdated);
                    sw.WriteLine();
                }
            }
        }

        //заполнение файла с данными о проекте
        public static void GetTechnologies(int number)
        {
            //запрос по ссылке и номеру проекта
            string url = "https://api.nasa.gov/techport/api/projects/" + number + "?api_key=" + key;
            //выполнение запроса и десериализация json строки в объект
            RootProject Project = JsonConvert.DeserializeObject<RootProject>(GetResponse(url));
            //запись в файл дынных по проекту
            using (StreamWriter sw = new StreamWriter(@"C:\Users\win10\source\repos\NASA_Bot\ProjectInfo.txt", false, Encoding.Default))
            {
                sw.WriteLine($"Id проекта: {Project.project.projectId}");
                sw.WriteLine($"Аббревиатура: {Project.project.acronym}");
                sw.WriteLine($"Название: {Project.project.title}");
                sw.WriteLine($"Преимущества: {Project.project.benefits}");
                sw.WriteLine($"Описание: {Project.project.description}");
                sw.WriteLine($"Год начала: {Project.project.startYear}, месяц начала: {Project.project.startMonth}");
                sw.WriteLine($"Год окончания: {Project.project.endYear}, месяц окончания: {Project.project.endMonth}");
                sw.WriteLine($"Статус проекта: {Project.project.statusDescription}");
                sw.WriteLine($"Последнее обновление: {Project.project.lastUpdated}");
                sw.WriteLine($"Статус выпуска: {Project.project.releaseStatusString}");
                sw.WriteLine("\n");

                sw.WriteLine("---Места работы над проектом---");
                for (int i = 0; i < Project.project.statesWithWork.Count; i++)
                {
                    sw.WriteLine($"Id страны: {Project.project.statesWithWork[i].countryId}");
                    sw.WriteLine($"Страна: {Project.project.statesWithWork[i].country.name} ({Project.project.statesWithWork[i].country.abbreviation})");
                    sw.WriteLine($"Id штата: {Project.project.statesWithWork[i].stateTerritoryId}");
                    sw.WriteLine($"Штат: {Project.project.statesWithWork[i].name} ({Project.project.statesWithWork[i].abbreviation})");
                    sw.WriteLine("\n");
                }

                sw.WriteLine("---Направления проекта---");
                for (int i = 0; i < Project.project.destinations.Count; i++)
                {
                    sw.WriteLine($"Код LKU: {Project.project.destinations[i].lkuCodeId}");
                    sw.WriteLine($"Тип кода LKU: {Project.project.destinations[i].lkuCodeType.codeType} ({Project.project.destinations[i].lkuCodeType.description})");
                    sw.WriteLine($"Код: {Project.project.destinations[i].code}");
                    sw.WriteLine($"Описание: {Project.project.destinations[i].description}");
                    sw.WriteLine("\n");
                }

                sw.WriteLine("---Основные узлы таксономии---");
                for (int i = 0; i < Project.project.primaryTaxonomyNodes.Count; i++)
                {
                    sw.WriteLine($"Id узла таксономии: {Project.project.primaryTaxonomyNodes[i].taxonomyNodeId}");
                    sw.WriteLine($"Id корня таксономии: {Project.project.primaryTaxonomyNodes[i].taxonomyRootId}");
                    sw.WriteLine($"Id родительского узла: {Project.project.primaryTaxonomyNodes[i].parentNodeId}");
                    sw.WriteLine($"Уровень: {Project.project.primaryTaxonomyNodes[i].level}");
                    sw.WriteLine($"Код: {Project.project.primaryTaxonomyNodes[i].code}");
                    sw.WriteLine($"Название: {Project.project.primaryTaxonomyNodes[i].title}");
                    sw.WriteLine($"Описание: {Project.project.primaryTaxonomyNodes[i].definition}");
                    sw.WriteLine("\n");
                }

                sw.WriteLine("---Главные исследователи---");
                for (int i = 0; i < Project.project.principalInvestigators.Count; i++)
                {
                    sw.WriteLine($"Id контакта: {Project.project.principalInvestigators[i].contactId}");
                    sw.WriteLine($"Имя: {Project.project.principalInvestigators[i].firstName}");
                    sw.WriteLine($"Фамилия: {Project.project.principalInvestigators[i].lastName}");
                    sw.WriteLine($"Полное имя: {Project.project.principalInvestigators[i].firstName}");
                    sw.WriteLine($"Основной Email: {Project.project.principalInvestigators[i].primaryEmail}");
                    sw.WriteLine("\n");
                }

                sw.WriteLine("---Менеджеры программы---");
                for (int i = 0; i < Project.project.programManagers.Count; i++)
                {
                    sw.WriteLine($"Id контакта: {Project.project.programManagers[i].contactId}");
                    sw.WriteLine($"Имя: {Project.project.principalInvestigators[i].firstName}");
                    sw.WriteLine($"Фамилия: {Project.project.principalInvestigators[i].lastName}");
                    sw.WriteLine($"Полное имя: {Project.project.principalInvestigators[i].fullName}");
                    sw.WriteLine($"Основной Email: {Project.project.principalInvestigators[i].primaryEmail}");
                    sw.WriteLine("\n");
                }

                sw.WriteLine("---Соисследователи---");
                for (int i = 0; i < Project.project.programManagers.Count; i++)
                {
                    sw.WriteLine($"Id контакта: {Project.project.coInvestigators[i].contactId}");
                    sw.WriteLine($"Имя: {Project.project.coInvestigators[i].firstName}");
                    sw.WriteLine($"Фамилия: {Project.project.coInvestigators[i].lastName}");
                    sw.WriteLine($"Полное имя: {Project.project.coInvestigators[i].fullName}");
                    sw.WriteLine($"Основной Email: {Project.project.coInvestigators[i].primaryEmail}");
                    sw.WriteLine("\n");
                }
            }
        }

        //информация по событиям
        public static EventInfo GetEventInfo(string date)
        {
            //запрос по ссылке и номеру страницы
            string url = "https://api.nasa.gov/DONKI/notifications?startDate=" + date + "&type=all&api_key=" + key;
            //выполнение запроса и десериализация json строки в объект
            List<EventInfo> eventInfo = JsonConvert.DeserializeObject<List<EventInfo>>(GetResponse(url));
            //возвращаем структуру с случ. событием дня
            return eventInfo[new Random().Next(0,7)];
        }

        //полихроматическое изображение
        public static List<string> GetPolychromaticImage(string date)
        {
            //запрос по ссылке и дате
            string url = "https://api.nasa.gov/EPIC/api/natural/date/" + date + "?api_key=" + key;
            //выполнение запроса и десериализация json строки в объект
            List<Polychromatic> polychromatics = JsonConvert.DeserializeObject<List<Polychromatic>>(GetResponse(url));
            //конвертируем дату для получения изображений
            string dateImage = date.Split('-')[0] + "/" + date.Split('-')[1] + "/" + date.Split('-')[2];

            //создаём список изображений и добавляем ссылки на них
            List<string> images = new List<string>();
            for (int i = 0; i < polychromatics.Count; i++)
            {
                images.Add("https://epic.gsfc.nasa.gov/archive/natural/" + dateImage + "/png/" + polychromatics[i].image + ".png");
            }

            //выводим информацию по фото в файл
            using(StreamWriter sw = new StreamWriter(@"C:\Users\win10\source\repos\NASA_Bot\PolychromaticImage.txt"))
            {
                for (int i = 0; i < 10; i++)
                {
                    sw.WriteLine($"---Снимок №{i+1}---");
                    sw.WriteLine($"Идентификатор: {polychromatics[i].identifier} ,версия: {polychromatics[i].version}");
                    sw.WriteLine($"Описание: {polychromatics[i].caption}");
                    sw.WriteLine($"Время снимка: {polychromatics[i].date}");
                    sw.WriteLine($"Географические координаты, на которые смотрит спутник: {polychromatics[i].coords.centroid_coordinates.lat} (широта) {polychromatics[i].coords.centroid_coordinates.lon}(долгота)");
                    sw.WriteLine($"Положение спутника в космосе: {polychromatics[i].coords.dscovr_j2000_position.x}(x)  {polychromatics[i].coords.dscovr_j2000_position.y}(y)  {polychromatics[i].coords.dscovr_j2000_position.z}(z)");
                    sw.WriteLine($"Положение Луны в космосе: {polychromatics[i].coords.lunar_j2000_position.x}(x)  {polychromatics[i].coords.lunar_j2000_position.y}(y)  {polychromatics[i].coords.lunar_j2000_position.z}(z)");
                    sw.WriteLine($"Положение Солнца в космосе: {polychromatics[i].coords.sun_j2000_position.x}(x)  {polychromatics[i].coords.sun_j2000_position.y}(y)  {polychromatics[i].coords.sun_j2000_position.z}(z)");
                    sw.WriteLine($"\n");
                }
            }

            //возврат списка строк с изображениями
            return images;
        }

        //делаем запрос и возвращаем ответ
        private static string GetResponse(string request_url)
        {
            HttpWebRequest http_request = (HttpWebRequest)WebRequest.Create(request_url); //объект запрос
            HttpWebResponse http_response = (HttpWebResponse)http_request?.GetResponse(); //возвращает ответ на запрос

            string response; //ответ, полученный с потока
            using (StreamReader reader = new StreamReader(http_response.GetResponseStream()))
            {
                response = reader.ReadToEnd(); //считываем весь ответ
            }
            return response; //возврат ответа 
        } 
    }
}
