
var data = {
    labels: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], // Метки по оси x
    datasets: [
        {
            data: [11000, 13500, 9000, 10000, 11000, 10000, 9000, 8000, 7000, 8000, 10000, 11000], // Данные 
            borderColor: "white", // Цвет линии 1
            // backgroundColor: gradient, 
        },
    ]
};


var myChart = new Chart(
    document.getElementById("chartLineStaticCountUsers"),
    {
        type: "line",
        data: data, // Объект с данными диаграммы
        options: {
            responsive: true,
            maintainAspectRatio: false,
            backgroundColor: "blue",
            borderLeftColor: "white",
            borderRightColor: "white",
            borderWidth: 2,
            scales: {
                x: {
                    gridLines: {
                        display: false // Скрываем сетку по оси x
                    }
                },
                y: {
                    gridLines: {
                        display: false // Скрываем сетку по оси y
                    },
                    min: 6000,
                    max: 14000
                }
            },
            elements: {
                line: {
                    tension: 0.4 // коэффициент натяжения кривой
                }
            },
            plugins: {
                title: {
                    display: true,
                    text: "Статистика количества пользователей",
                    position: "top",
                    align: "start",
                    color: "white",
                    font: {
                        color: "white"
                    }
                }
            }
        }
    });


