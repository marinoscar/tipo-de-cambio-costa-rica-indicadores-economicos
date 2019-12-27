// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


var cardHtmlTemplate = `
    <div id="card-<%= bankId %>" class="col-md-6 col-xs-12" >
        <div class="card">
            <div class="card-header">
                <a href="<%= bankUrl %>"><%= bankName %></a>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table class="table table-borderless table-sm">
                        <thead>
                            <tr>
                                <th scope="col">Compra</th>
                                <th scope="col">Venta</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <span class="card-title h4 font-weight-bolder"><%= formattedBuyRate %></span>
                                    <span class="badge <%= formattedPrevDayBuyRateGrowthClass %> "><%= formattedPrevDayBuyRateGrowth %> <i class="<%= formattedPrevDayBuyRateGrowthIconClass %>"></i></span>

                                </td>
                                <td>
                                    <span class="card-title h4 font-weight-bolder"><%= formattedSaleRate %></span>
                                    <span class="badge <%= formattedPrevDaySaleRateGrowthClass %> "><%= formattedPrevDayBuyRateGrowth %> <i class="<%= formattedPrevDaySaleRateGrowthIconClass %>"></i></span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="">
                    <canvas id="chart-<%= bankId %>"></canvas>
                </div>
            </div>
            <div class="card-footer text-muted small">
                <div>
                    <table class="table table-borderless table-sm">
                        <thead>
                            <tr>
                                <th>Compra</th>
                                <th>Venta</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>semana pasada <span id="buy-prev-day-1"><%= formattedPrevWeekBuyRateGrowth %><i class="<%= formattedPrevWeekBuyRateGrowthIconClass %>"></i></span></td>
                                <td>semana pasada <span id="sale-prev-day-1"><%= formattedPrevWeekSaleRateGrowth %><i class="<%= formattedPrevWeekSaleRateGrowthIconClass %>"></i></span></td>
                            </tr>
                            <tr>
                                <td>mes pasado <span id="buy-prev-day-1"><%= formattedPrevMonthBuyRateGrowth %><i class="<%= formattedPrevMonthBuyRateGrowthIconClass %>"></i></span></td>
                                <td>mes pasado <span id="sale-prev-day-1"><%= formattedPrevMonthSaleRateGrowth %><i class="<%= formattedPrevMonthSaleRateGrowthIconClass %>"></i></span></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div >
`

var templateEngine = {
    getCard: function (data) {
        var compiled = _.template(cardHtmlTemplate);
        return compiled(data);
    },
    drawHtml: function (data, onComplete) {
        var root = $('#work-area');
        var html = '<div class="row">'
        var cardCount = 0;
        for (i = 0; i < data.rates.length; i++) {
            html = html + '\n' + templateEngine.getCard(data.rates[i]);
            cardCount++;
            if (cardCount > 1) {
                cardCount = 0;
                html = html + '\n' + '</div>';
                html = html + '\n' + '<div class="row">'
            }
        }
        root.html(html);
        onComplete(data.rates)
    },
    processData: function () {
        $.getJSON('/home/GetBankData', function (data) {
            templateEngine.drawHtml(data, function (result) {
                for (i = 0; i < result.length; i++) {
                    var item = result[i];
                    chartEngine.create('chart-' + item.bankId, item.labels, item.pastBuyRates, item.pastSaleRates);
                    $('#data-date-reference').html(data.dateControl);
                }
            });
        });
    }
}

var chartEngine = {
    create: function (canvasId, categories, buyData, saleData) {
        var canvas = document.getElementById(canvasId);
        var chart = new Chart(canvas, {
            type: 'line',
            data: {
                labels: categories,
                datasets: [
                    {
                        label: 'Compra',
                        backgroundColor: '#007bff',
                        data: buyData,
                        fill: false
                    },
                    {
                        label: 'Venta',
                        backgroundColor: '#17a2b8',
                        data: saleData,
                        fill: false
                    }
                ]
            },
            options: {
                responsive: true,
                title: {
                    display: true,
                    text: 'Tipo de Cambio en los ultimos 90 dias'
                },
                scales: {
                    xAxes: [{
                        display: true,
                        ticks: {
                            autoSkip: true,
                            maxTicksLimit: 10
                        }
                    }],
                    yAxes: [{
                        display: true,
                        ticks: {
                            autoSkip: true,
                            maxTicksLimit: 5
                        }
                    }]
                }
            }
        });
    },
    buySample: [579.57, 578.67, 578.99, 577.93, 577.93, 577.93, 576.58, 576.29, 577.82, 579.41, 578.1, 578.1, 578.1, 577.42, 578.1, 578.5, 578.28, 576.37, 576.37, 576.37, 574.86, 574.45, 576.41, 575.95, 577.13, 577.13, 577.13, 578.93, 577.82, 579.2, 581.49, 579.25, 579.25, 579.25, 578.1, 579.36, 579.86, 581.05, 580.34, 580.34, 580.34, 578.86, 581.08, 581.66, 581.61, 579.76, 579.76, 579.76, 578.69, 577.98, 577.49, 576.19, 573.76, 573.76, 573.76, 570.97, 569.76, 569.74, 568.17, 565.43, 565.43, 565.43, 562.54, 561.57, 560.45, 559.83, 558.9, 558.9, 558.9, 560.22, 563.53, 563.99, 566.91, 565.08, 565.08, 565.08, 565.59, 567.59, 567.6, 566.61, 563.73, 563.73, 563.73, 562.33, 562.52, 563.65, 562.52, 563.21, 563.21, 563.21],
    saleSample: [585.84, 585.43, 584.74, 583.88, 583.88, 583.88, 583.46, 583.23, 585.11, 585.76, 585.88, 585.88, 585.88, 585.11, 584.46, 583.98, 583.69, 583.57, 583.57, 583.57, 581.85, 580.84, 582.42, 582.66, 583.79, 583.79, 583.79, 585.42, 585.88, 586, 586.66, 585.91, 585.91, 585.91, 584.91, 585.31, 585.39, 585.89, 587.5, 587.5, 587.5, 587.75, 587.94, 588.06, 586.18, 586.21, 586.21, 586.21, 585.86, 584.69, 583.26, 582.36, 579.99, 579.99, 579.99, 578.48, 576.44, 576.71, 573.72, 571.8, 571.8, 571.8, 571.23, 567.77, 566.37, 566.56, 565.89, 565.89, 565.89, 567.65, 572.01, 571.81, 573.19, 572.22, 572.22, 572.22, 572.08, 573.52, 574.59, 572.52, 570.76, 570.76, 570.76, 570.05, 569.16, 569.29, 569.89, 569.57, 569.57, 569.57],
    categoriesSample: ['2019-09-25', '2019-09-26', '2019-09-27', '2019-09-28', '2019-09-29', '2019-09-30', '2019-10-01', '2019-10-02', '2019-10-03', '2019-10-04', '2019-10-05', '2019-10-06', '2019-10-07', '2019-10-08', '2019-10-09', '2019-10-10', '2019-10-11', '2019-10-12', '2019-10-13', '2019-10-14', '2019-10-15', '2019-10-16', '2019-10-17', '2019-10-18', '2019-10-19', '2019-10-20', '2019-10-21', '2019-10-22', '2019-10-23', '2019-10-24', '2019-10-25', '2019-10-26', '2019-10-27', '2019-10-28', '2019-10-29', '2019-10-30', '2019-10-31', '2019-11-01', '2019-11-02', '2019-11-03', '2019-11-04', '2019-11-05', '2019-11-06', '2019-11-07', '2019-11-08', '2019-11-09', '2019-11-10', '2019-11-11', '2019-11-12', '2019-11-13', '2019-11-14', '2019-11-15', '2019-11-16', '2019-11-17', '2019-11-18', '2019-11-19', '2019-11-20', '2019-11-21', '2019-11-22', '2019-11-23', '2019-11-24', '2019-11-25', '2019-11-26', '2019-11-27', '2019-11-28', '2019-11-29', '2019-11-30', '2019-12-01', '2019-12-02', '2019-12-03', '2019-12-04', '2019-12-05', '2019-12-06', '2019-12-07', '2019-12-08', '2019-12-09', '2019-12-10', '2019-12-11', '2019-12-12', '2019-12-13', '2019-12-14', '2019-12-15', '2019-12-16', '2019-12-17', '2019-12-18', '2019-12-19', '2019-12-20', '2019-12-21', '2019-12-22', '2019-12-23']
}

