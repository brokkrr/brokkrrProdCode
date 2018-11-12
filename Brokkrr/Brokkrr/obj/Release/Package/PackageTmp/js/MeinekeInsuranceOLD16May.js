
var newPurchaseOrCurrentlyInsured, whenNeedInsurence, noOfEmployee, industry, contactInformation, coverageamount, siccode, grossrevenue, acquisitionIsInsured,
    typeOfAuto, valueOfHome, language, notes, insuranceCompany, howManyUnits, deductibleIfany, currentLimit, NumberofEmployees, GrossPayroll, SubmitWithLogin, NoofLocation, grossrevenueforgarage, howManyStalls, CurrentPlan, typeofinsurance, planSize, declaration, uploaddeclaration;
//grossSales, grossPayroll;
var colors;
var industryList, subIndustryList, insuranceCompanyList; deductibleIfany
var City, ZipCode, Longitude, Latitude, Country;
var IsUserLogin;
//13April18
var languageJson;

$(document).ready(function () {
    

    $('.main').hide();
    $('.firstQ').show();
    $('.hideline').hide();
    $('.firstbullet').addClass('bulletactive');

    //13April18
    languageJson = '{ "Language" : [' +
            '{ "Key":"English" , "Value":"English" },' +
             '{ "Key":"Spanish" , "Value":"Spanish" },' +
             '{ "Key":"Chinese(Cantonese)" , "Value":"Chinese(Cantonese)" },' +
             '{ "Key":"Chinese(Mandarin)" , "Value":"Chinese(Mandarin)" },' +
             '{ "Key":"Korean" , "Value":"Korean" },' +
             '{ "Key":"Vietnamese" , "Value":"Vietnamese" }]}';

    //11May18
    declaration = '<li id="declaration_li" class="main mainliremove">' +
          '<span id="spandeclaration" class="line hideline"></span> <span class="bullet"></span>' +
          '<div style="width:100%;">' +
              '<div class="question-bar">' +
                  '<div class="qusetion">Do you have a declaration for earlier insurance?</div>' +
                  '<div class="selectedanswer"></div>' +
                  '<input type="hidden" id="Declaration" class="submitanswer" />' +
              '</div>' +
              '<div class="question-div">' +
                  '<div class="next-question-div">' +
                      '<div class="next-question-button answer declarationYes" id="declarationYes"> <div class="isinsured">Yes</div></div>' +
                      '<div class="next-question-button answer declarationNo" id="declarationNo"> <div class="isinsured">No</div></div>' +
                  '</div>' +
              '</div>' +
          '</div>' +
      '</li>';

    //11May18
    uploaddeclaration = '<li id="uploaddeclaration_li" class="main mainliremove classuploaddeclaration">' +
       '<span id="spanuploaddeclaration" class="line hideline"></span> <span class="bullet"></span>' +
       '<div style="width:100%;">' +
           '<div class="question-bar">' +
               '<div class="qusetion">Upload declaration</div>' +
               '<div class="selectedanswer"></div>' +
               '<input type="hidden" id="UploadDeclaration" class="submitanswer" />' +
           '</div>' +
           '<div class="question-div">' +
               '<div class="next-question-div">' +
                   '<div class="row">' +
                       '<div class="col-sm-12 col-md-12">' +
                           '<div class="col-sm-6 col-md-6"> <div class="col-sm-10 col-md-10">' +
                               '<input type="text" readonly  id="txtUploadDeclaration" class="contactinfo" placeholder="Upload declaration" /><input type="text" readonly  id="txtHiddenUploadDeclaration" style="display:none;"/></div>' +
                                '<div class="col-sm-2 col-md-2"><input type="file" id="fileToUpload" name="file" style="display:none" accept=".jpe,.jpg,.jpeg,.png,.pdf,.doc,.docx" />' +
            '<input type="button" value="BROWSE" id="btnUpload" onclick="openfileDialog();" class="btn btn-default"  style="background-color: rgb(72,205,248);color: white;border-radius:21px;font-size: 1.1vw !important;margin-left: -230%;" /></div>' +
                           '</div>' +
                       '</div>' +
                       '<div class="col-sm-12 col-md-12">' +
                           '<div class="next-question-button answer-small-button" id="btnuploaddeclaration">Next >></div>' +
                       '</div>' +
                           '<div class="col-sm-12 col-md-12">' +
                              '<div class="errormessage"></div>' +
                           '</div>' +
                       '</div>' +
                '</div>' +
           '</div>' +
       '</div>' +
   '</li>';

    typeofinsurance = '<li id="typeofinsurance" class="main liinsurancetyperemove" style="line-height:1.2em;">' +
                   '<span class="line hideline hideline1"></span>   <span class="bullet secondbullet"></span>' +
                   '<div style="width:100%;">' +
                       '<div class="question-bar" style="line-height: 1.5em;">' +
                           '<div class="qusetion qusetion1">What type of insurance are you looking for today? <span class="typeofinsurance">(Select insurance type)</span></div>' +
                           '<div class="selectedanswer selectedanswer1"></div>' +
                           '<input type="hidden" id="insurancetype" class="submitanswer" />' +
                       '</div>' +
                       '<div class="question-div question-div1">' +
                           '<div class="next-question-div">' +

                               '<div id="Home_Ins" class="col-md-2 col-sm-2 col-xs-6"><center><img class="next-question-button img-responsive" id="Home Insurance" src="../Images/NewAssets/Mhome.png" alt="Home Insurance" /></center><div style="text-align: center;margin-top: 7%;">Home</div></div>' +
                               '<div id="Auto_Ins" class="col-md-2 col-sm-2 col-xs-6"><center><img class="next-question-button img-responsive" id="Auto Insurance" src="../Images/NewAssets/Mauto.png" alt="Auto Insurance" /></center><div style="text-align: center;margin-top: 7%;">Auto</div></div>' +
                               '<div id="Life_Ins" class="col-md-2 col-sm-2 col-xs-6"><center><img class="next-question-button img-responsive" id="Life Insurance" src="../Images/NewAssets/Mlife.png" alt="Life Insurance" /></center><div style="text-align: center;margin-top: 7%;">Life</div></div>' +

                               '<div id="divMAuto" class="col-md-2 col-sm-2 col-xs-6"><center><img class="next-question-button img-responsive" id="Garage Keepers" src="../Images/NewAssets/garage-keepers.png" alt="Garage Keepers" /></center><div style="text-align: center;margin-top: 7%;">Garage Keepers</div></div>' +
                               '<div id="divMWorkComp" class="col-md-2 col-sm-2 col-xs-6"><center><img class="next-question-button img-responsive" id="Workers Compensation" src="../Images/NewAssets/workers-comp.png" alt="Workers Compensation" /></center><div style="text-align: center;margin-top: 7%;">Workers Compensation</div></div>' +
                               '<div id="divMBenefits" class="col-md-2 col-sm-2 col-xs-6"><center><img class="next-question-button img-responsive" id="Benefits Insurance" src="../Images/NewAssets/MeinekeBenefits.png" alt="Benefits" /></center><div style="text-align: center;margin-top: 7%;">Benefits</div></div>' +
                           '</div>' +
                       '</div>' +
                   '</div>' +
               '</li>';

    planSize = '<li id="planSize_li" class="main mainliremove">' +
           '<span class="line hideline"></span><span class="bullet"></span>' +
           '<div style="width:100%;">' +
               '<div class="question-bar">' +
                   '<div class="qusetion">Plan Size?</div>' +
                   '<div class="selectedanswer"></div>' +
                   '<input type="hidden" id="planSize" class="submitanswer" />' +
               '</div>' +
               '<div class="question-div">' +
                   '<div class="next-question-div">' +
                       '<div class="next-question-button answer extra" id="Under $1 Million">Under $1 Million</div>' +
                       '<div class="next-question-button answer extra" id="$1 Million to $5 Million">$1 Million to <br/>$5 Million</div>' +
                       '<div class="next-question-button answer extra" id="$5 Million and above">$5 Million and above</div>' +
                       '</div>' +
               '</div>' +
           '</div>' +
       '</li>';

    CurrentPlan = '<li id="CurrentPlan_li" class="main mainliremove">' +
          '<span id="spanCurrentPlan" class="line hideline"></span> <span class="bullet"></span>' +
          '<div style="width:100%;">' +
              '<div class="question-bar">' +
                  '<div class="qusetion">Do you have a current plan?</div>' +
                  '<div class="selectedanswer"></div>' +
                  '<input type="hidden" id="CurrentPlan" class="submitanswer" />' +
              '</div>' +
              '<div class="question-div">' +
                  '<div class="next-question-div">' +
                      '<div class="next-question-button answer" id="Yes"> <div class="isinsured">Yes</div></div>' +
                      '<div class="next-question-button answer" id="No"> <div class="isinsured">No</div></div>' +
                  '</div>' +
              '</div>' +
          '</div>' +
      '</li>';



    NumberofEmployees = '<li id="NumberofEmployees_li" class="main mainliremove">' +
            '<span class="line hideline"></span> <span class="bullet"></span>' +
            '<div style="width:100%;">' +
                '<div class="question-bar">' +
                    '<div class="qusetion">Number of Employees?</div>' +
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
                   '<div class="qusetion">Gross Payroll?</div>' +
                   '<div class="selectedanswer"></div>' +
                   '<input type="hidden" id="GrossPayroll" class="submitanswer" />' +
               '</div>' +
               '<div class="question-div">' +
                  '<div class="next-question-div">' +
                       '<div class="next-question-button answer extra" id="$0-$500,000">$0-$500,000</div>' +
                       '<div class="next-question-button answer extra" id="$500,001-$1million">$500,001-$1million</div>' +
                       '<div style="text-align:left;padding-left:4%;" class="next-question-button answer extra" id="$1.1million-$1.5million">$1.1million - $1.5million</div>' +
                       '<div class="next-question-button answer extra" id="$1.6million-$2million+">$1.6million - $2million+</div>' +
                   '</div>' +
               '</div>' +
           '</div>' +
       '</li>';


    NoofLocation = '<li id="NoofLocation_li" class="main mainliremove">' +
          '<span class="line hideline"></span> <span class="bullet"></span>' +
          '<div style="width:100%;">' +
              '<div class="question-bar">' +
                  '<div class="qusetion">Number of Locations?</div>' +
                  '<div class="selectedanswer"></div>' +
                  '<input type="hidden" id="submitnooflocations" class="submitanswer" />' +
              '</div>' +
              '<div class="question-div">' +
                  '<div class="next-question-div">' +
                      '<div class="row">' +
                          '<div class="col-sm-12 col-md-12">' +
                              '<div class="col-sm-6 col-md-6">' +
                                  '<input type="text" maxlength="10" id="noofLocations" class="contactinfo" placeholder="Enter number of locations" />' +
                              '</div>' +
                          '</div>' +
                          '<div class="col-sm-12 col-md-12">' +
                              '<div class="next-question-button answer-small-button" id="btnnoofLocations">Next >></div>' +
                          '</div>' +
                              '<div class="col-sm-12 col-md-12">' +
                                 '<div class="errormessage"></div>' +
                              '</div>' +
                          '</div>' +
                   '</div>' +
              '</div>' +
          '</div>' +
      '</li>';

    howManyUnits = '<li id="howManyUnits_li" class="main mainliremove">' +
          '<span class="line hideline"></span> <span class="bullet"></span>' +
          '<div style="width:100%;">' +
              '<div class="question-bar">' +
                  '<div class="qusetion">How many units?</div>' +
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

    howManyStalls = '<li id="howManyStalls_li" class="main mainliremove">' +
           '<span class="line hideline"></span> <span class="bullet"></span>' +
           '<div style="width:100%;">' +
               '<div class="question-bar">' +
                   '<div class="qusetion">How many stalls?</div>' +
                   '<div class="selectedanswer"></div>' +
                   '<input type="hidden" id="submithowManyStalls" class="submitanswer" />' +
               '</div>' +
               '<div class="question-div">' +
                   '<div class="next-question-div">' +
                       '<div class="row">' +
                           '<div class="col-sm-12 col-md-12">' +
                               '<div class="col-sm-6 col-md-6">' +
                                   '<input type="text" maxlength="10" id="howManyStalls" class="contactinfo" placeholder="Enter number of stalls" />' +
                               '</div>' +
                           '</div>' +
                           '<div class="col-sm-12 col-md-12">' +
                               '<div class="next-question-button answer-small-button" id="btnhowManyStalls">Next >></div>' +
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
                   '<div class="qusetion">Deductible if any?</div>' +
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
                   '<div class="qusetion">Current Limit?</div>' +
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
                   '<div class="col-sm-12 col-md-12 padding0">' +

                       '<div id="language-select" style="border-bottom: 1px solid rgb(30,172,226);"></div>' +
                          '</div>' +

                      '</div>' +
                      '<div class="col-sm-12 col-md-12 padding7px">' +

                              '<div class="next-question-button answer-small-button" id="btnlanguage">Next >></div>' +
                              '<div id="languagemessage" class="messageClass" ></div>' +
                          '</div>';
    '</div>' +
'</div>' +
'</li>';

    valueOfHome = '<li id="valueOfHome_li" class="main mainliremove">' +
            '<span class="line hideline"></span> <span class="bullet"></span>' +
            '<div style="width:100%;">' +
                '<div class="question-bar">' +
                    '<div class="qusetion">Value of your home?</div>' +
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
                    '<div class="qusetion">Type of auto?</div>' +
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
                    '<div class="qusetion">Acquisition or currently insured?</div>' +
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
                    '<div class="qusetion">Gross Sales?</div>' +
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

    grossrevenueforgarage = '<li id="grossrevenueforgarage_li" class="main mainliremove">' +
           '<span class="line hideline"></span> ' +
           '<span class="bullet"></span>' +
           '<div style="width:100%;">' +
               '<div class="question-bar">' +
                   '<div class="qusetion">Gross revenue?</div>' +
                   '<div class="selectedanswer"></div>' +
                   '<input type="hidden" id="grossrevenueforgarage" class="submitanswer" />' +
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
                    '<div class="qusetion">Coverage amount?</div>' +
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
                    '<div class="qusetion">Are you currently insured?</div>' +
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
                '<div class="qusetion">What is your industry?</div>' +
                '<div class="selectedanswer"></div>' +
                '<input type="hidden" id="industry" class="submitanswer" />' +
            '</div>' +
            '<div class="question-div">' +
                '<div class="next-question-div">' +
                '<div class="col-sm-6 col-md-3 col-xs-12">' +
                    //'<select  id="industry-select" class="select-insurance">' +
                    //    '<option selected="selected" value="">Please choose</option>' +
                    //      '<option value="">I don\'t know</option>' +
                    //'</select>' +
                     '<div id="industry-select" style="border-bottom: 1px solid rgb(30,172,226);" ></div>' +
                        '</div>' +

                    '</div>' +
                        '<div class="col-sm-12 col-md-12">' +
                            '<div class="next-question-button answer-small-button" id="btnindustry">Next >></div>' +
                        '</div>';
    '</div>' +
'</div>' +
'</div>' +
'</li>';


    

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

               //'<img src="../Images/NewAssets/sumbit.PNG" class="SubmitInsurance" style="cursor:pointer;" alt="Submit" />' +
                '<input class="SubmitInsurance" type="button" value="See Matches" style="font-size: 1.3vw;font-weight: 600;color: #fff;width:15% !important;border-radius:31px;border: none;text-transform:none !important;font-family:open sans !important;font-weight:100;background-color: rgb(30,172,226);height: 46px;" />' +
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
                                                       //'<img src="../Images/NewAssets/sumbit.PNG" id="SubmitWithLogin" style="cursor:pointer;" class="SubmitInsuranceWithLogin" alt="Submit" />' +

                                                       '<input id="SubmitWithLogin" class="SubmitInsuranceWithLogin" type="button" value="See Matches" style="font-size: 1.3vw;font-weight: 600;color: #fff;width:15% !important;border-radius:31px;border: none;text-transform:none !important;font-family:open sans !important;font-weight:100;background-color: rgb(30,172,226);height: 46px;" />' +

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
        //alert('called');

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
        //alert("Answer clicked");


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



        //alert("isinsurancechange - " + isinsurancechange);

        if (isinsurancechange == 'BusinessLine') {
            $('.mainliremove').remove();
            $("#typeofinsurance").css("display", "none");
            $(".selectedanswer1").css("display", "none");
            $(".question-div1").css("display", "block");
            $(".qusetion1").css("display", "block");
            $(".hideline1").css("display", "none");
        }
        else if (isinsurancechange == 'typeofinsurance') {
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

    //alert("Id- " + selectedId.id);


    var SelectedAnswer = '';
    if (mode == 'click') {
        //take value if submited by textbox

        if (selectedId.id == 'Personal') {
            //alert("Personal");
            //$('.main').remove();
            //$('#InsuranceOtherQuestion').remove(typeofinsurance);
            $("#typeofinsurance").remove();
            $('#InsuranceOtherQuestion').append(typeofinsurance);

            $("#divMAuto").css("display", "none");
            $("#divMWorkComp").css("display", "none");
            $("#divMLiability").css("display", "none");
            $("#divMBenefits").css("display", "none");

            $("#Home_Ins").css("display", "block");
            $("#Auto_Ins").css("display", "block");
            $("#Life_Ins").css("display", "block");
            $("#Busi_Ins").css("display", "block");
            $("#EmpBene_Ins").css("display", "block");

           
        }
        else if (selectedId.id == 'Commercial') {
            //alert("Commercial");
            //$('.main').remove();
            //$('#InsuranceOtherQuestion').remove(typeofinsurance);
            $("#typeofinsurance").remove();
            $('#InsuranceOtherQuestion').append(typeofinsurance);

            $("#Home_Ins").css("display", "none");
            $("#Auto_Ins").css("display", "none");
            $("#Life_Ins").css("display", "none");
            $("#Busi_Ins").css("display", "none");
            $("#EmpBene_Ins").css("display", "none");

            $("#divMAuto").css("display", "block");
            $("#divMWorkComp").css("display", "block");
            $("#divMLiability").css("display", "block");
            $("#divMBenefits").css("display", "block");

            
        }
        else if (selectedId.id == '401k') {
         $("#typeofinsurance").remove();
         //$("#insurancetype").val('401k');
         
            $("#Home_Ins").css("display", "none");
            $("#Auto_Ins").css("display", "none");
            $("#Life_Ins").css("display", "none");
            $("#Busi_Ins").css("display", "none");
            $("#EmpBene_Ins").css("display", "none");

            $("#divMAuto").css("display", "none");
            $("#divMWorkComp").css("display", "none");
            $("#divMLiability").css("display", "none");
            $("#divMBenefits").css("display", "none");
        }

        //11May18
        if (selectedId.id == 'declarationYes') {
            SelectedAnswer = "Yes";
            $(selectedId).parents('.main').find('.submitanswer').val(SelectedAnswer);
        }
            //11May18
        else if (selectedId.id == 'declarationNo') {
            SelectedAnswer = "No";
            $(selectedId).parents('.main').find('.submitanswer').val(SelectedAnswer);
        }
        else if (selectedId.id == 'btnnote') {

            if ($('#notes1').val().trim() != '') {
                SelectedAnswer = $('#notes1').val();
                //bind answer for final submit
                $(selectedId).parents('.main').find('.submitanswer').val(SelectedAnswer);
            }
            else {
                SelectedAnswer = 'No notes added';
            }

        }
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
        else if (selectedId.id == 'btnhowManyStalls') {

            if ($('#howManyStalls').val().trim() != '') {
                SelectedAnswer = $('#howManyStalls').val();
                //bind answer for final submit
                $(selectedId).parents('.main').find('.submitanswer').val(SelectedAnswer);
            }
            else {
                $(".errormessage").text("Please enter stalls");
                $('.errormessage').fadeIn('fast').delay(2000).fadeOut('fast');
                return false;
            }
        }
        else if (selectedId.id == 'btnuploaddeclaration') {

            if ($('#txtUploadDeclaration').val().trim() != '') {
                SelectedAnswer = $('#txtUploadDeclaration').val();
                //bind answer for final submit
                $(selectedId).parents('.main').find('.submitanswer').val(SelectedAnswer);
            }
            else {
                $(".errormessage").text("Please upload declaration document");
                $('.errormessage').fadeIn('fast').delay(2000).fadeOut('fast');
                return false;
            }
        }
        else if (selectedId.id == 'btnnoofLocations') {

            if ($('#noofLocations').val().trim() != '') {
                SelectedAnswer = $('#noofLocations').val();
                //bind answer for final submit
                $(selectedId).parents('.main').find('.submitanswer').val(SelectedAnswer);
            }
            else {
                $(".errormessage").text("Please enter number of locations");
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
            //var siccodes = $('.ui-igcombo-hidden-field').val();//value of dropdown
            //var siccodestext = $('.ui-igcombo-field').val();// text of dropdown

            //13April18
            var siccodes = $(selectedId).parents('.main').find('.ui-igcombo-hidden-field').val();
            var siccodestext = $(selectedId).parents('.main').find('.ui-igcombo-field').val();

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

            //selectedindustry = $("#industry-select option:selected").text();
            //selectedindustryval = $("#industry-select option:selected").val();

            //13April18
            selectedindustryval = $(selectedId).parents('.main').find('.ui-igcombo-hidden-field').val();
            selectedindustry = $(selectedId).parents('.main').find('.ui-igcombo-field').val();
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
        else if (selectedId.id == 'btnlanguage') {

            SelectedAnswer = $(selectedId).parents('.main').find('.ui-igcombo-hidden-field').val();
            if (SelectedAnswer == '') {
                $("#languagemessage").text("Please select Language");
                $('#languagemessage').fadeIn('fast').delay(2000).fadeOut('fast');
                return false;
            }
            else {

                $(selectedId).parents('.main').find('.submitanswer').val(SelectedAnswer);
            }
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
            //alert('else ' + SelectedAnswer.replace("_"," "));
            //bind answer for final submit
            $(selectedId).parents('.main').find('.submitanswer').val(SelectedAnswer);

        }



    }
    else if (mode == 'select') {//take value if submited by dropdownlist
        SelectedAnswer = selectedId.options[selectedId.selectedIndex].text;
        //bind answer for final submit
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
        // alert("In Home");
        //alert("In Home Insurance");
        $('#InsuranceOtherQuestion').append(newPurchaseOrCurrentlyInsured + valueOfHome + language + notes + declaration + uploaddeclaration + appendcontatcus);
        $parent.nextAll('.main').hide();//hide all main question 
        fillInsuranceCompanyList();
        IsLogin();
        fillLanguage();
        $('.ui-igcombo-field').prop('readonly', true);
        $('.ui-igcombo-field').prop("placeholder", "Please choose");
        //$('.ui-igcombo-field').prop('readonly', true);//set readonly to sic code drodown
        //$('.ui-igcombo-field').prop("placeholder", "Please choose");

    }

    //fill  Auto Insurance question on selection of insurance type
    if (selectedId.id == 'Auto Insurance') {
        //alert("In Auto Insurance");
        $('#InsuranceOtherQuestion').append(typeOfAuto + newPurchaseOrCurrentlyInsured + language + notes + declaration + uploaddeclaration + appendcontatcus);
        $parent.nextAll('.main').hide();//hide all main question 
        fillInsuranceCompanyList();
        IsLogin();
        fillLanguage();
        $('.ui-igcombo-field').prop('readonly', true);
        $('.ui-igcombo-field').prop("placeholder", "Please choose");
    }

    //fill Life Insurance question on selection of insurance type
    if (selectedId.id == 'Life Insurance') {
        //alert("In Life Insurance");
        $('#InsuranceOtherQuestion').append(coverageamount + language + notes + declaration + uploaddeclaration + appendcontatcus);
        $parent.nextAll('.main').hide();//hide all main question 
        IsLogin();
        fillLanguage();
        $('.ui-igcombo-field').prop('readonly', true);
        $('.ui-igcombo-field').prop("placeholder", "Please choose");
    }

    //fill Business Insurance question on selection of insurance type
    if (selectedId.id == 'Business Insurance') {
        $('#InsuranceOtherQuestion').append(acquisitionIsInsured + grossrevenue + industry + siccode + language + notes + declaration + uploaddeclaration + appendcontatcus);
        $parent.nextAll('.main').hide();//hide all main question 
        fillIndustry();
        IsLogin();

        fillLanguage();
   

        $('.ui-igcombo-field').prop('readonly', true);//set readonly to sic code drodown
        $('.ui-igcombo-field').prop("placeholder", "Please choose");
    }

    //fill Employee Benefits question on selection of insurance type
    if (selectedId.id == 'Employee Benefits') {
        $('#InsuranceOtherQuestion').append(newPurchaseOrCurrentlyInsured + noOfEmployee + industry + siccode + language + notes + declaration + uploaddeclaration + appendcontatcus);
        $parent.nextAll('.main').hide();//hide all main question

        fillIndustry();
        //fillInsuranceCompanyList();
        IsLogin();

        fillLanguage();
      
        //set readonly to sic code drodown
        $('.ui-igcombo-field').prop('readonly', true);
        $('.ui-igcombo-field').prop("placeholder", "Please choose");
    }

    //fill  Commercial Auto Insurance question on selection of insurance type
    if (selectedId.id == 'Garage Keepers') {
        //alert("In Commercial Auto - " + $('#InsuranceOtherQuestion').val());
        $('#InsuranceOtherQuestion').append(howManyStalls + NoofLocation + grossrevenueforgarage + declaration + uploaddeclaration + appendcontatcus);
        $parent.nextAll('.main').hide();//hide all main question 
        //fillInsuranceCompanyList();
        IsLogin();
        fillLanguage();
        $('.ui-igcombo-field').prop('readonly', true);
        $('.ui-igcombo-field').prop("placeholder", "Please choose");
    }

    //fill Workers Compensation question on selection of insurance type
    if (selectedId.id == 'Workers Compensation') {
        $('#InsuranceOtherQuestion').append(NumberofEmployees + GrossPayroll + declaration + uploaddeclaration + appendcontatcus);
        $parent.nextAll('.main').hide();//hide all main question 
        IsLogin();
        fillLanguage();
        $('.ui-igcombo-field').prop('readonly', true);
        $('.ui-igcombo-field').prop("placeholder", "Please choose");
    }

    //fill Liability Insurance question on selection of insurance type
    if (selectedId.id == 'Liability Insurance') {
        $('#InsuranceOtherQuestion').append(industry + siccode + grossrevenue + deductibleIfany + declaration + uploaddeclaration + appendcontatcus);
        $parent.nextAll('.main').hide();//hide all main question 
        fillIndustry();
        $('#deduc').removeClass('line');
        IsLogin();

        fillLanguage();
    

        $('.ui-igcombo-field').prop('readonly', true);//set readonly to sic code drodown
        $('.ui-igcombo-field').prop("placeholder", "Please choose");
    }

    //fill Meineke Employee Benefits question on selection of insurance type
    if (selectedId.id == 'Benefits Insurance') {
        $('#InsuranceOtherQuestion').append(newPurchaseOrCurrentlyInsured + noOfEmployee + industry + siccode + language + notes + declaration + uploaddeclaration + appendcontatcus);
        $parent.nextAll('.main').hide();//hide all main question

        fillIndustry();
        fillInsuranceCompanyList();
        IsLogin();
        fillLanguage();
     
        //set readonly to sic code drodown
        $('.ui-igcombo-field').prop('readonly', true);
        $('.ui-igcombo-field').prop("placeholder", "Please choose");
    }

    if (selectedId.id == '401k') {
        //alert("In 401k - " + $('#InsuranceOtherQuestion').val());
        $(".liinsurancetyperemove").css("display", "none");

        $('#InsuranceOtherQuestion').append(CurrentPlan + noOfEmployee + planSize + declaration + uploaddeclaration + appendcontatcus);
        $parent.nextAll('.main').hide();//hide all main question 

        IsLogin();
        fillLanguage();
        $('.ui-igcombo-field').prop('readonly', true);
        $('.ui-igcombo-field').prop("placeholder", "Please choose");
    }

    //11May18
    if (selectedId.id == 'declarationYes') {
        if (IsUserLogin == 'Authorize') {
            $('#spandeclaration').addClass('line');
            $('#spanuploaddeclaration').removeClass('line');
        }
    }

    //11May18
    if (selectedId.id == 'declarationNo') {
        if (IsUserLogin == 'Authorize') {
            $('#spandeclaration').removeClass('line');
            // $('#spanuploaddeclaration').removeClass('line');
        }
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
        //11May18
    else if (selectedId.id == 'declarationNo') {

        $parent.next(".main").next(".main").children('.bullet').addClass('bulletactive');
        $parent.next('.main').next(".main").show();
        $parent.next('.main').next(".main").next('.question-div').show();

        $("classuploaddeclaration").css("display", "none");
        $(".bulletdeclaration").addClass("bulletactive");
        //alert("End");

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

        //11May18
        //$('#notes').removeClass('line');
        //$('#current').removeClass('line');
        //$('#gross').removeClass('line');
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

function fillLanguage() {
    var obj = JSON.parse(languageJson);
    var languageArray = [];
    for (var i in obj.Language) {

        languageArray.push({
            "Name": obj.Language[i].Value,
            "value": obj.Language[i].Key
        });
    }

    $("#language-select").igCombo({
        width: 300,
        dataSource: languageArray,
        textKey: "Name",
        valueKey: "value",
        multiSelection: {
            enabled: false,
            showCheckboxes: false
        },
        dropDownOrientation: "bottom"
    });

    $("#language-select").parents('.main').find('.ui-igcombo-hidden-field').val('English');
    $("#language-select").parents('.main').find('.ui-igcombo-field').val('English');
}

function fillIndustry() {

    var industryarray = [];

    industryarray.push({
        "Name": "Please choose",
        "value": ""
    });
    industryarray.push({
        "Name": "I don't know",
        "value": ""
    });

    for (var i in industryList.result) {

        //$('#industry-select').append(
        // $("<option></option>")
        //      .attr("value", industryList.result[i].Value)
        //     .text(industryList.result[i].Text)
        //);
        industryarray.push({
            "Name": industryList.result[i].Text,
            "value": industryList.result[i].Value
        });
    }

    $("#industry-select").igCombo({
        width: 300,
        dataSource: industryarray,
        textKey: "Name",
        valueKey: "value",
        multiSelection: {
            enabled: false,
            showCheckboxes: false
        },
        dropDownOrientation: "bottom"
    });


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
