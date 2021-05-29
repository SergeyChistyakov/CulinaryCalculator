# Сергей Чистяков - "Кулинарный калькулятор"

### Группа: 10 - Э - 3
### Электронная почта: sergeychist.polo@gmail.com
### Телефон: +7 926 404 83 11 (Telegram, WhatsApp)


**[ НАЗВАНИЕ ПРОЕКТА ]**

“Кулинарный калькулятор”

**[ ПРОБЛЕМНОЕ ПОЛЕ ]**

Есть много причин увлекаться кулинарией и любить готовить. Это может быть довольно интересно и для некоторых превращается в хобби. Кроме того, в наше время, "выпечка на заказ" является распространённым способом зарабатывания денег. Очевидно, что людям, которые проводят много времени у плиты и духовки, будет полезно приложение, которое автоматизирует некоторые операции, связанные с процессом приготовления пищи. К таким сервисам можно отнести: сохранение собственного рецепта с пошаговой инструкцией и фото, что позволит каждому пользователю создать свою собственную кулинарную книгу, перерасчёт рецепта, поиск нужного рецепта, возможность поделиться своим рецептом с другими.

**[ ЗАКАЗЧИК / ПОТЕНЦИАЛЬНАЯ АУДИТОРИЯ ]**

Кулинарное приложение, обладающее функцией калькулятора с разными возможностями, может заинтересовать широкую аудиторию. Оно будет полезно как обычным хозяйкам, так и тем, кто готовит на заказ. 
Кроме того, у нас есть непосредственный заказчик. Это человек, который занимается выпечкой на профессиональном уровне (для "себя" и на заказ). Неткач Анастасия Сергеевна, 916-311-87-88.

**[ АППАРАТНЫЕ ТРЕБОВАНИЯ ]** 

Продукт разрабатывается под операционные системы Android и Windows. Продукт будет разработан для следующих конфигураций:
* Windows 10: 4 Гб оперативной памяти и 1 Гб свободного дискового пространства 
* Android 10 и выше: 3Гб оперативной памяти, 1Гб свободного дискового пространства

Продукт может быть портирован для:
* Android 5.0 и выше
* IOS 9 и выше
* Samsung Tizen
* macOS 10.13 и выше
* GTK#
* WPF

Портирование выходит за рамки проекта, поскольку требует дополнительных ресурсов и добавляет риски. Так для портирования под IOS требуется MAC компьютер.
 
**[ ФУНКЦИОНАЛЬНЫЕ ТРЕБОВАНИЯ ]**

Программный продукт является персонализированным, не подразумевает многопользовательской работы и не ставит собой цель обмена информацией с другими пользователями.  

Требования заказчика:
* Сохранение собственного рецепта (текст, фото, ингредиенты)
* Группировка рецептов по разделам
* Перерасчёт массы ингредиентов при уменьшении/увеличении массы конечного продукта, изменении массы одного ингредиента
* Поиск рецептов по ингредиентам
* Выгрузка рецепта в файл PDF формата и сохранение его на устройстве для последующей отправки по почте и через любые мессенджеры
* Кулинарный калькулятор должен работать в отсутствие интернета
* Данные должны храниться в формате, доступном для переноса с устройства на устройство и хранения в облаке, например google диск
* Вводить длинный текст рецепта удобнее с компьютера, поэтому важна возможность работы калькулятора в Windows

**[ ПОХОЖИЕ / АНАЛОГИЧНЫЕ ПРОДУКТЫ ]**

Анализ 3 программных продуктов, которые максимально приближены к заданному функционалу, показал, что:

* "Кухонный помощник": не сохраняет рецепты (хотя в описании такая функция заявлена), нет возможности помножить на дробное число, нет группировки рецептов по разделам, нет поиска по ингредиенту, нет выгрузки рецепта в PDF, есть реклама
* "Калькулятор рецепта": нет группировки рецептов по разделам, нет поиска по ингредиенту, неудачный интерфейс (все опции перерасчета изображены с помощью миниатюрных картинок, значение которых тяжело угадать, приходится нажимать на все подряд, чтобы понять, где какая функция находится), основной функционал только в платной версии, бесплатная версия приложения предполагает наличие рекламы, а платную версию нельзя протестировать до покупки
* "Кухонный калькулятор (конвертер весов)" -  обладает самым ограниченным функционалом, т.к. позволяет только производить перерасчет между разными мерами (стакан, ложка, граммы), остальные функции отсутствуют
* Каждый из программных продуктов доcтупен только для Android

**[ ИНСТРУМЕНТЫ РАЗРАБОТКИ ]**

* Microsoft Visual Studio
* C#
* Xamarin.Forms

**[ ЭТАПЫ РАЗРАБОТКИ ]**

* Разработка пользовательских сценариев
* Проектирование пользовательского интерфейса
* Проектирование API
* Реализация API (“Data Layer”, “Data Access layer”, “Logic layer”)
* Реализация “User Interface Layer”
* Внутренниие тестирование Windows и Android версий продкута
* Приёмка программного продукта заказчиком
* Подготовка проекта к защите

**[ ВОЗМОЖНЫЕ РИСКИ ]**

* Нехватка времени на освоение стека технологий, которые мне необходимо изучить может повлиять на сроки разработки программного продукта
* Нехватка опыта может повлиять на качество программного продукта
* Недостатки универсальной платформы (Xamarin.Forms) могут повлиять на качество продукта. Так отзывы разработчиков говорят о большом размере и медленной работе приложений, построенных на базе платформы. Анализ информации репозитория плаформы на GitHub говорит о большом количестве ошибок.