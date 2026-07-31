$(document).ready(function () {

    CalculateGrandTotal();

    //=========================
    // ADD NEW ROW
    //=========================
    $("#btnAddRow").click(function () {

        var year = new Date().getFullYear();

        var html = "";

        html += "<tr data-id='0'>";

        html += "<td><input type='text' class='form-control year' value='" + year + "'/></td>";

        html += "<td><input type='text' class='form-control number patient'/></td>";

        html += "<td><input type='text' class='form-control cancer'/></td>";

        html += "<td class='cancerPer'>0.00%</td>";

        html += "<td><input type='text' class='form-control number male'/></td>";

        html += "<td class='malePer'>0.00%</td>";

        html += "<td><input type='text' class='form-control number female'/></td>";

        html += "<td class='femalePer'>0.00%</td>";

        html += "<td>";

        html += "<button class='btn btn-success btn-xs save'>";
        html += "<i class='fa fa-save'></i> Save";
        html += "</button> ";

        html += "<button class='btn btn-default btn-xs cancel'>";
        html += "Cancel";
        html += "</button>";

        html += "</td>";

        html += "</tr>";

        $("#tblData tbody tr:last").before(html);

    });

    //=========================
    // EDIT
    //=========================
    $("#tblData").on("click", ".edit", function () {

        var tr = $(this).closest("tr");

        var year = tr.find("td:eq(0)").text().trim();
        var patient = tr.find("td:eq(1)").text().replace(/,/g, "").trim();
        var cancer = tr.find("td:eq(2)").text().trim();
        var male = tr.find("td:eq(4)").text().replace(/,/g, "").trim();
        var female = tr.find("td:eq(6)").text().replace(/,/g, "").trim();

        tr.find("td:eq(0)").html("<input class='form-control year' value='" + year + "'>");
        tr.find("td:eq(1)").html("<input class='form-control number patient' value='" + patient + "'>");
        tr.find("td:eq(2)").html("<input class='form-control cancer' value='" + cancer + "'>");
        tr.find("td:eq(4)").html("<input class='form-control number male' value='" + male + "'>");
        tr.find("td:eq(6)").html("<input class='form-control number female' value='" + female + "'>");

        tr.find("td:last").html(
            "<button class='btn btn-success btn-xs save'><i class='fa fa-save'></i> Save</button> " +
            "<button class='btn btn-default btn-xs cancel'>Cancel</button>"
        );

    });

    //=========================
    // CANCEL
    //=========================
    $("#tblData").on("click", ".cancel", function () {

        var tr = $(this).closest("tr");

        if (tr.attr("data-id") == "0") {

            tr.remove();
            CalculateGrandTotal();
            return;
        }

        location.reload();

    });

    //=========================
    // DELETE
    //=========================
    $("#tblData").on("click", ".delete", function () {

        if (!confirm("Delete this record?"))
            return;

        var tr = $(this).closest("tr");

        $.ajax({

            url: "DeleteMagnitudeRegistration",

            type: "POST",

            data: {
                id: tr.attr("data-id")
            },

            success: function (r) {

                if (r.success) {

                    tr.remove();

                    CalculateGrandTotal();

                }
                else {

                    alert(r.message);

                }

            }

        });

    });

});
//=========================
// SAVE
//=========================
$("#tblData").on("click", ".save", function () {

    var tr = $(this).closest("tr");

    var patient = Number(tr.find(".patient").val().replace(/,/g, "")) || 0;

    var cancerText = tr.find(".cancer").val();

    var cancer = Number(cancerText.replace(/,/g, "").replace("#", "")) || 0;

    var male = Number(tr.find(".male").val().replace(/,/g, "")) || 0;

    var female = Number(tr.find(".female").val().replace(/,/g, "")) || 0;

    var obj = {

        Id: tr.attr("data-id"),

        Year: tr.find(".year").val(),

        TotalNewPatientsRegistered: patient,

        CancerCases: cancerText,

        MaleCases: male,

        FemaleCases: female

    };

    $.ajax({

        url: "SaveMagnitudeRegistration",

        type: "POST",

        data: obj,

        success: function (r) {

            if (!r.success) {

                alert(r.message);

                return;

            }

            tr.attr("data-id", r.id);

            var cancerPer = patient == 0 ? 0 :
                (cancer * 100 / patient);

            var malePer = cancer == 0 ? 0 :
                (male * 100 / cancer);

            var femalePer = cancer == 0 ? 0 :
                (female * 100 / cancer);

            tr.find("td:eq(0)").html(obj.Year);

            tr.find("td:eq(1)").html(patient.toLocaleString());

            tr.find("td:eq(2)").html(cancerText);

            tr.find("td:eq(3)").html(cancerPer.toFixed(2) + "%");

            tr.find("td:eq(4)").html(male.toLocaleString());

            tr.find("td:eq(5)").html(malePer.toFixed(2) + "%");

            tr.find("td:eq(6)").html(female.toLocaleString());

            tr.find("td:eq(7)").html(femalePer.toFixed(2) + "%");

            tr.find("td:last").html(

                "<button class='btn btn-primary btn-xs edit'>" +
                "<i class='fa fa-edit'></i> Edit</button> " +

                "<button class='btn btn-danger btn-xs delete'>" +
                "<i class='fa fa-trash'></i> Delete</button>"

            );

            CalculateGrandTotal();

        }

    });

});
function CalculateGrandTotal() {

    var patient = 0;

    var cancer = 0;

    var male = 0;

    var female = 0;

    $("#tblData tbody tr").each(function () {

        patient += Number($(this).find("td:eq(1)").text().replace(/,/g, "")) || 0;

        cancer += Number($(this).find("td:eq(2)").text().replace(/,/g, "").replace("#", "")) || 0;

        male += Number($(this).find("td:eq(4)").text().replace(/,/g, "")) || 0;

        female += Number($(this).find("td:eq(6)").text().replace(/,/g, "")) || 0;

    });

    $("#gtPatient").text(patient.toLocaleString());

    $("#gtCancer").text(cancer.toLocaleString());

    $("#gtMale").text(male.toLocaleString());

    $("#gtFemale").text(female.toLocaleString());

    if (patient == 0)
        $("#gtCancerPer").text("0.00%");
    else
        $("#gtCancerPer").text(((cancer * 100) / patient).toFixed(2) + "%");

    if (cancer == 0) {

        $("#gtMalePer").text("0.00%");

        $("#gtFemalePer").text("0.00%");

    }
    else {

        $("#gtMalePer").text(((male * 100) / cancer).toFixed(2) + "%");

        $("#gtFemalePer").text(((female * 100) / cancer).toFixed(2) + "%");

    }

}
$("#tblData").on("blur", ".year", function () {

    var years = [];

    var duplicate = false;

    $(".year").each(function () {

        var y = $(this).val();

        if ($.inArray(y, years) >= 0) {

            duplicate = true;

            return false;

        }

        years.push(y);

    });

    if (duplicate) {

        alert("Duplicate Year is not allowed.");

        $(this).focus();

    }

});
$("#tblData").on("keypress", ".number", function (e) {

    if (e.which != 8 &&
        e.which != 0 &&
        (e.which < 48 || e.which > 57))
        return false;

});
$("#tblData").on("blur", ".number", function () {

    var value = $(this).val().replace(/,/g, "");

    if (value == "")
        return;

    $(this).val(Number(value).toLocaleString());

});
$("#btnEditNote").click(function () {

    $("#noteText").hide();

    $("#txtNote").show();

    $("#btnEditNote").hide();

    $("#btnSaveNote").show();

});
$("#btnSaveNote").click(function () {

    $.ajax({

        url: 'SaveMagnitudeNote',

        type: 'POST',

        data: {

            text: $("#txtNote").val()

        },

        success: function () {

            $("#noteText").html($("#txtNote").val());

            $("#noteText").show();

            $("#txtNote").hide();

            $("#btnEditNote").show();

            $("#btnSaveNote").hide();

            alert("Saved Successfully");

        }

    });

});