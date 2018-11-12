
var newPurchaseOrCurrentlyInsured, whenNeedInsurence, noOfEmployee, industry, contactInformation, coverageamount, siccode, grossrevenue, acquisitionIsInsured,
    typeOfAuto, valueOfHome, language, notes, insuranceCompany, howManyUnits, deductibleIfany, currentLimit, NumberofEmployees, GrossPayroll, SubmitWithLogin, ZipCodeSection;
//grossSales, grossPayroll;
var colors;
var industryList, subIndustryList, insuranceCompanyList; deductibleIfany
var City, ZipCode, Longitude, Latitude, Country;
var IsUserLogin;

$(document).ready(function () {
    $('.main').hide();
    $('.firstQ').show();
    $('.hideline').hide();
    $('.firstbullet').addClass('bulletactive');


    // alert('js');
    //  alert('industryList ' + industryList);
    // alert('subIndustryList ' + subIndustryList);



    NumberofEmployees = '<li id="NumberofEmployees_li" class="main mainliremove">' +
            '<span class="line hideline"></span> <span class="bullet"></span>' +
            '<div style="width:100%;">' +
                '<div class="question-bar">' +
                    '<div class="qusetion">Number of Employees ?</div>' +
                    '<div class="selectedanswer"></div>' +
                    '<input type="hidden" id="NumberofEmployees" class="submitanswer" />' +
                '</div>' +
                '<div class="question-div">' +
                    '<div class="next-question-div">' +
                        '<div class="next-question-button answer extra" id="1-10">1-10</div>' +
                        '<div class="next-question-button answer extra" id="10-20">10-20</div>' +
                        '<div class="next-question-button answer extra" id="20-30">20-30</div>' +
                        '<div class="next-question-button answer extra" id="50+">50+</div>' +
                    '</div>' +
                '</div>' +
            '</div>' +
        '</li>';

    GrossPayroll = '<li id="GrossPayroll_li" class="main mainliremove">' +
           '<span id="gross" class="line hideline"></span> <span class="bullet"></span>' +
           '<div style="width:100%;">' +
               '<div class="question-bar">' +
                   '<div class="qusetion">Gross Payroll ?</div>' +
                   '<div class="selectedanswer"></div>' +
                   '<input type="hidden" id="GrossPayroll" class="submitanswer" />' +
               '</div>' +
               '<div class="question-div">' +
                   '<div class="next-question-div">' +
                       '<div class="next-question-button answer extra" id="1-5k">1-5k</div>' +
                       '<div class="next-question-button answer extra" id="5-20k">5-20k</div>' +
                       '<div class="next-question-button answer extra" id="20-50k">20-50k</div>' +
                       '<div class="next-question-button answer extra" id="50k+">50k+</div>' +
                   '</div>' +
               '</div>' +
           '</div>' +
       '</li>';



    howManyUnits = '<li id="howManyUnits_li" class="main mainliremove">' +
           '<span class="line hideline"></span> <span class="bullet"></span>' +
           '<div style="width:100%;">' +
               '<div class="question-bar">' +
                   '<div class="qusetion">How many units ?</div>' +
                   '<div class="selectedanswer"></div>' +
                   '<input type="hidden" id="submithowManyUnits" class="submitanswer" />' +
               '</div>' +
               '<div class="question-div">' +
                   '<div class="next-question-div">' +
                       '<div class="row">' +
                           '<div class="col-sm-12 col-md-12">' +
                               '<div class="col-sm-6 col-md-6">' +
                                   '<input type="text" maxlength="10" id="howManyUnits" class="contactinfo" placeholder="Enter units" />' +
                               '</div>' +
                           '</div>' +
                           '<div class="col-sm-12 col-md-12">' +
                               '<div class="next-question-button answer-small-button" id="btnhowManyUnits">Next >></div>' +
                                //'<img src="../Images/NewAssets/sumbit.PNG" id="SubmitWithLogin" style="cursor:pointer;" class="" alt="Submit" />' +
                           '</div>' +
                               '<div class="col-sm-12 col-md-12">' +
                                  '<div class="errormessage"></div>' +
                               '</div>' +
                           '</div>' +
                    '</div>' +
               '</div>' +
           '</div>' +
       '</li>';

    deductibleIfany = '<li id="deductibleIfany_li" class="main mainliremove">' +
           '<span id="deduc" class="line hideline"></span> <span class="bullet"></span>' +
           '<div style="width:100%;">' +
               '<div class="question-bar">' +
                   '<div class="qusetion">Deductible if any ?</div>' +
                   '<div class="selectedanswer"></div>' +
                   '<input type="hidden" id="deductibleIfany" class="submitanswer" />' +
               '</div>' +
               '<div class="question-div">' +
                   '<div class="next-question-div">' +
                       '<div class="next-question-button answer" id="Yes"> <div class="isinsured">Yes</div></div>' +
                       '<div class="next-question-button answer" id="No"> <div class="isinsured">No</div></div>' +
                   '</div>' +
               '</div>' +
           '</div>' +
       '</li>';

    currentLimit = '<li id="currentLimit_li" class="main mainliremove">' +
           '<span id="current" class="line hideline"></span> <span class="bullet"></span>' +
           '<div style="width:100%;">' +
               '<div class="question-bar">' +
                   '<div class="qusetion">Current Limit ?</div>' +
                   '<div class="selectedanswer"></div>' +
                   '<input type="hidden" id="submitcurrentLimit" class="submitanswer" />' +
               '</div>' +
               '<div class="question-div">' +
                   '<div class="next-question-div">' +
                       '<div class="row">' +
                           '<div class="col-sm-12 col-md-12">' +
                               '<div class="col-sm-6 col-md-6">' +
                                   '<input type="text" maxlength="10" id="currentLimit" class="contactinfo" placeholder="Enter current limit" />' +
                               '</div>' +
                           '</div>' +
                           '<div class="col-sm-12 col-md-12">' +
                               '<div class="next-question-button answer-small-button" id="btncurrentLimit">Next >></div>' +
                                //'<img src="../Images/NewAssets/sumbit.PNG" id="SubmitWithLogin" style="cursor:pointer;" class="" alt="Submit" />' +
                           '</div>' +
                               '<div class="col-sm-12 col-md-12">' +
                                  '<div class="errormessage"></div>' +
                               '</div>' +
                           '</div>' +
                    '</div>' +
               '</div>' +
           '</div>' +
       '</li>';

    notes = '<li id="notes_li" class="main mainliremove">' +
            '<span id="notes" class="line hideline"></span> <span class="bullet"></span>' +
            '<div style="width:100%;">' +
                '<div class="question-bar">' +
                    '<div class="qusetion">Notes</div>' +
                    '<div class="selectedanswer"></div>' +
                    '<input type="hidden" id="submitnotes" class="submitanswer" />' +
                '</div>' +
                '<div class="question-div">' +
                    '<div class="next-question-div">' +
                        '<div class="row">' +
                            '<div class="col-sm-12 col-md-12">' +
                                '<div class="col-sm-6 col-md-6">' +
                                    '<input type="text" id="notes1" class="contactinfo" placeholder="Enter any notes" />' +
                                '</div>' +
                            '</div>' +
                            '<div class="col-sm-12 col-md-12">' +
                                '<div class="next-question-button answer-small-button" id="btnnote">Next >></div>' +
                                 //'<img src="../Images/NewAssets/sumbit.PNG" id="SubmitWithLogin" style="cursor:pointer;" class="" alt="Submit" />' +
                            '</div>' +
                                '<div class="col-sm-12 col-md-12">' +
                                   //'<div class="errormessage"></div>' +
                                '</div>' +
                            '</div>' +
                     '</div>' +
                '</div>' +
            '</div>' +
        '</li>';

    language = '<li id="language_li" class="main mainliremove">' +
            '<span class="line hideline"></span> <span class="bullet"></span>' +
            '<div style="width:100%;">' +
                '<div class="question-bar">' +
                    '<div class="qusetion">Language</div>' +
                    '<div class="selectedanswer"></div>' +
                    '<input type="hidden" id="language" class="submitanswer" />' +
                '</div>' +
                '<div class="col-sm-6 col-md-3 col-xs-12 question-div">' +
                    '<div class="next-question-div">' +
                        '<select id="language-select" class="select-insurance select-change">' +
                            '<option selected="selected" value="">Please choose</option>' +
                            '<option value="English">English</option>' +
                            '<option value="Spanish">Spanish</option>' +
                            '<option value="Chinese(Cantonese)">Chinese(Cantonese)</option>' +
                            '<option value="Chinese(Mandarin)">Chinese(Mandarin)</option>' +
                            '<option value="Korean">Korean</option>' +
                            '<option value="Vietnamese">Vietnamese</option>' +
                        '</select>' +
                        '</div>' +
                '</div>' +
            '</div>' +
        '</li>';

    valueOfHome = '<li id="valueOfHome_li" class="main mainliremove">' +
            '<span class="line hideline"></span> <span class="bullet"></span>' +
            '<div style="width:100%;">' +
                '<div class="question-bar">' +
                    '<div class="qusetion">Value of your home ?</div>' +
                    '<div class="selectedanswer"></div>' +
                    '<input type="hidden" id="valueOfHome" class="submitanswer" />' +
                '</div>' +
                '<div class="question-div">' +
                    '<div class="next-question-div">' +
                        '<div class="next-question-button answer" id="$500,000 or less">$500,000 or less</div>' +
                        '<div class="next-question-button answer" id="$501,000 - $1,000,000">$501,000 - $1,000,000</div>' +
                        '<div class="next-question-button answer" id="More than $1,000,000">More than $1,000,000</div>' +
                        '</div>' +
                '</div>' +
            '</div>' +
        '</li>';

    typeOfAuto = '<li id="typeOfAuto_li" class="main mainliremove">' +
            '<span class="line hideline"></span> <span class="bullet"></span>' +
            '<div style="width:100%;">' +
                '<div class="question-bar">' +
                    '<div class="qusetion">Type of auto ?</div>' +
                    '<div class="selectedanswer"></div>' +
                    '<input type="hidden" id="typeOfAuto" class="submitanswer" />' +
                '</div>' +
                '<div class="question-div">' +
                    '<div class="next-question-div">' +
                        '<div class="next-question-button answer extra" id="Economy">Economy</div>' +
                        '<div class="next-question-button answer extra" id="Standard">Standard</div>' +
                        '<div class="next-question-button answer extra" id="Luxury">Luxury</div>' +
                        '<div class="next-question-button answer extra" id="Collectible">Collectible</div>' +
                    '</div>' +
                '</div>' +
            '</div>' +
        '</li>';

    acquisitionIsInsured = '<li id="acquisitionIsInsured_li" class="main mainliremove">' +
            '<span class="line hideline"></span> <span class="bullet"></span>' +
            '<div style="width:100%;">' +
                '<div class="question-bar">' +
                    '<div class="qusetion">Acquisition or currently insured ?</div>' +
                    '<div class="selectedanswer"></div>' +
                    '<input type="hidden" id="acquisitionIsInsured" class="submitanswer" />' +
                '</div>' +
                '<div class="question-div">' +
                    '<div class="next-question-div">' +
                        '<div class="next-question-button answer" id="Acquisition">Acquisition</div>' +
                        '<div class="next-question-button answer" id="Currently insured">Currently Insured</div>' +
                        '</div>' +
                '</div>' +
            '</div>' +
        '</li>';


    grossrevenue = '<li id="grossrevenue_li" class="main mainliremove">' +
            '<span class="line hideline"></span> <span class="bullet"></span>' +
            '<div style="width:100%;">' +
                '<div class="question-bar">' +
                    '<div class="qusetion">Gross Sales ?</div>' +
                    '<div class="selectedanswer"></div>' +
                    '<input type="hidden" id="grossrevenue" class="submitanswer" />' +
                '</div>' +
                '<div class="question-div">' +
                    '<div class="next-question-div">' +
                        '<div class="next-question-button answer extra" id="Up to $1,000,000">Up to $1,000,000</div>' +
                        '<div class="next-question-button answer extra" id="$1,000,001 and $5,000,000">$1,000,001 and $5,000,000</div>' +
                        '<div class="next-question-button answer extra" id="$5,000,001 and $10,000,000">$5,000,001 and $10,000,000</div>' +
                        '<div class="next-question-button answer extra" id="Over $10,000,001">Over $10,000,001</div>' +
                    '</div>' +
                '</div>' +
            '</div>' +
        '</li>';

    siccode = '<li id="siccode_li" class="main mainliremove">' +
            '<span class="line hideline"></span> <span class="bullet"></span>' +
            '<div style="width:100%;">' +
                '<div class="question-bar">' +
                    '<div class="qusetion">SIC code</div>' +
                    '<div class="selectedanswer"></div>' +
                    '<input type="hidden" id="siccode" class="submitanswer" />' +
                '</div>' +
                '<div class="question-div">' +
                    '<div class="next-question-div">' +
                    '<div class="col-sm-12 col-md-12">' +
                            '<div id="sic-select"></div>' +
                            '</div>' +

                            '<div class="col-sm-12 col-md-12">' +
                                '<div class="next-question-button answer-small-button" id="btnsiccode">Next >></div>' +
                                '<div id="siccodemessage" class="messageClass" ></div>' +
                            '</div>' +

                    '</div>' +
                '</div>' +
            '</div>' +
        '</li>';


    coverageamount = '<li id="coverageamount_li" class="main mainliremove">' +
            '<span class="line hideline"></span> <span class="bullet"></span>' +
            '<div style="width:100%;">' +
                '<div class="question-bar">' +
                    '<div class="qusetion">Coverage amount ?</div>' +
                    '<div class="selectedanswer"></div>' +
                    '<input type="hidden" id="coverageamount" class="submitanswer" />' +
                '</div>' +
                '<div class="question-div">' +
                    '<div class="next-question-div">' +
                        '<div class="next-question-button answer extra" id="$250,000 or less">$250,000 or less</div>' +
                        '<div class="next-question-button answer extra" id="$250,001 to $500,000">$250,001 to $500,000</div>' +
                        '<div class="next-question-button answer extra" id="$500,001 to $1,000,000">$500,001 to $1,000,000</div>' +
                        '<div class="next-question-button answer extra" id="More than $1,000,000">More than $1,000,000</div>' +
                    '</div>' +
                '</div>' +
            '</div>' +
        '</li>';

    newPurchaseOrCurrentlyInsured = '<li id="newPurchaseOrCurrentlyInsured_li" class="main mainliremove">' +
            '<span class="line hideline"></span> <span class="bullet"></span>' +
            '<div style="width:100%;">' +
                '<div class="question-bar">' +
                    '<div class="qusetion">Are you currently insured ?</div>' +
                    '<div class="selectedanswer"></div>' +
                    '<input type="hidden" id="newPurchaseOrCurrentlyInsured" class="submitanswer" />' +
                '</div>' +
                '<div class="question-div">' +
                    '<div class="next-question-div">' +
                        '<div class="next-question-button answer" id="Currently insured"> <div class="isinsured">Yes</div><div class="fontsize20">Currently Insured</div>  </div>' +
                        '<div class="next-question-button answer" id="New Purchase"> <div class="isinsured">No</div><div class="fontsize20">New Purchase</div>    </div>' +
                    '</div>' +
                '</div>' +
            '</div>' +
        '</li>';


    //whenNeedInsurence = '<li id="whenNeedInsurence_li" class="main mainliremove">' +
    //       '<span class="line hideline"></span> <span class="bullet"></span>' +
    //        '<div style="width:100%;">' +
    //'<div class="question-bar">' +
    //'<div class="qusetion">When do you need insurance ?</div>' +
    //'<div class="selectedanswer"></div>' +
    //'<input type="hidden" id="whenNeedInsurence" class="submitanswer" />' +
    //'</div>' +
    //'<div class="question-div">' +
    //'<div class="next-question-div">' +
    //'<div class="next-question-button answer" id="Need Insurance Now">Now</div>' +
    //'<div class="next-question-button answer" id="Coverage expires in 30 days or more">Coverage expires in 30 days or more</div>' +
    //'</div>' +
    //'</div>' +
    //'</div>' +
    //'</li>';

    noOfEmployee = '<li id="noOfEmployee_li" class="main mainliremove">' +
            '<span class="line hideline"></span> <span class="bullet"></span>' +
            '<div style="width:100%;">' +
                '<div class="question-bar">' +
                    '<div class="qusetion">Number of employees</div>' +
                    '<div class="selectedanswer"></div>' +
                    '<input type="hidden" id="noOfEmployee" class="submitanswer" />' +
                '</div>' +
                '<div class="question-div">' +
                    '<div class="next-question-div">' +
                        '<div class="next-question-button answer extra" id="1-25">1-25</div>' +
                        '<div class="next-question-button answer extra" id="26-50">26-50</div>' +
                        '<div class="next-question-button answer extra" id="51-100">51-100</div>' +
                        '<div class="next-question-button answer extra" id="More than 100">100+</div>' +
                    '</div>' +
                '</div>' +
            '</div>' +
        '</li>';





    industry = '<li id="industry_li" class="main mainliremove">' +
            '<span class="line hideline"></span> <span class="bullet"></span>' +
            '<div style="width:100%;">' +
                '<div class="question-bar">' +
                    '<div class="qusetion">What is your industry ?</div>' +
                    '<div class="selectedanswer"></div>' +
                    '<input type="hidden" id="industry" class="submitanswer" />' +
                '</div>' +
                '<div class="question-div">' +
                    '<div class="next-question-div">' +
                    '<div class="col-sm-6 col-md-3 col-xs-12">' +
                        '<select  id="industry-select" class="select-insurance">' +
                            '<option selected="selected" value="">Please choose</option>' +

                        '</select>' +
                        '</div>' +
                            '<div class="col-sm-12 col-md-12">' +
                                '<div class="next-question-button answer-small-button" id="btnindustry">Next >></div>' +
                            '</div>';
    '</div>' +
'</div>' +
'</div>' +
'</li>';


    //        insuranceCompany = '<li id="insuranceCompany_li" class="main mainliremove">' +
    //                '<span class="line hideline"></span> <span class="bullet"></span>' +
    //                '<div style="width:100%;">' +
    //                    '<div class="question-bar">' +
    //                        '<div class="qusetion">Company name ?</div>' +
    //                        '<div class="selectedanswer"></div>' +
    //                        '<input type="hidden" id="insuranceCompany" class="submitanswer" />' +
    //                    '</div>' +
    //                    '<div class="question-div">' +
    //                        '<div class="next-question-div">' +
    //                        '<div class="col-sm-12 col-md-12">' +
    //                            '<select width="300"  id="insurancecompany-select" class="select-insurance">' +
    //                                '<option selected="selected" value="">Please choose</option>' +

    //                            '</select>' +
    //                            '</div>' +
    //                                '<div class="col-sm-12 col-md-12">' +
    //                                    '<div class="next-question-button answer-small-button" id="btninsuranceCompany">Next >></div>' +
    //                                '</div>';
    //        '</div>' +
    //    '</div>' +
    //'</div>' +
    //'</li>';


    contactInformation = '<li id="contactInformation_li" class="main mainliremove">' +
'<span class="line hideline"></span> <span class="bullet"></span>' +
'<div style="width:100%;">' +
'<div class="question-bar">' +
    '<div class="qusetion">Contact information</div>' +
    '<div class="selectedanswer"></div>' +
    '<input type="hidden" id="contactInformation" class="submitanswer" />' +
'</div>' +
'<div class="question-div">' +
    '<div class="next-question-div">' +
            '<div class="row">' +
            '<div class="col-sm-12 col-md-12">' +
                '<div class="col-sm-6 col-md-6">' +
                    '<input type="text" id="firstName" class="contactinfo" placeholder="First and Last Name" />' +
                '</div>' +
                //'<div class="col-sm-6 col-md-6">' +
                //    '<input type="text" id="lastName" class="contactinfo" placeholder="Last Name" />' +
                //'</div>' +
                 '<div class="col-sm-6 col-md-6">' +
                    '<input type="text" id="email" class="contactinfo" placeholder="Email" />' +
                '</div>' +
            '</div>' +
            '<div class="col-sm-12 col-md-12">' +
                '<div class="col-sm-6 col-md-6">' +
                    '<input type="text" id="phone" class="contactinfo" placeholder="Phone" />' +
                '</div>' +

             '</div>' +
            //'<div class="col-sm-12 col-md-12">' +
            //    '<div class="col-sm-6 col-md-6">' +
            //        '<input type="password" id="password" class="contactinfo" placeholder="Password" />' +
            //    '</div>' +
            //    '<div class="col-sm-6 col-md-6">' +
            //        '<input type="password" id="conpassword" class="contactinfo" placeholder="Confirm Password" />' +
            //    '</div>' +
            //'</div>' +
            '<div class="col-sm-12 col-md-12">' +
             //'<img src="../Images/NewAssets/sumbit.PNG" id="SubmitWithLogin" style="cursor:pointer;" class="SubmitInsurance" alt="Submit" />' +
               '<img src="../Images/NewAssets/sumbit.PNG" class="SubmitInsurance" style="cursor:pointer;" alt="Submit" />' +
            '</div>' +
             '<div class="col-sm-12 col-md-12">' +
               '<div class="errormessage" style="display:none;"></div>' +
            '</div>' +
        '</div>' +
    '</div>' +
'</div>' +
'</div>' +
'</li>';


    SubmitWithLogin = '<li id="contactInformation_li" class="main mainliremove">' +

                                     '<div style="width:100%;">' +
                                       '<div class="question-div">' +
                                            '<div >' +
                                                    '<div class="row">' +
                                                    '<div class="col-sm-12 col-md-12">' +
                                                       '<img src="../Images/NewAssets/sumbit.PNG" id="SubmitWithLogin" style="cursor:pointer;" class="SubmitInsuranceWithLogin" alt="Submit" />' +
                                                    '</div>' +
                                                     '<div class="col-sm-12 col-md-12">' +
                                                       '<div class="errormessage" style="display:none;"></div>' +
                                                    '</div>' +
                                                '</div>' +
                                            '</div>' +
                                        '</div>' +
                                    '</div>' +
                                '</li>';

});




$(function () {
    // after click in any question answer

    // $(".next-question-button").on('click', function () {
    $(document.body).on('click', '.next-question-button', function () {
        // alert('called');

        $('.ui-igcombo-field').prop('readonly', true);
        mainfunction(this, 'click');


    });


    //  after click on sropdownlist
    $(document).on("change", ".select-change", function () {
        var insustry = this.options[this.selectedIndex].value;
        if (insustry != '') {
            mainfunction(this, 'select');
        }
    });



    $(document.body).on('click', '.selectedanswer', function () {
        //$(document.body).on('click', '.qusetion', function () { //27Sep17
        //clicked answer
        $parent = $(this).parents('.main');
        $(this).parents('.main').find('.question-div').show(); // Show current questions options
        $(this).parents('.main').find('.qusetion').show();//show current question   //3Oct17
        $(this).parents('.main').find('.selectedanswer').hide();//hide current answer  //3Oct17
        $(this).parents('.main').find('.selectedanswer').text('');//clear previous answer
        $('.bullet').removeClass('bulletactive');//clear all active bullet
        $(this).parents('.main').find('.bullet').addClass('bulletactive');// apply active bullet to clicked qustion
        $parent.children(".line").hide();//hide current line

        $(this).parents('.main').find('.submitanswer').val('');
        //$('.SubmitWithLogin1').hide();
        //  $(this).parents('.main').find('.qusetion').removeClass('selectedquestion');//27Sep17  //3Oct17


        var isinsurancechange = $parent.closest(".main").prop("id");
        if (isinsurancechange == 'typeofinsurance') {
            $('.mainliremove').remove();
        }
        else {
            // next all question after clicked and random answer to change
            $parent.nextAll('.main').find('.selectedanswer').hide(); //hide all next selectd answer
            $parent.nextAll('.main').find('.qusetion').show();// show all next question
            $parent.nextAll('.main').find('.question-div').show();//show all next answer options

            $parent.nextAll('.main').find('.line').hide();//hide all next lines
            $parent.nextAll('.main').hide();//hide all main question 

            //  $parent.nextAll('.main').find('.qusetion').removeClass("selectedquestion");//27Sep17  //3Oct17
            // $parent.nextAll('.main').find('.typeofinsurance').removeClass("selectedquestion");//27Sep17  //3Oct17

        }

        //27Sep17
        if (isinsurancechange != "contactInformation_li" || isinsurancechange != "notes_li") {
            $('#language-select').val('');
        }

    });



});

function mainfunction(selectedId, mode) {

    var selectedindustrytext = '', selectedindustryval;
    $parent = $(selectedId).parents('.main');
    $child = $(selectedId).parents('.question-div');




    var SelectedAnswer = '';
    if (mode == 'click') {
        //take value if submited by textbox

        if (selectedId.id == 'btnnote') {

            if ($('#notes1').val().trim() != '') {
                SelectedAnswer = $('#notes1').val();
                //bind answer for final submit
                
                $(selectedId).parents('.main').find('.submitanswer').val(SelectedAnswer);
            }
            else {
                SelectedAnswer = 'No notes added';
            }

        }    /*16-feb-2018 R.Shah*/
        else if (selectedId.id == 'btnhowManyUnits') {

            if ($('#howManyUnits').val().trim() != '') {
                SelectedAnswer = $('#howManyUnits').val();
                //bind answer for final submit
                $(selectedId).parents('.main').find('.submitanswer').val(SelectedAnswer);
            }
            else {
                $(".errormessage").text("Please enter units");
                $('.errormessage').fadeIn('fast').delay(2000).fadeOut('fast');
                return false;
            }
        }

        else if (selectedId.id == 'btnZipCodeSection') {
            //alert("Called");
            if ($('#ZipCodeSectionValue').val().trim() != '') {
                if ($('#ZipCodeSectionValue').val().trim().length == 5) {

                    //alert("Length - " + $('#ZipCodeSectionValue').val().trim().length);

                    SelectedAnswer = $('#ZipCodeSectionValue').val();
                    //bind answer for final submit
                    $('#longitude').val('');
                    $('#latitude').val('');
                    $('#zipCode').val(SelectedAnswer);

                    //alert("longitude - " + $('#longitude').val());

                    $(selectedId).parents('.main').find('.submitanswer').val(SelectedAnswer);

                    /********************* Get Longitude , Latitude , City for zip code**************************/
                    var IntrZipCode1 = setInterval(
                  function () {
                      var longitude = $('#longitude').val();
                      var latitude = $('#latitude').val();


                      if (longitude == '' && latitude == '') {
                          $.ajax({
                              url: 'https://maps.googleapis.com/maps/api/geocode/json?address=' + SelectedAnswer + '',
                              success: function (data) {
                                  lat = data.results[0].geometry.location.lat;
                                  lng = data.results[0].geometry.location.lng;

                                  $('#longitude').val(lng);
                                  $('#latitude').val(lat);

                                  for (var i = 0; i < data.results[0].address_components.length; i++) {

                                      for (var b = 0; b < data.results[0].address_components[i].types.length; b++) {
                                          if (data.results[0].address_components[i].types[b] == "postal_code") {
                                              var zip = data.results[0].address_components[i].long_name;

                                          }
                                          if (data.results[0].address_components[i].types[b] == "locality") {
                                              localCity = data.results[0].address_components[i].long_name;
                                              $('#city').val(localCity);
                                          }
                                          if (data.results[0].address_components[i].types[b] == "country") {
                                              Country = data.results[0].address_components[i].long_name;
                                              //alert("Country - " + Country);
                                              $('#Country').val(Country);
                                          }
                                      }
                                  }
                              }

                          });
                      }
                      else {
                          //alert("Zip - "+SelectedAnswer +" , City - "+$('#city').val() +" , Country - "+$('#Country').val()+ "Long - "+$('#longitude').val() + " , Lat - "+$('#latitude').val());

                          var c1 = $('#city').val();
                          var c2 = $('#Country').val();
                          var addr = '';

                          if (c1 != "" && c1 != "undefined" && c2 != "" && c2 != "undefined")
                          {
                              addr = c1 + "," + c2;
                              //alert("addr - " + addr + " selectedId" + selectedId);
                            
                              $(selectedId).parents('.main').find('.selectedanswer').text(addr);
                            
                          }

                          clearInterval(IntrZipCode1);
                      }
                  }, 2000);

                    /********************************************************************************************/
                }
                else {
                    $(".errormessage").text("Please enter valid zip code");
                    $('.errormessage').fadeIn('fast').delay(2000).fadeOut('fast');
                    return false;
                }
            }
            else {
                $(".errormessage").text("Please enter zip code");
                $('.errormessage').fadeIn('fast').delay(2000).fadeOut('fast');
                return false;
            }
        }

        else if (selectedId.id == 'btncurrentLimit') {

            if ($('#currentLimit').val().trim() != '') {
                SelectedAnswer = $('#currentLimit').val();


                //alert(SelectedAnswer + " " + " SelectedAnswer 1");
                //bind answer for final submit
                $(selectedId).parents('.main').find('.submitanswer').val(SelectedAnswer);

            }
            else {
                $(".errormessage").text("Please enter current limit");
                $('.errormessage').fadeIn('fast').delay(2000).fadeOut('fast');
                return false;
            }

        }
        else if (selectedId.id == 'btnsiccode') {
            var siccodes = $('.ui-igcombo-hidden-field').val();//value of dropdown

            var siccodestext = $('.ui-igcombo-field').val();// text of dropdown
            var seperatesic = siccodes.split(',');
            var selectedSicCode = [];

            if (seperatesic.length <= 5) {
                for (i = 0; i < seperatesic.length; i++) {
                    // alert(seperatesic[i]);
                    selectedSicCode.push(seperatesic[i]);

                }
                SelectedAnswer += siccodestext;

                if (selectedSicCode.length >= 1) {
                    if (selectedSicCode.length == 1) {
                        if (selectedSicCode[0] == '') {
                            $("#siccodemessage").text("Please select SIC code");
                            $('#siccodemessage').fadeIn('fast').delay(2000).fadeOut('fast');
                            return false;
                        }
                    }
                }
                else {
                    $("#siccodemessage").text("Please select SIC code");
                    $('#siccodemessage').fadeIn('fast').delay(2000).fadeOut('fast');
                    return false;
                }

                //bind answer for final submit
                $(selectedId).parents('.main').find('.submitanswer').val(siccodes);
            }
            else {

                $("#siccodemessage").text("Please select SIC code upto 5");
                $('#siccodemessage').fadeIn('fast').delay(2000).fadeOut('fast');
                return false
            }
            // alert(selectedSicCode.length);

        }
        else if (selectedId.id == 'btnindustry') {

            selectedindustry = $("#industry-select option:selected").text();
            selectedindustryval = $("#industry-select option:selected").val();
            //empty sic code

            fillSicCode(selectedindustryval);


            //$('.ui-igcombo-field').val('');
            if (selectedindustryval != '') {
                SelectedAnswer = selectedindustry;
            }
            else {
                SelectedAnswer = 'Industry not selected.';
            }

            //bind answer for final submit
            $(selectedId).parents('.main').find('.submitanswer').val(selectedindustryval);

            // alert($("#industry-select option:selected").val());

        }
        else if (selectedId.id == 'btninsuranceCompany') {

            var selectedcompanyname = $("#insurancecompany-select option:selected").text();
            var selectedcompanyid = $("#insurancecompany-select option:selected").val();
            if (selectedcompanyid == '') {
                return false;
            }
            else {
                SelectedAnswer = selectedcompanyname;
                $(selectedId).parents('.main').find('.submitanswer').val(selectedcompanyname);
            }
        }
        else { //take value if submited by fixed options
            //  alert('else');

            SelectedAnswer = selectedId.id;
            //  alert('else '+SelectedAnswer);
            //bind answer for final submit
            $(selectedId).parents('.main').find('.submitanswer').val(SelectedAnswer);

        }



    }
    else if (mode == 'select') {//take value if submited by dropdownlist
        SelectedAnswer = selectedId.options[selectedId.selectedIndex].text;
        //bind answer for final submit
        //alert("SelectedAnswer - " + SelectedAnswer);
        $(selectedId).parents('.main').find('.submitanswer').val(SelectedAnswer);
    }


    /////copy

    var appendcontatcus = '';
    if (IsUserLogin != 'Authorize') {
        appendcontatcus = contactInformation
    }
    else {
        appendcontatcus = SubmitWithLogin;
    }

    //fill  Home Insurance question on selection of insurance type
    if (selectedId.id == 'Home Insurance') {
        $('#InsuranceOtherQuestion').append(newPurchaseOrCurrentlyInsured + valueOfHome + language + notes + appendcontatcus);
        $parent.nextAll('.main').hide();//hide all main question 
        fillInsuranceCompanyList();
        IsLogin();


    }
    //fill  Auto Insurance question on selection of insurance type
    if (selectedId.id == 'Commercial Auto Insurance') {
        //alert("Called");
        $('#InsuranceOtherQuestion').append(howManyUnits + deductibleIfany + currentLimit + appendcontatcus);
        $parent.nextAll('.main').hide();//hide all main question 
        fillInsuranceCompanyList();
        IsLogin();
    }

    //fill Life Insurance question on selection of insurance type
    if (selectedId.id == 'Workers Compensation') {
        $('#InsuranceOtherQuestion').append(NumberofEmployees + GrossPayroll + appendcontatcus);
        $parent.nextAll('.main').hide();//hide all main question 
        IsLogin();
    }

    //fill Business Insurance question on selection of insurance type
    if (selectedId.id == 'Liability Insurance') {
        $('#InsuranceOtherQuestion').append(industry + siccode + grossrevenue + deductibleIfany + appendcontatcus);
        $parent.nextAll('.main').hide();//hide all main question 
        fillIndustry();
        $('#deduc').removeClass('line');
        IsLogin();

        $('.ui-igcombo-field').prop('readonly', true);//set readonly to sic code drodown
        $('.ui-igcombo-field').prop("placeholder", "Please choose");
    }

    //fill Employee Benefits question on selection of insurance type
    if (selectedId.id == 'Meineke Benefits Insurance') {
        $('#InsuranceOtherQuestion').append(newPurchaseOrCurrentlyInsured + noOfEmployee + industry + siccode + language + notes + appendcontatcus);
        $parent.nextAll('.main').hide();//hide all main question

        fillIndustry();
        fillInsuranceCompanyList();
        IsLogin();

        //set readonly to sic code drodown
        $('.ui-igcombo-field').prop('readonly', true);
        $('.ui-igcombo-field').prop("placeholder", "Please choose");
    }


    $child.hide();
    $parent.children(".line").show();
    $('.bullet').removeClass('bulletactive');


    $(selectedId).parents('.main').find('.qusetion').hide(); //27Sep17  //3Oct17
    $(selectedId).parents('.main').find('.selectedanswer').show(); //27Sep17  //3Oct17

    //$(selectedId).parents('.main').find('.qusetion').addClass("selectedquestion");//27Sep17   //3Oct17
    //$(selectedId).parents('.main').find('.typeofinsurance').addClass("selectedquestion");//27Sep17  //3Oct17



    $(selectedId).parents('.main').find('.selectedanswer').text(SelectedAnswer);


    if (selectedId.id == 'btnindustry') {
        if (selectedindustryval == '') {

            $parent.next(".main").next(".main").children('.bullet').addClass('bulletactive');
            $parent.next('.main').next(".main").show();
            $parent.next('.main').next(".main").next('.question-div').show();
        }
        else {
            $parent.next(".main").children('.bullet').addClass('bulletactive');
            $parent.next('.main').show();
            $parent.next('.main').next('.question-div').show();
        }

    }
        //else if (selectedId.id == 'New Purchase')
        //{
        //       $parent.next(".main").next(".main").children('.bullet').addClass('bulletactive');
        //        $parent.next('.main').next(".main").show();
        //        $parent.next('.main').next(".main").next('.question-div').show();

        //}
    else {
        $parent.next(".main").children('.bullet').addClass('bulletactive');
        $parent.next('.main').show();
        $parent.next('.main').next('.question-div').show();
    }
}

function IsLogin() {
    //alert("In IsLogin - " + IsUserLogin);
    if (IsUserLogin == 'Authorize') {

        //$('#SubmitWithLogin').show();
        //$('#btncurrentLimit').hide();
        //$('#btnnote').hide();
        $('#notes').removeClass('line');
        $('#current').removeClass('line');
        $('#gross').removeClass('line');
    }
    else {
        //$('#SubmitWithLogin').hide();
        //$('#btncurrentLimit').show();
        //$('#btnnote').show();
        $('#deduc').addClass('line');
        $('#gross').addClass('line');
        $('#notes').addClass('line');
    }

}

function fillIndustry() {

    for (var i in industryList.result) {

        $('#industry-select').append(
         $("<option></option>")
              .attr("value", industryList.result[i].Value)
             .text(industryList.result[i].Text)
       );
    }
}

function fillInsuranceCompanyList() {

    var obj = JSON.parse(insuranceCompanyList);
    var cmpList = obj.CompanyList;
    $.each(cmpList, function (i, obj) {
        $('#insurancecompany-select').append(
        $("<option></option>")
             .attr("value", obj.CompanyId)
            .text(obj.CompanyName)
      );
    });
}

function fillSicCode(industryid) {
    //alert('industryid ' + industryid);
    var obj = JSON.parse(subIndustryList);


    var SubIndustryMaster = obj.Table;
    var subindustry = [];

    $.each(SubIndustryMaster, function (i, IndSubCat) {


        if (industryid == IndSubCat.IndustryId) {
            subindustry.push({
                "Name": IndSubCat.SICCode + '_' + IndSubCat.SubIndustryName,
                "value": IndSubCat.SubIndustryId
            });
        }
    });



    $("#sic-select").igCombo({
        width: 300,
        dataSource: subindustry,
        textKey: "Name",
        valueKey: "value",
        multiSelection: {
            enabled: true,
            showCheckboxes: true
        },
        dropDownOrientation: "bottom"
    });
}


function checkRequiredField(id) {
    //   alert('called '+id.val());

    if (id.val().trim() != "") {
        //  alert('checkRequiredField if');
        id.removeClass('validationerror');
        return true;
    }
    else {
        // alert('checkRequiredField else');
        id.addClass('validationerror');
        return false;

    }
}

function globalvalidateName(id) {

    var name = id.val();
    if (name != '') {
        var split = name.split(' ');

        if (split.length >= 2) {
            if (split[0] != '' && split[1] != '') {
                id.removeClass('validationerror');
                return true;
            }
            else {
                $(".errormessage").append('Please enter First and Last Name.</br>');
                id.removeClass('validationerror');
                return false;
            }

        }
        else {
            $(".errormessage").append('Please enter First and Last Name.</br>');
            id.removeClass('validationerror');
            return false;
        }
    }
    else {
        id.addClass('validationerror');
        return false;

    }

}

function globalvalidateEmail(id) {
    var pattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;

    //if (id.val().trim() == "") {
    // //   alert('globalvalidateEmail if');
    //    id.addClass("validationerror");
    //    return false;
    //}
    //else {

    var EmailId = id.val();
    if (EmailId != '') {
        EmailId = jQuery.trim(EmailId);

        if ((pattern.test(EmailId))) {
            // alert('valid');
            //  alert('globalvalidateEmail else if');
            // id.removeClass("validationerror");
            return true;
        }
        else {
            //alert('globalvalidateEmail else else');
            //alert('invalid');
            // id.addClass("validationerror");

            $(".errormessage").append('Please enter valid email id.</br>');
            return false;
        }
    }
    //}
}

function globalvalidatePassword(id) {
    var pattern = /^[^-\s]{8,15}$/;
    var conpassword = $('#conpassword');
    //if (id.val().trim() == "") {
    //    // alert('if');
    //    //alert('globalvalidatePassword if');
    //    id.addClass("validationerror");
    //    return false;
    //}
    //else {
    // alert('else');

    if (id.val() != '') {
        if ((pattern.test(id.val()))) {
            //alert('valid');
            //alert('globalvalidatePassword else if');
            //id.removeClass("validationerror");
            return true;
        }
            //if it's NOT valid
        else {
            //alert('invalid');
            //alert('globalvalidatePassword else else');
            //id.addClass("validationerror");


            $(".errormessage").append('Please enter password 8 to 15 characters.</br>');

            return false;
        }
    }
    //}
}

function globalvalidateConPassword(id) {
    var pattern = /^[^-\s]{8,15}$/;
    var password = $('#password');

    //if (id.val().trim() == "") {
    //    //  alert('c if');
    //    //alert('globalvalidateConPassword if');
    //    id.addClass("validationerror");
    //    return false;
    //}
    //else {
    // alert('c else');
    if (id.val() != '') {
        if ((pattern.test(id.val()))) {

            if (password.val().trim() == id.val().trim()) {
                // alert('globalvalidateConPassword else if');
                //id.removeClass("validationerror");
                return true;
            }
            else {
                // alert('globalvalidateConPassword else else');
                // id.addClass("validationerror");

                if (password.val() != '' && id.val() != '') {
                    $(".errormessage").append('Password and confirm password must be same.</br>');
                }
                return false;
            }
        }
        else {
            //  alert('globalvalidateConPassword else else 1');
            // id.addClass("validationerror");

            $(".errormessage").append('Please enter confirm password 8 to 15 characters.</br>');

            if (password.val() != '' && id.val() != '') {
                $(".errormessage").append('Password and confirm password must be same.</br>');
            }
            return false;
        }
    }
    //}
}
