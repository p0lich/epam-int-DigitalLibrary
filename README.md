# Digital library(Электронная библиотека)

### Введение
Приложение "Электронная библиотека" позволяет работать с перечнем книг, выпусков газет и патентами. Для пользоветелей библиотеки предусмотрены следующие роли: гость, читатель, библиотекарь, администратор. Гости могут только просматривать список записей в библотеке. Для читателей доступен просмотр сведений о заметке. Бибилоекарь может редактировать, создавать и удалять записи в библотеке. Администратор имеет возможность управлять пользователями(редактировать роли).

## Разработка

### Построен с помощью
Приложение написано на .NET Core 5.0, для тестов используется MSTest. В качестве сервера выступает MSSQL, для доступа к базе данных используется ADO.NET. В качестве веб сервисов были использованы ASP.NET Core MVC и ASP.NET Core Web API.

### Подготовка к разработке
Для работы с приложением необходимы .NET Core 5.0, ASP.NET Core. Для работы с базой данных нужен MSSQL Server версии 15.0(Строка подключения для консольного приложения находится в /epam-int-DigitalLibrary/Epam.DigitalLibrary/Epam.DigitalLibrary.ConsolePL/App.config в connectionStrings. Для ASP.NET Core MVC: /https://github.com/p0lich/epam-int-DigitalLibrary/blob/main/Epam.DigitalLibrary/Epam.DigitalLibrary.LibraryMVC/appsettings.json в ConnectionStrings/SSPIConnString. Для ASP.NET Core Web API: /https://github.com/p0lich/epam-int-DigitalLibrary/blob/main/Epam.DigitalLibrary/Epam.DigitalLibrary.LibraryMVC/appsettings.json в ConnectionStrings/SSPIConnString). 

## База данных

Сылка на базу данных: https://drive.google.com/file/d/1f4ftZKb1IKlPINKZp4trZotzvChGtBZa/view?usp=sharing
