
if ($("#line-chart").length) {
    $.ajax({
        url: "/Home/TicketData",
        type: 'GET',
        success: function (result) {
            var chartData = JSON.parse(result);


            new Morris.Line({
                element: 'line-chart',
                data: chartData,
                xkey: 'week',
                ykeys: ['ticketsubs'],
                labels: ['Tickets Sumbitted'],
                fillOpacity: 0.6,
                hideHover: 'auto',
                behaveLikeLine: true,
                resize: true,
                pointFillColors: ['#ffffff'],
                pointStrokeColors: ['black'],
                lineColors: ['gray', 'red']
            })
        }
    });
}
//if ($("#yourdonut").length) {
//    $.ajax({
//        url: "/Home/YourProjectData",
//        type: 'GET',
//        success: function (result) {
//            var chartData = JSON.parse(result);
//            new Morris.Donut({
//                element: 'yourdonut',
//                data: chartData,
//                colors: ['#3498db', '#2980b9', '#34495e'],
//                formatter: function (y) { return y + "%" }
//            });
//        }
//    });
//}

if ($("#donut").length) {
    $.ajax({
        url: "/Home/ProjectData",
        type: 'GET',
        success: function (result) {
            var chartData = JSON.parse(result);
            new Morris.Donut({
                element: 'donut',
                data: chartData,
                colors: ['#3498db', '#2980b9', '#34495e'],
                formatter: function (y) { return y + "%" }
            });
        }
    });
}