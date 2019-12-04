<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Raport.aspx.vb" Inherits="SysLogWebRedi.Raport" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report System Log</title>
    <script src="Scripts/jquery-3.4.1.min.js"></script>
    <script src="Scripts/umd/popper.min.js"></script>
    <script src="Scripts/bootstrap.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <!--HTML TABLE TO PDF PLUGIN  https://github.com/simonbengtsson/jsPDF-AutoTable   -->
    <script src="https://unpkg.com/jspdf@1.5.3/dist/jspdf.min.js"></script>
    <script src="jspdf.plugin.autotable.js"></script>
    <!-- HTML TABLE TO EXCEL PLUGIN https://github.com/sheetjs/js-xlsx  -->
    <script src="xlsx.full.min.js"></script>
    <!-- FileSaver Per saveAs https://github.com/eligrey/FileSaver.js   -->
    <script src="FileSaver.js"></script>
    <!-- Per Te Hequr Errorin : Failed to load resource favicon.icon -->
    <link rel="shortcut icon" href="#" />
    <link href="https://cdn.datatables.net/1.10.18/css/dataTables.bootstrap4.min.css" rel="stylesheet"/>
    <!-- Page level plugin JavaScript-->
    <script src="https://cdn.datatables.net/1.10.18/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/1.10.18/js/dataTables.bootstrap4.min.js"></script>


    <script>

        var headerArr = [];
        var bodyArr = [];
        var emri;

        var data;
        var data2;

        $(document).ready(function () {
            afishoDitor();
            // RAPORT MIDIS 2 DATAVE
            $("#rptJavor").on("click", function () {
                var data = new Date($("#date1").val());
                var data2 = new Date($("#date2").val());

                if (isNaN(data) || isNaN(data2)) {
                    alert("Fusni Datat");
                }
                else {
                    var data = formatDate(data);
                    var data2 = formatDate(data2);
                    afishoMidisDatave(data, data2);
                }

            });

            // RAPORT DITOR
            $("#rptDitor").on("click", function () {
                $("#date1").val("");
                $("#date2").val("");
                afishoDitor();
            });



            // RAPORT EXCEL 
            $("#excel").click(function () {
                setFileName();
                //htmlTableToExcel();

                var d = document.createElement('div');
                d.innerHTML = getTable();

                var wb = XLSX.utils.table_to_book(d.firstChild, { sheet: "Sheet 1" });
                var wbout = XLSX.write(wb, { bookType: 'xlsx', bookSST: true, type: 'binary' });
                saveAs(new Blob([s2ab(wbout)], { type: "application/octet-stream" }), emri + '.xlsx');

            });

            // RAPORT PDF
            $("#pdf").click(function () {
                headerArr = [];
                bodyArr = [];

                setFileName();

                var fjala;
                $("#afishim table thead th").each(function () {
                    fjala = $(this).clone().find("ul").remove().end().text();
                    fjala = fjala.replace(/\n/g, "");
                    fjala = fjala.replace(/\s/g, "");
                    headerArr.push(fjala);
                });

                var insideBody = [];
                $("#afishim table tbody tr").each(function () {
                    insideBody = [];
                    $(this).find("td").each(function () {
                        insideBody.push($(this).text());
                    });
                    bodyArr.push(insideBody);
                });
                tableToPdf();
            });


        });

        function afishoDitor() {
            $.ajax({
                type: "POST",
                url: "Raport.aspx/afishoDitor",
                contentType: "application/json;charset=utf-8",
                success: function (data) {
                    $("#afishim").html(data.d);
                    $('#tabela').DataTable();
                },
                error: function (xhr, status, error) {
                    alert(status + " " + error);
                }
            });
        }

        function afishoMidisDatave(data1, data2) {
            $.ajax({
                type: "POST",
                url: "Raport.aspx/afishoMidisDatave",
                contentType: "application/json;charset=utf-8",
                data: JSON.stringify({ dateStart: data1, dateEnd: data2}),
                success: function (data) {
                    $("#afishim").html(data.d);
                    $('#tabela').DataTable();
                },
                error: function (xhr, status, error) {
                    alert(status + " " + error);
                }
            });
        }

    

        function formatDate(data) {
            var day = String("0" + data.getDate()).slice(-2);
            var month = String("0" + (data.getMonth() + 1)).slice(-2);
            var year = data.getFullYear();

            var data = [day, month, year].join("/");
            return data;
        }

        // FILENAME
        function setFileName() {
            var data3 = new Date();
            if ($("#afishim table thead").find("th:contains('DITE_PUNE')").length > 0) {
                data = new Date($("#date1").val());
                data2 = new Date($("#date2").val());
                emri = "Raport " + formatDate(data) + "-" + formatDate(data2) + " " + data3.getHours() + ":" + data3.getMinutes();
            }
            else {
                emri = "Raport Ditor " + formatDate(data3) + " " + data3.getHours() + ":" + data3.getMinutes();
            }
        }

        // RAPORT PDF
        function tableToPdf() {
            //var pdf = new jsPDF('p', 'pt', 'letter');
            var doc = new jsPDF('l');

            doc.autoTable({
                head: [headerArr],
                body: bodyArr,
                theme: 'grid',
                columnStyles: {
                    0: { cellWidth: 10 },
                    1: { cellWidth: 30 },
                    2: { cellWidth: 30 },
                    3: { cellWidth: 30 },
                    4: { cellWidth: 30 },
                    5: { cellWidth: 5 },
                    6: { cellWidth: 10 }
                }
            });

            doc.save(emri + ".pdf");
        }


        // RAPORT EXCEL
        function getTable() {

            headerArr = [];
            var fjala;
            $("#afishim table thead th").each(function () {
                fjala = $(this).clone().find("ul").remove().end().text();
                fjala = fjala.replace(/\n/g, "");
                fjala = fjala.replace(/\s/g, "");
                headerArr.push(fjala);
            });

            var str = "<table id = 'tbl' class = 'table table-hover'><thead class = 'thead-light'>";

            var header = "<tr>";
            for (var i = 0; i < headerArr.length; i++) {
                header += "<th scope = 'col'>" + headerArr[i] + "</th>";
            }
            header += "</tr>"

            str += header;
            str += "</thead><tbody>";

            var body = $("#afishim table tbody").html();

            str += body;
            str += "</tbody></table>";

            return str;
        }

        function s2ab(s) {
            var buf = new ArrayBuffer(s.length);
            var view = new Uint8Array(buf);
            for (var i = 0; i < s.length; i++) view[i] = s.charCodeAt(i) & 0xFF;
            return buf;
        }

        function htmlTableToExcel() {
            var blob,
                wb = { SheetNames: [], Sheets: {} };
            var ws1 = XLSX.read(getTable(), { type: "binary" }).Sheets.Sheet1;
            wb.SheetNames.push("Sheet1"); wb.Sheets["Sheet1"] = ws1;

            blob = new Blob([s2ab(XLSX.write(wb, { bookType: 'xlsx', type: 'binary' }))], {
                type: "application/octet-stream"
            });

            saveAs(blob, emri + ".xlsx");
        }

    </script>

    

</head>
<body>
    <form id="form1" runat="server">

        <div class="container" style="align-content:center;">
            <h1 class="titull" style="text-align:center;">LOGS</h1>
            <br />

            <b class="sp1"> Nga : </b><input type="date" id="date1" />
            <b class="sp2"> Deri : </b><input type="date" id="date2" />
            <button type="button" class="btn btn-danger" id="rptJavor">Raport Javor</button>
            <button type="button" class="btn btn-primary" id="rptDitor" style="float: right">Raport Ditor</button>
            <button class="btn btn-success dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                Export
            </button>
            <div class="dropdown-menu" aria-labelledby="dropdownMenuButton">
                <button class="dropdown-item" id="excel">Excel</button>
                <button class="dropdown-item" id="pdf">PDF</button>
            </div>
        </div>
        <br />
        <div id="body">

            <div id="afishim">
            </div>

        </div>


    </form>
</body>
</html>
