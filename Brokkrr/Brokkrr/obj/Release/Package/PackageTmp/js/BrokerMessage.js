var webserviceURL = "";
var GlobalShowingMsg = "";
var GlobleUserIDCustomer = "";
var CustomerProfilePic = '';

var GlobalMsgClicked = "";

var SubmitChatCount = 1;

var deletemsgarrayCustomer = [];
var messagelist = [];
var ChatMessageList = [];
var ChatIdToDelete = [];
var LoadedMessagesId = [];
var NewLoadingMessagesId = [];

var editclick = "false";
var IsNetworkAvailable = "false";

var DeviceId = "";
var FirstBrokerMsgId = "";
var FirstCustomerMsgId = "";
var FirstMessageId = "";
var FirstBrokerId = "";
var FirstCustomerId = "";
var FirstBrokerName = "";
var FirstMessage = "";
var Firstmsgdate = "";

var FirstName = "";
var LastName = "";
var CustomerName = "";

var TopBrokerMessageId = "";
var TopCustomerMessageId = "";
var TopBrokerId = "";
var TopCustomerId = "";

var TopBrokerName = "";
var TopMessage = "";
var TopMsgDate = "";
var TopBrokerProfilePic = '';

function GetCustomerMessages(UserId, WeServiceUrl, ProfilePic, CustName) {

    CustomerName = CustName;
    webserviceURL = WeServiceUrl;
    GlobleUserIDCustomer = UserId;
    if (ProfilePic != '') {
        CustomerProfilePic = ProfilePic;
    }
    else {
        CustomerProfilePic = '../Images/Profile/customer.jpg';
    }
    var profileimg = '';

    //alert("UserId - " + UserId + " , WeServiceUrl - " + WeServiceUrl + " , CustName - " + CustName + " , ProfilePic - " + ProfilePic);

    $.ajax({
        type: "POST",
        url: webserviceURL,
        data: { UserId: UserId, TimeStamp: "", ActionName: "DoGetMessages" },
        //contentType: "application/json; charset=utf-8",
        //dataType: "json",
        success: function (data) {
            //alert('Success data '+ data);
            var obj = JSON.parse(data);
            // alert(data);
            var issuccess = obj.IsSuccess;
            var ErrorMessage = obj.ErrorMessage;

            if (issuccess == true) {
                //alert('array '+messagelist);
                var ContactedMessageList = obj.ContactedMessageList;
                GlobalShowingMsg = obj.ContactedMessageList;
                //alert('msg count :'+parseInt(ContactedMessageList.length));
                if (parseInt(ContactedMessageList.length) > 0) {

                    $.each(ContactedMessageList, function (i, res) {
                        // alert(messagelist);
                        if (messagelist.indexOf(res.MessageId) >= 0) {//alert('contains');
                        }
                        else {
                            //  alert(res);
                            var content = '';
                            if (res.ProfilePictureImg != '') {

                                profileimg = res.ProfilePictureImg;
                            }
                            else {
                                profileimg = '../Images/Profile/customer.jpg';
                            }

                            //var dt = res.ContactDate;
                            var dt = res.LocalDateTime;
                            var dt1 = dt.split(' ');
                            var datedb = dt1[0];
                            var timedb = dt1[1];
                            var ampmdb = dt1[2];
                            msgdate = datedb;

                            var timeconvert = timedb.split(':');
                            var hrs = timeconvert[0];
                            var min = timeconvert[1];
                            var sec = timeconvert[2];

                            var nowdt = new Date();

                            var nowdate = (nowdt.getMonth() + 1) + '/' + nowdt.getDate() + '/' + nowdt.getFullYear();
                            //var nowdate = nowdt.getDate() + '-' + month + '-' + nowdt.getFullYear();
                            //var nowtime = nowdt.getHours() + ":" + nowdt.getMinutes() + ":" + nowdt.getSeconds();
                            //var ampm = (nowdt.getHours() >= 12) ? "PM" : "AM";
                            var msgdate = '';


                            if (datedb == nowdate) {
                                //alert('datedb '+ datedb);
                                //alert('nowdate '+ nowdate);
                                msgdate = timedb;

                                //alert(hrs);
                                if (hrs > 12) {
                                    hrs = hrs - 12;
                                    //sec="AM";
                                }
                                else {
                                    //sec="PM";

                                }
                                sec = ampmdb;

                                if (hrs <= 9) { hrs = "0" + hrs; }
                                if (min <= 9) { min = "0" + min; }


                                msgdate = "Today " + hrs.slice(-2) + ':' + min.slice(-2) + ' ' + sec;

                                //msgdate=hrs+':'+min+' '+sec;
                                //alert("msdate"+msgdate);
                            }
                            else {
                                msgdate = datedb;
                            }

                            content = '<div id="MessageId' + res.MessageId + '"  class="col-md-12 col-sm-12 col-xs-12" style="text-align:justify;padding-top:2%;border-bottom:1px solid rgb(226,226,226);padding-bottom:2%;background-color:#fff;border-right: 1px solid rgb(226,226,226);cursor: default;">'
                             + '<div style="display:none;" id="Hidden' + res.MessageId + '" class="BrokerName"> ' + res.CustomerName + ' </div>'
                         + '<div class="col-md-12 col-sm-12 col-xs-12" style="padding: 0;margin: 1%;">'
                         + '<div class="col-md-10 col-sm-10 col-xs-10" style="cursor: pointer;padding-left: 0;padding-right: 0;"  href="#" onclick="getChatMessageCustomer(\'' + res.CustomerMsgId + '\',\'' + res.MessageId + '\' ,\'' + res.BrokerId + '\',\'' + res.CustomerId + '\',\'' + res.CustomerName + '\', \'' + res.CustomerName + '\',\'' + msgdate + '\',\'' + profileimg + '\')"><div class="col-md-3 col-sm-3 col-xs-3">'
                                + '<img class="imageFit circular--landscape circular--square circular--portrait" src=' + profileimg + ' id="profilepicprofile" width="50" height="50" style="border-radius:50%;margin:5%;height: 55px;" />'
                            + '</div>'
                             + '<div class="col-md-9 col-sm-9 col-xs-9" style=" text-align: left;padding-left: 0;padding-right: 0;padding-top: 3%;"> '
                             + ' <div id="Name' + res.MessageId + '" class="col-md-12 col-sm-12 col-xs-12" style="font-weight:bold;font-size:20px;padding-left: 0;padding-right: 0;">' + res.CustomerName + '</div>'
                             + '<div class="col-md-12 col-sm-12 col-xs-12" style="font-size:10px;color:rgb(186,186,186);direction: ltr;padding-left: 0;padding-right: 0;">' + msgdate + '</div>'
                             + '</div></div>'
                             + '<div class="col-md-2 col-sm-2 col-xs-2" style="padding-top: 4%;cursor: default;">'
                              + '<span class="countmsg" id="Count' + res.MessageId + '" ></span>'
                             +'<span class="tickdiv"><img src="../Images/Assets/redioButton.png" id="tick' + res.MessageId + '" onclick="getCustomerDeletedItems(\'' + res.MessageId + '\')" class="tickimg" style="height: 20px;float: right;margin-top: 35px;margin-left: 0;cursor: pointer;""/></span></div>'
                          + '</div>'
                         + '</div>';

                            $('#customerMeassage').prepend(content);
                            messagelist.push(res.MessageId);

                            FirstBrokerMsgId = res.MessageId;
                            FirstMessageId = res.CustomerMsgId;
                            FirstBrokerId = res.BrokerId;
                            FirstCustomerId = res.CustomerId;
                            TopCustomerId = FirstCustomerId;
                            FirstBrokerName = res.CustomerName;
                            FirstMessage = res.Message;
                            Firstmsgdate = msgdate;

                            //alert(FirstBrokerMsgId)

                        }
                    });
                    //getChatMessageCustomer(FirstBrokerMsgId, FirstMessageId, FirstBrokerId, FirstCustomerId, FirstBrokerName, FirstMessage, Firstmsgdate, profileimg);

                    if (window.screen.width >= 1024) {

                        getChatMessageCustomer(FirstBrokerMsgId, FirstMessageId, FirstBrokerId, FirstCustomerId, FirstBrokerName, FirstMessage, Firstmsgdate, profileimg);
                    }
                    else {
                        $('#loadMessages').hide();
                        $('.bodyMessages').removeClass('bodyblur');
                    }

                }
                else {
                    if (messagelist.length <= 0) {
                        //alert("Called - No Messages");
                        $('#loadMessages').hide();
                        $('.bodyMessages').removeClass('bodyblur');
                        $("#divNoMessage").css("display", "block");
                        $("#divEditDeleteButton").css("display", "none");
                        $("#divMessageBox").css("display", "none");
                    }
                }

                /*get latest message*/
            }
        },
        error: function (e) {
            //alert('GetCustomerMessages error: ' + e.msg);
        }
    });
}

/*get latest message*/

setInterval(function () {

    // Check Message is deleted from other device then remove msg .
    $.ajax({
        type: "POST",
        url: webserviceURL,
        data: { UserId: GlobleUserIDCustomer, TimeStamp: "", ActionName: "DoGetMessages" },
        success: function (data) {


            var arrayList = [];
            var obj = JSON.parse(data);

            var issuccess = obj.IsSuccess;
            var ErrorMessage = obj.ErrorMessage;

            if (issuccess == true) {
                //alert('array '+messagelist);
                var ContactedMessageList = obj.ContactedMessageList;
                //alert('msg count :'+parseInt(ContactedMessageList.length));
                if (parseInt(ContactedMessageList.length) > 0) {

                    $.each(ContactedMessageList, function (i, res) {
                        arrayList.push(res.MessageId);
                        //alert("arrayList - " + arrayList);
                    });

                    $("#divNoMessage").css("display", "none");
                    $("#divEditDeleteButton").css("display", "block");
                    $("#divMessageBox").css("display", "block");
                }
                else {
                    $("#divNoMessage").css("display", "block");
                    $("#divEditDeleteButton").css("display", "none");
                    $("#divMessageBox").css("display", "none");
                    //alert("No");
                }

                $.each(messagelist, function (idx, value) {
                    if ($.inArray(value, arrayList) == -1) {
                        //alert("In If - " + arrayList);
                        $('#MessageId' + value).remove();//remove

                        messagelist = jQuery.grep(messagelist, function (value1) {
                            return value1 != value;
                        });
                    }
                });

                if (messagelist.length <= 0) {
                    //$("#divNoMessage").css("display", "block");
                    //$("#divEditDeleteButton").css("display", "none");
                    //$("#divMessageBox").css("display", "none");
                }
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            // alert('interval 1 error: ' + e.msg);
        }
    });


    //  alert('hi');
    $.ajax({
        type: "POST",
        url: webserviceURL,
        data: { UserId: GlobleUserIDCustomer, ActionName: "DoGetUnreadMsgCount" },
        success: function (data) {
           //alert(data)
            var obj = JSON.parse(data);
            var issuccess = obj.IsSuccess;
            var ErrorMessage = obj.ErrorMessage;
            var MessageDetails = obj.MessageDetails;

            var UnreadMsgCnt = obj.UnreadMsgCnt;

            var ContactedMessageList = obj.ContactedMessageList;
            var mainmsgarray = [];

            if (issuccess == true) {
                var totalunreadconver = 0;

                $.each(GlobalShowingMsg, function (i, res) {
                    //alert("GlobalShowingMsg" + GlobalShowingMsg + " res.MessageId" + res.MessageId);
                    //alert(res.MessageId)
                    jQuery('#Count' + res.MessageId).removeClass('circlechatcountOnMessage');
                    $('#Count' + res.MessageId).text('');
                });

                if (parseInt(ContactedMessageList.length) > 0) {

                   

                    totalunreadconver = parseInt(ContactedMessageList.length);

                    //alert(totalunreadconver);

                    //20Sep17

                    //$.each(messagelist, function (idx, value) {

                    //    jQuery('#Count' + value).removeClass('circlechatcountOnMessage');
                    //    jQuery('#date' + value).removeClass('UnreadMsgdate');
                    //    $('#Count' + value).text('');

                    //});
                    
                    //$.each(GlobalShowingMsg, function (i, res) {
                    //    //alert("GlobalShowingMsg" + GlobalShowingMsg + " res.MessageId" + res.MessageId);
                    //    //alert(res.MessageId)
                    //    jQuery('#Count' + res.MessageId).removeClass('circlechatcountOnMessage');
                    //    $('#Count' + res.MessageId).text('');
                    //});

                    $.each(ContactedMessageList, function (i, res) {
                        mainmsgarray.push(res.MessageId);
                        //20Sept17
                        jQuery('#Count' + res.MessageId).addClass('circlechatcountOnMessage');
                        	//jQuery('#date'+res.MessageId).addClass('UnreadMsgdate');
                        	$('#Count'+res.MessageId).text('1');

                        if (messagelist.indexOf(res.MessageId) >= 0) {//alert('contains');
                        }
                        else {// alert('not contains '+ res.MessageId);
                            var content = '';
                            var profileimg = '';

                            if (res.ProfilePictureImg != '') {

                                profileimg = res.ProfilePictureImg;
                            }
                            else {
                                profileimg = '../Images/Profile/customer.jpg';
                            }


                            //var dt = res.ContactDate;
                            var dt = res.LocalDateTime;
                            var dt1 = dt.split(' ');
                            var datedb = dt1[0];
                            var timedb = dt1[1];
                            var ampmdb = dt1[2];
                            msgdate = datedb;


                            //alert();
                            var timeconvert = timedb.split(':');
                            var hrs = timeconvert[0];
                            var min = timeconvert[1];
                            var sec = timeconvert[2];



                            var nowdt = new Date();

                            var nowdate = (nowdt.getMonth() + 1) + '/' + nowdt.getDate() + '/' + nowdt.getFullYear();
                            //var nowdate = nowdt.getDate() + '-' + month + '-' + nowdt.getFullYear();
                            //var nowtime = nowdt.getHours() + ":" + nowdt.getMinutes() + ":" + nowdt.getSeconds();
                            //var ampm = (nowdt.getHours() >= 12) ? "PM" : "AM";
                            var msgdate = '';

                            if (datedb == nowdate) {
                                //alert('datedb '+ datedb);
                                //alert('nowdate '+ nowdate);
                                msgdate = timedb;

                                //alert(hrs);
                                if (hrs > 12) {
                                    hrs = hrs - 12;
                                    //sec="AM";
                                }
                                else {
                                    //sec="PM";

                                }
                                sec = ampmdb;
                                //alert('sec ' + sec);
                                if (hrs <= 9) { hrs = "0" + hrs; }
                                if (min <= 9) { min = "0" + min; }


                                msgdate = "Today " + hrs.slice(-2) + ':' + min.slice(-2) + ' ' + sec;

                                //msgdate=hrs+':'+min+' '+sec;
                                //alert("msdate"+msgdate);
                            }
                            else {
                                msgdate = datedb;
                            }

                            content = '<div id="MessageId' + res.MessageId + '"  class="col-md-12 col-sm-12 col-xs-12" style="text-align:justify;padding-top:2%;border-bottom:1px solid rgb(226,226,226);border-right: 1px solid rgb(226,226,226);padding-bottom:2%;background-color:#fff;cursor: default;">'

                              + '<div style="display:none;" id="Hidden' + res.MessageId + '" class="BrokerName"> ' + res.CustomerName + ' </div>'

                          + '<div class="col-md-12 col-sm-12 col-xs-12" style="padding: 0;margin: 1%;">'
                          + '<div class="col-md-10 col-sm-10 col-xs-10" style="cursor: pointer;padding-left: 0;padding-right: 0;"  href="#" onclick="getChatMessageCustomer(\'' + res.CustomerMsgId + '\',\'' + res.MessageId + '\' ,\'' + res.BrokerId + '\',\'' + res.CustomerId + '\',\'' + res.CustomerName + '\', \'' + res.CustomerName + '\',\'' + msgdate + '\',\'' + profileimg + '\')"><div class="col-md-3 col-sm-3 col-xs-3">'
                + '<img class="imageFit circular--landscape circular--square circular--portrait" src=' + profileimg + ' id="profilepicprofile" width="50" height="50" style="border-radius:50%;margin:5%;height: 55px;" />'
                             + '</div>'

                              + '<div class="col-md-9 col-sm-9 col-xs-9" style=" text-align: left;padding-left: 0;padding-right: 0;padding-top: 3%;"> '
                              + ' <div id="Name' + res.MessageId + '" class="col-md-12 col-sm-12 col-xs-12" style="font-weight:bold;font-size:20px;padding-left: 0;padding-right: 0;">' + res.CustomerName + '</div>'
                              + '<div class="col-md-12 col-sm-12 col-xs-12" style="font-size:10px;color:rgb(186,186,186);direction: ltr;padding-left: 0;padding-right: 0;">' + msgdate + '</div>'
                              + '</div></div>'
                              + '<div class="col-md-2 col-sm-2 col-xs-2" style="padding-top: 4%;cursor: default;">'
                               + '<span class="countmsg" id="Count' + res.MessageId + '" ></span>'
                            +'<span class="tickdiv"><img src="../Images/Assets/redioButton.png" id="tick' + res.MessageId + '" onclick="getCustomerDeletedItems(\'' + res.MessageId + '\')" class="tickimg" style="height: 20px;float: right;margin-top: 35px;margin-left: 0;cursor: pointer;""/></span></div>'
                           + '</div>'
                          + '</div>';

                            $('#customerMeassage').prepend(content);

                            messagelist.push(res.MessageId);

                        }
                    });
                }

                var UnreadMsgArray = [];
                $.each(MessageDetails, function (i, res) {

                    var msgcountevery = 0;
                    if (mainmsgarray.indexOf(res.MessageId) < 0) {
                        if (parseInt(res.Cnt) > 0) {
                            totalunreadconver++;
                            UnreadMsgArray.push(res.MessageId);
                        }
                    }

                    $.each(ContactedMessageList, function (i, res1) {
                        UnreadMsgArray.push(res1.MessageId);
                        if (parseInt(res.MessageId) == parseInt(res1.MessageId)) {
                            msgcountevery++
                        }

                    });


                    msgcountevery = parseInt(res.Cnt) + parseInt(msgcountevery);

                    if (parseInt(msgcountevery) >= 1) {
                        //20Sept17
                        jQuery('#Count' + res.MessageId).addClass('circlechatcountOnMessage');
                        //jQuery('#date' + res.MessageId).addClass('UnreadMsgdate');
                        $('#Count' + res.MessageId).text(msgcountevery);
                    }

                    var dt = res.dtime;
                    var dt1 = dt.split(' ');
                    var datedb = dt1[0];
                    var timedb = dt1[1];
                    var ampmdb = dt1[2];
                    msgdate = datedb;
                    var timeconvert = timedb.split(':');
                    var hrs = timeconvert[0];
                    var min = timeconvert[1];
                    var sec = timeconvert[2];


                    var nowdt = new Date();

                    var nowdate = (nowdt.getMonth() + 1) + '/' + nowdt.getDate() + '/' + nowdt.getFullYear();
                    //var nowdate = nowdt.getDate() + '-' + month + '-' + nowdt.getFullYear();

                    //var nowdate= (nowdt.getMonth() + 1) + '/' + nowdt.getDate() + '/' + nowdt.getFullYear();
                    //var nowtime = nowdt.getHours() + ":" + nowdt.getMinutes() + ":" + nowdt.getSeconds();
                    //var ampm = (nowdt.getHours() >= 12) ? "PM" : "AM";
                    var msgdate = '';



                    if (datedb == nowdate) {
                        //alert('datedb '+ datedb);
                        //alert('nowdate '+ nowdate);
                        msgdate = timedb;

                        //alert(hrs);
                        if (hrs > 12) {
                            hrs = hrs - 12;
                            //sec="AM";
                        }
                        else {
                            //sec="PM";

                        }
                        sec = ampmdb;

                        if (hrs <= 9) { hrs = "0" + hrs; }
                        if (min <= 9) { min = "0" + min; }

                        msgdate = hrs.slice(-2) + ':' + min.slice(-2) + ' ' + sec;

                        //msgdate=hrs+':'+min+' '+sec;
                        //alert("msdate"+msgdate);
                    }
                    else {
                        msgdate = datedb;
                    }

                    $('#date' + res.MessageId).text(msgdate);
                    $('#LatestMsg' + res.MessageId).text(res.BrokerMessage);



                });
                if (MessageDetails.length > 0) {
                    $.each(messagelist, function (idx, value) {
                        if ($.inArray(value, UnreadMsgArray) == -1) {

                            //alert('value '+value);
                            //TodeleteList.push(value);

                            //jQuery('#Count' + value).removeClass('circlechatcountOnMessage');
                            //jQuery('#date' + value).removeClass('UnreadMsgdate');
                            //$('#Count' + value).text('');

                        }
                    });
                }

                if (UnreadMsgCnt.length > 0) {
                    $.each(UnreadMsgCnt, function (i, res) {



                        if (res.UnreadMsgCnt > 0) {
                            //jQuery('#totalunreadcount').addClass('circlechatcountheader');
                            //$('#totalunreadcount').text(res.UnreadMsgCnt);


                        }
                        else {
                            //jQuery('#totalunreadcount').removeClass('circlechatcountheader');
                            //$('#totalunreadcount').text('');


                        }

                    });
                }
                else {
                    //jQuery('#totalunreadcount').removeClass('circlechatcountheader');
                    //$('#totalunreadcount').text('');
                }


                //totalUnreadMsgGlobleBroker=totalunreadconver;
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert('interval 2 error: ' + e.msg);
        }
    });
}, 2000);

/*get latest message*/


//**********************************************************************************************************
//***************** Regarding Chat Message Functionality ***************************************************


function getChatMessageCustomer(BrokerMessageId, CustomerMessageId, BrokerId, CustomerId, BrokerName, Message, MsgDate, profileimg) {
    //alert("Here - " + BrokerId+" , "+BrokerMessageId)
    SetIsReadFlag("", BrokerId, BrokerMessageId);
    //alert("Here - " + CustomerMessageId)
    //If GlobalMsgClicked contains the same message Id as of CustomerMessageId then do not allow to load custome chat messages again otherwise clear current chat messages and then load new chat messages of selected customer message.

    if (GlobalMsgClicked != CustomerMessageId) {

        GlobalMsgClicked = CustomerMessageId;
        if (window.screen.width >= 320 && window.screen.width <= 1024) {
            $('#customerChatMessage').css("display", "block");
            $('#customerMeassage').css("display", "none");
            $('#list').css("display", "none");
            $('#Edit').css("display", "none");
            $('#textenter').css("display", "block");
            $('#backdiv').css("display", "block");
        }
        else {
            $('#customerChatMessage').css("display", "block");
            $('#customerMeassage').css("display", "block");
            $('#list').css("display", "block");
            $('#Edit').css("display", "block");
            $('#textenter').css("display", "block");
            $('#divTextArea').css("display", "block");
        }

        if (messagelist.length > 0) {
            for (var i = 0; i < messagelist.length; i++) {
                // alert(messagelist[i]);
                $("#MessageId" + messagelist[i]).css("border-right", "1px solid rgb(226,226,226)");
            }
        }
        $("#MessageId" + CustomerMessageId).css("border-right", "none");
        $('#brokerChatcontent').val('');
        $('#brokerChatcontent').focus();
        /*Clear Main Messages checkboxes if clicked*/
        //alert("Here");
        if (deletemsgarrayCustomer.length > 0) {
            for (var j = 0; j <= deletemsgarrayCustomer.length; j++) {
                if (deletemsgarrayCustomer.length > 0) {
                    //alert("deletemsgarrayCustomer on click of div -" + deletemsgarrayCustomer + " ,j -" + j);
                    removeBrokerTick(deletemsgarrayCustomer[j]);
                    j = -1;
                }
                else {
                    break;
                }
            }
        }
        deletemsgarrayCustomer = [];
        $('.tickimg').css({ display: "none" });
        editclick = "false";
        $("#Editmainmsg").show();
        $("#deletemultiplemainmsg").hide();

        /*End of Clear Main Messages checkboxes if clicked*/

        TopBrokerMessageId = BrokerMessageId;
        TopCustomerMessageId = CustomerMessageId;
        TopBrokerId = BrokerId;
        TopCustomerId = CustomerId;
        TopBrokerName = BrokerName;
        TopMessage = Message;
        TopBrokerProfilePic = profileimg;

        $("div#divTextArea").show();
        $("div#divCustMsgDeleted").hide();
        $("#customerChatMessage").html("");

        /*Get DeviceId of Broker*/
        $.ajax({
            type: "POST",
            url: webserviceURL,
            data: { UserId: CustomerId, ActionName: "DoGetDeviceId" },
            success: function (data) {
                //alert('data '+data);
                var obj = JSON.parse(data);

                var issuccess = obj.IsSuccess;
                var ErrorMessage = obj.ErrorMessage;

                if (issuccess == true) {
                    var objDeviceid = obj.DeviceId;
                    $.each(objDeviceid, function (i, res) {
                        if (res.DeviceId != '') {
                            //alert("In If -" + res.DeviceId);
                            DeviceId = res.DeviceId;
                        }
                        else {
                            //alert("In Else -" + res.DeviceId);
                            DeviceId = "";
                        }
                        //alert(DeviceId);
                    });
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

            }
        });
        /*End of Get DeviceId of Broker*/

        /* Get main message for login Customer*/

        //alert("Main Message - " + webserviceURL);
        var chat = '';
        $.ajax({
            type: "POST",
            url: webserviceURL,
            data: { BrokerId: BrokerId, CustomerId: CustomerId, BrokerMessageId: BrokerMessageId, CustomerMessageId: CustomerMessageId, TimeStamp: "", TableName: "BrokerMessages", ActionName: "DoGetMainMessage" },

            success: function (data) {
                var obj = JSON.parse(data);

                var issuccess = obj.IsSuccess;
                var ErrorMessage = obj.ErrorMessage;

                if (issuccess == true) {

                    var MainMessage = obj.MainMessageDetails;
                    if (parseInt(MainMessage.length) > 0) {
                        /*If Main message is available*/
                        $.each(MainMessage, function (i, res) {
                            chat = '<div class="col-md-12 col-sm-12 col-xs-12 me" style="margin-top: 15px;">'
                                + '<img style="height: 38px;" src="' + CustomerProfilePic + '" width=32 height=32 />'
                                + '<div class="message"><p>' + res.Message
                                    + '</p></div></div> <div class="cleardiv"></div>';
                            $('#customerChatMessage').append(chat);

                        });
                    }

                }
            },
            error: function (e) {
                //alert('GetCustomerMessages error: ' + e.msg);
            }
        });

        /* Get chat messages(conversation) for login Customer*/
        var listforread = [];
        ChatMessageList = [];
        setTimeout(function () {
            $.ajax({
                type: "POST",
                url: webserviceURL,
                data: { UserId: BrokerId, TimeStamp: "", MessageId: BrokerMessageId, ActionName: "DoGetChatMessagesByMessageId" },
                success: function (data) {

                    var obj = JSON.parse(data);
                    var issuccess = obj.IsSuccess;
                    var ErrorMessage = obj.ErrorMessage;

                    if (issuccess == true) {

                        var chatMessage = obj.ChatMessages;
                        var IsMessageDeleted = obj.IsMessageDeleted;

                        $.each(chatMessage, function (i, res) {
                            LoadedMessagesId.push(parseInt(res.ChatId));
                            var dt = res.LocalDateTime;
                            var dt1 = dt.split(' ');
                            var datedb = dt1[0];
                            var timedb = dt1[1];
                            var ampmdb = dt1[2];
                            msgdate = datedb;

                            var nowdt = new Date();
                            var nowdate = (nowdt.getMonth() + 1) + '/' + nowdt.getDate() + '/' + nowdt.getFullYear();
                            var msgdate = '';

                            if (datedb == nowdate) {
                                msgdate = timedb;

                                var timeconvert = timedb.split(':');
                                var hrs = timeconvert[0];
                                var min = timeconvert[1];
                                var sec = timeconvert[2];
                                //alert(hrs);
                                if (hrs > 12) {
                                    hrs = hrs - 12;
                                    //sec="AM";
                                }
                                else if (hrs == 0) {
                                    hrs = hrs + 12;
                                    //sec="PM";
                                }
                                sec = ampmdb;

                                //msgdate=hrs+':'+min+' '+sec;
                                if (hrs <= 9) { hrs = "0" + hrs; }
                                if (min <= 9) { min = "0" + min; }

                                msgdate = hrs.slice(-2) + ':' + min.slice(-2) + ' ' + sec;
                                //alert("chatmsdate"+msgdate);
                            }
                            else {
                                msgdate = datedb;
                            }
                            if (res.BrokerMessage != '') {

                                //$('#customerChatMessage').append('<a id="link' + res.ChatId + '" data-transition="slide" href="#" onclick="ClickonChat(\'' + res.ChatId +
                                //  '\')"><div id="Chat' + res.ChatId + '" class="col-md-12 col-sm-12 col-xs-12 me">'
                                //  + '<img src="' + CustomerProfilePic + '" width=32 height=32 />'
                                //  + '<div class="message"><p>' + res.CustomerMessage + '</p></div></div></a><div class="cleardiv"></div>');

                                $('#customerChatMessage').append('<div style="margin-bottom: 2%;" id="Chat' + res.ChatId + '" class="col-md-12 col-sm-12 col-xs-12 me">'
                                + '<img style="height: 38px;" src="' + CustomerProfilePic + '" width=32 height=32 />'
                                + '<div class="message" style="margin-top: 10px;word-wrap: break-word;"><p>' + res.BrokerMessage + '</p></div></div><div class="cleardiv"></div>');


                            }
                            else {
                                if (res.IsRead == 'False') {
                                    listforread.push(parseInt(res.ChatId));
                                }

                                //$('#customerChatMessage').append('<a id="link' + res.ChatId + '" data-transition="slide" href="#" onclick="ClickonChat(\'' + res.ChatId +
                                // '\')"><div id="Chat' + res.ChatId + '" class="col-md-12 col-sm-12 col-xs-12 friend">'
                                // + '<img src="' + TopBrokerProfilePic + '" width=32 height=32 />'
                                // + '<div class="message"><p>' + res.BrokerMessage + '</p></div></div></a><div class="cleardiv"></div>');

                                $('#customerChatMessage').append('<div style="margin-bottom: 2%;" id="Chat' + res.ChatId + '" class="col-md-12 col-sm-12 col-xs-12 friend">'
                             + '<img style="height: 38px;" src="' + TopBrokerProfilePic + '" width=32 height=32 />'
                             + '<div class="message" style="margin-top: 10px;word-wrap: break-word;"><p>' + res.CustomerMessage + '</p></div></div><div class="cleardiv"></div>');
                            }
                        });

                        $.each(IsMessageDeleted, function (i, res) {
                            IsDeletedonBroker1 = res.IsDeleted;
                            //alert("Deleted - " + res.IsDeleted);
                            if (res.IsDeleted == 'True') {
                                //alert("1");
                                $("#brokerChatcontentError").text('You can not send message because customer is no longer available for this chat.');
                                $("div#divTextArea").hide();
                                //$("#brokerChatcontentError").css("font-weight", "bold");
                                $("div#divCustMsgDeleted").show();
                                $("#brokerChatcontentError").attr("disabled", "disabled");

                            }
                            else {
                                $("div#divTextArea").show();
                                $("div#divCustMsgDeleted").hide();

                            }

                        });

                        //Set IsRead=true for unread messages
                        //alert("listforread outer- " + listforread);
                        if (parseInt(listforread.length) > 0) {
                            //alert("listforread - " + listforread);
                            SetIsReadFlag(listforread, BrokerId, BrokerMessageId);
                        }
                        else {
                            // GetChatFlag = 'Yes';
                        }

                    }
                    else if (issuccess == false) {

                        //GetChatFlag = 'Yes';

                        //$("#lblError").text(ErrorMessage);
                        //$("#ModalError").modal();


                    }
                    else {
                        //$("#lblError").text('Please Try again !');
                        //$("#ModalError").modal();
                    }

                    //$('#brokerChatcontent').val('');
                    //$('#brokerChatcontent').focus();
                    // var chatBoxheight1 = $("#customerChatMessage")[0].scrollHeight;
                    //alert(chatBoxheight1);
                    ScrollDown();

                },

                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    if (errorThrown != "") {

                        //$("#lblError").text('We are unable to connect the server. Please try again later. !');
                        //$("#ModalError").modal();
                    }
                    else {
                    }
                }
            });
        }, 600);
    }
    /* End of Get chat messages(conversation) for login Customer*/

    $('#loadMessages').hide();
    $('.bodyMessages').removeClass('bodyblur');
    //alert("Here 1");
}

function ScrollDown() {
    var obj = document.getElementById("customerChatMessage");
    obj.scrollTop = obj.scrollHeight;
}

/* Set IsRead=true flag for unread messages*/
function SetIsReadFlag(listforread, UserId, MainMessageId) {
    //alert('Called :' + UserId + ' ' + MainMessageId + '' + listforread);
    var list = listforread.toString();
    //alert('array :' + list + " webServiceUrl - " + webServiceUrl);

    $.ajax({
        type: "POST",
        url: webserviceURL,
        data: { MessageId: list, UserId: UserId, MainMessageId: MainMessageId, ActionName: "DoSetIsRead" },
        success: function (data) {

            var obj = JSON.parse(data);
            var issuccess = obj.IsSuccess;
            var ErrorMessage = obj.ErrorMessage;
            if (issuccess == true) {
                //GetChatFlag = 'Yes';
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        }
    });
}
/* End of Set IsRead=true flag for unread messages*/

MyVarBrokerMsg = setInterval(function () {
    $.ajax({
        type: "POST",
        url: webserviceURL,
        data: { UserId: TopBrokerId, TimeStamp: "", MessageId: TopBrokerMessageId, ActionName: "DoGetChatMessagesByMessageId" },
        success: function (data) {
            //alert('data load '+data);
            var obj = JSON.parse(data);
            var issuccess = obj.IsSuccess;
            var ErrorMessage = obj.ErrorMessage;

            if (issuccess == true) {
                NewLoadingMessagesId = [];
                var chatMessage = obj.ChatMessages;
                var IsMessageDeleted = obj.IsMessageDeleted;

                $.each(chatMessage, function (i, res) {
                    NewLoadingMessagesId.push(parseInt(res.ChatId));
                });


                var DeleteMsgId = [];
                // alert("New - " + NewLoadingMessagesId + ", Delete - " + DeleteMsgId + ", Old - " + LoadedMessagesId);

                //alert("LoadedMessagesId - " + LoadedMessagesId);
                //alert("NewLoadingMessagesId - " + NewLoadingMessagesId);

                $.grep(LoadedMessagesId, function (el) {

                    if ($.inArray(el, NewLoadingMessagesId) == -1) {
                        DeleteMsgId.push(el);
                    }
                });

                if (DeleteMsgId.length > 0) {

                    for (i = 0; i <= DeleteMsgId.length; i++) {
                        // alert("DeleteMsgId.length - " + DeleteMsgId[i]);
                        $('Chat#' + DeleteMsgId[i]).remove();
                    }
                }
                LoadedMessagesId = [];
                for (var j = 0; j < NewLoadingMessagesId.length; j++) {
                    LoadedMessagesId.push(NewLoadingMessagesId[j]);
                }
            }
            else if (issuccess == false) {

                //GetChatFlag = 'Yes';

                //$("#lblError").text(ErrorMessage);
                //$("#ModalError").modal();


            }
            else {
                //$("#lblError").text('Please Try again !');
                //$("#ModalError").modal();
            }

            //$('#brokerChatcontent').val('');
            //$('#brokerChatcontent').focus();
            //var chatBoxheight1 = $("#customerChatMessage")[0].scrollHeight;
            //ScrollDown(chatBoxheight1);

        },

        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert("here");
            if (errorThrown != "") {
                if (IsNetworkAvailable == "false") {
                    // $("#lblError").text('We are unable to connect the server. Please try again later. !');
                    //$("#ModalError").modal();
                    IsNetworkAvailable = "true";
                }
            }
            else {
            }
        }
    });
}, 4000);

MyVarBroker = setInterval(function () {

    var SetUnreadArrayInterval = [];
    //alert('Interval Called ');
    //if (GetChatFlag == 'Yes') {
    //    GetChatFlag = 'No';

    //alert("TopBrokerId - " + TopBrokerId + " MessageId - " + TopBrokerMessageId)

    $.ajax({
        type: "POST",
        url: webserviceURL,
        data: { UserId: TopBrokerId, MessageId: TopBrokerMessageId, ActionName: "DoGetUnreadChatMessages" },
        success: function (data) {

            //alert('Interval responce :'+ data);
            var obj = JSON.parse(data);
            var issuccess = obj.IsSuccess;
            var ErrorMessage = obj.ErrorMessage;
            //alert(JSON.stringify(obj));
            if (issuccess == true) {
                // alert('IN Success ajax: ' + GetChatFlag);
                var chatMessage = obj.ChatMessages;
                $.each(chatMessage, function (i, res) {

                    //alert('IN Each: ' + GetChatFlag);

                    //if (chatlist.indexOf(res.ChatId) >= 0) {
                    //    SetUnreadArrayInterval.push(parseInt(res.ChatId));
                    //}
                    //else {
                    var msgdate1 = '';
                    //var dt = res.MessageDate;
                    var dt = res.LocalDateTime;
                    var dt1 = dt.split(' ');
                    var datedb = dt1[0];
                    var timedb = dt1[1];
                    var ampmdb = dt1[2];
                    msgdate1 = datedb;

                    var nowdt = new Date();
                    var nowdate = (nowdt.getMonth() + 1) + '/' + nowdt.getDate() + '/' + nowdt.getFullYear();

                    var msgdate1 = '';


                    if (datedb == nowdate) {

                        msgdate1 = timedb;

                        var timeconvert = timedb.split(':');
                        var hrs = timeconvert[0];
                        var min = timeconvert[1];
                        var sec = timeconvert[2];
                        //alert(hrs);
                        if (hrs > 12) {
                            hrs = hrs - 12;
                            //sec="AM";
                        }
                        else if (hrs == 0) {
                            hrs = hrs + 12;
                            //sec="PM";
                        }
                        sec = ampmdb;
                        //msgdate1=hrs+':'+min+' '+sec;
                        if (hrs <= 9) { hrs = "0" + hrs; }
                        msgdate1 = hrs + ':' + min + ' ' + sec; if (min <= 9) { min = "0" + min; }
                        msgdate1 = hrs.slice(-2) + ':' + min.slice(-2) + ' ' + sec;
                        //msgdate1=hrs+':'+min+' '+sec;
                        //alert("msdate2"+msgdate2);
                    }
                    else {
                        msgdate1 = datedb;
                    }

                    SetUnreadArrayInterval.push(parseInt(res.ChatId));

                    //$('#customerChatMessage').append('<a id="link' + res.ChatId + '" data-transition="slide" href="#" onclick="ClickonChat(\'' + res.ChatId +
                    // '\')"><div id="Chat' + res.ChatId + '" class="col-md-12 col-sm-12 col-xs-12 friend">'
                    // + '<img src="' + TopBrokerProfilePic + '" width=32 height=32 />'
                    // + '<div class="message"><p>' + res.BrokerMessage + '</p></div></div></a><div class="cleardiv"></div>');

                    $('#customerChatMessage').append('<div style="margin-bottom: 2%;" id="Chat' + res.ChatId + '" class="col-md-12 col-sm-12 col-xs-12 friend">'
                   + '<img style="height: 38px;" src="' + TopBrokerProfilePic + '" width=32 height=32 />'
                   + '<div class="message" style="margin-top: 10px;word-wrap: break-word;"><p>' + res.CustomerMessage + '</p></div></div><div class="cleardiv"></div>');

                    //  var chatBoxheight1 = $("#customerChatMessage")[0].scrollHeight;
                    ScrollDown();
                });

                var IsMessageDeleted = obj.IsMessageDeleted;
                

                $.each(IsMessageDeleted, function (i, res) {
                    var IsDeletedonBroker2 = res.IsDeleted;

                    if (res.IsDeleted == 'True') {
                      //alert("2");
                        $("#brokerChatcontentError").text('You can not send message because customer is no longer available for this chat.');
                        $("div#divTextArea").hide();
                        //$("#brokerChatcontentError").css("font-weight", "bold");
                        $("div#divCustMsgDeleted").show();
                        $("#brokerChatcontentError").attr("disabled", "disabled");

                    }
                    else {
                        $("div#divTextArea").show();
                        $("div#divCustMsgDeleted").hide();
                    }
                });

                if (parseInt(SetUnreadArrayInterval.length) > 0) {
                    //alert("After - -- " + SetUnreadArrayInterval + " , " + TopCustomerId);
                    SetIsReadFlag(SetUnreadArrayInterval, TopBrokerId, "");
                }
                else {
                    // GetChatFlag = 'Yes';
                }
                //Old Here
            }
            else {
                GetChatFlag = 'Yes';
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        }
    });
    //}
    //else {
    //}
    //var chatBoxheight1 = $("#chat1")[0].scrollHeight;
    //alert(chatBoxheight1);
    //ScrollDown(chatBoxheight1);
    // var chatBoxheight1 = $("#customerChatMessage")[0].scrollHeight;
    //ScrollDown(chatBoxheight1);

}, 4000);

/**************************** Save Customer Chat Message *************************************/

function SaveCustomerChat(chatcontent) {
    //alert("Called " + chatcontent + "," + webserviceURL + ", CustomerMessageId -" + TopCustomerMessageId);

    var msgdate2 = '';

    var nowdt = new Date();

    var onlydate = (nowdt.getMonth() + 1) + '/' + nowdt.getDate() + '/' + nowdt.getFullYear();
    var nowtime = nowdt.getHours() + ":" + nowdt.getMinutes() + ":" + nowdt.getSeconds();

    var sec = '';
    var hrs = nowdt.getHours();

    if (hrs > 12) {
        hrs = hrs - 12;
        sec = "PM";

    }
    else if (hrs == 12) {
        sec = "PM";
    }
    else {
        sec = "AM";
    }

    var LocalDateTime = onlydate + ' ' + nowtime + ' ' + sec;


    $('#customerChatMessage').append('<div id="Chat" class="col-md-12 col-sm-12 col-xs-12 me" style="margin-bottom:5px;">'
              + '<img style="height: 38px;" src="' + CustomerProfilePic + '" width=32 height=32 />'
              + '<div class="message" style="margin-top: 10px;word-wrap: break-word;"><p>' + chatcontent + '</p>'
              //+ '<p id="Send' + SubmitChatCount + '" style="font-size: 10px;margin-top: -20px !important;text-align: right;">Sending...</p>'
              + '</div></div><div class="cleardiv"></div>');

    setTimeout(function () {
        ScrollDown();
    }, 1000);

    //alert("TopCustomerMessageId - " + TopCustomerMessageId)
  
    $.ajax({
        type: "POST",
        url: webserviceURL,
        data: { BrokerMsgId: TopBrokerMessageId, CustomerMsgId: TopCustomerMessageId, CustomerId: TopCustomerId, BrokerId: TopBrokerId, BrokerMessage: chatcontent, OldMessageId: 0, LocalDateTime: LocalDateTime, ActionName: "DoSaveBrokerChat" },
        success: function (data) {
            //alert('data '+data);
            var obj = JSON.parse(data);
            var issuccess = obj.IsSuccess;
            var ErrorMessage = obj.ErrorMessage;

            if (issuccess == true) {
                var response = obj.Response;
                var listforread = [];
                //alert("Success");
                $.each(response, function (i, res) {

                   
                    //$("#Send" + SubmitChatCount).remove();

                    /*************** Notification Code ******************/
                    var ActionName = '';
                    var NewDevice = '';

                   // if (DeviceId != '') {
                        //var indexid = DeviceId.indexOf('Android');

                        //var indexid1 = DeviceId.indexOf('iOS');

                        //if (parseInt(indexid) >= 0) {

                        //    ActionName = 'DoPushNotification';
                        //    NewDevice = DeviceId.replace('Android', '');
                        //}
                        //else if (parseInt(indexid1) >= 0) {

                        //    ActionName = 'DoPushNotificationForiOS';
                        //    NewDevice = DeviceId.replace('iOS', '');
                        //}

                        //var title = FirstName + ' ' + LastName;
                        
                        var title = CustomerName;
                        var msgcnt = 1;
                        var message = chatcontent;
                        $.ajax({
                            type: "POST",
                            url: webserviceURL,
                            data: { message: message, title: title, msgcnt: msgcnt, UserId: TopCustomerId, ActionName: "DoSendNotification" },
                            success: function (data) {
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                            }
                        });

                    //}
                    //else {
                    //   // alert("CustomerName - " + CustomerName);
                    //}

                    /**************** End of Notofication Code ***********/

                    //var dt = res.LocalDateTime;
                    //var dt1 = dt.split(' ');
                    //var datedb = dt1[0];
                    //var timedb = dt1[1];
                    //var ampmdb = dt1[2];
                    //msgdate2 = datedb;

                    //var nowdt = new Date();
                    //var nowdate = (nowdt.getMonth() + 1) + '/' + nowdt.getDate() + '/' + nowdt.getFullYear();

                    //var msgdate2 = '';
                    //if (datedb == nowdate) {
                    //    msgdate2 = timedb;
                    //    var timeconvert = timedb.split(':');
                    //    var hrs = timeconvert[0];
                    //    var min = timeconvert[1];
                    //    var sec = timeconvert[2];
                       
                    //    if (hrs > 12) {
                    //        hrs = hrs - 12;                           
                    //    }
                    //    else if (hrs == 0) {
                    //        hrs = hrs + 12;                           
                    //    }
                    //    sec = ampmdb;
                      
                    //    if (hrs <= 9) { hrs = "0" + hrs; }
                    //    msgdate2 = hrs + ':' + min + ' ' + sec; if (min <= 9) { min = "0" + min; }
                    //    msgdate2 = hrs.slice(-2) + ':' + min.slice(-2) + ' ' + sec;
                    //}
                    //else {
                    //    msgdate2 = datedb;
                    //}

                    SubmitChatCount++;
                });
            }
            else if (issuccess == false) {
                $("#lblError").text('Please Try again !');
                $("#ModalError").modal();
            }
            else {
                $("#lblError").text('Please Try again !');
                $("#ModalError").modal();
            }

        },

        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (errorThrown != "") {
                //alert("here");
                if (IsNetworkAvailable == "false") {
                    // $("#lblError").text('We are unable to connect the server. Please try again later.');
                    //$("#ModalError").modal();
                    IsNetworkAvailable = "true";
                }
            }
        }
    });


}

function ClickonChat(ChatId) {
    //alert("Called ChatIdToDelete -"+ChatIdToDelete);
    if (jQuery.inArray(ChatId, ChatIdToDelete) != -1) {
        //alert("In Array");
        ChatIdToDelete.splice(ChatIdToDelete.indexOf(ChatId), 1);
        $("#link" + ChatId).removeClass("RemoveElt");
        //alert("Length - " + ChatIdToDelete.length);
        if (ChatIdToDelete.length == 0) {

            $("#deletemultiplechatmsg").css("display", "none");
        }
        else {

            $("#deletemultiplechatmsg").css("display", "block");
        }
    }
    else {
        //alert("Not In Array");
        ChatIdToDelete.push(ChatId);
        $("#link" + ChatId).addClass("RemoveElt");
        $("#deletemultiplechatmsg").css("display", "block");
    }
    //alert(ChatIdToDelete);
}

function CancelDeleteChatMessages() {
    for (i = 0; i <= ChatIdToDelete.length; i++) {
        $("#link" + ChatIdToDelete[i]).removeClass("RemoveElt");
    }
    ChatIdToDelete = [];
}

function DeleteChatMessages() {
    //$('#loadMessages').show();
    //$('.bodyMessages').addClass('bodyblur');

    var listtodelete = ChatIdToDelete.toString();
    //alert("Called -" + listtodelete + " TopCustomerId - " + TopCustomerId);
    $.ajax({
        type: "POST",
        url: webserviceURL,
        data: { UserId: TopBrokerId, MessageId: listtodelete, ActionName: "DoDeleteMultipleChatMessage" },
        success: function (data) {

            var obj = JSON.parse(data);
            var issuccess = obj.IsSuccess;
            var ErrorMessage = obj.ErrorMessage;
            if (issuccess == true) {
                //alert("Success");
                for (i = 0; i <= ChatIdToDelete.length; i++) {

                    $('#Chat' + ChatIdToDelete[i]).remove();
                }

                ChatIdToDelete = [];

            }
            else if (issuccess == false) {
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        }
    });
    //$('#loadMessages').hide();
    //$('.bodyMessages').removeClass('bodyblur');
}

/********************** Functions for deleting main messages *****************************/
//Functions for deleting main messages

$(function () {
    $("#Editmainmsg").click(function (e) {
        //alert("deletemsgarrayCustomer - " + deletemsgarrayCustomer);
        if (editclick == "false") {
            $('.tickimg').css({ display: "block" });
            editclick = "true";
            //alert('called ' + editclick);
        }
        else {
            $('.tickimg').css({ display: "none" });
            editclick = "false";
            //alert('called ' + editclick);
        }
    });

    $("#deletemultiplemainmsg").click(function (e) {
        //alert("Called");
        $("#myModalMessage").modal('show');
    });

    $("#deletemultiplemessagesconfirm").click(function (e) {
        var listtodeletemainmsg = deletemsgarrayCustomer.toString();
        //alert("Called - " + listtodeletemainmsg + ", TopCustomerId -" + TopCustomerId);

        $.ajax({
            type: "POST",
            url: webserviceURL,
            data: { UserId: TopBrokerId, MessageId: listtodeletemainmsg, ActionName: "DoDeleteMultipleMessage" },
            success: function (data) {

                var obj = JSON.parse(data);
                var issuccess = obj.IsSuccess;
                var ErrorMessage = obj.ErrorMessage;
                if (issuccess == true) {

                    $('#Editmainmsg').hide();
                    // $('#brokerSearchbtn').show();

                    $("#deletemultiplemainmsg").hide();
                    $("#Editmainmsg").show();
                    $('.tickimg').css({ display: "none" });
                    editclick = "false";

                    $('#myModalMessage').modal('hide');
                    //alert(messagelist);
                    for (i = 0; i <= deletemsgarrayCustomer.length; i++) {

                        $('#MessageId' + deletemsgarrayCustomer[i]).remove();

                        messagelist = jQuery.grep(messagelist, function (value) {
                            return value != deletemsgarrayCustomer[i];
                        });
                    }
                    deletemsgarrayCustomer = [];
                    //alert(messagelist);

                    if (messagelist.length <= 0) {
                        messagelist = [];
                        deletemsgarrayCustomer = [];
                        ChatMessageList = [];
                        ChatIdToDelete = [];
                        LoadedMessagesId = [];
                        NewLoadingMessagesId = [];
                        GlobalMsgClicked = '';
                        $('#Nocontent').text('No Messages Found !');
                    }
                    else {
                        /*If we delete main messages then show the top most message again*/
                        for (p = 0; p <= messagelist.length; p++) {
                            $("#MessageId" + messagelist[p]).css("border-bottom", "none");
                            $("#MessageId" + messagelist[p]).css("border-right", "none");
                        }
                        jQuery('#customerMeassage div').html('');
                        messagelist = [];
                        deletemsgarrayCustomer = [];
                        ChatMessageList = [];
                        ChatIdToDelete = [];
                        LoadedMessagesId = [];
                        NewLoadingMessagesId = [];
                        GlobalMsgClicked = '';
                        GetCustomerMessages(GlobleUserIDCustomer, webserviceURL, CustomerProfilePic);
                    }

                }
                else if (issuccess == false) {
                    $("#deletemultiplemainmsg").hide();//16Mar17
                    $("#Editmainmsg").show();//16Mar17
                    $('.tickimg').css({ display: "none" });//16Mar17
                    editclick = "false";

                    $('#myModalMessage').modal('hide');

                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $('#myModalMessage').modal('hide');
            }
        });

    });
});

function getCustomerDeletedItems(id) {

    //alert(id);

    deletemsgarrayCustomer.push(id);
    //alert("deletemsgarrayCustomer -"+deletemsgarrayCustomer);
    if (parseInt(deletemsgarrayCustomer.length) > 0) {

        if (deletemsgarrayCustomer.indexOf(id) >= 0) {

            $('#tick' + id).attr('src', '../Images/Assets/radioBtn_checked.png');
            $("#Editmainmsg").hide();
            $("#deletemultiplemainmsg").show();

            //  alert('len1 ' + deletemsgarrayCustomer.length);

            for (var i = 0; i < deletemsgarrayCustomer.length; i++) {
                for (var j = 0; j < deletemsgarrayCustomer.length; j++) {
                    if (i != j) {
                        if (deletemsgarrayCustomer[i] == deletemsgarrayCustomer[j]) {

                            // alert('remove fun ' + deletemsgarrayCustomer[i]);
                            removeBrokerTick(deletemsgarrayCustomer[i]);

                        }
                    }
                }
            }
        }
    }
}

function removeBrokerTick(removeid) {

    //alert('remove - ' + removeid);
    $('#tick' + removeid).attr('src', '../Images/Assets/redioButton.png');
    //  alert(deletemsgarrayCustomer.length);
    deletemsgarrayCustomer = jQuery.grep(deletemsgarrayCustomer, function (value) {
        return value != removeid;
    });

    //alert("After Remove -" + deletemsgarrayCustomer);
    if (parseInt(deletemsgarrayCustomer.length) <= 0) {
        //alert(deletemsgarrayCustomer.length);
        $('.tickimg').css({ display: "none" });
        $("#Editmainmsg").show();
        $("#deletemultiplemainmsg").hide();
        editclick = "false";
    }
}




