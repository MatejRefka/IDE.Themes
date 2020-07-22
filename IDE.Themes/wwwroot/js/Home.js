//show the file name uploaded upon 'choose file' button event change.
let fileName = document.getElementById("chooseFile");
let fileNameField = document.getElementById("fileName");

fileName.addEventListener("change", function () {
    let uploadedFileName = event.target.files[0].name;
    fileNameField.textContent = uploadedFileName;
});

//choose a correct extension based on IDEFrom
$(document).ready(function () {

    $("select#IDEFrom").change(function () {

        if ($("select#IDEFrom").find(":selected").text() === "Eclipse")

            $("input#chooseFile").attr("accept", ".xml,.epf");

        else if ($("select#IDEFrom").find(":selected").text() === "Visual Studio")

            $("input#chooseFile").attr("accept", ".vssettings");

        else $("input#chooseFile").attr("accept", "");

    });
});

//show/hide optional parameters(3.5) on selector change IDEFrom==Visual Studio
$(document).ready(function () {

    $("select#IDEFrom").change(function () {

        if ($("select#IDEFrom").find(":selected").text() === "Select IDE")

            $("div#optionalParameters").hide();

        else if ($("select#IDEFrom").find(":selected").text() === "Eclipse")

            $("div#optionalParameters").hide();

        else if ($("select#IDEFrom").find(":selected").text() === "Visual Studio")

            $("div#optionalParameters").show();

    });

});

//if the user is converting to the same IDE, show a warning
$(document).ready(function () {

    $("select#IDEFrom").change(function () {

        if ($("select#IDEFrom").find(":selected").text() === $("select#IDETo").find(":selected").text())

            $("p.warning").text("Conversion to the same IDE.");

        else $("p.warning").text("");

        if ($("p.error").text() === "Select an IDE to convert from.") {
            $("p.error").text("");
        }

    });

    $("select#IDETo").change(function () {

        if ($("select#IDETo").find(":selected").text() === $("select#IDEFrom").find(":selected").text())

            $("p.warning").text("Conversion to the same IDE.");

        else $("p.warning").text("");

        if ($("p.error").text() === "Select an IDE to convert to.") {
            $("p.error").text("");
        }

    });

});

//if a user hasn't selected a file or it is the wrong file, show error and don't POST
$(document).ready(function () {

    $("form").submit(function () {

        var fileExtension = $("input#chooseFile").val().split(".").pop();

        if (fileExtension === null || fileExtension === "") {

            $("p.error").text("Choose a theme to convert.")
            return false;
        }

        if ($("select#IDEFrom").find(":selected").text() === "Eclipse" && !(fileExtension === "xml") && !(fileExtension === "epf")) {

            $("p.error").text("Choose .xml or .epf file.")
            return false;
        }

        if ($("select#IDEFrom").find(":selected").text() === "Visual Studio" && !(fileExtension === "vssettings")) {

            $("p.error").text("Choose a .vssettings file.")
            return false;
        }

        if ($("select#IDEFrom").find(":selected").text() === $("select#IDETo").find(":selected").text()) {

            return false;
        }

        return true;

    });
});

//extra control for the convert button. takes control after download.
$(document).ready(function () {

    $("#Convert").on("click", function () {

        $("p.error").show();
        $("p.error").css("color", "rgb(231, 29, 54)");

        var fileExtension = $("input#chooseFile").val().split(".").pop();

        if (fileExtension === null || fileExtension === "") {

            $("p.error").text("Choose a theme to convert.")
            return false;
        }

        if ($("select#IDEFrom").find(":selected").text() === "Eclipse" && !(fileExtension === "xml") && !(fileExtension === "epf")) {

            $("p.error").text("Choose .xml or .epf file.")
            return false;
        }

        if ($("select#IDEFrom").find(":selected").text() === "Visual Studio" && !(fileExtension === "vssettings")) {

            $("p.error").text("Choose a .vssettings file.")
            return false;
        }

        if ($("select#IDEFrom").find(":selected").text() === $("select#IDETo").find(":selected").text()) {

            return false;
        }

        return true;

    });
});

//if a user hasn't selected either IDEFrom or IDETo, show error and don't POST
$(document).ready(function () {

    $("form").submit(function () {

        if ($("select#IDETo").find(":selected").text() === "Select IDE" && $("select#IDEFrom").find(":selected").text() === "Select IDE") {
            $("p.error").text("Select an IDE to convert from.")
            return false;
        }

        if ($("select#IDETo").find(":selected").text() === "Select IDE") {
            $("p.error").text("Select an IDE to convert to.")
            return false;
        }

        if ($("select#IDEFrom").find(":selected").text() === "Select IDE") {
            $("p.error").text("Select an IDE to convert from.")
            return false;
        }

        return true;

    });
});

//reset 'choose file error' on choosing a new file
$(document).ready(function () {

    $("input#chooseFile").on("click", function () {

        if ($("p.error").text().includes("Choose a")) {

            $("p.error").text("");
        }

    });
});

//reset 'successful' on choosing a new file
$(document).ready(function () {

    $("button#Download").on("click", function () {

        $("p.error").hide();
        $("p.error").css("color", "E71D36");

        $("button#Download").prop("disabled", true).addClass("disabled");
        //location.reload();

    });
});


