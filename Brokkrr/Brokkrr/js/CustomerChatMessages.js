var webServiceUrl = "";
var BrokerMessageId = "";
var CustomerMessageId = "";
var BrokerId = "";
var CustomerId = "";
var BrokerName = "";
var MainMessage = "";
var GetChatFlag = "No";
var submitChatCount = 1;
var MyVarBroker = "";
var MainMsgIsRead = "";
var DeviceId = "";
var FirstName = "";
var LastName = "";
var ChatIdToDelete = [];
var LocalDateTime = "";

var LoadedMessagesId = [];
var NewLoadingMessagesId = [];
var MyVarBrokerMsg = "";

function pageBeforeLoad(webServiceUrlfromView, BrokerMessageId1, CustomerMessageId1, BrokerId1, CustomerId1, CustomerName1, MainMessage1) {
    webServiceUrl = webServiceUrlfromView;
    BrokerMessageId = BrokerMessageId1;
    CustomerMessageId = CustomerMessageId1;
    BrokerId = BrokerId1;
    CustomerId = CustomerId1;
    BrokerName = CustomerName1;


    $.ajax({
        type: "POST",
        url: webServiceUrl,
        data: { UserId: CustomerId, MessageId: CustomerMessageId, ActionName: "DoGetUnreadChatMessages" },
        success: function (data) {

            //alert('Interval responce :'+ data);
            var obj = JSON.parse(data);
            var issuccess = obj.IsSuccess;
            var ErrorMessage = obj.ErrorMessage;

            if (issuccess == true) {
                var IsMessageDeleted = obj.IsMessageDeleted;

                $.each(IsMessageDeleted, function (i, res) {
                    var IsDeletedonBroker2 = res.IsDeleted;
                    // alert('In pageBeforeLoad : ' + res.IsDeleted);
                    if (res.IsDeleted == 'True') {
                        //$("#brokerChatcontentError").val('You can not send message because broker is no longer available for this chat.');
                        $("#brokerChatcontentError").text('You can not send message because broker is no longer available for this chat.');
                        $("div#divTextArea").hide();
                        $("#brokerChatcontentError").css("font-weight", "bold");
                        $("div#divCustMsgDeleted").show();
                        $("#brokerChatcontentError").attr("disabled", "disabled");
                    }
                    else {
                        $("div#divTextArea").show();
                        $("div#divCustMsgDeleted").hide();
                    }

                });
            }
            else {
                GetChatFlag = 'Yes';
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        }
    });
    $('#brokerChatcontent').val('');
    $('#brokerChatcontent').focus();
    //alert("end of pageBeforeLoad");
}


//Get webServiceUrl to make database connection
function getWebServiceURL(webServiceUrlfromView, BrokerMessageId1, CustomerMessageId1, BrokerId1, CustomerId1, CustomerName1, MainMessage1, IsRead1, FirstName1, LastName1, LocalDateTime1) {
    webServiceUrl = webServiceUrlfromView;
    BrokerMessageId = BrokerMessageId1;
    CustomerMessageId = CustomerMessageId1;
    BrokerId = BrokerId1;
    CustomerId = CustomerId1;
    BrokerName = CustomerName1;
    var msg = MainMessage1.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
    MainMessage = msg;
    MainMsgIsRead = IsRead1;
    FirstName = FirstName1;
    LastName = LastName1;
    LocalDateTime = LocalDateTime1;

    //alert("CustomerMessageId"+CustomerMessageId);

    /* Get Device Id of Customer*/

    $.ajax({
        type: "POST",
        url: webServiceUrl,
        data: { UserId: BrokerId, ActionName: "DoGetDeviceId" },
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
    /* End of Get Device Id of Customer*/


    $("#lblCustomerName").text(BrokerName);
    /*******************************************************************************************************/
    var dt = LocalDateTime;
    var dt1 = dt.split(' ');
    var datedb = dt1[0];
    var timedb = dt1[1];
    var ampmdb = dt1[2];
    var lmsgdate = '';
    lmsgdate = datedb;

    var nowdt = new Date();
    var nowdate = (nowdt.getMonth() + 1) + '/' + nowdt.getDate() + '/' + nowdt.getFullYear();
   


    if (datedb == nowdate) {
        lmsgdate = timedb;

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

        lmsgdate = hrs.slice(-2) + ':' + min.slice(-2) + ' ' + sec;
        //alert("chatmsdate"+msgdate);
    }
    else {
        lmsgdate = datedb;
    }

    /***********************************************************************************************/

    //var control = '<div><p class="speechright msgRight">' + MainMessage + ' <span class="msgRight msgdatetime" style="padding-top:5%;">&nbsp;&nbsp;' + CustomerId + '</span></p></div><div class="cleardiv"></div>';

    var control = '<div><p class="speechright msgRight">' + MainMessage +
        ' <span class="msgRight msgdatetime" style="padding-top:5%;">&nbsp;&nbsp;' + lmsgdate +
        '</span></p></div><div class="cleardiv"></div>';


    $("#chat1").append(control);

    var listforread = [];
    $.ajax({
        type: "POST",
        url: webServiceUrl,
        data: { UserId: CustomerId, TimeStamp: "", MessageId: CustomerMessageId, ActionName: "DoGetChatMessagesByMessageId" },
        success: function (data) {
            //alert('data load '+data);
            var obj = JSON.parse(data);
            var issuccess = obj.IsSuccess;
            var ErrorMessage = obj.ErrorMessage;

            if (issuccess == true) {
                //alert('IN Success ajax: '+GetChatFlag);
                var chatMessage = obj.ChatMessages;
                var IsMessageDeleted = obj.IsMessageDeleted;
                // alert(obj.ChatMessages);

                $.each(chatMessage, function (i, res) {
                    LoadedMessagesId.push(parseInt(res.ChatId));
                    var dt = res.LocalDateTime;
                    // alert(dt);
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


                    if (res.CustomerMessage != '') {

                        //$("#ulChatMessages").append('<li style="width:100%;" class="Chat" id="' + res.ChatId + '">' +
                        //    '<a id="link' + res.ChatId + '" href="#" onclick="ClickonChat(\'' + res.ChatId + '\')"' +
                        //        '<div class="msj-rta macro">' +
                        //        '<div class="text text-r">' +
                        //                '<p>' + res.BrokerMessage + '</p>' +
                        //                '<p style="float:left;"><small>' + msgdate + '</small></p>' +
                        //            '</div>' +
                        //            '</div>' +
                        //  '</a></li><div class="cleardiv"></div>');

                        //$("#chat1").append('<a id="link' + res.ChatId + '" data-transition="slide" href="#" onclick="ClickonChat(\'' + res.ChatId + '\')"><p id=' + res.ChatId + ' class="Chat speechright msgRight fontcolorchat">' + res.BrokerMessage + ' <span class="msgRight msgdatetime" style="padding-top:5%;">&nbsp;&nbsp;' + msgdate + '</span></p></a><div class="cleardiv"></div>')

                        $('#chat1').append('<a id="link' + res.ChatId + '" data-transition="slide" href="#" onclick="ClickonChat(\'' + res.ChatId +
                          '\')"><p id=' + res.ChatId + ' class="Chat speechright msgRight fontcolorchat">' + res.CustomerMessage +
                          ' <span class="msgRight msgdatetime fontcolorchat" style="padding-top:5%;">&nbsp;&nbsp;' + msgdate +
                          '</span></p></a><div class="cleardiv"></div>');
                    }
                    else {
                        if (res.IsRead == 'False') {
                            listforread.push(parseInt(res.ChatId));
                        }

                        //$("#ulChatMessages").append('<li style="width:100%" class="Chat" id="' + res.ChatId + '">' +
                        //    '<a id="link' + res.ChatId + '" href="#" onclick="ClickonChat(\'' + res.ChatId + '\')"' +
                        //        '<div class="msj macro">' +
                        //       '<div class="text text-l">' +
                        //               '<p>' + res.CustomerMessage + '</p>' +
                        //                '<p style="float:left;"><small>' + msgdate + '</small></p>' +
                        //            '</div>' +
                        //        '</div>' +
                        //    '</a></li><div class="cleardiv"></div>');

                        //$('#chat1').append('<a id="link' + res.ChatId + '" data-transition="slide" href="#" onclick="ClickonChat(\'' + res.ChatId + '\')"><p id=' + res.ChatId + ' class="Chat speechleft fontcolorchat">' + res.CustomerMessage + ' <span class="msgRight msgdatetime fontcolorchat" style="padding-top:5%;">&nbsp;&nbsp;' + msgdate + '</span></p></a>');


                        $('#chat1').append('<a id="link' + res.ChatId + '" data-transition="slide" href="#" onclick="ClickonChat(\'' + res.ChatId +
                           '\')"><p id=' + res.ChatId + ' class="Chat speech fontcolorchat"><span style="padding-left: 2px;">' + res.BrokerMessage +
                            '</span> <span class="msgRight msgdatetime fontcolorchat" style="padding-top:5%;">&nbsp;&nbsp;' + msgdate +
                            '</span></p></a>');
                    }
                });

                $.each(IsMessageDeleted, function (i, res) {
                    IsDeletedonBroker1 = res.IsDeleted;
                    //alert('In Main : '+IsDeletedonBroker1);
                    if (res.IsDeleted == 'True') {
                        //$("#brokerChatcontentError").val('You can not send message because broker is no longer available for this chat.');
                        $("#brokerChatcontentError").text('You can not send message because broker is no longer available for this chat.');
                        $("div#divTextArea").hide();
                        $("#brokerChatcontentError").css("font-weight", "bold");
                        $("div#divCustMsgDeleted").show();
                        $("#brokerChatcontentError").attr("disabled", "disabled");
                    }
                    else {
                        $("div#divTextArea").show();
                        $("div#divCustMsgDeleted").hide();
                    }

                });

                //Set IsRead=true for unread messages

                if (parseInt(listforread.length) > 0 || MainMsgIsRead == 'False') {
                    SetIsReadFlag(listforread, CustomerId, CustomerMessageId);
                }
                else {
                    GetChatFlag = 'Yes';
                }
                
            }
            else if (issuccess == false) {

                GetChatFlag = 'Yes';

                $("#lblError").text(ErrorMessage);
                $("#ModalError").modal();


            }
            else {
                $("#lblError").text('Please Try again !');
                $("#ModalError").modal();
            }

            $('#brokerChatcontent').val('');
            $('#brokerChatcontent').focus();
            var chatBoxheight1 = $("#chat1")[0].scrollHeight;
            ScrollDown(chatBoxheight1);

        },

        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (errorThrown != "") {

                $("#lblError").text('We are unable to connect the server. Please try again later. !');
                $("#ModalError").modal();
            }
            else {
            }
        }
    });

    //Get Deleted messages by interval

    MyVarBrokerMsg = setInterval(function () {
        $.ajax({
            type: "POST",
            url: webServiceUrl,
            data: { UserId: CustomerId, TimeStamp: "", MessageId: CustomerMessageId, ActionName: "DoGetChatMessagesByMessageId" },
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

                    $.grep(LoadedMessagesId, function (el) {

                        if ($.inArray(el, NewLoadingMessagesId) == -1) {
                            DeleteMsgId.push(el);
                        }
                    });

                    if (DeleteMsgId.length > 0) {
                        
                        for (i = 0; i <= DeleteMsgId.length; i++) {
                            $('#' + DeleteMsgId[i]).remove();
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
                //var chatBoxheight1 = $("#chat1")[0].scrollHeight;
                //ScrollDown(chatBoxheight1);

            },

            error: function (XMLHttpRequest, textStatus, errorThrown) {
                if (errorThrown != "") {

                    $("#lblError").text('We are unable to connect the server. Please try again later. !');
                    $("#ModalError").modal();

                }
                else {
                }
            }
        });
    }, 2000);
   
    //Get Unread Msg Count by calling interval

    MyVarBroker = setInterval(function () {

        var SetUnreadArrayInterval = [];
        //alert('Interval Called :' + GetChatFlag);
        if (GetChatFlag == 'Yes') {
            GetChatFlag = 'No';

            $.ajax({
                type: "POST",
                url: webServiceUrl,
                data: { UserId: CustomerId, MessageId: CustomerMessageId, ActionName: "DoGetUnreadChatMessages" },
                success: function (data) {

                    //alert('Interval responce :'+ data);
                    var obj = JSON.parse(data);
                    var issuccess = obj.IsSuccess;
                    var ErrorMessage = obj.ErrorMessage;

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

                            //$('#chat1').append('<a data-transition="slide" href="#" onclick="ClickonChat(\'' + res.ChatId + '\')"><p id=' + res.ChatId + ' class="Chat speech fontcolorchat">' + res.CustomerMessage + ' <span class="msgRight msgdatetime" style="padding-top:5%;">&nbsp;&nbsp;' + msgdate1 + '</span></p></a>');

                            //$("#ulChatMessages").append('<li style="width:100%" class="Chat" id="' + res.ChatId + '">' +
                            //   '<a id="link' + res.ChatId + '" href="#" onclick="ClickonChat(\'' + res.ChatId + '\')"' +
                            //       '<div class="msj macro">' +
                            //      '<div class="text text-l">' +
                            //              '<p>' + res.CustomerMessage + '</p>' +
                            //               '<p style="float:left;"><small>' + msgdate1 + '</small></p>' +
                            //           '</div>' +
                            //       '</div>' +
                            //   '</a></li><div class="cleardiv"></div>');

                            //$('#chat1').append('<a id="link' + res.ChatId + '" data-transition="slide" href="#" onclick="ClickonChat(\'' + res.ChatId + '\')"><p id=' + res.ChatId +
                            //    ' class="Chat speechleft fontcolorchat">' + res.CustomerMessage +
                            //    ' <span class="msgRight msgdatetime" style="padding-top:5%;">&nbsp;&nbsp;' + msgdate1 +
                            //    '</span></p></a>');


                            $('#chat1').append('<a id="link' + res.ChatId + '" data-transition="slide" href="#" onclick="ClickonChat(\'' + res.ChatId +
                                '\')"><p id=' + res.ChatId + ' class="Chat speech fontcolorchat"><span style="padding-left: 2px;">' + res.BrokerMessage +
                                '</span> <span class="msgRight msgdatetime" style="padding-top:5%;">&nbsp;&nbsp;' + msgdate1 +
                                '</span></p></a>');


                            var chatBoxheight1 = $("#chat1")[0].scrollHeight;
                            //alert(chatBoxheight1);
                            ScrollDown(chatBoxheight1);
                        });

                        var IsMessageDeleted = obj.IsMessageDeleted;
                        //alert(IsMessageDeleted);

                        $.each(IsMessageDeleted, function (i, res) {
                            var IsDeletedonBroker2 = res.IsDeleted;
                            //alert('In set Interval : '+res.IsDeleted);
                            if (res.IsDeleted == 'True') {
                                //$("#brokerChatcontentError").val('You can not send message because broker is no longer available for this chat.');
                                $("#brokerChatcontentError").text('You can not send message because broker is no longer available for this chat.');
                                $("div#divTextArea").hide();
                                $("#brokerChatcontentError").css("font-weight", "bold");
                                $("div#divCustMsgDeleted").show();
                                $("#brokerChatcontentError").attr("disabled", "disabled");

                            }
                            else {
                                $("div#divTextArea").show();
                                $("div#divCustMsgDeleted").hide();
                            }

                        });

                        //alert('array : '+listforread);
                        if (parseInt(SetUnreadArrayInterval.length) > 0) {
                            SetIsReadFlag(SetUnreadArrayInterval, CustomerId, "");
                        }
                        else {
                            GetChatFlag = 'Yes';
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
        }
        else {
        }
        //var chatBoxheight1 = $("#chat1")[0].scrollHeight;
        //alert(chatBoxheight1);
        //ScrollDown(chatBoxheight1);

    }, 2000);
}

function SetIsReadFlag(listforread, UserId, MainMessageId) {
    //alert('Called :' + UserId + ' ' + MainMessageId + '' + listforread);
    var list = listforread.toString();
    //alert('array :' + list + " webServiceUrl - " + webServiceUrl);

    $.ajax({
        type: "POST",
        url: webServiceUrl,
        data: { MessageId: list, UserId: UserId, MainMessageId: MainMessageId, ActionName: "DoSetIsRead" },
        success: function (data) {
            //alert('data '+data);
            var obj = JSON.parse(data);
            var issuccess = obj.IsSuccess;
            var ErrorMessage = obj.ErrorMessage;
            if (issuccess == true) {
                //alert('success');
                GetChatFlag = 'Yes';
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        }
    });
}

function SaveBrokerChat(chatcontent) {
    //alert("Called " + chatcontent + "," + webServiceUrl + ", CustomerMessageId -" + CustomerMessageId);

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

    // $.mobile.loading("show");

    $.ajax({
        type: "POST",
        url: webServiceUrl,
        data: { BrokerMsgId: BrokerMessageId, CustMsgId: CustomerMessageId, CustomerId: CustomerId, BrokerId: BrokerId, CustomerMessage: chatcontent, OldMessageId: 0, LocalDateTime: LocalDateTime, ActionName: "DoSaveCustomerChat" },
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

                    //$("#ulChatMessages").append('<li style="width:100%;" id="' + res.NewMessageId + '"> class="Chat"' +
                    //      '<a id="link' + res.NewMessageId + '" href="#" onclick="ClickonChat(\'' + res.NewMessageId + '\')"' +
                    //          '<div class="msj-rta macro">' +
                    //          '<div class="text text-r">' +
                    //                  '<p id=' + res.NewMessageId + '>' + chatcontent + '</p>' +
                    //                  '<p id="date' + submitChatCount + '" style="float:left;"><small></small></p>' +
                    //              '</div>' +
                    //              '</div>' +
                    //    '</a></li><div class="cleardiv"></div>');

                    //$('#chat1').append('<a id="link' + res.NewMessageId + '" data-transition="slide" href="#" onclick="ClickonChat(\'' + res.NewMessageId +
                    //   '\')"><p id=' + res.NewMessageId + ' class="Chat speechright msgRight fontcolorchat">' + chatcontent +
                    //   '&nbsp;&nbsp; <span class="msgRight msgdatetime" style="padding-top:5%;" id="date' + submitChatCount +
                    //   '"></span></p></a><div class="cleardiv"></div>');


                    $('#chat1').append('<a id="link' + res.NewMessageId + '" data-transition="slide" href="#" onclick="ClickonChat(\'' + res.NewMessageId +
                        '\')"><p id=' + res.NewMessageId + ' class="Chat speechright msgRight fontcolorchat">' + chatcontent +
                        '&nbsp;&nbsp; <span class="msgRight msgdatetime" style="padding-top:5%;" id="date' + submitChatCount +
                        '"> </span></p></a><div class="cleardiv"></div>');

                    $("#date" + submitChatCount).text('Sending....');

                    /*************** Notification Code ******************/
                    //alert('DEviceID ' + DeviceId);

                    var ActionName = '';
                    var NewDevice = '';
                    //alert('DeviceID ' + DeviceId);
                    if (DeviceId != '') {
                        var indexid = DeviceId.indexOf('Android');
                        //alert("indexid - "+indexid)

                        var indexid1 = DeviceId.indexOf('iOS');
                        //alert("indexid1 - "+indexid1)

                        if (parseInt(indexid) >= 0) {
                            //alert('IF ' + DeviceId.indexOf('Android'));
                            ActionName = 'DoPushNotification';
                            NewDevice = DeviceId.replace('Android', '');
                        }
                        else if (parseInt(indexid1) >= 0) {
                            //alert('else IF ' + DeviceId.indexOf('iOS'));
                            ActionName = 'DoPushNotificationForiOS';
                            NewDevice = DeviceId.replace('iOS', '');
                        }
                        //alert("After Loop - "+DeviceId);
                        var title = FirstName + ' ' + LastName;
                        var msgcnt = 1;
                        var message = chatcontent;
                        //alert(webServiceUrl);
                        $.ajax({
                            type: "POST",
                            url: webServiceUrl,
                            data: { DeviceId: NewDevice, message: message, title: title, msgcnt: msgcnt, ActionName: ActionName },
                            success: function (data) {
                                //alert('data '+data);
                                //var obj = JSON.parse(data);

                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                //alert('error'+textStatus);
                            }
                        });

                    }
                    else { }
                    // alert("After if ");
                    /**************** End of Notofication Code ***********/

                    var dt = res.LocalDateTime;
                    var dt1 = dt.split(' ');
                    var datedb = dt1[0];
                    var timedb = dt1[1];
                    var ampmdb = dt1[2];
                    msgdate2 = datedb;


                    var nowdt = new Date();
                    var nowdate = (nowdt.getMonth() + 1) + '/' + nowdt.getDate() + '/' + nowdt.getFullYear();

                    var msgdate2 = '';


                    if (datedb == nowdate) {

                        msgdate2 = timedb;

                        var timeconvert = timedb.split(':');
                        var hrs = timeconvert[0];
                        var min = timeconvert[1];
                        var sec = timeconvert[2];
                        //alert(hrs);
                        if (hrs > 12) {
                            hrs = hrs - 12;
                            //sec="PM";
                        }
                        else if (hrs == 0) {
                            hrs = hrs + 12;
                            //sec="PM";
                        }
                        sec = ampmdb;
                        //msgdate2=hrs+':'+min+' '+sec;
                        if (hrs <= 9) { hrs = "0" + hrs; }
                        msgdate2 = hrs + ':' + min + ' ' + sec; if (min <= 9) { min = "0" + min; }
                        msgdate2 = hrs.slice(-2) + ':' + min.slice(-2) + ' ' + sec;
                        //msgdate2=hrs+':'+min+' '+sec;
                        //alert("msdate2"+msgdate2);
                    }
                    else {
                        msgdate2 = datedb;
                    }

                    // message was here
                    //alert("MsgDate - " + msgdate2 + ',' + " submitChatCount - " + submitChatCount);
                    $("#date" + submitChatCount).text(msgdate2);
                    submitChatCount++;

                    var chatBoxheight1 = $("#chat1")[0].scrollHeight;
                   
                    ScrollDown(chatBoxheight1);

                });

            }
            else if (issuccess == false) {
                $("#lblError").text(ErrorMessage);
                $("#ModalError").modal();

            }
            else {

                $("#lblError").text('Please Try again !');
                $("#ModalError").modal();

            }

        },

        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (errorThrown != "") {
                $("#lblError").text('We are unable to connect the server. Please try again later.');
                $("#ModalError").modal();
            }
        }
    });
}

function ScrollDown(boxheight) {
    var obj = document.getElementById("chat1");
    //alert("Called obj- "+obj);
    obj.scrollTop = obj.scrollHeight;
    //alert("Called1 obj- " + obj);
}


function ClickonChat(ChatId) {
    //alert(ChatIdToDelete);
    if (jQuery.inArray(ChatId, ChatIdToDelete) != -1) {
        //alert("In Array");
        ChatIdToDelete.splice(ChatIdToDelete.indexOf(ChatId), 1);
        $("#link" + ChatId).removeClass("RemoveElt");
        //alert("Length - " + ChatIdToDelete.length);
        if (ChatIdToDelete.length == 0) {

            $("#deletemultiplemsg").css("display", "none");
        }
        else {

            $("#deletemultiplemsg").css("display", "block");
        }
    }
    else {
        //alert("Not In Array");
        ChatIdToDelete.push(ChatId);
        $("#link" + ChatId).addClass("RemoveElt");
        $("#deletemultiplemsg").css("display", "block");
    }
    //alert(ChatIdToDelete);
}

function DeleteChatMessages() {
    var listtodelete = ChatIdToDelete.toString();
    
    $.ajax({
        type: "POST",
        url: webServiceUrl,
        data: { UserId: CustomerId, MessageId: listtodelete, ActionName: "DoDeleteMultipleChatMessage" },
        success: function (data) {
           
            var obj = JSON.parse(data);
            var issuccess = obj.IsSuccess;
            var ErrorMessage = obj.ErrorMessage;
            if (issuccess == true) {
                //alert("Success");
                for (i = 0; i <= ChatIdToDelete.length; i++) {

                    $('#' + ChatIdToDelete[i]).remove();
                }

                ChatIdToDelete = [];
              
            }
            else if (issuccess == false) {
               
                //$("#deletemultiplemsg").hide();//16Mar17
                //$("#deletemessages").show();//16Mar17
                //$('.tickimg').css({ display: "none" });//16Mar17
                //editclick = "false";

                //$('#DeleteMessageModel').dialog('close');
                //return false;
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        }
    });

}

function CancelDeleteChatMessages()
{    
    for (i = 0; i <= ChatIdToDelete.length; i++) {
        $("#link" + ChatIdToDelete[i]).removeClass("RemoveElt");
    }
    ChatIdToDelete = [];
}








