# Сайт службы доставки продуктов "Велосипед"

Данный проект делался как курсовая работа на 3 курсе в МГТУ им. Н. Э. Баумана

### Что на данный момент работает:
- Каталог товаров: подгрузка из базы данных, возможность добавить товар в корзину
- Корзина: изменение количества товаров, удаление товаров из корзины. В корзину можно добавить только товары из одного магазина
- Вход и регистрация: регистрация пользователя, отправка письма с подтверждением на электронную почту, вход в существующий активированный аккаунт, сброс забытого пароля

В качестве фреймворка для frontend используется React.JS, который требует наличие установленного [Node.js](https://nodejs.org/en/download)

В качестве backend фреймворка используется [ASP .NET Core 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

В качестве БД используется [PostgreSQL](https://www.postgresql.org/), который работает в контейнере [Docker](https://www.docker.com/products/docker-desktop/) (файл для docker-compose находится в папке с проектом)

## Приятного аппетита