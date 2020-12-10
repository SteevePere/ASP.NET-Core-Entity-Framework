$(document).ready(function () {

    var practiceIsUnique = false;
    var emailIsUnique = false;

    $("#adminButton").click(function () {
        hideAccountTypeButtons();
        showRegisterForm("ADMIN");
    });


    $("#doctorButton").click(function () {
        hideAccountTypeButtons();
        showRegisterForm("DOCTOR");
    });


    $("#emailInput").on('keyup blur', function () {
        checkIfEmailExists();
    });


    $("#practiceNameInput").on('keyup blur', function () {
        checkIfPracticeNameExists();
    });


    $("#confirmationPasswordInput").blur(function () {
        checkPasswords();
    });


    $("#createPracticeLink").click(function () {
        showCreatePracticeForm();
        setCreatingPractice();
    });


    function hideAccountTypeButtons() {
        $("#accountTypeButtons").fadeOut();
    }


    function hidePracticeSelect() {
        $("#practiceSelect").hide();
    }


    function setCreatingPractice() {
        $("#creatingPracticeInput").val("1");
    }


    function showRegisterForm(accountType) {

        if (accountType === "ADMIN") {
            hidePracticeSelect();
        }

        $("#roleInput").val(accountType);
        $("#accountTypeButtons").replaceWith($("#registerForm"));
        $("#registerForm").fadeIn();
    }


    function showCreatePracticeForm() {
        $("#practiceSelect").replaceWith($("#createPracticeForm"));
        $("#createPracticeForm").fadeIn();
    }


    function checkPasswords() {

        var passwordsMatch = true;
        var password = $("#passwordInput").val();
        var confirmedPassword = $("#passwordConfirmationInput").val();

        if (password !== confirmedPassword) {

            $("#passwordAlert").fadeTo(1000, 500).slideUp(700, function () {
                $("#passwordAlert").slideUp(500);
            });

            passwordsMatch = false;
        }

        return passwordsMatch;
    }


    function checkIfEmailExists() {

        var email = $("#emailInput").val();

        $.ajax({
            url: "/Auth/Ajax_ValidateEmail",
            type: "POST",
            data: { "email": email },
            dataType: 'json',
            complete: function (data) {

                if (data.responseText !== "404") {
                    $("#emailAlert").fadeTo(1000, 500).slideUp(700, function () {
                        $("#emailAlert").slideUp(500);
                    });

                    emailIsUnique = false;
                }

                else {
                    emailIsUnique = true;
                }
            }
        });
    }


    function checkIfPracticeNameExists() {

        var practiceName = $("#practiceNameInput").val();

        $.ajax({
            url: "/Auth/Ajax_ValidatePractice",
            type: "POST",
            data: { "practiceName": practiceName },
            dataType: 'json',
            complete: function (data) {

                if (data.responseText !== "404") {
                    $("#practiceExistsAlert").fadeTo(1000, 500).slideUp(700, function () {
                        $("#practiceExistsAlert").slideUp(500);
                    });

                    practiceIsUnique = false;
                }

                else {
                    practiceIsUnique = true;
                }
            }
        });
    }


    $('#submitButton').click(function (event) {

        event.preventDefault();
        checkIfEmailExists();

        var isAdmin = $("#roleInput").val() === 'ADMIN';
        var creatingPractice = $("#creatingPracticeInput").val() === "1";
        var password = $("#passwordInput").val();
        var firstName = $("#firstNameInput").val();
        var lastName = $("#lastNameInput").val();
        var email = $("#emailInput").val();
        var genders = $("input[name='Genders']:checked").val();
        var practiceName = $("#practiceNameInput").val();
        var address = $("#addressInput").val();
        var city = $("#cityInput").val();
        var zipCode = $("#zipCodeInput").val();


        if (firstName.length === 0) {

            $("#firstNameAlert").fadeTo(1000, 500).slideUp(500, function () {
                $("#firstNameAlert").slideUp(500);
            });

            return;
        }

        if (lastName.length === 0) {

            $("#lastNameAlert").fadeTo(1000, 500).slideUp(500, function () {
                $("#lastNameAlert").slideUp(500);
            });

            return;
        }

        if (email.length === 0) {

            $("#emptyEmailAlert").fadeTo(1000, 500).slideUp(500, function () {
                $("#emptyEmailAlert").slideUp(500);
            });

            return;
        }

        if (!emailIsUnique || !checkPasswords()) {
            return;
        }

        if (password.length < 6) {

            $("#shortPasswordAlert").fadeTo(1000, 500).slideUp(500, function () {
                $("#shortPasswordAlert").slideUp(500);
            });

            return;
        }

        if (!genders) {

            $("#genderAlert").fadeTo(1000, 500).slideUp(500, function () {
                $("#genderAlert").slideUp(500);
            });

            return;
        }

        if (!isAdmin && !creatingPractice && !$("#practiceSelect option:selected").length) {

            $("#practiceAlert").fadeTo(1000, 500).slideUp(500, function () {
                $("#practiceAlert").slideUp(500);
            });

            return;
        }

        if (creatingPractice) {

            checkIfPracticeNameExists();

            if (!practiceIsUnique) {
                return;
            }

            if (practiceName.length === 0) {

                $("#practiceNameAlert").fadeTo(1000, 500).slideUp(500, function () {
                    $("#practiceNameAlert").slideUp(500);
                });

                return;
            }

            if (address.length === 0) {

                $("#addressAlert").fadeTo(1000, 500).slideUp(500, function () {
                    $("#addressAlert").slideUp(500);
                });

                return;
            }

            if (city.length === 0) {

                $("#cityAlert").fadeTo(1000, 500).slideUp(500, function () {
                    $("#cityAlert").slideUp(500);
                });

                return;
            }

            if (zipCode.length === 0) {

                $("#zipCodeAlert").fadeTo(1000, 500).slideUp(500, function () {
                    $("#zipCodeAlert").slideUp(500);
                });

                return;
            }
        }

        $("#register").submit();
    });
});