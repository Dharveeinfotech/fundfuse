
function InitLayout() {

    jQuery(function ($) {
        $(".FormattedMobNumber").mask("999-99-99999?99999999999999999999", { autoclear: false });
    });

    $(".CommonValidations").validationEngine(
 'attach', {
     promptPosition: "bottomRight",
     scroll: false,
     autoHidePrompt: true,
     autoHideDelay: 1000,
     fadeDuration: 0.3,
     focusFirstField: false,
     MaxErrorsPerField: 1
 });

    $(".CommonAttachValidation").click(function () {
        $(".CommonValidations").validationEngine(
        'attach', {
            promptPosition: "bottomRight",
            scroll: false,
            autoHidePrompt: true,
            autoHideDelay: 1000,
            fadeDuration: 0.3,
            focusFirstField: false,
            MaxErrorsPerField: 1
        });
    });

    $(".CommonDetachValidation").click(function () {
        $(".CommonValidations").validationEngine("detach")
    });


    $(".CommonValidations1").validationEngine(
'attach', {
    promptPosition: "bottomRight",
    scroll: false,
    autoHidePrompt: true,
    autoHideDelay: 1000,
    fadeDuration: 0.3,
    focusFirstField: false,
    MaxErrorsPerField: 1
});

    $(".CommonAttachValidation1").click(function () {
        $(".CommonValidations1").validationEngine(
        'attach', {
            promptPosition: "bottomRight",
            scroll: false,
            autoHidePrompt: true,
            autoHideDelay: 1000,
            fadeDuration: 0.3,
            focusFirstField: false,
            MaxErrorsPerField: 1
        });
    });

    $(".CommonDetachValidation1").click(function () {
        $(".CommonValidations1").validationEngine("detach")
    });


    $(".formattedCalender").datepicker({
        dateFormat: 'dd-M-yy',
        minDate: '0',
        required: true,
        changeMonth: true,
        changeYear: true,
        yearRange: "-0:+100"
    });

    $(".formattedCalenderinc").datepicker({
        dateFormat: 'dd-M-yy',
        maxDate: '0',
        required: true,
        changeMonth: true,
        changeYear: true,
        yearRange: "-100:+0"
    });


    $(".formattedCalenderAll").datepicker({
        dateFormat: 'dd-M-yy',
        required: true,
        changeMonth: true,
        changeYear: true,
        yearRange: "-100:+0"
    });

    $(".NoFutureDate").datepicker('option', 'minDate', null);
    $(".NoFutureDate").datepicker('option', 'maxDate', '0');

    $(".formattedCalenderinc, .formattedCalender, .formattedCalenderAll").keydown(function () {
        $(this).val('');
        return false;
    });

    $(".tel").intlTelInput();
    $(".tel,.num,.per,.num1").keydown(function (e) {
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            return;
        }
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
    $('.per').blur(function () {
        var intPremPer = $('.per').val();
        if (intPremPer > 100) {
            //alert("Percentage can not be more than 100%.");
            $("#errorMsg").html("Percentage can not be more than 100%.");
            $('#Error').modal('show');
            $(this).val("");
            $(this).focus();
            return false;
        }
        else
            $(this).tof
        return true;
    })

    var number = $('.num').val();
    if (number == 0) {
        $('.num').val('');
    }

    var percent = $('.per').val();
    if (percent == 0) {
        $('.per').val("");
    }

    $('.file').on('change', function () {
        myfile = $(this).val();
        var ext = myfile.split('.').pop();
        if (ext == "jpeg" || ext == "jpg" || ext == "pdf" || ext == "png" || ext == "docx" || ext == "doc") {

            var id = $(this).attr("id");
            var spanid = '#' + id + '_span';
            var filename = myfile.replace(/C:\\fakepath\\/i, '');
            var spanfilename = '';
            if (filename.length > 25) {

                spanfilename = filename.substring(0, 20) + '...' + ext;
            }
            else {
                spanfilename = filename = myfile.replace(/C:\\fakepath\\/i, '').substring(0, 25)
            }
            $(spanid).html(spanfilename);
        }
        else {
            //alert("Only allow files like *.jpg,*.png,*.doc,*.pdf");
            $("#errorMsg").html("Only allow files like *.jpg,*.png,*.doc,*.pdf");
            $('#Error').modal('show');
            $(this).val('');
        }

    });

    $('.onlyPDFfile').on('change', function () {
        myfile = $(this).val();
        var ext = myfile.split('.').pop();
        if (ext == "pdf") {

            var id = $(this).attr("id");
            var spanid = '#' + id + '_span';
            var filename = myfile.replace(/C:\\fakepath\\/i, '');
            var spanfilename = '';
            if (filename.length > 25) {

                spanfilename = filename.substring(0, 20) + '...' + ext;
            }
            else {
                spanfilename = filename = myfile.replace(/C:\\fakepath\\/i, '').substring(0, 25)
            }
            $(spanid).html(spanfilename);
        }
        else {
            $("#errorMsg").html("Only allow files like *.pdf");
            $('#Error').modal('show');
            $(this).val('');
        }
    });

    $('.onlyCSVfile').on('change', function () {
        myfile = $(this).val();
        var ext = myfile.split('.').pop();
        if (ext == "csv") {

            var id = $(this).attr("id");
            var spanid = '#' + id + '_span';
            var filename = myfile.replace(/C:\\fakepath\\/i, '');
            var spanfilename = '';
            if (filename.length > 25) {

                spanfilename = filename.substring(0, 20) + '...' + ext;
            }
            else {
                spanfilename = filename = myfile.replace(/C:\\fakepath\\/i, '').substring(0, 25)
            }
            $(spanid).html(spanfilename);
        }
        else {
            $("#errorMsg").html("Only allow files like *.csv");
            $('#Error').modal('show');
            $(this).val('');
        }
    });
    $('.Docfile').on('change', function () {
        myfile = $(this).val();
        var ext = myfile.split('.').pop();
        if (ext == "jpeg" || ext == "jpg" || ext == "pdf" || ext == "png" || ext == "docx" || ext == "doc") {

            var id = $(this).attr("id");
            var spanid = '#' + 'span_' + id;
            var filename = myfile.replace(/C:\\fakepath\\/i, '');
            var spanfilename = '';
            if (filename.length > 25) {

                spanfilename = filename.substring(0, 20) + '...' + ext;
            }
            else {
                spanfilename = filename = myfile.replace(/C:\\fakepath\\/i, '').substring(0, 25)
            }
            $(spanid).html(spanfilename);
        }
        else {
            $("#errorMsg").html("Only allow files like *.jpg,*.png,*.doc,*.pdf");
            $('#Error').modal('show');
            $(this).val('');
        }

    });
    $('.OpenTC').click(function () {
        $.ajax({
            url: "/MasterPage/TermsCondistions?tempName=Terms-Conditions",
            type: "POST",
            data: "{}",
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#termscondiDetail").html(data);
                return false;
            },
            error: function () {
                alert(Error);
            }
        });

    });
    $('.OpenPP').click(function () {
        $.ajax({
            url: "/MasterPage/TermsCondistions?tempName=privacy-policy",
            type: "POST",
            data: "{}",
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#termscondiDetail").html(data);
                return false;
            },
            error: function () {
                alert(Error);
            }
        });

    });
    $('.OpenSTC').click(function () {
        $.ajax({
            url: "/MasterPage/TermsCondistions?tempName=subscription-letter",
            type: "POST",
            data: "{}",
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $("#termscondiDetail").html(data);
                return false;
            },
            error: function () {
                alert(Error);
            }
        });

    });

    //$('.isEmailGood').blur(function () {
    //    var email = $('.isEmailGood').val();
    //    var invalidDomains = ["@gmail.", "@zoho.", "@yandex.", "@outlook.", "@aim.", "@icloud.", "@yahoo.", "@mail.", "@myway.", "@hotmail.", "@hushmail.", "@gmx.", "@aol.", "@lycos.", "@inbox.", "@easy.", "@mail2web.", "@india."];
    //    if (email != "")
    //        for (var i = 0; i < invalidDomains.length; i++) {
    //            var domain = invalidDomains[i];
    //            if (email.indexOf(domain) != -1) {
    //                $('.isEmailGood').val('');
    //                $('.isEmailGood').focus();
    //                $("#errorMsg").html("Must be business email.");
    //                $('#Error').modal('show');
    //                return false;
    //            }
    //        }
    //    return true;
    //});
    /*Factoring View*/
    $("#example, #exampleDB2, #exampleDB3, #exampleDB4").on("click", ".factoringView", function () {
        $("#processMsg").html("Please wait...");
        $('#process').modal('show');
        var row = $(this).closest("tr");
        var InvoiceID = row.find(".InvoiceID").find("span").html();
        var ProgramType = row.find(".ProgramType").find("span").html();
        var options = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: "/InvoiceCommon/FactoringView?InvoiceID=" + InvoiceID + "&ProgramType=" + ProgramType,
            contentType: "application/json; charset=utf-8",
            data: "{}",
            datatype: "json",
            success: function (data) {
                $('#factoringview').html(data);
                $('#process').modal('hide');
                $('#factoringModal').modal('show');
            },
        });
    });
    /*Invoice View*/
    $("#example, #exampleDB2, #exampleDB3, #funderTable").on("click", ".InvoiceView", function () {
        $("#processMsg").html("Please wait...");
        $('#process').modal('show');
        var row = $(this).closest("tr");
        var InvoiceID = row.find(".InvoiceID").find("span").html();
        var ProgramType = row.find(".ProgramType").find("span").html();
        var options = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: "/InvoiceCommon/InvoiceDetailsPopup?InvoiceID=" + InvoiceID + "&ProgramType=" + ProgramType,
            contentType: "application/json; charset=utf-8",
            data: "{}",
            datatype: "json",
            success: function (data) {
                $('#Invoiceview').html(data);
                $('#process').modal('hide');
                $('#InvoiceModal').modal('show');
            },
        });
    });
    /*Invoice Comment History View*/
    $("#example, #exampleDB2, #exampleDB3").on("click", ".InvoiceCommentHistoryView", function () {
        $("#processMsg").html("Please wait...");
        $('#process').modal('show');
        var row = $(this).closest("tr");
        var InvoiceID = row.find(".InvoiceID").find("span").html();
        var options = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: "/InvoiceCommon/InvoiceCommentHistoryPopup?InvoiceID=" + InvoiceID,
            contentType: "application/json; charset=utf-8",
            data: "{}",
            datatype: "json",
            success: function (data) {

                $('#Invoiceview').html(data);
                $('#process').modal('hide');
                $('#InvoiceModal').modal('show');
            },
        });
    });

    /*Transaction Popup*/
    $("#tranSig").on('click', function () {
        debugger;
        $('#TransPDFModal').modal('hide');
        $("#processMsg").html("Please wait...");
        $('#process').modal('show');
        var InvoiceID = $("#TranInvoiceID").val();
        var TranTemplateName = $("#TranTemplateName").val();
        var TranProgramType = $("#TranProgramType").val();
        var isSafari = /constructor/i.test(window.HTMLElement) || (function (p) { return p.toString() === "[object SafariRemoteNotification]"; })(!window['safari'] || (typeof safari !== 'undefined' && safari.pushNotification));
        if (isSafari) {
            var poppedWindow = window.open('about:blank', '', "top=100,left=100,width=1100,height=600");
        }
        $.ajax({
            url: "/InvoiceCommon/PreRequestPDF?TemplateName=" + TranTemplateName + "&InvoiceID=" + InvoiceID + "&ProgramType=" + TranProgramType,
            type: "GET",
            data: "{}",
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $('#process').modal('hide');
                var url = '/OnlineReg/eMudhra?UserSignID=' + data.UserSignID + '#SigDocPopup';
                if (isSafari) {
                    poppedWindow.location.replace(url);
                }
                else {
                    $('#Sigframe').attr('src', url)
                    $("#SigDocPopup").modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                    $('#SigDocPopup').modal({ show: true });
                }
                return false;
            },
        });
        return false;
    });

    $("#sigClose").on('click', function () {
        $('#SigDocPopup').modal('hide');
        setTimeout(function () {
            window.location.reload(true);
        });
    });

    /*Agreement Popup*/
    $("#AgreementSign").on('click', function () {
        $('#AgreementPDFModal').modal('hide');
        $("#processMsg").html("Please wait...");
        $('#process').modal('show');
        var TranTemplateName = $("#AgreementTemplateName").val();
        var isSafari = /constructor/i.test(window.HTMLElement) || (function (p) { return p.toString() === "[object SafariRemoteNotification]"; })(!window['safari'] || (typeof safari !== 'undefined' && safari.pushNotification));
        if (isSafari) {
            var poppedWindow = window.open('about:blank', '', "top=100,left=100,width=1100,height=600");
        }
        $.ajax({
            url: "/InvoiceCommon/PreAgreementRequestPDF?TemplateName=" + TranTemplateName,
            type: "GET",
            data: "{}",
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $('#process').modal('hide');
                var url = '/OnlineReg/eMudhra?UserSignID=' + data.UserSignID + '';
                if (isSafari) {
                    poppedWindow.location.replace(url);
                }
                else {
                    $('#AgreementSignframe').attr('src', url)
                    $("#AgreementSignDocPopup").modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                    $('#AgreementSignDocPopup').modal({ show: true });
                }
                return false;
            },
        });
        return false;
    });

    $("#AgreementSign1").on('click', function () {
        $('#AgreementeSigDocPopup').modal('hide');
        $("#processMsg").html("Please wait...");
        $('#process').modal('show');
        var TranTemplateName = $("#AgreementTemplateName").val();
        var isSafari = /constructor/i.test(window.HTMLElement) || (function (p) { return p.toString() === "[object SafariRemoteNotification]"; })(!window['safari'] || (typeof safari !== 'undefined' && safari.pushNotification));
        if (isSafari) {
            var poppedWindow = window.open('about:blank', '', "top=100,left=100,width=1100,height=600");
        }
        $.ajax({
            url: "/InvoiceCommon/PreAgreementRequestPDF?TemplateName=" + TranTemplateName,
            type: "GET",
            data: "{}",
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $('#process').modal('hide');
                var url = '/OnlineReg/eMudhra?UserSignID=' + data.UserSignID + '';
                if (isSafari) {
                    poppedWindow.location.replace(url);
                }
                else {
                    $('#AgreementSignframe').attr('src', url)
                    $("#AgreementSignDocPopup").modal({
                        backdrop: 'static',
                        keyboard: false
                    });
                    $('#AgreementSignDocPopup').modal({ show: true });
                }
                return false;
            },
        });
        return false;
    });

    $("#AgreementsignClose").on('click', function () {
        $('#AgreementSignDocPopup').modal('hide');
        setTimeout(function () {
            //location.reload();
            window.location.reload(true);
        });
    });
}

/* Customer Registration */
function OnlineUser() {
    $('#ConfirmEmailID').blur(function () {

        var Email = $("#EmailID").val();
        var ConfirmEmailID = $("#ConfirmEmailID").val();
        if (ConfirmEmailID != "")
            //if (!isEmailGood(Email)) {
            //    $("#result").html("<span id='spanRed' style='color:red'><i class='fa fa-times 3x'></i>Must be Business email.</span>");
            //    $("#ConfirmEmailID").css('color', 'red');
            //    return false;
            //}

            if (Email == ConfirmEmailID) {
                $("#result").html("<span id='spanGreen' style='color:green'><i class='fa fa-check 3x'></i></span>");
                $("#ConfirmEmailID").css('color', 'green');
                return true;
            }
            else {
                $("#result").html("<span id='spanRed' style='color:red'><i class='fa fa-times 3x'></i>Confirm email does not match.</span>");
                $("#ConfirmEmailID").css('color', 'red');
                return false;
            }
    })
    $("#submit").click(function () {
        if ($("#spanRed").length > 0) {
            return false;
        }
        var Email = $("#EmailID").val();
        var ConfirmEmailID = $("#ConfirmEmailID").val();

        if (Email != ConfirmEmailID) {
            $("#errorMsg").html("Email dosen't match !!");
            $('#Error').modal('show');
            return false;
        }

    });

    var IndustryID = $("#IndustryID").val();
    if (IndustryID != "") {
        if (typeof (IndustryID) != 'undefined') {
            var hdnSubIndustryID = $("#hdnSubIndustryID").val();
            GetSubIndustry(IndustryID, hdnSubIndustryID);
        }
    }
    else {
        $('#SubIndustryID').append($('<option></option>').val("").html("--Select--"));
        $('#SubIndustryID').val("");
    }

    $("#IndustryID").on("change", function () {
        var IndustryID = $(this).val();
        $("#SubIndustryID").empty();
        GetSubIndustry(IndustryID, 0);
        $('#SubIndustryID').append($('<option></option>').val("").html("--Select--"));
    });

    function GetSubIndustry(IndustryID, hdnSubIndustryID) {
        if (typeof (IndustryID) != 'undefined' && typeof (hdnSubIndustryID) != 'undefined') {
            $.ajax({
                url: "/OnlineReg/GetSubIndustryData?IndustryID=" + IndustryID,
                type: "POST",
                data: "{}",
                datatype: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    for (var i = 0; i < data.length; i++) {
                        if (data[i] != '') {
                            $("#SubIndustryID").append("<option value=" + data[i].IndustryID + ">" + data[i].IndustryName + "</option>");
                        }
                    }
                    if (parseInt(hdnSubIndustryID) > 0) {
                        $("#SubIndustryID").val(hdnSubIndustryID);
                    }

                    return false;
                },
                error: function () {
                    alert(Error);
                }
            });
        }
    }

}
/* Customer More Info */
function CompanyMoreInfo() {
    $("#cb1").change(function () {
        document.getElementById("cb1").checked = true;
        document.getElementById("cb2").checked = false;
        $(".group").css("display", "block");
    });

    $("#cb2").change(function () {
        document.getElementById("cb2").checked = true;
        document.getElementById("cb1").checked = false;
        $(".group").css("display", "none");
    });

    $("#cb3").change(function () {
        document.getElementById("cb3").checked = true;
        document.getElementById("cb4").checked = false;
        $(".publicly").css("display", "block");
    });

    $("#cb4").change(function () {
        document.getElementById("cb4").checked = true;
        document.getElementById("cb3").checked = false;
        $(".publicly").css("display", "none");
    });
    $("#cb5").change(function () {
        document.getElementById("cb5").checked = true;
        document.getElementById("cb6").checked = false;
        $(".financial").css("display", "block");
    });

    $("#cb6").change(function () {
        document.getElementById("cb6").checked = true;
        document.getElementById("cb5").checked = false;
        $(".financial").css("display", "none");
    });

    $("#AnnTurnOver").val(parseFloat($("#AnnTurnOver").val(), 10).toFixed(2));
    $("#SaleVolume").val(parseFloat($("#SaleVolume").val(), 10).toFixed(2));
    $("#NetProfit").val(parseFloat($("#NetProfit").val(), 10).toFixed(2));



}

/*shareholder*/

function shareholder(Urlval) {

    if (Urlval == "Indi") {
        $(".Share-company").css("display", "none");
        $(".Share-individual").css("display", "block");
        $(".authorized").css("display", "none");
        $("#FirstName").focus();

    } else if (Urlval == "Comp") {
        $(".Share-company").css("display", "block");
        $(".Share-individual").css("display", "none");
        $("#CompanyName").focus();
    } else {
        $(".Share-company").css("display", "none");
        $(".Share-individual").css("display", "none");
    }


    $("#GetAuthSignCombo").on("change", function () {
        var CustomerShareHoldID = $(this).val();
        if (CustomerShareHoldID != "") {
            GetAuthSignDetails(CustomerShareHoldID);
        }
        else {
            $('#FirstName').val("");

            $('#FirstName').val("");
            $('#MiddleName').val("");
            $('#LastName').val("");
            $('#Gender').val("");
            $('#LocCountryID').val("");
            $('#NatCountryID').val("");
            $('#PassCountryID').val("");
            $('#PassNo').val("");
            $('#PassIssueDate').val("");
            $('#PassExpDate').val("");
        }
    });

    function GetAuthSignDetails(CustomerShareHoldID) {
        $.ajax({
            url: "/CustomerReg/GetAuthSignDetails?CustomerAuthSignID=" + CustomerShareHoldID,
            type: "POST",
            data: "{}",
            datatype: "json",
            async: false,
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data != null) {
                    $.each(data, function (i, CustData) {

                        $('#FirstName').val(CustData.FirstName);
                        $('#MiddleName').val(CustData.MiddleName);
                        $('#LastName').val(CustData.LastName);
                        $('#Gender').val(CustData.Gender);
                        $('#LocCountryID').val(CustData.LocCountryID);
                        $('#NatCountryID').val(CustData.NatCountryID);
                        $('#PassCountryID').val(CustData.PassCountryID);
                        $('#PassNo').val(CustData.PassNo);
                        var ConvertIssuanceDate = Dateconverts(CustData.PassIssueDate);
                        $('#PassIssueDate').val(ConvertIssuanceDate);

                        var ConvertExpiryDate = Dateconverts(CustData.PassExpDate);
                        $('#PassExpDate').val(ConvertExpiryDate);

                        var a = document.getElementById('ViewPassport');
                        a.href = "/Files/Upload/" + CustData.UpdPassport;
                        $("#ViewPassport").attr("style", "display:block");
                        $('#UpdPassport').val(CustData.UpdPassport);
                        $("#1UpdPassport").attr("class", "1validate123[required] file");


                        $('#UpdNatIden1').val(CustData.UpdNatIden);
                        var a1 = document.getElementById('ViewNationalID');
                        a1.href = "/Files/Upload/" + CustData.UpdNatIden;
                        if (CustData.UpdNatIden != "") {
                            $("#ViewNationalID").attr("style", "display:block");
                        }
                    });
                }
                return false;
            }
        });
    }

    function Dateconverts(DBDate) {
        var datedb = eval(DBDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
        var newdate = datedb.toLocaleDateString();
        var d = new Date(newdate);
        var curr_date = d.getDate();
        if (curr_date < 10) {
            curr_date = '0' + curr_date
        }
        var curr_year = d.getFullYear();

        //var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
        var monthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']

        var m = new Date(newdate);
        var ReturnDate = curr_date + "-" + monthNames[m.getMonth()] + "-" + curr_year;
        return ReturnDate;
    }

    $("#ComSaveAdd,#ComNextSave").on("click", function () {
        $('.ValRemo2').removeClass(" input-validation-error");
        $('.ValRemo2').rules('remove');
        $('.ValRemo1').removeClass(" input-validation-error");
        $('.ValRemo1').rules('remove');
    });
    $('.per1').blur(function () {
        var intPremPer = $('.per1').val();
        if (intPremPer > 100) {

            $("#errorMsg").html("Percentage can not be more than 100%.");
            $('#Error').modal('show');
            $(this).val("");
            $(this).focus();
            return false;
        }
        else
            return true;
    })

    $("#cb1").change(function () {
        document.getElementById("cb1").checked = true;
        document.getElementById("cb2").checked = false;
        $(".Share-individual").css("display", "block");
        $(".Share-company").css("display", "none");
        $(".Share-individual-header").css("display", "block");
        $(".Share-company-header").css("display", "none");
        $("#FirstName").focus();
        return false;
    });

    $("#cb2").change(function () {
        document.getElementById("cb2").checked = true;
        document.getElementById("cb1").checked = false;
        $(".Share-company").css("display", "block");
        $(".Share-individual").css("display", "none");
        $(".Share-company-header").css("display", "block");
        $(".Share-individual-header").css("display", "none");
        $("#CompanyName").focus();
        return false;
    });
    $("#SharePer").val(parseFloat($("#SharePer").val(), 10).toFixed(2));
    $("#CompSharePer").val(parseFloat($("#CompSharePer").val(), 10).toFixed(2));
}

function ShareBeneficial() {
    $('#benSave').on('click', function () {

        var ShareFirstName = $(".ShareFirstName").val();
        if (ShareFirstName == "") {
            $("#errorMsg").html("Please enter first name.");
            $('#Error').modal('show');
            $(".ShareFirstName").focus();
            return false;
        }

        var ShareLastName = $(".ShareLastName").val();
        if (ShareLastName == "") {
            $("#errorMsg").html("Please enter Last name.");
            $('#Error').modal('show');
            $(".ShareLastName").focus();
            return false;
        }
        var ShareGenderID = $(".Gender").val();
        if (ShareGenderID == "0" || ShareGenderID == "") {
            $("#errorMsg").html("Please Select Gender.");
            $('#Error').modal('show');
            $(".ShareGender").focus();
            return false;
        }

        var ShareLocCountryID = $(".ShareLocCountryID").val();
        if (ShareLocCountryID == "0" || ShareLocCountryID == "") {
            $("#errorMsg").html("Please Select Country of Residence.");
            $('#Error').modal('show');
            $(".ShareLocCountryID").focus();
            return false;
        }
        var childSharePer = $(".childSharePer").val();
        if (childSharePer == "") {
            $("#errorMsg").html("Please enter Share Per %.");
            $('#Error').modal('show');
            $(".childSharePer").focus();
            return false;
        }

        var ShareNatCountryID = $(".ShareNatCountryID").val();
        if (ShareNatCountryID == "0" || ShareNatCountryID == "") {
            $("#errorMsg").html("Please Select Nationality.");
            $('#Error').modal('show');
            $(".ShareNatCountryID").focus();
            return false;
        }

        var SharePassNo = $(".SharePassNo").val();
        if (SharePassNo == "") {
            $("#errorMsg").html("Please enter Passport Number.");
            $('#Error').modal('show');
            $(".SharePassNo").focus();
            return false;
        }

        var SharePassCountryID = $(".SharePassCountryID").val();
        if (SharePassCountryID == "0" || SharePassCountryID == "") {
            $("#errorMsg").html("Please Select Place of Issuance.");
            $('#Error').modal('show');
            $(".SharePassCountryID").focus();
            return false;
        }

        var SharePassNo = $(".SharePassIssueDate").val();
        if (SharePassNo == "") {
            $("#errorMsg").html("Please enter Issue Date.");
            $('#Error').modal('show');
            $(".SharePassIssueDate").focus();
            return false;
        }


        var SharePassNo = $(".SharePassExpDate").val();
        if (SharePassNo == "") {
            $("#errorMsg").html("Please enter Expiry Date.");
            $('#Error').modal('show');
            $(".SharePassExpDate").focus();
            return false;
        }



        //var ShareUpdPassport = $("#ShareUpdPassport").val();
        //if (ShareUpdPassport == "") {
        //    $("#errorMsg").html("Please Upload Passport copy.");
        //    $('#Error').modal('show');
        //    $("#UpdPassport").focus();
        //    return false;
        //}


        var data = new FormData();
        var ShareFirstName = document.getElementById("ShareFirstName").value;
        data.append("ShareFirstName", ShareFirstName);

        var ShareLastName = document.getElementById("ShareLastName").value;
        data.append("ShareLastName", ShareLastName);

        var ShareMiddleName = document.getElementById("ShareMiddleName").value;
        data.append("ShareMiddleName", ShareMiddleName);

        var childSharePer = document.getElementById("childSharePer").value;
        data.append("childSharePer", childSharePer);

        var SharePassCountryID = document.getElementById("SharePassCountryID").value;
        data.append("SharePassCountryID", SharePassCountryID);

        var SharePassNo = document.getElementById("SharePassNo").value;
        data.append("SharePassNo", SharePassNo);

        var ShareNatCountryID = document.getElementById("ShareNatCountryID").value;
        data.append("ShareNatCountryID", ShareNatCountryID);

        var SharePassIssueDate = document.getElementById("SharePassIssueDate").value;
        data.append("SharePassIssueDate", SharePassIssueDate);

        var SharePassExpDate = document.getElementById("SharePassExpDate").value;
        data.append("SharePassExpDate", SharePassExpDate);

        var ShareCustomerShareHoldID = document.getElementById("ShareCustomerShareHoldID").value;
        data.append("ShareCustomerShareHoldID", ShareCustomerShareHoldID);

        var ParentID = document.getElementById("ParentID").value;
        data.append("ParentID", ParentID);

        if (document.getElementById('ShareUpdNatIden') != null) {
            var ShareUpdNatIden = document.getElementById("ShareUpdNatIden").value;
            data.append("ShareUpdNatIden", ShareUpdNatIden);
        }

        var ShareUpdPassport = document.getElementById("ShareUpdPassport").value;
        data.append("ShareUpdPassport", ShareUpdPassport);

        var Status = document.getElementById("Status").value;
        data.append("Status", Status);

        var CustomerID = document.getElementById("CustomerID").value;
        data.append("CustomerID", CustomerID);


        var ShareGender = document.getElementById("ShareGender").value;
        data.append("ShareGender", ShareGender);

        var ShareLocCountryID = document.getElementById("ShareLocCountryID").value;
        data.append("ShareLocCountryID", ShareLocCountryID);


        var ShareUpdNatIden1 = document.getElementById("ShareUpdNatIden1").files[0];
        var ShareUpdPassport1 = document.getElementById("ShareUpdPassport1").files[0];
        data.append("ShareUpdNatIden1", ShareUpdNatIden1);
        data.append("ShareUpdPassport1", ShareUpdPassport1);
        //var data = $('form').serialize(); 
        $.ajax({

            type: 'POST',
            dataType: 'json',
            cache: false,
            url: '/CustomerReg/ShareholdersBeneficialPopUpPost',
            data: data,
            contentType: false,
            processData: false,
            success: function (data) {

                if (data != null) {
                    $('#Beneficial').modal('hide');
                    setTimeout(function () {
                        location.reload();
                    }, 500);
                } else {
                    $("#errorMsg").html("Error in update information.");
                    $('#Error').modal('show');
                }
                return false;
            },
            error: function (jqXHR) {
                $("#errorMsg").html("Error in update information.");
                $('#Error').modal('show');
                return false;
            }

        });

    });


    $("#ComSaveAdd,#ComNextSave,#parSave").on("click", function () {
        $('.ValRemo2').removeClass(" input-validation-error");
        $('.ValRemo2').rules('remove');
        $('.ValRemo1').removeClass(" input-validation-error");
        $('.ValRemo1').rules('remove');
    });

    $('.childSharePer').blur(function () {

        var intPremPer = $('.childSharePer').val();
        if (intPremPer > 100) {

            $("#errorMsg").html("Percentage can not be more than 100%.");
            $('#Error').modal('show');
            $(this).val("");
            $(this).focus();
            return false;
        }
        else
            return true;
    })
    $(".SharePassExpDate").datepicker({
        dateFormat: 'dd-M-yy',
        minDate: '0',
        required: true,
        changeMonth: true,
        changeYear: true,
        yearRange: "-0:+100"
    });

    $(".SharePassIssueDate").datepicker({
        dateFormat: 'dd-M-yy',
        maxDate: '0',
        required: true,
        changeMonth: true,
        changeYear: true,
        yearRange: "-100:+0"
    });
    $(".childSharePer").keydown(function (e) {
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            (e.keyCode >= 35 && e.keyCode <= 40)) {
            return;
        }
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
    $("#childSharePer").val(parseFloat($("#childSharePer").val(), 10).toFixed(2));
}

function ShareCompany() {
    $("#example21").on("click", ".OpenList", function () {
        var row = $(this).closest("tr");
        var CustomerID = row.find(".CustomerID").find("span").html();
        var CustomerShareHoldDetID = row.find(".CustomerShareHoldDetID").find("span").html();
        var options = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: "/CustomerReg/GridShareholdersCompIndiPop?CustomerID=" + CustomerID + "&ParentID=" + CustomerShareHoldDetID,
            contentType: "application/json; charset=utf-8",
            data: "{}",
            datatype: "json",
            success: function (data) {

                $('#myModalContent').html(data);
                $('#myModal').modal('show');
            },
        });
    });
}

/*Bank Account*/

function BankAccount() {

    $("#BankSave,#BankSaveNext").on("click", function () {
        var SwiftCode = $("#Swift").val();
        var IFSCCode = $("#IFSC").val();
        var BankCountryID = $("#BankCountryID").val();
        if (BankCountryID != "" && SwiftCode == "" && IFSCCode == "") {
            $("#errorMsg").html("Please enter either swift or IFSC code.");
            $('#Error').modal('show');

            return false;
        }

        else {
            return true;
        }
    });
    
    $("#NextSave").on("click", function () {
        $('.ValRemo2').removeClass(" input-validation-error");
        $('.ValRemo2').rules('remove');
        $('.ValRemo1').removeClass(" input-validation-error");
        $('.ValRemo1').rules('remove');
    });
    $("#FacLimit").val(parseFloat($("#FacLimit").val(), 10).toFixed(2));
    $("#FacUtilize").val(parseFloat($("#FacUtilize").val(), 10).toFixed(2));
}

/*Supplier*/
function Supplier() {
    var CountryID = $("#CountryID").val();
    if (CountryID != "") {
        //$.LoadingOverlay("show");
        $("#CityID").empty();
        $('#CityID').append($('<option></option>').val("0").html("-- Select --"));
        GetCity(CountryID);
        document.getElementById("CityID").className = document.getElementById("CityID").className.replace("error ", "");
        //setTimeout(function () {
        //    $.LoadingOverlay("hide");
        //}, 500);
    }
    else {
        $('#CityID').append($('<option></option>').val("0").html("-- Select --"));
        $('#CityID').val("0");
    }

    $("#CountryID").on("change", function () {
        $.LoadingOverlay("show");
        $('#DV_OtherCity').hide();
        var CountryID = $(this).val();
        $("#CityID").empty();
        $('#CityID').append($('<option></option>').val("0").html("-- Select --"));
        GetCity(CountryID);
        $("#CityID").focus();
        setTimeout(function () {
            $.LoadingOverlay("hide");
        }, 500);
    });

    function GetCity(CountryID) {
        $.ajax({
            url: "/ServiceProvider/GetCityData?CountryID=" + CountryID,
            type: "POST",
            data: "{}",
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                $.each(data, function (i, City) {
                    $("#CityID").append(
                        $('<option></option>').val(City.CityID).html(City.CityName));
                });
                $('#CityID').val($("#GetCityID").val());
                $('#CityID').append($('<option></option>').val("00").html("Other"));

                document.getElementById("CityID").className = document.getElementById("CityID").className.replace("error ", "");
                document.getElementById("CityID").className = "error " + document.getElementById("CityID").className;

                var DtCityID = $("#CityID").val();
                if (DtCityID != 0) {
                    document.getElementById("CityID").className = document.getElementById("CityID").className.replace("error ", "");
                }
                if (DtCityID == null) {
                    document.getElementById("CityID").className = "error " + document.getElementById("CityID").className;
                }

                var OtherCity = $("#OtherCity").val();
                if (OtherCity != "" && typeof (OtherCity) != 'undefined') {
                    $("#CityID").val("00");
                }
                return false;
            },
        });
    }

    // Add Other City Code
    $("#CityID").on("change", function () {
        AddOtherCity();
    });

    var OtherCity = $("#OtherCity").val();
    if (OtherCity != "" && typeof (OtherCity) != 'undefined') {
        EditOtherCity();
    }

    function AddOtherCity() {
        var CityID = $("#CityID").val();
        if (CityID != "") {
            var skillsSelect = document.getElementById("CityID");
            var selectedText = skillsSelect.options[skillsSelect.selectedIndex].text;
            if (selectedText == "Other") {
                $('#DV_OtherCity').show(); $("#OtherCity").val(""); $("#OtherCity").focus();
            }
            else {
                $('#DV_OtherCity').hide(); $("#OtherCity").val("");
            }
        }
    }

    function EditOtherCity() {
        var OtherCity = $("#OtherCity").val();
        if (OtherCity != "" && typeof (OtherCity) != 'undefined') {
            $('#DV_OtherCity').show(); $("#OtherCity").focus();

        }
        else { $('#DV_OtherCity').hide(); $("#OtherCity").val(""); }
    }

    var IndustryID = $("#IndustryID").val();
    if (IndustryID != "") {
        GetSubIndustry(IndustryID);
    }
    else {
        $('#SubIndustryID').append($('<option></option>').val("0").html("-- Select --"));
        $('#SubIndustryID').val("0");
    }

    $("#IndustryID").on("change", function () {
        var IndustryID = $(this).val();
        $("#SubIndustryID").empty();
        GetSubIndustry(IndustryID);
        $("#SubIndustryID").focus();
    });

    function GetSubIndustry(IndustryID) {
        $.ajax({
            url: "/OnlineReg/GetSubIndustryData?IndustryID=" + IndustryID,
            type: "POST",
            data: "{}",
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i] != '') {
                        $("#SubIndustryID").append("<option value=" + data[i].IndustryID + ">" + data[i].IndustryName + "</option>");
                    }
                }
                $("#SubIndustryID").val($("#hdnSubIndustryID").val());
                return false;
            }
        });
    }

    $("#InvAmt").val(parseFloat($("#InvAmt").val(), 10).toFixed(2));



    // Add Other Fields


    // Add Other Designation Code
    var DesignationID = $("#DesignationID").val();
    if (DesignationID != "") {
        AddOtherDesignation();
    }

    function AddOtherDesignation() {
        var IndustryID = $("#DesignationID").val();
        if (IndustryID != "") {
            var skillsSelect = document.getElementById("DesignationID");
            var selectedText = skillsSelect.options[skillsSelect.selectedIndex].text;
            if (selectedText == "Other") {
                $('#DV_OtherDesignation').show();
                $("#OtherDesignation").focus();
            }
            else {
                $('#DV_OtherDesignation').hide();
                $("#OtherDesignation").val("");
            }
        }
    }

    $("#DesignationID").on("change", function () {
        AddOtherDesignation();
    });


    // Add Other Industry Code
    var IndustryID = $("#IndustryID").val();
    if (IndustryID != "") {
        if (typeof (IndustryID) != 'undefined') {
            AddOtherIndustry();
        }
    }

    function AddOtherIndustry() {
        var IndustryID = $("#IndustryID").val();
        if (IndustryID != "") {
            var skillsSelect = document.getElementById("IndustryID");
            var selectedText = skillsSelect.options[skillsSelect.selectedIndex].text;
            if (selectedText == "Other") {
                $('#DV_OtherIndustry').show();
                //$('#DV_OtherSubIndustry').show();
                $('#DV_SubIndustry').hide();
                $("#OtherIndustry").focus();
            }
            else {
                $('#DV_OtherIndustry').hide();
                //$('#DV_OtherSubIndustry').hide();
                $('#DV_SubIndustry').show();
                $("#OtherIndustry").val("");
                $("#OtherSubIndustry").val("");
            }
        }
    }

    $("#IndustryID").on("change", function () {
        AddOtherIndustry();
    });
}

function SupplierPop() {
    $("#example").on("click", ".OpenList", function () {
        $("#processMsg").html("Please wait...");
        $('#process').modal('show');
        var row = $(this).closest("tr");
        var CustomerID = row.find(".CustomerID").find("span").html();
        var CustomerSuppID = row.find(".CustomerSuppID").find("span").html();
        var options = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: "/CustomerReg/SupplierDetailPop?CustomerID=" + CustomerID + "&CustomerSuppID=" + CustomerSuppID,
            contentType: "application/json; charset=utf-8",
            data: "{}",
            datatype: "json",
            success: function (data) {
                $('#myModalContent').html(data);
                $('#process').modal('hide');
                $('#myModal').modal('show');
            },
        });
    });
}
/*Bank Account*/

function ExporterBankAccount() {

    $("#bankSave,#bankNextSave").on("click", function () {
        var SwiftCode = $("#Swift").val();
        var IFSCCode = $("#IFSC").val();
        var BankCountryID = $("#BankCountryID").val();
        if (BankCountryID != "" && SwiftCode == "" && IFSCCode == "") {
            $("#errorMsg").html("Please enter either swift or IFSC code.");
            $('#Error').modal('show');

            return false;
        }

        else {
            return true;
        }
    });
}

/*Document*/
function Document() {


    $(document).ready(function () {
        $(".newClassExistData").each(function () {

            var ID = $(this).attr("id");
            var value = $(this).val();
            var IDNEw = ID.split('_')[1];

            if (value == "") {
                $("#chk_" + IDNEw).prop("checked", false);
            }
            else {
                $("#chk_" + IDNEw).prop("checked", true);
            }
        });

    });

    $(".newClassExistData").on("change", function () {
        var ID = $(this).attr("id");
        var value = $(this).val();
        var IDNEw = ID.split('_')[1];

        if (value == "") {
            $("#chk_" + IDNEw).prop("checked", false);
        }
        else {
            $("#chk_" + IDNEw).prop("checked", true);
        }
    });
    $("#Submit").click(function () {
        if (document.getElementById("cb1").checked == false) {
            $("#errorMsg").html("Please accept the declaration before submitting the form.");
            $('#Error').modal('show');
            //alert("Please accept the declaration before submitting the form.");
            document.getElementById("cb1").focus();
            return false;
        }
    });
}

/*AccountActivation*/
function AccountActivation() {
    $("#cb1").change(function () {
        document.getElementById("cb1").checked = true;
        document.getElementById("cb2").checked = false;
        $(".div-MultiProgram").css("display", "none");
        $(".div-incomplate").css("display", "block");
        $(".div-complate").css("display", "none");
        $(".btn-incomplate").css("display", "block");
        $("#Activate").hide();
        $("#onhold").show();
        $("#Reject").show();
        $("#Return").show();
        return false;
    });

    $("#cb2").change(function () {
        document.getElementById("cb2").checked = true;
        document.getElementById("cb1").checked = false;
        $(".div-complate").css("display", "block");
        $(".div-incomplate").css("display", "none");
        $(".btn-incomplate").css("display", "block");
        $("#Activate").show();
        $("#onhold").hide();
        $("#Reject").hide();
        $("#Return").hide();
        return false;
    });

    $("#cb3").change(function () {
        document.getElementById("cb3").checked = true;
        document.getElementById("cb4").checked = false;
        $(".divInsurance").css("display", "block");
        return false;
    });

    $("#cb4").change(function () {
        document.getElementById("cb4").checked = true;
        document.getElementById("cb3").checked = false;
        $(".divInsurance").css("display", "none");
        $("#PolicyNo").val("");
        $("#Attach").val("");
        $("#IssueDate").val("");
        $("#ExpDate").val("");
        return false;
    });

    var selected_tags_arr = new Array();
    var selected_tags = $('#CustomerShareHoldIDs').val();
    selected_tags_arr = selected_tags.split(",");
    $('#CustomerShareHoldIDs_List option').each(function () {
        var option_val = this.value;
        for (var i in selected_tags_arr) {
            if (selected_tags_arr[i] == option_val) {
                $("#CustomerShareHoldIDs_List option[value='" + this.value + "']").attr("selected", 1);
            }
        }
    });

    $('#CustomerShareHoldIDs_List').multiselect({
        includeSelectAllOption: true,
        maxHeight: '300',
        buttonWidth: '254.5',

    });

    var selected_tags_arr1 = new Array();
    var selected_tags1 = $('#DocumentIDs').val();
    selected_tags_arr1 = selected_tags1.split(",");
    $('#DocumentIDs_List option').each(function () {
        var option_val1 = this.value;
        for (var i in selected_tags_arr1) {
            if (selected_tags_arr1[i] == option_val1) {
                $("#DocumentIDs_List option[value='" + this.value + "']").attr("selected", 1);
            }
        }
    });

    $('#DocumentIDs_List').multiselect({
        includeSelectAllOption: true,
        maxHeight: '300',
        buttonWidth: '254.5',

    });

    $("#Save,#onhold,#Activate").click(function () {
        if ($("#CompleteDate").val() != "" && document.getElementById("cb1").checked == false && document.getElementById("cb2").checked == false) {
            $("#errorMsg").html("Please complete the verification process.");
            $('#Error').modal('show');
            document.getElementById("cb1").focus();
            return false;
        } else {
            return true;
        }


    });

    $("#Reject,#onhold").click(function () {
        $('#ProgramCode').removeClass(" validate[required]");
        $('#ProgramCode').rules('remove');
        return true;
    });

    if (document.getElementById("cb1").checked) {
        $(".div-incomplate").css("display", "block");
        $(".div-complate").css("display", "none");
    }
    if (document.getElementById("cb2").checked) {
        $(".div-complate").css("display", "block");
        $(".div-incomplate").css("display", "none");
    }
    var cb3 = document.getElementById('cb3');
    if (cb3 != null) {
        if (document.getElementById("cb3").checked) {
            $(".divInsurance").css("display", "block");
        }
    }

}

/*Maker Index*/
function MakerIndex() {
    //$("#cb1").change(function () {
    //    document.getElementById("cb1").checked = true;
    //    document.getElementById("cb2").checked = false;
    //    return false;
    //});

    //$("#cb2").change(function () {
    //    document.getElementById("cb2").checked = true;
    //    document.getElementById("cb1").checked = false;
    //    return false;
    //});

    //$("#Maker").click(function () {
    //    if (document.getElementById("cb1").checked == false && document.getElementById("cb2").checked == false) {
    //        $("#errorMsg").html("Please select Process.");
    //        $('#Error').modal('show');
    //        document.getElementById("cb1").focus();
    //        return false;
    //    } else {
    //        return true;
    //    }
    //});
}

/*Credit Review*/
function CreditReview() {

    $("#ProfitRate").on("change", function () {
        var ProfitRate = $(this).val();
        var BaseRate = $("#BaseRate").val();
        if (ProfitRate != "" && BaseRate != "") {
            var DiscRate = parseFloat(ProfitRate) + parseFloat(BaseRate);
            $('#DiscRate').val(parseFloat(DiscRate));
        }
        else {
            $('#DiscRate').val("");
        }
    });
    $("#BaseRate").on("change", function () {

        var BaseRate = $(this).val();
        var ProfitRate = $("#ProfitRate").val();
        if (ProfitRate != "" && BaseRate != "") {
            var DiscRate = parseFloat(ProfitRate) + parseFloat(BaseRate);
            $('#DiscRate').val(parseFloat(DiscRate));
        }
        else {
            $('#DiscRate').val("");
        }
    });

    $("#Submit").on("click", function () {
        var DtLicExpDate = $("#OtherFee").val();
        if (DtLicExpDate == "") {
            document.getElementById("OtherFee").className = document.getElementById("OtherFee").className.replace("error ", "");
            document.getElementById("OtherFee").className = "error " + document.getElementById("OtherFee").className;
        } else {
            document.getElementById("OtherFee").className = document.getElementById("OtherFee").className.replace("error ", "");
            document.getElementById("OtherFee").className = document.getElementById("OtherFee").className.replace(" input-validation-error", "");
        }
        $('#OtherFee').rules('remove');
        //input-validation-error
    });
}

/*Credit Review*/

//function RedirectToPageName1(id, bredCrum) {
//    $.ajax({
//        url: "/mUserMasters/RedirectToPageName",
//        type: "POST",
//        data: '{id:"' + ID + '",bredCrum:"' + bredCrum + '"}',
//        datatype: "json",
//        contentType: "application/json; charset=utf-8",
//        success: function (result) {
//            window.location = result;
//            return true;
//        },
//        error: function () {
//            return false;
//        }
//    });

//}

function MenuRedirectToPageName() {
    $(document).ready(function () {
        $('.Parent-sub').click(function (evant) {
            // event.stopPropagation();
            if ($(this).parent('.Parent').find('.child').hasClass('Open') == false) {
                $('.Parent').find('.child').removeClass('Open');
                $(this).parent('.Parent').find('.child').addClass('Open');
            }
            else {
                $('.Parent').find('.child').removeClass('Open');
            }
        });
    });
}

/*Auth USer Agreement*/
function AuthUserAgree() {
    $("#accept,#reject").click(function () {
        if ((document.getElementById("cb2").checked) && (document.getElementById("cb3").checked) && (document.getElementById("cb4").checked)) {
            return true;
        }
        else {
            $("#errorMsg").html("Please select all Document.");
            $('#Error').modal('show');
            return false;
        }
    });
}

function AuthUserAgreeFunder() {
    $("#accept,#reject").click(function () {
        if ((document.getElementById("cb2").checked) && (document.getElementById("cb3").checked)) {
            return true;
        }
        else {
            $("#errorMsg").html("Please select all Document.");
            $('#Error').modal('show');
            return false;
        }
    });
}
function SupplierUserAgree() {
    $("#accept,#reject").click(function () {
        if ((document.getElementById("cb2").checked)) {
            return true;
        }
        else {
            $("#errorMsg").html("Please select all Document.");
            $('#Error').modal('show');
            return false;
        }
    });
}

/*Document Popup*/
function CommonDocPopUp(TempName) {
    $.ajax({
        type: "GET",
        url: "/InvoiceCommon/CommonDocPopUp?TemplateName=" + TempName,
        contentType: "application/json; charset=utf-8",
        data: "{}",
        datatype: "json",
        success: function (data) {
            $('#factoringview').html(data);
            $('#process').modal('hide');
            $('#factoringModal').modal('show');
        },
    });
}

/*Transaction Popup*/
function CommonTransPopUp(TempName, InvoiceID, ProgramType) {
    $("#processMsg").html("Please wait...");
    $('#process').modal('show');
    $.ajax({
        type: "GET",
        url: "/InvoiceCommon/CommonTransPopUp?TemplateName=" + TempName + "&InvoiceID=" + InvoiceID + "&ProgramType=" + ProgramType,
        contentType: "application/json; charset=utf-8",
        data: "{}",
        datatype: "json",
        success: function (data) {
            $('#TransPDFview').html(data);
            $('#process').modal('hide');
            $('#TransPDFModal').modal('show');
        },
    });
}


/*Document Popup*/
function CommonAgreementPopUp(TempName, CustomerID) {
    $("#processMsg").html("Please wait...");
    $('#process').modal('show');
    $.ajax({
        type: "GET",
        url: "/InvoiceCommon/CommonAgreementPopUp?TemplateName=" + TempName + "&CustomerID=" + CustomerID,
        contentType: "application/json; charset=utf-8",
        data: "{}",
        datatype: "json",
        success: function (data) {
            $('#AgreementPDFview').html(data);
            $('#process').modal('hide');
            $('#AgreementPDFModal').modal('show');
        },
    });
}


/*CIF pdf Popup*/
function CommonDocPDFGene(CustomerID) {

    $("#processMsg").html("Please wait...");
    $('#process').modal('show');
    $.ajax({
        type: "POST",
        url: "/OnlineReg/GeneratePDF?CustomerID=" + CustomerID,
        contentType: "application/json; charset=utf-8",
        data: "{}",
        datatype: "json",
        success: function (data) {

            var url = '/Files/PDF/' + data.filename + '#toolbar=0';
            $('#dociframe').attr('src', url)
            $('#process').modal('hide');
            $('#DocPopup').modal({ show: true });
        },
    });
}


