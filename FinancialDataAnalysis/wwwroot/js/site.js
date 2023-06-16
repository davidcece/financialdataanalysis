let chart;

$(function () {

    $('#volitility-form').submit(function (e) {
        e.preventDefault();
        let form = $(this);
        fetchVolitility(form);
    });

    $('#correlations-form').submit(function (e) {
        e.preventDefault();
        let form = $(this);
        fetchCorrelations(form);
    });

    $('#returns-form').submit(function (e) {
        e.preventDefault();
        let form = $(this);
        fetchReturns(form);
    });
  
})

async function fetchVolitility(form) {
    $('#btn-submit-volitility').attr('disabled', 'disabled');
    $('#btn-submit-volitility').val('Fetching Volitility...');

    let formData = getFormData(form);
    let response = await fetchAPIData("/Home/Volitility", "POST", formData);

    if (response.success) {
        let data = response.data;
        $("#volitility-data").text(`The volitity is ${data.value}`);
        plotGraph("volitility-chart", data.graphData);
    }else {
        alert(response.message);
    }

    $('#btn-submit-volitility').val('Calculate Volitility');
    $('#btn-submit-volitility').removeAttr('disabled');
}

async function fetchCorrelations(form) {
    $('#btn-submit-correlations').attr('disabled', 'disabled');
    $('#btn-submit-correlations').val('Fetching Correlations...');

    let formData = getFormData(form);
    let response = await fetchAPIData("/Home/Correlation", "POST", formData);

    if (response.success) {
        let data = response.data;
        $("#correlations-data").text(`The correlation is ${data.value}`);
        plotGraph("correlations-chart", data.graphData);
    } else {
        alert(response.message);
    }

    $('#btn-submit-correlations').val('Calculate Correlations');
    $('#btn-submit-correlations').removeAttr('disabled');
}

async function fetchReturns(form) {
    $('#btn-submit-returns').attr('disabled', 'disabled');
    $('#btn-submit-returns').val('Fetching Returns...');

    let formData = getFormData(form);
    let response = await fetchAPIData("/Home/Returns", "POST", formData);

    if (response.success) {
        let data = response.data;
        let profit = data.value >= 0 ? 'profit' : 'loss';
        $("#returns-data").text(`The ${profit} is ${data.value}`);
        plotGraph("returns-chart", data.graphData);
    } else {
        alert(response.message);
    }

    $('#btn-submit-returns').val('Calculate Returns');
    $('#btn-submit-returns').removeAttr('disabled');
}

function getFormData(form) {
    let unindexed_array = form.serializeArray();
    let indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return JSON.stringify(indexed_array);
}

async function fetchAPIData(url, method, body) {
    const response = await fetch(url, {
        method: method,
        body: body,
        headers: {
            'Content-Type': 'application/json'
        }
    });

    const data = await response.json();
    return data;
}

function plotGraph(canvas, graphData) {

    let datasets = [];

    Object.keys(graphData.data).forEach(key => {
        let dataset = {
            label: key,
            data: graphData.data[key],
            borderWidth: 1
        };
        datasets.push(dataset);
    });


    if (chart) {
        chart.destroy();
    }
    const ctx = document.getElementById(canvas);
    chart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: graphData.dates,
            datasets: datasets
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}