using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace NASA_Bot
{
    //структура для получения фото дня
    public struct APOD
    {
        public string Date { get; set; } //дата
        public string Explanation { get; set; } //описание
        public string Title { get; set; } //подпись к картинке
        public string url { get; set; } //ссылка на картинку
    } 

    //структура для получения фото местности
    public struct Earth
    {
        public string Date { get; set; } //дата
        public string url { get; set; } //фото
    } 

    //структуры для получения фото из Марса
    public struct Camera
    {
        public int id { get; set; } //айди
        public string name { get; set; } //имя
        public int rover_id { get; set; } //айди манифеста
        public string full_name { get; set; } //полное имя
    } //структура камеры с данными
    public struct Rover
    {
        public int id { get; set; } //айди
        public string name { get; set; } //имя
        public string landing_date { get; set; } //дата посадки марсохода на Марс
        public string launch_date { get; set; } //дата запуска марсохода с Земли
        public string status { get; set; } //статус
    } //структура манифеста марсохода
    public struct Photo
    {
        public int id { get; set; } //айди
        public int sol { get; set; } //СОЛ
        public Camera camera { get; set; } //объект камеры
        public string img_src { get; set; } //фото
        public string earth_date { get; set; } //дата снимка
        public Rover rover { get; set; } //объект манифеста
    } //структура с фото и манифестом
    public struct Mars
    {
        public List<Photo> photos { get; set; }
    } //структура для десериализации

    //структуры для получения инфы по спутникам
    public struct Member
    {
        [JsonProperty("@type")]
        public string Type { get; set; } //тип спутника
        public int satelliteId { get; set; } //id спутника
        public string name { get; set; } //имя спутника
        public DateTime date { get; set; } //ближайшая дата инфы по спутнику

        //формат двустрочного элемента TLE
        public string line1 { get; set; } //первая строка
        public string line2 { get; set; } //вторая строка
    } //структура спутника с его инфой
    public struct Satellite
    {
        public int totalItems { get; set; } //всего спутников
        public List<Member> member { get; set; } //спутники
    } //структура для десериализации

    //структуры для получения инфы по проектам
    public struct ProjectCount
    {
        public int projectId { get; set; }
        public string lastUpdated { get; set; }
    } //структура проекта
    public struct RootProjectCount
    {
        public List<ProjectCount> projects { get; set; }
        public int totalCount { get; set; }
    } //структура списка проектов

    public struct PrimaryTaxonomyNode
    {
        public int taxonomyNodeId { get; set; }
        public int taxonomyRootId { get; set; }
        public int parentNodeId { get; set; }
        public int level { get; set; }
        public string code { get; set; }
        public string title { get; set; }
        public string definition { get; set; }
    } ////доп.инфа по проекту
    public struct LkuCodeType
    {
        public string codeType { get; set; }
        public string description { get; set; }
    } ////доп.инфа по проекту
    public struct Destination //доп.инфа по проекту
    {
        public int lkuCodeId { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public int lkuCodeTypeId { get; set; }
        public LkuCodeType lkuCodeType { get; set; }
    } 
    public struct PrincipalInvestigator
    {
        public int contactId { get; set; }
        public int displayOrder { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fullName { get; set; }
        public string fullNameInverted { get; set; }
        public string middleInitial { get; set; }
        public string primaryEmail { get; set; }
        public bool publicEmail { get; set; }
        public bool nacontact { get; set; }
    } //руководители
    public struct ProgramManager
    {
        public int contactId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fullName { get; set; }
        public string fullNameInverted { get; set; }
        public string middleInitial { get; set; }
        public string primaryEmail { get; set; }
        public bool publicEmail { get; set; }
        public bool nacontact { get; set; }
    } //менеджеры программы
    public struct ProjectManager
    {
        public int contactId { get; set; }
        public int displayOrder { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fullName { get; set; }
        public string fullNameInverted { get; set; }
        public string middleInitial { get; set; }
        public string primaryEmail { get; set; }
        public bool publicEmail { get; set; }
        public bool nacontact { get; set; }
    } //менеджеры проекта
    public struct CoInvestigator
    {
        public int contactId { get; set; }
        public int displayOrder { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string fullName { get; set; }
        public string fullNameInverted { get; set; }
        public string primaryEmail { get; set; }
        public bool publicEmail { get; set; }
        public bool nacontact { get; set; }
        public string middleInitial { get; set; }
    } //соучредители
    public struct Country
    {
        public string abbreviation { get; set; }
        public int countryId { get; set; }
        public string name { get; set; }
    } //страна
    public struct StatesWithWork
    {
        public string abbreviation { get; set; }
        public Country country { get; set; }
        public int countryId { get; set; }
        public string name { get; set; }
        public int stateTerritoryId { get; set; }
    } //штат работы
    public struct ProjectInfo
    {
        public string acronym { get; set; } //акроним
        public int projectId { get; set; } //id проекта
        public string title { get; set; } //заголовок
        public List<PrimaryTaxonomyNode> primaryTaxonomyNodes { get; set; } //Первичный узел таксономии
        public string benefits { get; set; } //преимущества
        public string description { get; set; } //описание
        public List<Destination> destinations { get; set; } //направления
        public int startYear { get; set; } //год начала
        public int startMonth { get; set; } //месяц начала
        public int endYear { get; set; } //год окончания
        public int endMonth { get; set; } //месяц окончания
        public string statusDescription { get; set; } //статус описания
        public List<PrincipalInvestigator> principalInvestigators { get; set; } //главные исследователи
        public List<ProgramManager> programManagers { get; set; } //менеджеры программы
        public List<ProjectManager> projectManagers { get; set; } //менеджеры проекта
        public List<CoInvestigator> coInvestigators { get; set; } //соисследователи
        public List<StatesWithWork> statesWithWork { get; set; } //штаты работы
        public string lastUpdated { get; set; } //последнее обновление
        public string releaseStatusString { get; set; } //статус выпуска
    }  //инфа по проекту
    public struct RootProject
    {
        public ProjectInfo project { get; set; } //проекты
    }  //для десериализации

    //структура для событий
    public struct EventInfo
    {
        public string messageType { get; set; }
        public string messageID { get; set; }
        public string messageURL { get; set; }
        public string messageIssueTime { get; set; }
        public string messageBody { get; set; }
    }

    //структуры для полихроматического изображения
    public struct CentroidCoordinates
    {
        public double lat { get; set; } //широта
        public double lon { get; set; } //долгота
    } //Географические координаты, на которые смотрит спутник
    public struct DscovrJ2000Position
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    } //Положение спутника в космосе
    public struct LunarJ2000Position
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    } //Положение Луны в космосе
    public struct SunJ2000Position
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
    } //Положение Солнца в космосе
    public struct Coords
    {
        public CentroidCoordinates centroid_coordinates { get; set; }
        public DscovrJ2000Position dscovr_j2000_position { get; set; }
        public LunarJ2000Position lunar_j2000_position { get; set; }
        public SunJ2000Position sun_j2000_position { get; set; }
    } //координаты
    public struct Polychromatic
    {
        public string identifier { get; set; } //идентификатор
        public string caption { get; set; } //подпись снимка
        public string image { get; set; } //изображение для запроса
        public string version { get; set; } //версия
        public string date { get; set; } //дата снимка
        public Coords coords { get; set; } //структура координат
    } //для десериализации


}
